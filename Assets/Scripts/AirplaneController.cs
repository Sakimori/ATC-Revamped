using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    GameObject nextDestination;

    Jurisdiction jurisdiction;
    
    [SerializeField]
    int airspeedMin;
    
    [SerializeField]
    int airspeedMax;

    [SerializeField]
    float airspeedCurrent;

    bool inMotion;

    // Start is called before the first frame update
    void Start()
    {
        inMotion = true;
    }

    // Update is called once per frame
    void Update()
    {
        float step = airspeedCurrent * Time.deltaTime;
        if (inMotion)
        {
            transform.LookAt(nextDestination.transform);
            transform.position = Vector3.MoveTowards(transform.position, nextDestination.transform.position, step);
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Bang!");
        if (collision.gameObject == nextDestination)
        {
            if (!nextDestination.GetComponent<Waypoint>().isEnd)
            {
                nextDestination = nextDestination.GetComponent<Waypoint>().nextWaypoint.GetComponent<Waypoint>().gameObject;
            }
            else
            {
                inMotion = false;
                nextDestination = gameObject;
            }
        }
    }
}
