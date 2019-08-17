using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public static Crafting reference;
    public static bool CanCraft = false;

    public InventorySlot topLeft;
    public InventorySlot topMiddle;
    public InventorySlot topRight;
    public InventorySlot middleLeft;
    public InventorySlot middle;
    public InventorySlot middleRight;
    public InventorySlot bottomLeft;
    public InventorySlot bottomMiddle;
    public InventorySlot bottomRight;
    public InventorySlot result;
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

	void Start ()
    {
        reference = this;
        CanCraft = false;
	}
	
	void Update ()
    {
        if (!InventorySlot.HoldingObject)
        foreach (CraftingRecipe recipe in recipes)
        {
            if (topLeft.ItemName == recipe.topLeft && topMiddle.ItemName == recipe.topMiddle && topRight.ItemName == recipe.topRight &&
                middleLeft.ItemName == recipe.middleLeft && middle.ItemName == recipe.middle && middleRight.ItemName == recipe.middleRight && 
                bottomLeft.ItemName == recipe.bottomLeft && bottomMiddle.ItemName == recipe.bottomMiddle && bottomRight.ItemName == recipe.bottomRight)
            {
                result.ItemName = recipe.resultName;
                result.ItemIcon = recipe.resultIcon;
                result.ItemFlavor = recipe.itemFlavor;
                CanCraft = true;

                break;
            }
            else
            {
                result.ItemName = "NA";
                result.ItemFlavor = "Oh shit fuck ;-;";
                result.ItemIcon = null;
            }
        }
	}

    public void DestroyCraftingMaterials()
    {
        CanCraft = false;

        topLeft.ItemName = "NA";
        topLeft.ItemIcon = null;

        topMiddle.ItemName = "NA";
        topMiddle.ItemIcon = null;

        topRight.ItemName = "NA";
        topRight.ItemIcon = null;

        middleLeft.ItemName = "NA";
        middleLeft.ItemIcon = null;

        middle.ItemName = "NA";
        middle.ItemIcon = null;

        middleRight.ItemName = "NA";
        middleRight.ItemIcon = null;

        bottomLeft.ItemName = "NA";
        bottomLeft.ItemIcon = null;

        bottomMiddle.ItemName = "NA";
        bottomMiddle.ItemIcon = null;

        bottomRight.ItemName = "NA";
        bottomRight.ItemIcon = null;
    }
}