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

    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case WaypointPathType.Entry:
            case WaypointPathType.Exit:
                jurisdiction = Jurisdiction.Handoff;
                break;

            case WaypointPathType.Holding:
            case WaypointPathType.Approach:
            case WaypointPathType.Landing:
            case WaypointPathType.Takeoff:
                jurisdiction = Jurisdiction.Tower;
                break;

            case WaypointPathType.TaxiArrival:
            case WaypointPathType.TaxiDeparture:
                jurisdiction = Jurisdiction.Ground;
                break;

            default:
                Debug.LogError($"No jurisdiction found for path type {jurisdiction}.");
                break;
        }
    }
}
