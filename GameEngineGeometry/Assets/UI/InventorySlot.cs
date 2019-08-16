using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public static bool HoldingObject = false;
    static List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public Image sprite;
    public GameObject canvas;
    public string ItemName = "NA";
    public Sprite ItemIcon;
    public Material overMat;
    private bool IsHovered = false;
    private bool IsDragging = false;
    private bool SpecialHover = false;

    private void Awake()
    {
        inventorySlots.Clear();
    }

    private void Start()
    {
        inventorySlots.Add(this);
        HoldingObject = false;
    }

    void Update ()
    {
        if (!Input.GetMouseButton(0) && !HoldingObject)
        {
            if (SpecialHover) IsHovered = true;
        }

        if (!sprite) return;

        if(Input.GetMouseButton(0))
        {
            if (IsHovered) IsDragging = true;

            if (IsHovered || IsDragging)
            {
                if (ItemName != "NA")
                {
                    if (Crafting.CanCraft && gameObject.name == "Result")
                    {
                        for (int i = 0; i < inventorySlots.Count; i++)
                        {
                            if (inventorySlots[i].ItemName == "NA")
                            {
                                InventorySlot slot = inventorySlots[i];

                                string hitItemName = slot.ItemName;
                                Sprite hitItemIcon = slot.ItemIcon;
                                string myItemName = ItemName;
                                Sprite myItemIcon = ItemIcon;

                                slot.ItemName = myItemName;
                                slot.ItemIcon = myItemIcon;
                                ItemName = hitItemName;
                                ItemIcon = hitItemIcon;

                                sprite.sprite = hitItemIcon;
                                slot.sprite.sprite = myItemIcon;

                                break;
                            }
                        }

                        Crafting.reference.DestroyCraftingMaterials();
                    }
                    else
                    {
                        HoldingObject = true;
                        Vector2 pos;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, CameraRef.cam, out pos);
                        sprite.rectTransform.position = canvas.transform.TransformPoint(pos);
                        sprite.material = overMat;
                    }
                }
            }
        }
        else
        {
            if (IsDragging)
            {
                for (int i = 0; i < inventorySlots.Count; i++)
                {
                    if (inventorySlots[i].name == "Result") continue;

                    if (GetWorldSpaceRect(inventorySlots[i].gameObject.GetComponent<Image>().rectTransform).Overlaps(GetWorldSpaceRect(sprite.rectTransform)))
                    {
                        //print(ItemName);
                        InventorySlot slot = inventorySlots[i];

                        string hitItemName = slot.ItemName;
                        Sprite hitItemIcon = slot.ItemIcon;
                        string myItemName = ItemName;
                        Sprite myItemIcon = ItemIcon;

                        slot.ItemName = myItemName;
                        slot.ItemIcon = myItemIcon;
                        ItemName = hitItemName;
                        ItemIcon = hitItemIcon;

                        sprite.sprite = hitItemIcon;
                        slot.sprite.sprite = myItemIcon;
                    }
                }

                HoldingObject = false;
                IsDragging = false;
            }
        }

        if(!IsDragging)
        {
            sprite.material = null;
            sprite.rectTransform.localPosition = new Vector3(0, 0, 0);
        }

        if (ItemName == "NA")
        {
            sprite.color = Color.clear;
            sprite.enabled = false;
        }
        else
        {
            if (ItemIcon && sprite)
                sprite.sprite = ItemIcon;

            if (sprite.sprite)
                sprite.enabled = true;

            sprite.color = Color.white;
        }
    }

    private void OnDrawGizmos()
    {
        if (ItemIcon && sprite)
            sprite.sprite = ItemIcon;
    }

    private void OnMouseEnter()
    {
        SpecialHover = true;
        if (HoldingObject) return;
        IsHovered = true;
        //print("Hovered");
    }

    private void OnMouseExit()
    {
        SpecialHover = false;
        IsHovered = false;
        //print("Unhovered");
    }

    Rect GetWorldSpaceRect(RectTransform rt)
    {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = rt.TransformVector(r.size);
        return r;
    }
}