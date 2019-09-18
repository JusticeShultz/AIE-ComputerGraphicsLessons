using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAlter : MonoBehaviour
{
    public Vector3 Offset;
    public float SpawnRate = 5.0f;
    public GameObject Skeleton;
    float currentTime = 0;

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.player.transform.position) > 30)
            return;

        currentTime += Time.deltaTime;

        if(currentTime >= SpawnRate)
        {
            Instantiate(Skeleton, transform.position + Offset, Quaternion.identity);
            currentTime = 0;
        }
    }
}
