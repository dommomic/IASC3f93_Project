using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointMover : MonoBehaviour
{
    //Stores a reference to the waypoint system
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float distanceThreshold = 0.1f;
    public Canvas canvas1; // The game Canvas
    public Canvas canvas2; // The canvas of the chat
    private Transform currentWaypoint;
    public Text chatText;
    private String[] chatStrings = new []{"First Stop", "Second Stop", "Third Stop" , "Fourth Stop"};
     
    
    // Start is called before the first frame update
    void Start()
    {
        //Set Initial pos
        
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;
        
        //set next waypoint target
        transform.LookAt(currentWaypoint);
       
    }

    // Update is called once per frame
    void Update()
    {
       
        moveToNextWaypoint();
       
        
    }

    private void moveToNextWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

    }
    
    public void AdvanceToNextWaypoint()
    {
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint);
        
    }

    private void displayInfo()
    {
        Debug.Log(currentWaypoint.GetSiblingIndex()- 1);
    }
    
    private void SwitchCanvasAndUpdateText()
    {
        if (canvas1 != null)
        {
            canvas1.gameObject.SetActive(false);
        }

        if (canvas2 != null)
        {
            canvas2.gameObject.SetActive(true);
           
            
        }
    }
    
    
}
