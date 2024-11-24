using System.Runtime.CompilerServices;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private PlayerMovement playerMovementSc;

    public GameObject panel;

    public GameObject[] attackCards;
    public GameObject[] defenseCards;
    public GameObject[] passiveCards;

    private int attackCardIndex;
    private int defenseCardIndex;
    private int passiveCardIndex;

    public GameObject attackTransform;
    public GameObject defenseTransform;
    public GameObject passiveTransform;

    public void Awake()
    {
        playerMovementSc = FindAnyObjectByType<PlayerMovement>();

        if (playerMovementSc == null)
        {
            Debug.LogError("PlayerMovement bileþeni bu GameObject üzerinde bulunamadý.");
        }
    }

    public void Start()
    {


    }

    public void Update()
    {
        attackCardIndex = Random.Range(0, attackCards.Length);
        defenseCardIndex = Random.Range(0, defenseCards.Length);
        passiveCardIndex = Random.Range(0, passiveCards.Length);

        if (HomeBorderSwitch.Instance.isPlayerInside && playerMovementSc != null && playerMovementSc.playerTouched && Input.GetKeyDown(KeyCode.E))
        {
            SpawnCards();
        }
    }

    private void SpawnCards()
    {
        

        GameObject spawned1 =Instantiate(attackCards[attackCardIndex], attackTransform.transform.position, attackTransform.transform.rotation);
        GameObject spawned2 = Instantiate(defenseCards[defenseCardIndex], defenseTransform.transform.position, defenseTransform.transform.rotation);
        GameObject spawned3 = Instantiate(passiveCards[passiveCardIndex], passiveTransform.transform.position, passiveTransform.transform.rotation);

        spawned1.transform.SetParent(panel.transform, true);
        spawned2.transform.SetParent(panel.transform, true);
        spawned3.transform.SetParent(panel.transform, true);


    }
}
