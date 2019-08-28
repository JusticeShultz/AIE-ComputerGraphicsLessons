using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public static bool HoldingObject = false;
    public static InventorySlot[] inventorySlots = new InventorySlot[29];

    public Image sprite;
    public GameObject canvas;
    public string ItemName = "NA";
    public string ItemFlavor = "Little is known about this item...";
    public Sprite ItemIcon;
    public Material overMat;
    public GameObject BaseTooltip;
    public Vector3 tooltipOffset = Vector3.zero;
    public int inventoryIndex = 0;

    private bool IsHovered = false;
    private bool IsDragging = false;
    private bool SpecialHover = false;
    private GameObject tooltip;

    private void Start()
    {
        inventorySlots[inventoryIndex] = this;
        HoldingObject = false;
    }

    void Update ()
    {
        if(IsHovered && ItemName != "NA")
        {
            if (!tooltip)
            {
                tooltip = Instantiate(BaseTooltip);
                tooltip.transform.SetParent(transform);
                RectTransform transformOfTooltip = tooltip.GetComponent<RectTransform>();
                transformOfTooltip.localScale = Vector3.one;
                transformOfTooltip.localPosition = tooltipOffset;
                transformOfTooltip.localRotation = transform.localRotation;
                TooltipData informationFields = tooltip.GetComponent<TooltipData>();
                informationFields.nameObject.text = ItemName;
                informationFields.flavorObject.text = ItemFlavor;
            }
            else
            {
                Vector2 pos01;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, CameraRef.cam, out pos01);
                
                if(tooltipOffset.x > 0)
                    tooltip.transform.localPosition = Vector3.Lerp(tooltip.transform.localPosition, tooltipOffset + (new Vector3(pos01.x, 0, 0) * 0.6f) + new Vector3(500, 0, 0), 0.1f);
                else if (tooltipOffset.x == 0)
                    tooltip.transform.localPosition = Vector3.Lerp(tooltip.transform.localPosition, tooltipOffset + (new Vector3(pos01.x, 0, 0) * 0.6f), 0.1f);
                else
                    tooltip.transform.localPosition = Vector3.Lerp(tooltip.transform.localPosition, tooltipOffset + (new Vector3(pos01.x, 0, 0) * 0.6f) - new Vector3(500, 0, 0), 0.1f);
            }
        }
        else
        {
            if (tooltip) Destroy(tooltip);
        }

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
                        for (int i = 0; i < 29; i++)
                        {
                            if (inventorySlots[i].ItemName == "NA")
                            {
                                InventorySlot slot = inventorySlots[i];

                                slot.ItemName = ItemName;
                                slot.ItemFlavor = ItemFlavor;
                                slot.ItemIcon = ItemIcon;
                                slot.sprite.sprite = ItemIcon;

                                ItemName = "NA";
                                ItemFlavor = "NA";
                                ItemIcon = null;

                                break;
                            }
                        }

                        if(ItemName == "NA")
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
                for (int i = 0; i < 29; i++)
                {
                    if (inventorySlots[i].name == "Result") continue;

                    if (GetWorldSpaceRect(inventorySlots[i].gameObject.GetComponent<Image>().rectTransform).Overlaps(GetWorldSpaceRect(sprite.rectTransform)))
                    {
                        //print(ItemName);
                        InventorySlot slot = inventorySlots[i];

                        string hitItemName = slot.ItemName;
                        string hitItemFlavor = slot.ItemFlavor;
                        Sprite hitItemIcon = slot.ItemIcon;
                        string myItemName = ItemName;
                        string myItemFlavor = ItemFlavor;
                        Sprite myItemIcon = ItemIcon;

                        slot.ItemName = myItemName;
                        slot.ItemFlavor = myItemFlavor;
                        slot.ItemIcon = myItemIcon;
                        ItemName = hitItemName;
                        ItemFlavor = hitItemFlavor;
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