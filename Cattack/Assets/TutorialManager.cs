using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private Button continueButton;

    [Header("Tutorial Settings")]
    [SerializeField] private float autoDisplayDelay = 0.5f;
    [SerializeField] private float typingSpeed = 0.05f; // Yazý yazma hýzý

    private Queue<string> tutorialMessages;
    private bool isTutorialActive = false;
    private bool isTyping = false;
    private string currentMessage = "";
    private bool additionalMessageDisplayed = false;

    private void Awake()
    {
        InitializeTutorialMessages();
        SetupContinueButton();
        tutorialPanel.SetActive(false);
    }

    private void SetupContinueButton()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners(); // Önceki listenerleri temizle
            continueButton.onClick.AddListener(() => {
                if (isTyping)
                {
                    // Eðer yazý yazýlýyorsa, hemen tamamla
                    CompleteCurrentMessage();
                }
                else
                {
                    // Deðilse bir sonraki mesaja geç
                    DisplayNextMessage();
                }
            });

            // Button için animator ekleyelim
            Animator buttonAnimator = continueButton.gameObject.GetComponent<Animator>();
            if (buttonAnimator == null)
            {
                buttonAnimator = continueButton.gameObject.AddComponent<Animator>();
            }
        }
    }

    private void Start()
    {
        Invoke(nameof(StartTutorial), autoDisplayDelay);
    }

    public void StartTutorial()
    {
        if (tutorialMessages.Count > 0)
        {
            PauseGame();
            DisplayNextMessage();
        }
    }

    private void CompleteCurrentMessage()
    {
        StopAllCoroutines();
        isTyping = false;
        tutorialText.text = currentMessage;
        continueButton.interactable = true;
    }

    private System.Collections.IEnumerator TypeText(string message)
    {
        isTyping = true;
        continueButton.interactable = false;
        tutorialText.text = "";
        currentMessage = message;

        foreach (char letter in message.ToCharArray())
        {
            tutorialText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
        continueButton.interactable = true;

        // Button'a dikkat çekmek için animasyon
        PulseButton();
    }

    private void PulseButton()
    {
        // Animator bileþenini al
        Animator buttonAnimator = continueButton.gameObject.GetComponent<Animator>();
        if (buttonAnimator != null)
        {
            // Pulse animasyonunu tetikle
            buttonAnimator.SetTrigger("Pulse");
        }
    }

    public void DisplayNextMessage()
    {
        if (tutorialMessages.Count > 0)
        {
            string message = tutorialMessages.Dequeue();
            tutorialPanel.SetActive(true);
            isTutorialActive = true;
            StartCoroutine(TypeText(message));
        }
        else
        {
            CompleteTutorial();
        }
    }

    private void CompleteTutorial()
    {
        if (!additionalMessageDisplayed)
        {
            additionalMessageDisplayed = true;
            tutorialMessages.Enqueue("You can move with A and D keys,\n\njump with Space key,\n\nand Dash with Left Shift key!\n\nPress Tab to open the inventory.\n\nHave fun!");
            DisplayNextMessage();
        }
        else
        {
            tutorialPanel.SetActive(false);
            isTutorialActive = false;
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public bool IsTutorialActive()
    {
        return isTutorialActive;
    }

    private void InitializeTutorialMessages()
    {
        tutorialMessages = new Queue<string>();
        tutorialMessages.Enqueue("Welcome to Cattack!");
        tutorialMessages.Enqueue("Defend your home from skeleton armies!");
        tutorialMessages.Enqueue("You gain a new skill every day.");
        tutorialMessages.Enqueue("Visit the witch before nightfall and ask for a skill [E]");
        tutorialMessages.Enqueue("Your skills will be automatically assigned to Q, W, and E in order.");
        tutorialMessages.Enqueue("On days 4 and 5, you won’t get new skills, but you can master the ones you have to defeat stronger skeletons!");
        tutorialMessages.Enqueue("Remember, on day 5, you’ll never know who’s friend or foe...");
    }
}
