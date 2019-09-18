using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//It's a pun, it's not spelled wrong :(
public class Meateor : MonoBehaviour
{
    public float SpawnRate = 4.0f;
    public GameObject Indicator;
    public GameObject Dropdown;
    float currentTime = 0;

    private void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.player.transform.position) > 50)
            return;

        currentTime += Time.deltaTime;

        if (currentTime >= SpawnRate)
        {
            GameObject ind = Instantiate(Indicator, new Vector3(transform.position.x, 0.3f, transform.position.z) + new Vector3(Random.Range(-25f, 25f), 0, Random.Range(-25f, 25f)), Quaternion.Euler(-90, 0, 0));
            Instantiate(Dropdown, ind.transform.position + (Vector3.up * 20), Quaternion.identity);

            Destroy(ind, 2.25f);
            currentTime = 0;
        }
    }
}
