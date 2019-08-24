using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string itemName = "NA";
    public string itemFlavor = "It's a thingy that does nothing";
    public Sprite itemIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
            InventoryToggle.collectable = new CollectableData(itemName, itemFlavor, itemIcon, gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
            InventoryToggle.collectable = null;
    }
}