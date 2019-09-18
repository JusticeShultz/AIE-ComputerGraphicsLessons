using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayshrineSystem : MonoBehaviour
{
    public static Vector3 shrinePoint = new Vector3(0, 1.236f, 0);
    public static GameObject currentShrine = null;

    public MeshRenderer Top;
    public MeshRenderer Bottom;
    public Material CurrentMarker1;
    public Material CurrentMarker2;
    public Material NonCurrent1;
    public Material NonCurrent2;

    void Start ()
    {
		if(shrinePoint == Vector3.zero) shrinePoint = new Vector3(0, 1.236f, 0);

        PlayerController.player.transform.position = shrinePoint;
    }

    private void Update()
    {
        if(currentShrine != gameObject)
        {
            Top.material = NonCurrent1;
            Bottom.material = NonCurrent2;
        }
        else
        {
            Top.material = CurrentMarker1;
            Bottom.material = CurrentMarker2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player") return;

        currentShrine = gameObject;
        shrinePoint = new Vector3(transform.position.x, 1.236f, transform.position.z);
    }
}
