using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private bool waitingForCounter = false;
    private bool waitingForDash = false;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
            return;

        Instance = this;
    }

    public void TriggerPunchTutorial()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
            return;
        if (inTutorial) return;

        inTutorial = true;
        waitingForPunch = true;
        FindObjectOfType<Conductor>().pausedExternally = true;
        tutorialText.text = "Press <color=#FFD700>E</color> to Punch!";
        StartCoroutine(FadeIn());

        Time.timeScale = 0f;
        music?.Pause();
        if (enemyAnimator != null) enemyAnimator.speed = 0;
    }

    public void TriggerGuardTutorial()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
            return;
        if (inTutorial) return;

        inTutorial = true;
        waitingForGuard = true;
        FindObjectOfType<Conductor>().pausedExternally = true;

        tutorialText.text = "Press <color=#00BFFF>W</color> to Guard!";
        StartCoroutine(FadeIn());

        Time.timeScale = 0f;
        music?.Pause();
        if (enemyAnimator != null) enemyAnimator.speed = 0;
    }

    public void TriggerCounterTutorial()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
            return;
        if (inTutorial) return;

        inTutorial = true;
        waitingForCounter = true;
        FindObjectOfType<Conductor>().pausedExternally = true;

        tutorialText.text = "Press <color=#FFD700>E</color> to counterattack!";
        StartCoroutine(FadeIn());

        Time.timeScale = 0f;
        music?.Pause();
        if (enemyAnimator != null) enemyAnimator.speed = 0;
    }

    public void TriggerDashTutorial()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
            return;
        if (inTutorial) return;

        inTutorial = true;
        waitingForDash = true;
        FindObjectOfType<Conductor>().pausedExternally = true;

        tutorialText.text = "Press <color=#FF69B4>A</color> to Dodge the Kick!";
        StartCoroutine(FadeIn());

        Time.timeScale = 0f;
        music?.Pause();
        if (enemyAnimator != null) enemyAnimator.speed = 0;
    }

    void Update()
    {
        if (waitingForPunch && Input.GetKeyDown(KeyCode.E))
        {
            ResumeGame1();
        }
        if (waitingForGuard && Input.GetKeyDown(KeyCode.W))
        {
            ResumeGame();
        }
        if (waitingForCounter && Input.GetKeyDown(KeyCode.E))
        {
            ResumeGameCompletely();
        }
        if (waitingForDash && Input.GetKeyDown(KeyCode.A))
        {
            ResumeGameCompletely(); // reuse the same fade/resume logic
        }
    }
    void ResumeGame1()
    {
        StartCoroutine(FadeOut());
        Conductor conductor = FindObjectOfType<Conductor>();
        conductor.ResumeAndRecalculateDSPTime(); //  Reset DSP timer to fix beat jump
        conductor.pausedExternally = false;
        Time.timeScale = 1f;
        music?.Play();
        if (enemyAnimator != null) enemyAnimator.speed = 1;

        inTutorial = false;
        waitingForPunch = false;
        waitingForGuard = false;
        waitingForCounter = false;
        waitingForDash = false;
    }

    void ResumeGame()
    {
        
        Conductor conductor = FindObjectOfType<Conductor>();
        conductor.ResumeAndRecalculateDSPTime(); //  Reset DSP timer to fix beat jump
        conductor.pausedExternally = false;
        Time.timeScale = 1f;
        music?.Play();
        if (enemyAnimator != null) enemyAnimator.speed = 1;

        inTutorial = false;
        waitingForPunch = false;
        waitingForGuard = false;
        waitingForCounter = false;
        waitingForDash = false;
    }

    void ResumeGameCompletely()
    {
        StartCoroutine(FadeOut());
        Conductor conductor = FindObjectOfType<Conductor>();
        conductor.ResumeAndRecalculateDSPTime(); //  Reset DSP timer to fix beat jump
        conductor.pausedExternally = false;
        Time.timeScale = 1f;
        music?.Play();
        if (enemyAnimator != null) enemyAnimator.speed = 1;

        inTutorial = false;
        waitingForPunch = false;
        waitingForGuard = false;
        waitingForCounter = false;
        waitingForDash = false;
    }

    IEnumerator FadeIn()
    {
        tutorialPopup.SetActive(true); 
        if (!tutorialGroup.gameObject.activeSelf)
            tutorialGroup.gameObject.SetActive(true);

        tutorialGroup.alpha = 0f;

        float duration = 0.4f;  // Duration of the fade
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            tutorialGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        tutorialGroup.alpha = 1f;  // Force max alpha
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