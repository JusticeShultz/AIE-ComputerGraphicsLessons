using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WayshrineSystem : MonoBehaviour
{
    public enum Mod { None, LoadLevel };

    public static Vector3 shrinePoint = new Vector3(0, 1.236f, 0);
    public static GameObject currentShrine = null;

    public MeshRenderer Top;
    public MeshRenderer Bottom;
    public Material CurrentMarker1;
    public Material CurrentMarker2;
    public Material NonCurrent1;
    public Material NonCurrent2;

    //This stuff could probably hidden with some editor GUI down the line
    public Mod _Mod = Mod.None;
    public float Timer = 8.0f;
    public int LevelIndex;
    public Animator Fader;
    private bool Triggered = false;

    void Start ()
    {
		if(shrinePoint == Vector3.zero) shrinePoint = new Vector3(0, 1.236f, 0);

        PlayerController.player.transform.position = shrinePoint;
    }

    private void Update()
    {
        if (_Mod == Mod.None)
        {
            if (currentShrine != gameObject)
            {
                Top.material = NonCurrent1;
                Bottom.material = NonCurrent2;
            }
            else
            {
                Top.material = CurrentMarker1;
                Bottom.material = CurrentMarker2;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player") return;

        if (_Mod == Mod.LoadLevel)
        {
            if(!Triggered)
            {
                Triggered = true;
                Fader.SetTrigger("Fade");
                StartCoroutine(LoadLevel());
            }
        }
        else
        {
            currentShrine = gameObject;
            shrinePoint = new Vector3(transform.position.x, 1.236f, transform.position.z);
        }
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(Timer);
        SceneManager.LoadScene(LevelIndex);
    }
}
