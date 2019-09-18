using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public static bool HoldingObject = false; //Is there an inventory object held by the player?
    public static InventorySlot[] inventorySlots = new InventorySlot[30]; //Global array of our inventory slots.

    public Image sprite; //The image that the items sprite will display to.
    public GameObject canvas; //The canvas object that we are working with.
    public string ItemName = "NA"; //The name of the current socketed item(This is used to give the player a starting inventory). Usesd for the tooltip.
    public string ItemFlavor = "Little is known about this item..."; //The items flavor that is socketed in this slot. Usesd for the tooltip.
    public Sprite ItemIcon; //The icon of the current socketed item. This will be what "sprite" is set to render.
    public Material overMat; //This is the material applied to the image when being moved, this material has a shader that makes it overlay all other things being rendered.
    public GameObject BaseTooltip; //The tooltip this item slot will use(This was added to allow a modular way to make items have different tooltip borders).
    public GameObject Splat; //The splat this item slot will generate when the item on it is moved to an empty slot.
    public Vector3 tooltipOffset = Vector3.zero; //The offset the tooltip should be from this object.
    public int inventoryIndex = 0; //The index point in the inventorySlots this object belongs.

    public bool IsHovered = false; //Is this game object hovered?
    public bool IsDragging = false; //Is this game object being dragged?
    public bool SpecialHover = false; //Is this game object an exception to the rest?(This helps with a bug of not being able to be picked up once released)
    public GameObject tooltip; //The current instantiated tooltip. Will be null unless the object is hovered.

    private void Start()
    {
        inventorySlots[inventoryIndex] = this; //Set the designated array slot to be this object.
        HoldingObject = false; //Make sure that the static field is reset to its default state.
    }

    void Update ()
    {
        //If this object is hovered and doesn't contain nothing
        if(IsHovered && ItemName != "NA")
        {
            //If there is no tooltip.
            if (!tooltip)
            {
                //Instantiate a new tooltip object and set all of its properties.
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
                //Otherwise, update the tooltip objects position to have a slight offset from the mouse, this is done as a polish to make the tooltips feel a little more alive.

                Vector2 pos01;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, CameraRef.cam, out pos01);
                
                //This handles making sure that the tooltip isn't offscreen for any reason.
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
            //If we aren't hovering this object anymore and have a tooltip alive, destroy it.
            if (tooltip) Destroy(tooltip);
        }

        //If the left mouse button is down and we're not holding an object.
        if (!Input.GetMouseButton(0) && !HoldingObject)
            if (SpecialHover) IsHovered = true; //If we have the ok to special hover, set hovered to true.

        //If there is no sprite object provided don't keep going and print an error that something isn't right.
        if (!sprite)
        {
            Debug.LogError("Something ain't right, we seem to be missing the sprite object on " + gameObject.name);
            return;
        }

        //If the left mouse button is down.
        if(Input.GetMouseButton(0))
        {
            //If this game object is hovered and the left mouse button is down mark it as being dragged.
            if (IsHovered) IsDragging = true;

            //If hovered or dragging.
            if (IsHovered || IsDragging)
            {
                //And the item name isn't non-existent.
                if (ItemName != "NA")
                {
                    //If we can craft and we are grabbing from the "Result" slot.
                    if (Crafting.CanCraft && gameObject.name == "Result")
                    {
                        //Iterate through every slot.
                        for (int i = 0; i < 30; i++)
                        {
                            //Find an empty inventory slot.
                            if (inventorySlots[i].ItemName == "NA")
                            {
                                //Replace the empty slot with the crafted item and replace the crafted item slot with nothing.
                                InventorySlot slot = inventorySlots[i];

                                slot.ItemName = ItemName;
                                slot.ItemFlavor = ItemFlavor;
                                slot.ItemIcon = ItemIcon;
                                slot.sprite.sprite = ItemIcon;

                                ItemName = "NA";
                                ItemFlavor = "NA";
                                ItemIcon = null;

                                //Break out of the lookup.
                                break;
                            }
                        }

                        //If the item has beeen successfully crafted then destroy the crafting materials.
                        if(ItemName == "NA")
                            Crafting.reference.DestroyCraftingMaterials();
                    }
                    else
                    {
                        //Otherwise, if this isn't a crafting slot, move the render object with the mouse and make sure the overlay material is applied.
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
            //If the mouse button isn't down and we are still dragging.
            if (IsDragging)
            {
                //Iterate through all of the inventory slots.
                for (int i = 0; i < 30; i++)
                {
                    //Skip the slot named "Result".
                    if (inventorySlots[i].name == "Result") continue;

                    //Check if the item has been dropped into a valid spot.
                    if (GetWorldSpaceRect(inventorySlots[i].gameObject.GetComponent<Image>().rectTransform).Overlaps(GetWorldSpaceRect(sprite.rectTransform)))
                    {
                        //Swap the 2 item slots contents.
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

                        //Update the slot icons.
                        sprite.sprite = hitItemIcon;
                        slot.sprite.sprite = myItemIcon;

                        //If we've dragged our object into an empty slot, do a little puff of smoke effect.
                        if ((ItemName == "NA" && slot.ItemName != "NA") || (ItemName != "NA" && slot.ItemName == "NA"))
                        {
                            GameObject splat = Instantiate(Splat);
                            splat.transform.SetParent(slot.transform);
                            RectTransform transformOfTooltip = splat.GetComponent<RectTransform>();
                            transformOfTooltip.localScale = Vector3.one * 4;
                            transformOfTooltip.localPosition = Vector3.zero;
                            transformOfTooltip.localRotation = Quaternion.identity;

                            Destroy(splat, 2.0f);
                        }

                        //Break out of the loop.
                        break; 
                    }
                }

                //Indentify we have dropped our object.
                HoldingObject = false;
                IsDragging = false;
            }
        }

        //If we aren't dragging this object around.
        if(!IsDragging)
        {
            //Remove the material from the object.
            sprite.material = null;
            //Center the object back in the slot position.
            sprite.rectTransform.localPosition = new Vector3(0, 0, 0);
        }

        //If this slot is empty.
        if (ItemName == "NA")
        {
            //Set the color of the render object to be clear.
            sprite.color = Color.clear;
            //Turn off the render object.
            sprite.enabled = false;
        }
        else
        {
            //If the ItemIcon and Image object exist, update the icon to the current icon.
            if (ItemIcon && sprite)
                sprite.sprite = ItemIcon;

            //Make sure the object is enabled.
            if (sprite.sprite)
                sprite.enabled = true;

            //Set the objects color to white. (Non-transparent)
            sprite.color = Color.white;
        }

        if(inventoryIndex == 29)
        {
            //If this is the trash slot, remove the item.
            ItemName = "NA";
            ItemFlavor = "NA";
            ItemIcon = null;
        }
    }

    private void OnDrawGizmos()
    {
        //To make working with this in the editor easier, automatically update the sprite objects sprite with the set ItemIcon. This avoids unecessary work.
        if (ItemIcon && sprite)
            sprite.sprite = ItemIcon;
    }

    private void OnMouseEnter()
    {
        //Specify this object is hovered, but it may or may not have been triggered while holding an object.
        SpecialHover = true;

        //If we are holding an object return.
        if (HoldingObject) return;

        //Set that we are hovered.
        IsHovered = true;
        
        //Debug
            //print("Hovered");
    }

    private void OnMouseExit()
    {
        //Tag this object as not hovered.
        SpecialHover = false;
        IsHovered = false;

        //Debug
            //print("Unhovered");
    }

    //Do some simple calculations to get the world space of a RectTransform.
    Rect GetWorldSpaceRect(RectTransform input)
    {
        var result = input.rect;

        result.center = input.TransformPoint(result.center);
        result.size = input.TransformVector(result.size);

        return result;
    }
}