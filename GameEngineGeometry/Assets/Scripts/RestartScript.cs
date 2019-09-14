using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScript : MonoBehaviour
{
    public Image Image;
    public Button Button;
    public Color Color;

	void Update ()
    {
        Button.enabled = Image.color != Color;

		if(Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
