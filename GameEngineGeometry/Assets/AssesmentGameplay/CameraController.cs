using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float FollowSpeed = 0.1f;
    public GameObject Target;
    public Vector3 Offset = new Vector3(0, 16, 0);

    void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, Target.transform.position + Offset, FollowSpeed);
	}
}