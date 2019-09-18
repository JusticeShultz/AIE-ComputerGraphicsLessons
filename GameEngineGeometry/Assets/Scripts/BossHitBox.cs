using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitBox : MonoBehaviour
{
    public static bool InRange = false;

    private void Awake()
    {
        InRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.name == "Player")
            InRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.name == "Player")
            InRange = false;
    }
}