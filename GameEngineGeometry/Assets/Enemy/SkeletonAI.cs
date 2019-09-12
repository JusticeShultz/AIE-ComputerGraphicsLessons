using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    public bool DebugMode = false;

    public float FalloffRange = 25f;
    public float AttackRange = 3f;
    public float AttackSpeed = 1.0f;
    public NavMeshAgent NavAgent;
    public Animator Animator;
    private float attackCooldown = 0f;

    void Update ()
    {
        if (PlayerController.controller.Health <= 0) return;

        if (Vector3.Distance(transform.position, PlayerController.player.transform.position) <= FalloffRange)
        {
            if (Vector3.Distance(transform.position, PlayerController.player.transform.position) <= AttackRange)
            {
                //NavAgent.isStopped = true;
                attackCooldown += Time.deltaTime;

                if (DebugMode)
                    print("Trying to attack: " + attackCooldown);

                if (attackCooldown >= AttackSpeed)
                {
                    if (DebugMode)
                        print("Attack triggered");

                    attackCooldown = 0f;
                    Animator.SetTrigger("Attack");
                }

                NavAgent.isStopped = true;
            }
            else
            {
                if (DebugMode)
                    print("Trying to get in range of player, distance to go still: " + NavAgent.remainingDistance);

                attackCooldown = 0;
                NavAgent.isStopped = false;
                NavAgent.SetDestination(PlayerController.player.transform.position);
            }
        }
        else
        {
            if (DebugMode)
                print("Going idle");
            
            NavAgent.isStopped = true;
        }

        Animator.SetBool("Moving", !NavAgent.isStopped);
	}
}
