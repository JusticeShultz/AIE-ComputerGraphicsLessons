using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public static Crafting reference; //Reference to the crafting system. [Singleton]
    public static bool CanCraft = false; //If we can craft or not.

    //Each slot of the crafting
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

    //All the recipes(scriptable objects) included in this crafting system go in this list.
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

	void Start ()
    {
        //Grab the reference and reset the static object back to its default state.
        reference = this;
        CanCraft = false;
	}
	
	void Update ()
    {
        //If we're not holding an object.
        if (!InventorySlot.HoldingObject)
        foreach (CraftingRecipe recipe in recipes)
        {
            //Check if we have a valid recipe set up in the inventory. (Ideally, a system should be put in place to allow slots in any combo down the line)
            if (topLeft.ItemName == recipe.topLeft && topMiddle.ItemName == recipe.topMiddle && topRight.ItemName == recipe.topRight &&
                middleLeft.ItemName == recipe.middleLeft && middle.ItemName == recipe.middle && middleRight.ItemName == recipe.middleRight && 
                bottomLeft.ItemName == recipe.bottomLeft && bottomMiddle.ItemName == recipe.bottomMiddle && bottomRight.ItemName == recipe.bottomRight)
            {
                //Grab the recipes data and put it into the result recipe slot.
                result.ItemName = recipe.resultName;
                result.ItemIcon = recipe.resultIcon;
                result.ItemFlavor = recipe.itemFlavor;
                CanCraft = true;
                //Break out of this foreach loop to not look for every other recipe.
                break;
            }
            else
            {
                //If nothing valid is craftable set the slot to null.
                result.ItemName = "NA";
                result.ItemFlavor = "nope :)";
                result.ItemIcon = null;
            }
        }
	}

    public void DestroyCraftingMaterials()
    {
        //Set each slot to be empty and make is so we are no longer crafting.

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