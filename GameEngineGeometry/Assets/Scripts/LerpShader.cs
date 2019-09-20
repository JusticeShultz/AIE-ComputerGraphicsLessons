using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpShader : MonoBehaviour
{
    public MeshRenderer Renderer;
    public string ShaderID;
    public float Target = 1f;
    public float Speed = 0.01f;
    private float CurrentNum = 0f;

    private void Start()
    {
        Renderer.material.enableInstancing = true;
    }

    void Update ()
    {
        CurrentNum = Mathf.Lerp(CurrentNum, Target, Speed);
        Renderer.material.SetFloat(ShaderID, CurrentNum);
    }
}
