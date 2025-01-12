using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private List<CardData> availableCards;
    [SerializeField] private GameObject cardSelectionPanel;
    [SerializeField] private GameObject[] cardButtons; // 3 adet kart butonu
    [SerializeField] private Image[] cardImages;
    [SerializeField] private TextMeshProUGUI[] cardDescriptions;

    [Header("Player References")]
    [SerializeField] private PlayerSkills playerSkills;

    private bool canSelectCard = false;
    private List<CardData> currentDayCards = new List<CardData>();
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

        // Button click olaylarını ayarla
        for (int i = 0; i < cardButtons.Length; i++)
        {
            int index = i; // Closure için local değişken
            cardButtons[i].GetComponent<Button>().onClick.AddListener(() => SelectCard(index));
        }
    }

    private void Update()
    {
        if (canSelectCard && Input.GetKeyDown(KeyCode.E))
        {
            ShowCardSelection();
        }
    }

    private void OnDayChanged(int newDay)
    {
        if (newDay > 3)
        {
            // Oyun bitti işlemleri
            return;
        }
    }

    public void ShowCardSelection()
    {
        cardSelectionPanel.SetActive(true);
        SetupDailyCards();
    }

    private void SetupDailyCards()
    {
        currentDayCards.Clear();
        List<CardData> tempCards = new List<CardData>(availableCards);

        // Rastgele 3 kart seç
        for (int i = 0; i < 3; i++)
        {
            if (tempCards.Count > 0)
            {
                int randomIndex = Random.Range(0, tempCards.Count);
                currentDayCards.Add(tempCards[randomIndex]);
                tempCards.RemoveAt(randomIndex);

                // UI güncelleme
                cardImages[i].sprite = currentDayCards[i].cardImage;
                cardDescriptions[i].text = $"{currentDayCards[i].cardName}\n{currentDayCards[i].description}";
            }
        }
    }

    public void SelectCard(int index)
    {
        if (index < currentDayCards.Count)
        {
            CardData selectedCard = currentDayCards[index];
            AssignSkillToPlayer(selectedCard);
            cardSelectionPanel.SetActive(false);
            canSelectCard = false;
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
        if (other.CompareTag("Player") && !nightCheck)
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