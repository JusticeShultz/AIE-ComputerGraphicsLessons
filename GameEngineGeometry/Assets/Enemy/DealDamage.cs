using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public float Damage = 1.0f;

	public void InflictDamage()
    {
        PlayerController.TakeDamage(Damage);
    }
}
