using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
{
    public bool DebugMode = false;
    public bool IsHeadBoss = false;

    public float FalloffRange = 25f;
    public float AttackRange = 3f;
    public float AttackSpeed = 1.0f;
    public NavMeshAgent NavAgent;
    public Animator Animator;
    public float DashCooldown = 20.0f;
    public float DashForce = 320.0f;
    private float currentDashCooldown = 0f;
    private float attackCooldown = 0f;

    void Update ()
    {
        if (PlayerController.controller.Health <= 0)
        {
            if(NavAgent.enabled)
                NavAgent.isStopped = true;

            Animator.SetBool("Moving", false);
            return;
        }

        if (Vector3.Distance(transform.position, PlayerController.player.transform.position) <= FalloffRange)
        {
            if(IsHeadBoss)
            {
                currentDashCooldown += Time.deltaTime;

                if(currentDashCooldown >= DashCooldown)
                {
                    currentDashCooldown = 0f;
                    Animator.SetTrigger("Dash");
                    StartCoroutine(DashOver());
                    GetComponent<Rigidbody>().AddForce(transform.forward * DashForce, ForceMode.Force);
                }
            }

            if (NavAgent.enabled == false) return;

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
            if (NavAgent.enabled == false) return;

            if (DebugMode)
                print("Going idle");
            
            NavAgent.isStopped = true;
        }

        Animator.SetBool("Moving", !NavAgent.isStopped);
	}

    IEnumerator DashOver()
    {
        NavAgent.enabled = false;
        yield return new WaitForSeconds(1.0f);
        NavAgent.enabled = true;
    }
}
