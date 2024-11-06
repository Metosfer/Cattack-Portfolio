using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; // �rne�in, "Item1", "Item2", "Item3" gibi.
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryManager.AddItem(itemName);
            Destroy(gameObject); // Itemi ald�ktan sonra yok et
        }
    }
}
