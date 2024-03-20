using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;

public class CollectibleItem : Interactable
{
    public string itemName; // Item name to be set in the Unity Editor

    private Renderer itemRenderer;
    private Material itemMaterial;
    private Color originalEmissionColor;

    void Start()
    {
        itemRenderer = GetComponent<Renderer>();
        if (itemRenderer != null)
        {
            itemMaterial = itemRenderer.material;
            // Store the original emission color
            originalEmissionColor = itemMaterial.GetColor("_EmissionColor");
        }
    }

    protected override void Interact()
    {
        Debug.Log("Collected " + itemName);

        // Save the item name to cloud
        UpdateCollectedItemsInCloud(itemName);

        // Turn off emission to remove the glow
        if (itemMaterial != null)
        {
            itemMaterial.SetColor("_EmissionColor", Color.black);
            // Make sure to update global illumination if needed
            DynamicGI.SetEmissive(itemRenderer, Color.black);
        }
    }

    // Reset emission when the game object is disabled (e.g., re-enable glow when the item is respawned)
    void OnDisable()
    {
        if (itemMaterial != null)
        {
            itemMaterial.SetColor("_EmissionColor", originalEmissionColor);
        }
    }

    private async void UpdateCollectedItemsInCloud(string newItem)
    {
        try
        {
            // Load the existing list of collected items from the cloud
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "lastCollectedItems" });
            List<string> collectedItems;

            if (playerData.TryGetValue("lastCollectedItems", out var existingItems))
            {
                collectedItems = existingItems.Value.GetAs<List<string>>();
            }
            else
            {
                collectedItems = new List<string>();
            }

            // Add the new item to the list
            collectedItems.Add(newItem);

            // Save the updated list back to the cloud
            await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object> { { "lastCollectedItems", collectedItems } });
            Debug.Log("Updated collected items in the cloud.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error updating collected items in the cloud: " + ex.Message);
        }
    }
}
