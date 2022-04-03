using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTracker : MonoBehaviour
{
    private Vector3 houseStartLoc;
    // Start is called before the first frame update
    void Start()
    {
        houseStartLoc = transform.position;
    }

    public float houseMoveDistance()
    {
        return (houseStartLoc - transform.position).magnitude;
    }
    
}
