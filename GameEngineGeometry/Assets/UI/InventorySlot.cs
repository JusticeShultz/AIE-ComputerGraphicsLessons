using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    static bool HoldingObject = false;

    public Image sprite;
    public GameObject canvas;
    public string ItemName = "NA";
    public Sprite ItemIcon;
    public Material overMat;
    private bool IsHovered = false;
    private bool IsDragging = false;

    private void Start()
    {
        HoldingObject = false;
    }

    void Update ()
    {
        if (!sprite) return;

        if(Input.GetMouseButton(0))
        {
            if (IsHovered) IsDragging = true;

            if (IsHovered || IsDragging)
            {
                HoldingObject = true;
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, CameraRef.cam, out pos);
                sprite.rectTransform.position = canvas.transform.TransformPoint(pos);
                sprite.material = overMat;
                //sprite.rectTransform.position = new Vector3(CameraRef.cam.ScreenToWorldPoint(Input.mousePosition).x, CameraRef.cam.ScreenToWorldPoint(Input.mousePosition).y, 5);
            }
        }
        else
        {
            if (IsDragging)
            {
                HoldingObject = false;
                IsDragging = false;
            }
        }

        if(!IsDragging)
        {
            //if check for slot
            //else
            sprite.material = null;
            sprite.rectTransform.localPosition = new Vector3(0, 0, 5);
        }
    }

    private void OnDrawGizmos()
    {
        if (ItemIcon && sprite)
            sprite.sprite = ItemIcon;
    }

    private void OnMouseEnter()
    {
        if (HoldingObject) return;
        IsHovered = true;
        print("Hovered");
    }

    private void OnMouseExit()
    {
        IsHovered = false;
        print("Unhovered");
    }
}