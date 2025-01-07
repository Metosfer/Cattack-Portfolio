using JetBrains.Annotations;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private PlayerMovement playerMovementSc;
    private PlayerCombat playerCombat;
    int currentDay;
    public bool nightCheck = false;
    public bool isCardsOpened = false;

    public GameObject panel;

    public GameObject spawned1;
    public GameObject spawned2;
    public GameObject spawned3;

    public List<GameObject> Cards;


    public int currentIndex1;
    public int currentIndex2;
    public int currentIndex3;


    public GameObject attackTransform;
    public GameObject defenseTransform;
    public GameObject passiveTransform;



    public static CardManager Instance { get; set; }


    public void Awake()
    {
        // Singleton ayarý
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerCombat = GetComponent<PlayerCombat>();
        playerMovementSc = FindAnyObjectByType<PlayerMovement>();

        if (playerMovementSc == null)
        {
            Debug.LogError("PlayerMovement bileþeni bu GameObject üzerinde bulunamadý.");
        }
    }

    public void Start()
    {
        TimeManager.Instance.OnDayChanged += HandeDayChanged;
        //TimeManager'daki gece mi kontrolünü al
        nightCheck = TimeManager.Instance.isNight;
    }

    //Gün deðiþtiðinde yap
    private void HandeDayChanged(int day)
    {
        isCardsOpened = false;
        panel.SetActive(true);
    }

    public void Update()
    {
        if (nightCheck == true)
        {
            Destroy(spawned1);
            Destroy(spawned2);
            Destroy(spawned3);
        }

        if (HomeBorderSwitch.Instance.isPlayerInside && playerMovementSc != null && playerMovementSc.playerTouched && Input.GetKeyDown(KeyCode.E))
        {
            panel.SetActive(true);
            if (isCardsOpened == false && nightCheck == false)
            {
                SpawnCards();
                isCardsOpened = true;
            }
        }
    }
    //private void ClearCardIndex(int index)
    //{
    //    Cards.RemoveAt(index);

    //}
    public void SpawnCards()
    {
        // Orijinal kartların kopyasını oluştur
        List<GameObject> tempCards = new List<GameObject>(Cards);

        // İlk kart için random seçim
        currentIndex1 = Random.Range(0, tempCards.Count);
        GameObject card1 = tempCards[currentIndex1];
        tempCards.RemoveAt(currentIndex1);

        // İkinci kart için random seçim
        currentIndex2 = Random.Range(0, tempCards.Count);
        GameObject card2 = tempCards[currentIndex2];
        tempCards.RemoveAt(currentIndex2);

        // Üçüncü kart için random seçim
        currentIndex3 = Random.Range(0, tempCards.Count);
        GameObject card3 = tempCards[currentIndex3];

        // Kartları spawn et
        if (spawned1 == null)
        {
            spawned1 = Instantiate(card1, attackTransform.transform.position, attackTransform.transform.rotation);
            spawned1.transform.SetParent(panel.transform, true);
            Debug.Log(spawned1);
        }

        if (spawned2 == null)
        {
            spawned2 = Instantiate(card2, defenseTransform.transform.position, defenseTransform.transform.rotation);
            spawned2.transform.SetParent(panel.transform, true);
            Debug.Log(spawned2);
        }

        if (spawned3 == null)
        {
            spawned3 = Instantiate(card3, passiveTransform.transform.position, passiveTransform.transform.rotation);
            spawned3.transform.SetParent(panel.transform, true);
            Debug.Log(spawned3);
        }
    }
}