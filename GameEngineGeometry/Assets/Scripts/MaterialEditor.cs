using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode] 
#endif
public class MaterialEditor : MonoBehaviour
{
    Renderer rend;

    public Color albedoColor;
    public List<Texture> albedoMaps;
    public float FrameSwitchTime;
    public int currentFrame;
    public bool Fade = false;
    public float FadeStart;
    public float FadeSpeed;
    public GameObject Camera;

    private float timeSinceLastFrameSwitch = 0;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update ()
    {
        if(Fade)
        {
            float fade = Vector3.Distance(transform.position, Camera.transform.position);

            if (fade > FadeStart)
            {
                albedoColor.a = Mathf.Lerp(albedoColor.a, FadeStart / fade, FadeSpeed);
            }
            else albedoColor.a = Mathf.Lerp(albedoColor.a, 1, FadeSpeed);

            rend.sharedMaterial.SetColor("_Color", albedoColor);
        }
        else
            rend.sharedMaterial.SetColor("_Color", albedoColor);

        if (albedoMaps.Count > 0)
        {
            if (albedoMaps.Count > 1)
            {
                timeSinceLastFrameSwitch += Time.deltaTime;

                if(timeSinceLastFrameSwitch >= FrameSwitchTime)
                {
                    ++currentFrame;

                    if(currentFrame > albedoMaps.Count - 1)
                        currentFrame = 0;

                    rend.sharedMaterial.SetTexture("_MainTex", albedoMaps[currentFrame]);
                    timeSinceLastFrameSwitch = 0;
                }
            }
            else rend.sharedMaterial.SetTexture("_MainTex", albedoMaps[0]);
        }
	}
}