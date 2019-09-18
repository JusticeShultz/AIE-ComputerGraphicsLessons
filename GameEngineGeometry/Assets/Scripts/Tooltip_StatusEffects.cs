using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip_StatusEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool Hovered = false;
    public GameObject Tooltip;
    public Vector3 Offset;

    private Vector3 startPos = Vector3.zero;

    void Start()
    {
        startPos = Tooltip.transform.position;
    }

    void Update ()
    {
        Tooltip.SetActive(Hovered);
        Tooltip.transform.position = Vector3.Lerp(Tooltip.transform.position, new Vector3((startPos.x + Input.mousePosition.x) / 2, startPos.y, startPos.z) + Offset, 0.1f);
	}

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Hovered = true;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Hovered = false;
    }
}
