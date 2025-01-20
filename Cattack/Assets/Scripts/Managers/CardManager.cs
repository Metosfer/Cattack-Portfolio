using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private List<CardData> availableCards;
    [SerializeField] private GameObject cardSelectionPanel;
    [SerializeField] private GameObject[] cardButtons;
    [SerializeField] private Image[] cardImages;
    [SerializeField] private TextMeshProUGUI[] cardDescriptions;

    [Header("Player References")]
    [SerializeField] private PlayerSkills playerSkills;

    private bool canSelectCard = false;
    private List<CardData> currentDayCards = new List<CardData>();
    private List<CardData> usedCards = new List<CardData>(); // Kullanılmış kartları takip etmek için
    public bool nightCheck = false;

    public static CardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cardSelectionPanel.SetActive(false);
        TimeManager.Instance.OnDayChanged += OnDayChanged;

        for (int i = 0; i < cardButtons.Length; i++)
        {
            int index = i;
            cardButtons[i].GetComponent<Button>().onClick.AddListener(() => SelectCard(index));
        }
    }

    private void Update()
    {
        // Gün kontrolü ekle: 3. günden sonra kart seçimine izin verilmez
        if (TimeManager.Instance.GetCurrentDay() > 3)
        {
            canSelectCard = false;
            return;
        }

        if (canSelectCard && Input.GetKeyDown(KeyCode.E))
        {
            ShowCardSelection();
        }
    }

    private void OnDayChanged(int newDay)
    {
        // 4. gün ve sonrasında kart seçimini tamamen devre dışı bırak
        if (newDay > 3)
        {
            canSelectCard = false;
            cardSelectionPanel.SetActive(false);
            return;
        }
    }

    public void ShowCardSelection()
    {
        cardSelectionPanel.SetActive(true);
        SetupDailyCards();
        Time.timeScale = 0f;
    }

    private void SetupDailyCards()
    {
        currentDayCards.Clear();

        // Kullanılmamış kartları içeren geçici liste oluştur
        List<CardData> availableUnusedCards = new List<CardData>();
        foreach (CardData card in availableCards)
        {
            if (!usedCards.Contains(card))
            {
                availableUnusedCards.Add(card);
            }
        }

        // Eğer yeterli kullanılmamış kart kalmadıysa, kullanılmış kartları sıfırla
        if (availableUnusedCards.Count < 3)
        {
            usedCards.Clear();
            availableUnusedCards = new List<CardData>(availableCards);
        }

        // Rastgele 3 kart seç
        for (int i = 0; i < 3; i++)
        {
            if (availableUnusedCards.Count > 0)
            {
                int randomIndex = Random.Range(0, availableUnusedCards.Count);
                CardData selectedCard = availableUnusedCards[randomIndex];

                currentDayCards.Add(selectedCard);
                availableUnusedCards.RemoveAt(randomIndex);

                // UI güncelleme - Sprite ve text eşleşmesi garanti edildi
                cardImages[i].sprite = selectedCard.cardImage;
                cardDescriptions[i].text = $"{selectedCard.cardName}\n{selectedCard.description}";
            }
        }
    }

    public void SelectCard(int index)
    {
        if (index < currentDayCards.Count)
        {
            CardData selectedCard = currentDayCards[index];
            usedCards.Add(selectedCard); // Seçilen kartı kullanılmış kartlara ekle
            AssignSkillToPlayer(selectedCard);
            cardSelectionPanel.SetActive(false);
            canSelectCard = false;
            Time.timeScale = 1f;
        }
    }

    private void AssignSkillToPlayer(CardData card)
    {
        switch (TimeManager.Instance.GetCurrentDay())
        {
            case 1:
                playerSkills.AssignQSkill(card);
                break;
            case 2:
                playerSkills.AssignWSkill(card);
                break;
            case 3:
                playerSkills.AssignESkill(card);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Gün kontrolü: 3. günden sonra tetiklenmemesi sağlanır
        if (other.CompareTag("Player") && !nightCheck && TimeManager.Instance.GetCurrentDay() <= 3)
        {
            canSelectCard = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canSelectCard = false;
            cardSelectionPanel.SetActive(false);
        }
    }
}
