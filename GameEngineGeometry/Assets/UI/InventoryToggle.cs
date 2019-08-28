using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToggle : MonoBehaviour
{
    public static CollectableData collectable;
    public static bool InventoryOpened = false;

    public GameObject InventoryObject;
    public GameObject InventoryCamera;
    public GameObject GameCamera;

    public GameObject PickupTooltip;
    public Text pickupName;
    
    private bool InventoryToggled = false;

	void Start ()
    {
        collectable = null;
    }

    void Update()
    {
        PickupTooltip.SetActive(collectable != null);

        if (collectable != null)
        {
            pickupName.text = "Press E to pick up [1] " + collectable.itemName;

            if (Input.GetKeyDown(KeyCode.E))
            {
                for (int i = 0; i < 29; i++)
                {
                    if (InventorySlot.inventorySlots[i].ItemName == "NA" && InventorySlot.inventorySlots[i].transform.parent.name != "CraftingWindow")
                    {
                        InventorySlot slot = InventorySlot.inventorySlots[i];

                        slot.ItemName = collectable.itemName;
                        slot.ItemFlavor = collectable.itemFlavor;
                        slot.ItemIcon = collectable.itemIcon;

                        slot.sprite.sprite = collectable.itemIcon;

                        Destroy(collectable._gameObject);
                        collectable = null;
                        break;
                    }
                }
            }
        }
        else pickupName.text = "";

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryToggled = !InventoryToggled;
            InventoryOpened = InventoryToggled;
        }

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

            //if (InventorySlot.tooltip)
            //    Destroy(InventorySlot.tooltip);

            GameCamera.SetActive(true);
        }
    }
}
