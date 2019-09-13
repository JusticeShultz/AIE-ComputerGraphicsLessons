using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meateor_Logic : MonoBehaviour
{
    public GameObject MeateorExplosion;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(MeateorExplosion, transform.position, Quaternion.Euler(-90,0,0));

        if(collision.gameObject.name == "Player")
        {
            PlayerController.TakeDamage(9999);
        }

        Destroy(gameObject);
    }
}
