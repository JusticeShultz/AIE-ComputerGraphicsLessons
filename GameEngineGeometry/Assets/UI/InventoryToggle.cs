using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject InventoryObject;
    public GameObject InventoryCamera;
    public GameObject GameCamera;

    private bool InventoryToggled = false;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            InventoryToggled = !InventoryToggled;

        if(InventoryToggled)
        {
            InventoryObject.SetActive(true);
            InventoryCamera.SetActive(true);
            GameCamera.SetActive(false);
        }
        else
        {
            InventoryObject.SetActive(false);
            InventoryCamera.SetActive(false);
            GameCamera.SetActive(true);
        }
    }
}
