using System.Runtime.CompilerServices;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private PlayerMovement playerMovementSc;
    int currentDay;
    public bool nightCheck = false;
    public bool isCardsOpened = false;

    public GameObject panel;

    public GameObject spawned1;
    public GameObject spawned2;
    public GameObject spawned3;

    public GameObject[] attackCards;
    public GameObject[] defenseCards;
    public GameObject[] passiveCards;

    public int attackCardIndex;
    public int defenseCardIndex;
    public int passiveCardIndex;

    public GameObject attackTransform;
    public GameObject defenseTransform;
    public GameObject passiveTransform;

    public static CardManager Instance { get; set; }
    public void Awake()
    {
        // Singleton ayar�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerMovementSc = FindAnyObjectByType<PlayerMovement>();

        if (playerMovementSc == null)
        {
            Debug.LogError("PlayerMovement bile�eni bu GameObject �zerinde bulunamad�.");
        }
    }

    public void Start()
    {
        TimeManager.Instance.OnDayChanged += HandeDayChanged;
        //TimeManager'daki gece mi kontrol�n� al
        nightCheck = TimeManager.Instance.isNight;
    }

    //G�n de�i�ti�inde yap
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

    public void SpawnCards()
    {
        attackCardIndex = Random.Range(1, attackCards.Length);
        defenseCardIndex = Random.Range(1, defenseCards.Length);
        passiveCardIndex = Random.Range(1, passiveCards.Length);




        if (spawned1 == null)
        {
            spawned1 = Instantiate(attackCards[attackCardIndex], attackTransform.transform.position, attackTransform.transform.rotation);
            spawned1.transform.SetParent(panel.transform, true);
            Debug.Log("ATAK SPAWN OLUYOR");
        }
        

        if (spawned2 == null)
        {
            
            spawned2 = Instantiate(defenseCards[defenseCardIndex], defenseTransform.transform.position, defenseTransform.transform.rotation);
            spawned2.transform.SetParent(panel.transform, true);
            Debug.Log("DEFANS SPAWN OLUYOR");
        }

        if (spawned3 == null)
        {

            spawned3 = Instantiate(passiveCards[passiveCardIndex], passiveTransform.transform.position, passiveTransform.transform.rotation);
            spawned3.transform.SetParent(panel.transform, true);
            Debug.Log("Pasif SPAWN OLUYOR");
        }



    }
}
