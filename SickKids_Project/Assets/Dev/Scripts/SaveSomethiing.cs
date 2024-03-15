using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine.UI; // Import the UI namespace

public class SaveSomethiing : MonoBehaviour
{
    public InputField usernameInputField; // Reference to the username input field
    public InputField passwordInputField; // Reference to the password input field
    public InputField itemInputField; // Reference to the item input field
    private Dictionary<string, object> playerData;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
   
       
    }
    

    public async void SaveData()
    {
        // Initialize playerData with default values only if it hasn't been initialized yet
        if (playerData == null)
        {
            playerData = new Dictionary<string, object>
            {
                { "firstKeyName", "jose2" },
                { "ThirdKey", 0 } // Assuming initial value is 0
            };
        }

        // Now playerData is guaranteed to be initialized, proceed with checking and updating "ThirdKey"
        if (playerData.ContainsKey("ThirdKey"))
        {
            // Retrieve the current value of "ThirdKey"
            int currentValue = (int)playerData["ThirdKey"];

            // Increment the value by 1
            int newValue = currentValue + 1;

            // Update the dictionary with the new value
            playerData["ThirdKey"] = newValue;
        }
        else
        {
            // If "ThirdKey" does not exist, add it to the dictionary with a value of 1
            playerData.Add("ThirdKey", 1);
        }

        // Proceed to save the updated player data
        var result = await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");
    }
    
    public async void IncrementAndSaveData()
    {
        // Check if "ThirdKey" exists in the playerData dictionary
        if (playerData.ContainsKey("ThirdKey"))
        {
            // Ensure the value associated with "ThirdKey" is an integer
            if (playerData["ThirdKey"] is int currentValue)
            {
                // Increment the value by 1
                playerData["ThirdKey"] = currentValue + 1;
            }
            else
            {
                // Handle the case where the value is not an integer, if necessary
                Debug.LogError("The value of 'ThirdKey' is not an integer.");
            }
        }
        else
        {
            // If "ThirdKey" does not exist, add it to the dictionary with a value of 1
            playerData["ThirdKey"] = 1;
        }

        // Proceed to save the updated player data
        var result = await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");
    }

    public async void SignUpWithUsernamePassword()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                AuthenticationService.Instance.SignOut();
            }

            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");

            // Initialize player data upon successful signup
            var initialPlayerData = new Dictionary<string, object>
            {
                { "username", username },
                { "itemsFound", new List<string>() }, // Empty list for items found
                { "questsCompleted", new List<string>() } // Empty list for quests completed
            };

            await CloudSaveService.Instance.Data.Player.SaveAsync(initialPlayerData);
            Debug.Log("Initialized player data for new user: "+ username);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during sign up or initializing data: {ex.Message}");
        }
    }
    
    public async void SignInWithUsernamePasswordAsync()
    {
        
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        
        try
        {
            AuthenticationService.Instance.SignOut();
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    
    public async void AddItemToFoundItems()
    {
        // Use the text from the input field as the item name
        string item = itemInputField.text;

        try
        {
            // Load the current "itemsFound" list
            var keysToLoad = new HashSet<string> { "itemsFound" };
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(keysToLoad);

            if (playerData.TryGetValue("itemsFound", out var itemsFoundObj) && itemsFoundObj.Value.GetAs<List<string>>() is List<string> itemsFoundList)
            {
                // Add the new item
                itemsFoundList.Add(item);

                // Prepare the updated data
                var updatedData = new Dictionary<string, object> { { "itemsFound", itemsFoundList } };

                // Save the updated player data
                await CloudSaveService.Instance.Data.Player.SaveAsync(updatedData);
                Debug.Log($"Item '{item}' added to the player's found items.");
            }
            else
            {
                Debug.LogError("Failed to retrieve or parse 'itemsFound' list.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to add item to 'itemsFound': " + ex.Message);
        }
    }






}