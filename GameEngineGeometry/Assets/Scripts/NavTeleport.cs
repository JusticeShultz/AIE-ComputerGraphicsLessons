using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTeleport : MonoBehaviour
{
    public float Frequency = 20f;
    public float MaxDistance = 350f;
    public GameObject WarmUpEffect;
    public GameObject EndEffect;
    public NavMeshAgent Agent;

    private float ChargeUpTime = 0f;

	void Update ()
    {
        ChargeUpTime += Time.deltaTime;

        if (ChargeUpTime >= Frequency)
        {
            StartCoroutine(Teleport());
            ChargeUpTime = 0f;
        }
	}

    IEnumerator Teleport()
    {
        GameObject effect = Instantiate(WarmUpEffect);
        effect.transform.parent = transform;
        effect.transform.localPosition = new Vector3(0, 0.25f, 0);
        effect.transform.localScale = new Vector3(1f, 1f, 1f);
        effect.transform.localRotation = Quaternion.Euler(-90, 0, -180);

        yield return new WaitForSeconds(5.5f);

        Instantiate(EndEffect, transform.position + new Vector3(0, -1.8f, 0), transform.rotation);

        Vector3 randomDirection = Random.insideUnitSphere * MaxDistance;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, MaxDistance, 1);
        Vector3 finalPosition = hit.position;

        Agent.Warp(finalPosition);

        Destroy(effect);
    }
}

//0,-1.8,0
//84.51665, 77.1367, 84.5167