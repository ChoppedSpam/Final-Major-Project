using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("Tutorial UI")]
    public GameObject tutorialPopup;
    public CanvasGroup tutorialGroup;
    public TextMeshProUGUI tutorialText;

    [Header("References")]
    public AudioSource music;
    public Animator enemyAnimator;

    public bool inTutorial = false;
    private bool waitingForPunch = false;
    private bool waitingForGuard = false;

    void Awake()
    {
        Instance = this;
    }

    public void TriggerPunchTutorial()
    {
        if (inTutorial) return;

        inTutorial = true;
        waitingForPunch = true;

        tutorialText.text = "Press <color=#FFD700>E</color> to Punch!";
        StartCoroutine(FadeIn());

        Time.timeScale = 0f;
        music?.Pause();
        if (enemyAnimator != null) enemyAnimator.speed = 0;
    }

    public void TriggerGuardTutorial()
    {
        if (inTutorial) return;

        inTutorial = true;
        waitingForGuard = true;

        tutorialText.text = "Press <color=#00BFFF>W</color> to Guard!";
        StartCoroutine(FadeIn());

        Time.timeScale = 0f;
        music?.Pause();
        if (enemyAnimator != null) enemyAnimator.speed = 0;
    }

    void Update()
    {
        if (waitingForPunch && Input.GetKeyDown(KeyCode.E))
        {
            ResumeGame();
        }
        if (waitingForGuard && Input.GetKeyDown(KeyCode.W))
        {
            ResumeGame();
        }
    }

    void ResumeGame()
    {
        StartCoroutine(FadeOut());

        Time.timeScale = 1f;
        music?.Play();
        if (enemyAnimator != null) enemyAnimator.speed = 1;

        inTutorial = false;
        waitingForPunch = false;
        waitingForGuard = false;
    }

    IEnumerator FadeIn()
    {
        tutorialPopup.SetActive(true);
        tutorialGroup.alpha = 0f;

        while (tutorialGroup.alpha < 1f)
        {
            tutorialGroup.alpha += Time.unscaledDeltaTime * 2f;
            yield return null;
        }

        tutorialGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        while (tutorialGroup.alpha > 0f)
        {
            tutorialGroup.alpha -= Time.unscaledDeltaTime * 2f;
            yield return null;
        }

        tutorialGroup.alpha = 0f;
        tutorialPopup.SetActive(false);
    }
}