using UnityEngine;


public class HideInfo : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject ShowText;

    void Start()
    {
        ShowText.SetActive(true);
        infoPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            infoPanel.SetActive(!infoPanel.activeSelf);
            ShowText.SetActive(!ShowText.activeSelf);
        }
    }
}
