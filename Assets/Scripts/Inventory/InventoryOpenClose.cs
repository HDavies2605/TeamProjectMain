using UnityEngine;

public class InventoryOpenClose : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject inventoryCanvas;
    void Start()
    {
        inventoryCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }

    }
}
