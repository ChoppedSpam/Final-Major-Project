using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    
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

    public float typeSpeed = 0.03f;

    private int currentLine = 0;
    private bool isTyping = false;

    void Start()
    {
        Time.timeScale = 0f;

        conductor.musicSource.Stop();
        enemyAnimator.speed = 0f;
        conductor.enabled = false;

        gameplayUIGroup.SetActive(false);
        fightGraphic.SetActive(false);
        dialoguePanel.SetActive(true);
        StartCoroutine(PlayDialogue());
    }

    IEnumerator PlayDialogue()
    {
        foreach (DialogueLine line in dialogueLines)
        {
            // Switch camera
            if (line.isPlayerTalking)
            {
                vcam_Player.Priority = 11;
                vcam_Enemy.Priority = 10;
            }
            else
            {
                vcam_Player.Priority = 10;
                vcam_Enemy.Priority = 11;
            }

            // Type text
            yield return StartCoroutine(TypeSentence(line.text));

            // Wait for input to continue
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // End Dialogue FIGHT Graphic
        dialoguePanel.SetActive(false);
        StartCoroutine(ShowFightGraphic());
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed); // works even with timescale = 0
        }
        isTyping = false;
    }

    IEnumerator ShowFightGraphic()
    {
        main.Priority = 12;
        fightGraphic.SetActive(true);

        // Optional zoom-in effect
        fightGraphic.transform.localScale = Vector3.zero;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 2f;
            fightGraphic.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(1.5f);
        fightGraphic.SetActive(false);
        music?.Play();
        conductor.enabled = true;
        enemyAnimator.speed = 1f;
        conductor.musicSource.Play();
        gameplayUIGroup.SetActive(true);

        // Start the game!
        Time.timeScale = 1f;
        FindObjectOfType<Conductor>().enabled = true;
    }
}
