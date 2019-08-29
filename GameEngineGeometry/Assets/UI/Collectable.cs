using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple data about the collectable object
public class CollectableData
{
    public string itemName = "NA";
    public string itemFlavor = "It's a thingy that does nothing";
    public Sprite itemIcon;
    public GameObject _gameObject;

    public CollectableData(string _itemName, string _itemFlavor, Sprite _itemIcon, GameObject _object)
    {
        itemName = _itemName;
        itemFlavor = _itemFlavor;
        itemIcon = _itemIcon;
        _gameObject = _object;
    }
}

public class Collectable : MonoBehaviour
{
    public string itemName = "NA"; //The name of the collectable.
    public string itemFlavor = "It's a thingy that does nothing"; //The flavor of the collectable.
    public Sprite itemIcon; //The icon of the collectable.

    private void OnTriggerEnter(Collider other)
    {
        //If the collided with object is the player give it some data about this object.
        if (other.gameObject.name == "Player")
            InventoryToggle.collectable = new CollectableData(itemName, itemFlavor, itemIcon, gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        //If the player left the pickup region remove the collectables data.
        if (other.gameObject.name == "Player")
            InventoryToggle.collectable = null;
    }
}