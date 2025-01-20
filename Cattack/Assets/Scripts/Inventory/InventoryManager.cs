using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    private bool isOpen = false;

    public TextMeshProUGUI mushroomAgaricText;
    public TextMeshProUGUI mushroomShaggyText;
    public TextMeshProUGUI seashellText;
    public TextMeshProUGUI seaweedText;
    public TextMeshProUGUI wolfsbaneText;

    private int mushroomAgaricCount = 0;
    private int mushroomShaggyCount = 0;
    private int seashellCount = 0;
    private int seaweedCount = 0;
    private int wolfsbaneCount = 0;

    AudioSource audioSourceInventory;
    private void Start()
    {
        audioSourceInventory = GetComponent<AudioSource>();
        inventoryPanel.SetActive(false);
        ResetUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
        }
    }

    // Bu metot, item toplandýðýnda çaðrýlýr
    public void AddItem(string itemName)
    {
        audioSourceInventory.Play();
        switch (itemName)
        {
            case "MushroomAgaric":
                mushroomAgaricCount++;
                UpdateUI(mushroomAgaricText, mushroomAgaricCount, "MushroomAgaric");
                break;

            case "MushroomShaggy":
                mushroomShaggyCount++;
                UpdateUI(mushroomShaggyText, mushroomShaggyCount, "MushroomShaggy");
                break;

            case "Seashell":
                seashellCount++;
                UpdateUI(seashellText, seashellCount, "Seashell");
                break;

            case "Seaweed":
                seaweedCount++;
                UpdateUI(seaweedText, seaweedCount, "Seaweed");
                break;

            case "Wolfsbane":
                wolfsbaneCount++;
                UpdateUI(wolfsbaneText, wolfsbaneCount, "Wolfsbane");
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

    private void ResetUI()
    {
        UpdateUI(mushroomAgaricText, mushroomAgaricCount, "Mushroom Agaric");
        UpdateUI(mushroomShaggyText, mushroomShaggyCount, "Mushroom Shaggy");
        UpdateUI(seashellText, seashellCount, "Seashell");
        UpdateUI(seaweedText, seaweedCount, "Seaweed");
        UpdateUI(wolfsbaneText, wolfsbaneCount, "Wolfsbane");
    }
}

