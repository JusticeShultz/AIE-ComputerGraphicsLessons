using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRef : MonoBehaviour
{
    public static Camera cam;
	
	void Start ()
    {
        cam = GetComponent<Camera>();
	}
}
