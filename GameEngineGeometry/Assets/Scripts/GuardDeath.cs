using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDeath : MonoBehaviour
{
    public bool ShouldLandOnGround = true;

    bool Hit = false;

    private void Update()
    {
        if(Hit)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -1.2f, transform.position.z), 0.025f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Arrow"))
        {
            Destroy(other.gameObject);
            GetComponent<Animator>().SetTrigger("Die");

            if(GetComponent<Guard>())
            {
                Destroy(GetComponent<Guard>());
            }

            if (ShouldLandOnGround) Hit = true;
        }
    }
}
