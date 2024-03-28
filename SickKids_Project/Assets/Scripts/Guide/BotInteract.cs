using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotInteract : Interactable
{
    private WaypointMover waypointMover;
    private bool isInteracted = false;

    void Start()
    {
        // Get the WaypointMover component from this GameObject
        waypointMover = GetComponent<WaypointMover>();
    }

    protected override void Interact()
    {
        if (waypointMover != null)
        {
            if (!isInteracted)
            {
                waypointMover.displayInfo();
            }
            else
            {
                waypointMover.AdvanceToNextWaypoint();
            }
            isInteracted = !isInteracted; // Toggle the interacted state
        }
    }
}