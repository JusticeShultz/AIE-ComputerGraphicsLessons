using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeath : MonoBehaviour
{
    public float Time = 4.0f;

    void Start ()
    {
        Destroy(gameObject, Time);
	}
}