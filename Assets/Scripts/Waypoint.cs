using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    public bool isEnd;

    [SerializeField]
    public GameObject nextWaypoint;

    [SerializeField]
    WaypointPathType type;

    [SerializeField]
    Jurisdiction jurisdiction;

    public int forcedSpeed;

    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case WaypointPathType.Entry:
            case WaypointPathType.Exit:
                //forcedSpeed = 0;
                jurisdiction = Jurisdiction.Handoff;
                break;

            case WaypointPathType.Holding:
            case WaypointPathType.Takeoff:
                //forcedSpeed = 0;
                jurisdiction = Jurisdiction.Tower;
                break;
            
            case WaypointPathType.Approach:
            case WaypointPathType.Landing:
                //forcedSpeed = 2;
                jurisdiction = Jurisdiction.Tower;
                break;

            case WaypointPathType.TaxiArrival:
            case WaypointPathType.TaxiDeparture:
                //forcedSpeed = 1;
                jurisdiction = Jurisdiction.Ground;
                break;

            default:
                Debug.LogError($"No jurisdiction found for path type {jurisdiction}.");
                break;
        }
    }
}
