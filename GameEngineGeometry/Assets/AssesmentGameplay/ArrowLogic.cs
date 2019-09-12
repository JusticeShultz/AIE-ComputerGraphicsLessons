using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLogic : MonoBehaviour
{
    public GameObject ArrowDeath;

    void Start ()
    {
        Destroy(gameObject, 8);
	}

    //Lodge the arrow in things.
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position += transform.forward * 0.8f;
    }

    private void OnDestroy()
    {
        GameObject arrowsDeath = Instantiate(ArrowDeath, transform.position, transform.rotation);
    }
}
