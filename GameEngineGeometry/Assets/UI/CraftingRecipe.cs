using UnityEngine;

[CreateAssetMenu(fileName = "Empty_Recipe", menuName = "Crafting Recipe", order = 1)]
public class CraftingRecipe : ScriptableObject
{
    public string resultName = "NA";
    public Sprite resultIcon;
    public string topLeft = "NA";
    public string topMiddle = "NA";
    public string topRight = "NA";
    public string middleLeft = "NA";
    public string middle = "NA";
    public string middleRight = "NA";
    public string bottomLeft = "NA";
    public string bottomMiddle = "NA";
    public string bottomRight = "NA";
    public string itemFlavor = "Little is known about this item...";
}