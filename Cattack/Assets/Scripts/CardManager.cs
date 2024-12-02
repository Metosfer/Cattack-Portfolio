using System.Runtime.CompilerServices;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private PlayerMovement playerMovementSc;

    public bool nightCheck = false;
    public bool isCardsOpened = false;

    public GameObject panel;

    private GameObject spawned1;
    private GameObject spawned2;
    private GameObject spawned3;

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
        // Singleton setup
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
            Debug.LogError("PlayerMovement bileþeni bu GameObject üzerinde bulunamadý.");
        }
    }

    public void Start()
    {
        //TimeManager da ki gece mi kontrolünü al
        nightCheck = TimeManager.Instance.isNight;

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
            if (isCardsOpened == false && nightCheck == false ) 
            {
                SpawnCards();

                isCardsOpened = true;
            }

            
        }
    }

    private void SpawnCards()
    {
        attackCardIndex = Random.Range(0, attackCards.Length);
        defenseCardIndex = Random.Range(0, defenseCards.Length);
        passiveCardIndex = Random.Range(0, passiveCards.Length);

        spawned1 =Instantiate(attackCards[attackCardIndex], attackTransform.transform.position, attackTransform.transform.rotation);
        spawned2 = Instantiate(defenseCards[defenseCardIndex], defenseTransform.transform.position, defenseTransform.transform.rotation);
        spawned3 = Instantiate(passiveCards[passiveCardIndex], passiveTransform.transform.position, passiveTransform.transform.rotation);

        spawned1.transform.SetParent(panel.transform, true);
        spawned2.transform.SetParent(panel.transform, true);
        spawned3.transform.SetParent(panel.transform, true);


    }
}
