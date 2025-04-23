using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEditor;

public class DialogueManager : MonoBehaviour
{
    private Vector3 fightGraphicOriginalScale;
    public GameObject portraitPlayer;
    public GameObject portraitEnemy;

    public GameObject speechBubblePlayer;
    public GameObject speechBubbleEnemy;

    public TextMeshProUGUI dialogueTextPlayer;
    public TextMeshProUGUI dialogueTextEnemy;

    [System.Serializable]
    public class DialogueLine
    {
        public string text;
        public bool isPlayerTalking;
    }

    public GameObject gameplayUIGroup;
    public List<DialogueLine> dialogueLines;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject fightGraphic;

    public CinemachineVirtualCamera vcam_Player;
    public CinemachineVirtualCamera vcam_Enemy;
    public CinemachineVirtualCamera main;
    public AudioSource music;
    public Animator enemyAnimator;
    public Conductor conductor;

    public GameObject postProcessVolume;

    public float typeSpeed = 0.03f;

    private bool isTyping = false;

    void Start()
    {
        postProcessVolume.SetActive(true);
        fightGraphicOriginalScale = fightGraphic.transform.localScale;
        Time.timeScale = 0f;

        conductor.musicSource.Stop();
        conductor.pausedExternally = true;
        conductor.enabled = false;
        enemyAnimator.speed = 0f;
        music?.Pause();

        gameplayUIGroup.SetActive(false);
        fightGraphic.SetActive(false);
        dialoguePanel.SetActive(true);

        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {

        foreach (DialogueLine line in dialogueLines)
        {
            // Switch portrait and camera
            if (line.isPlayerTalking)
            {
                //portraitPlayer.SetActive(true);
                //portraitEnemy.SetActive(false);
                speechBubblePlayer.SetActive(true);
                speechBubbleEnemy.SetActive(false);
                vcam_Player.Priority = 11;
                vcam_Enemy.Priority = 10;
            }
            else
            {
                //portraitPlayer.SetActive(false);
                //portraitEnemy.SetActive(true);
                speechBubblePlayer.SetActive(false);
                speechBubbleEnemy.SetActive(true);
                vcam_Player.Priority = 10;
                vcam_Enemy.Priority = 11;
            }

            // Type text
            yield return StartCoroutine(TypeSentence(line.text, line.isPlayerTalking));

            // Wait for space to continue
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // End Dialogue Show fight graphic
        dialoguePanel.SetActive(false);
        StartCoroutine(ShowFightGraphic());
    }

    IEnumerator TypeSentence(string sentence, bool isPlayer)
    {
        isTyping = true;

        if (isPlayer)
        {
            dialogueTextPlayer.text = "";
            dialogueTextEnemy.text = "";
        }
        else
        {
            dialogueTextPlayer.text = "";
            dialogueTextEnemy.text = "";
        }

        for (int i = 0; i < sentence.Length; i++)
        {
            if (isPlayer)
                dialogueTextPlayer.text += sentence[i];
            else
                dialogueTextEnemy.text += sentence[i];

            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        isTyping = false;
    }


    IEnumerator ShowFightGraphic()
    {
        postProcessVolume.SetActive(false);
        speechBubbleEnemy.SetActive(false);
        speechBubblePlayer.SetActive(false);
        main.Priority = 12;
        fightGraphic.SetActive(true);
        fightGraphic.transform.localScale = Vector3.zero;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 2f;
            fightGraphic.transform.localScale = Vector3.Lerp(Vector3.zero, fightGraphicOriginalScale, t);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.5f);
        fightGraphic.SetActive(false);

        StartCoroutine(FadeInUIAndStartGame());
    }


    IEnumerator FadeInUIAndStartGame()
    {

        CanvasGroup group = gameplayUIGroup.GetComponent<CanvasGroup>();
        if (group == null)
        {
            group = gameplayUIGroup.AddComponent<CanvasGroup>();
            group.alpha = 0f;
        }

        gameplayUIGroup.SetActive(true);
        float t = 0f;

        while (group.alpha < 1f)
        {
            t += Time.unscaledDeltaTime * 1.5f;
            group.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        // RESUME FULL GAME
        Time.timeScale = 1f;
        music?.Play();
        enemyAnimator.speed = 1f;

        conductor.ResumeAndRecalculateDSPTime();
        conductor.pausedExternally = false;
        conductor.enabled = true;
    }
}
