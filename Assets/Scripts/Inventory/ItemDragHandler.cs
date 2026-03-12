using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Transform originalParent;  //original slot
    CanvasGroup canvasGroup;

    private InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryController = InventoryController.Instance;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Only allow dragging with the left mouse button
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

            originalParent = transform.parent;  //save original parent
        transform.SetParent(transform.root);  //above other canvas'
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f; //becomes semi-transparent when dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Only allow dragging with the left mouse button
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        transform.position = eventData.position; //follow the mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Only allow drag-end logic if left button was used
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        canvasGroup.blocksRaycasts = true; //re-enable raycast
        canvasGroup.alpha = 1f;  //no longere transparent

        InventorySlot dropSlot = eventData.pointerEnter?.GetComponentInParent<InventorySlot>();
        InventorySlot originalSlot = originalParent.GetComponent<InventorySlot>();


        if(dropSlot != null)
        {
            //is slot under the drop point? if so, move item to that slot
            if (dropSlot.currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetItem = dropSlot.currentItem.GetComponent<Item>();

                if(draggedItem.ID == targetItem.ID)
                {
                    //same item, stack them
                    targetItem.AddToStack(draggedItem.quantity);
                    originalSlot.currentItem = null;
                    Destroy(gameObject); //destroy the dragged item since it's now part of the stack
                }
                else
                {
                    //slot has an item? swap the items
                    dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;  //snap to middle

                    transform.SetParent(dropSlot.transform);
                    dropSlot.currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }
            }
            else
            {
                originalSlot.currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }   
        }
        else
        {
            //no slot under drop point
            transform.SetParent(originalParent); //sends back to original slot
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            SplitStack();
        }
    }
    private void SplitStack()
    {
        Item item = GetComponent<Item>();
        if (item == null || item.quantity <= 1)
        {
            return; //can't split if no item or only 1 in stack
        }
        int splitAmount = item.quantity / 2;

        if(splitAmount <= 0)
        {
            return; //can't split if split amount is 0
        }

        item.RemoveFromStack(splitAmount); //remove from original stack
        GameObject newItem = item.CloneItem(splitAmount); //create new item with split amount

        if (inventoryController == null || newItem == null)
        {
            return;
        }

        foreach(Transform slotTransform in inventoryController.inventoryPage.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if(slot!= null && slot.currentItem == null)
            {
                //found empty slot, place new item here
                slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //snap to middle
                return;
            }
        }

        //no empty slot found, return split item to original stack
        item.AddToStack(splitAmount);
        Destroy(newItem); //destroy the split item since it couldn't be placed

    }
}
