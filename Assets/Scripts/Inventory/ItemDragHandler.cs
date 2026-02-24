using UnityEngine;
using UnityEngine.EventSystems;


public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;  //original slot
    CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;  //save original parent
        transform.SetParent(transform.root);  //above other canvas'
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f; //becomes semi-transparent when dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; //re-enable raycast
        canvasGroup.alpha = 1f;  //no longere transparent

        InventorySlot dropSlot = eventData.pointerEnter?.GetComponentInParent<InventorySlot>();

        InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();


        if(dropSlot != null)
        {
            if(dropSlot.currentItem != null)
            {
                //slot has an item? swap the items
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;  //snap to middle
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            //no slot under drop point
            transform.SetParent(originalParent); //sends back to original slot
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

}
