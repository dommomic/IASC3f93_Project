using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotInteract : Interactable
{
    private WaypointMover waypointMover;

    void Start()
    {
        // Get the WaypointMover component from this GameObject
        waypointMover = GetComponent<WaypointMover>();
    }

    protected override void Interact()
    {
        // Trigger the waypoint mover to advance to the next waypoint
        if (waypointMover != null)
        {
            waypointMover.AdvanceToNextWaypoint();
        }
    }
}