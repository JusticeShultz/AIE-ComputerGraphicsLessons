using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public List<GameObject> wanderPoints = new List<GameObject>();
    public int currentDestination = 0;

    void Update()
    {
        if (wanderPoints.Count > currentDestination)
        {
            transform.LookAt(wanderPoints[currentDestination].transform.position, Vector3.up);
            transform.position += transform.forward * 0.05f;

            if(Vector3.Distance(transform.position, wanderPoints[currentDestination].transform.position) <= 1.25f)
            {
                ++currentDestination;
            }
        }
        else
        {
            currentDestination = 0;
        }
	}

    private void OnDrawGizmos()
    {
        foreach (GameObject obj in wanderPoints)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
