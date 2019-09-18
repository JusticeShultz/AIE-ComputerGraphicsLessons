using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public bool IsBoss = false;
    public float Damage = 1.0f;
    public float AttackDistance = 3.5f;
    
	public void InflictDamage()
    {
        if (!IsBoss)
        {
            if (Vector3.Distance(PlayerController.player.transform.position, transform.position) <= AttackDistance)
                PlayerController.TakeDamage(Damage);
        }
        else
        {
            if(BossHitBox.InRange)
                PlayerController.TakeDamage(Damage);
        }
    }
}