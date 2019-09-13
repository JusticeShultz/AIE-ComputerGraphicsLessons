using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTarget : MonoBehaviour
{
    public GameObject DestroyedState;
    public int Hits = 1;
    public bool DestroyParent = true;
    public bool IsBoss = true;
    private int storedHits = 0;

    private void OnCollisionEnter(Collision collision)
    {
        //This will destroy the target and spawn the debris object.
        if (collision.gameObject.name.Contains("Arrow"))
        {
            Destroy(collision.gameObject);
            ++storedHits;

            if (storedHits >= Hits)
            {
                if (IsBoss)
                    Instantiate(DestroyedState, transform.position, Quaternion.identity);
                else
                    Instantiate(DestroyedState, transform.position, transform.rotation);

                if (DestroyParent)
                    Destroy(gameObject.transform.parent.gameObject);
                else Destroy(gameObject);
            }
        }
    }
}
