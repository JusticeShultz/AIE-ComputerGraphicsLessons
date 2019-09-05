using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public new Light light;

    void Start()
    {
        Destroy(gameObject, 0.8f);
    }

    void Update()
    {
        light.intensity = Mathf.Lerp(light.intensity, 0, 0.17f);
    }
}
