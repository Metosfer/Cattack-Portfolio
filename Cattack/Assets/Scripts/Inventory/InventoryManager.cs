using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isOpen = false;


    public TextMeshProUGUI item1Text;
    public TextMeshProUGUI item2Text;
    public TextMeshProUGUI item3Text;

    private int item1Count = 0;
    private int item2Count = 0;
    private int item3Count = 0;


    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isOpen == false) 
        { 
            
            inventoryPanel.SetActive(true); 
            isOpen = true;
        
        
        }
       else if (Input.GetKeyDown(KeyCode.Tab) && isOpen == true)
        {
            inventoryPanel.SetActive(false);
            isOpen = false;
        }
    }
    // Bu metot, item toplandýðýnda çaðrýlýr
    public void AddItem(string itemName)
    {
        switch (itemName)
        {
            case "Item1":
                item1Count++;
                UpdateUI(item1Text, item1Count, "Item1");
                break;

            case "Item2":
                item2Count++;
                UpdateUI(item2Text, item2Count, "Item2");
                break;

            case "Item3":
                item3Count++;
                UpdateUI(item3Text, item3Count, "Item3");
                break;

            default:
                Debug.LogWarning("Bilinmeyen bir item adý: " + itemName);
                break;
        }
    }

    private void UpdateUI(TextMeshProUGUI textElement, int count, string itemName)
    {
        textElement.text = itemName + ": " + count;
    }
}
