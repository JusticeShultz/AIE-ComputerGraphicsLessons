using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is simply made to do CameraRef.cam to access the games camera object.

public class CameraRef : MonoBehaviour
{
    public static Camera cam;
	
	void Start ()
    {
        cam = GetComponent<Camera>();
	}
}
