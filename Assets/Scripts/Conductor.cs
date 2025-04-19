using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public bool pausedExternally = false;
    public float delay = 0;

    public GameObject tutorialManagerObject;
    private bool guardTutorialTriggered = false;
    private bool tutorialTriggered = false;
    private bool counterTutorialTriggered = false;
    private bool dashTutorialTriggered = false;
    public List<int> kickBeats;
    public List<int> beatsToMiss;

    public Transform starSpawnPoint;
    public float stunduration = 0f;
    public GameObject hitbox2;
    public GameObject StarFlyPrefab;

    public Animator anim;
    public GameObject hitbox1;
    public GameObject hitboxKick;
    public float songBpm;
    public AudioSource musicSource;

    public float secPerBeat;
    public float dspSongTime;
    public float songPosition;
    public float songPositionInBeats;
    public float lastBeat = -1;

    public bool isAttacking = false;
    public bool inHitReaction = false;
    public bool isstunned = false;
    public bool isKicking;

    public int beatRoundedUp;

    public GameObject earlyHitbox;
    public GameObject perfectHitbox;
    public GameObject lateHitbox;
    public GameObject missHitbox;
    public GameObject parryHitbox;

    private Vector3 enemyOriginalPosition;


    void Start()
    {
        enemyOriginalPosition = tutorialManagerObject.transform.position;
        AddBeatRangeToMiss(0, 2);
        AddBeatRangeToMiss(6, 8);
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    void Update()
    {
        if (pausedExternally) return;

        delay += Time.deltaTime;
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (inHitReaction && !isAttacking && state.IsName("idle") && stunduration > 2.5f && GetComponent<test>().enemyhealth > 0f)
        {
            Debug.LogWarning("SafetyNet triggered: reset full enemy state");
            inHitReaction = false;
            isAttacking = false;
            stunduration = 0f;
            anim.Play("idle", 0, 0f);
        }

        

        stunduration += Time.deltaTime;

        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        songPositionInBeats = songPosition / secPerBeat;

        

        isAttacking = false;

        
        int beatRounded = Mathf.FloorToInt(songPositionInBeats);
        if (beatRounded == lastBeat) return;

        if (beatRounded == 5 && !tutorialTriggered)
        {
            tutorialTriggered = true;
            tutorialManagerObject.GetComponent<TutorialManager>().TriggerPunchTutorial();
            return; // Prevent animation firing this frame

        }

        if (beatRounded == 11 && !guardTutorialTriggered)
        {
            guardTutorialTriggered = true;
            tutorialManagerObject.GetComponent<TutorialManager>().TriggerGuardTutorial();
            return;
        }

        if (!counterTutorialTriggered && songPositionInBeats >= 11.02f)
        {
            counterTutorialTriggered = true;
            tutorialManagerObject.GetComponent<TutorialManager>().TriggerCounterTutorial();
            return;
        }

        if (beatRounded == 17 && !dashTutorialTriggered)
        {
            dashTutorialTriggered = true;
            tutorialManagerObject.GetComponent<TutorialManager>().TriggerDashTutorial();
            return;
        }
        lastBeat = beatRounded;

        beatRoundedUp = Mathf.RoundToInt(lastBeat + 1f);

        // DEBUG INFO
        Debug.Log($"[Conductor] Beat: {beatRounded} | isAttacking: {isAttacking} | inHitReaction: {inHitReaction}");

        if (beatsToMiss.Contains(beatRounded)) return;

        isAttacking = false;


        float nextBeatTime = (beatRounded + 1) * secPerBeat;
        float timeUntilNextBeat = nextBeatTime - songPosition;

        // Want to trigger the animation 0.1s before the next beat
        float playDelay = Mathf.Max(0f, timeUntilNextBeat - 0.2f);

        

        // Schedule the animation
        StartCoroutine(PlayEnemyAnimationEarly(playDelay, beatRounded));
    }

    IEnumerator ActivateHitbox()
    {
        yield return new WaitForSeconds(0.1f); // delay before hitbox appears
        hitbox1.SetActive(true);
        yield return new WaitForSeconds(0.3f); // active time
        hitbox1.SetActive(false);
        yield return new WaitForSeconds(0.2f); // cooldown
        isAttacking = false;
    }

    IEnumerator ActivateHitboxKick()
    {
        if (inHitReaction) yield break;

        yield return new WaitForSeconds(0.3f); // delay before hitbox appears
        hitboxKick.SetActive(true);
        yield return new WaitForSeconds(0.2f); // active time
        hitboxKick.SetActive(false);
        yield return new WaitForSeconds(0.2f); // cooldown
        isAttacking = false;
    }

    IEnumerator ResetAttackFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    void AddBeatRangeToMiss(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            if (!beatsToMiss.Contains(i))
                beatsToMiss.Add(i);
        }
    }

    public void ResumeAndRecalculateDSPTime()
    {
        dspSongTime = (float)AudioSettings.dspTime - songPosition;
    }

    public void StartHitReaction()
    {
        StartCoroutine(HitReactionRoutine());
    }



    IEnumerator HitReactionRoutine()
    {
        hitbox1.SetActive(false);
        hitboxKick.SetActive(false);
        inHitReaction = true;
        stunduration = 0f;

        // Force play animation
        anim.Play("idle", 0, 0f);
        yield return null;
        anim.Play("Empty", 0, 0f);

        // Spawn stars
        for (int i = 0; i < 6; i++)
        {
            Vector3 spawnPos = starSpawnPoint.position;
            GameObject star = Instantiate(StarFlyPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = star.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float angle = Random.Range(20f, 60f) * Mathf.Deg2Rad;
                float speed = Random.Range(15f, 20f);
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                rb.velocity = dir * speed;
                rb.angularVelocity = Random.Range(-360f, 360f);
            }

            Destroy(star, 1.5f);
        }
        if (hitbox1.activeSelf == true)
        {
            hitbox1.SetActive(false);
            hitboxKick.SetActive(false);
        }

        yield return new WaitForSeconds(0.2f);

        inHitReaction = false;

        
    }

    IEnumerator PlayEnemyAnimationEarly(float delay, int beat)
    {
        yield return new WaitForSeconds(delay);

        if (inHitReaction) yield break;

        if (beat % 2 != 0)
        {
            anim.Play("Startup1", 0, 0f);
        }
        else if(!isstunned)
        {
            isAttacking = true;

            if (kickBeats.Contains(beat))
            {
                isKicking = true;
                anim.Play("kick", 0, 0f);
                StartCoroutine(ActivateHitboxKick());
                StartCoroutine(KickStepBack());
            }
            else
            {
                isKicking = false;
                anim.Play("hit1", 0, 0f);
                StartCoroutine(ActivateJudgmentHitboxes());
            }
        }
    }

    IEnumerator ActivateJudgmentHitboxes()
    {
        // Delay before early
        yield return new WaitForSeconds(0.15f);

        if (inHitReaction || isstunned) yield break;
        earlyHitbox.SetActive(true);
        yield return new WaitForSeconds(0.08f);
        earlyHitbox.SetActive(false);
        yield return new WaitForSeconds(0.01f);

        if (inHitReaction || isstunned) yield break;
        perfectHitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        perfectHitbox.SetActive(false);
        yield return new WaitForSeconds(0.01f);

        if (inHitReaction || isstunned) yield break;
        lateHitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        lateHitbox.SetActive(false);
        yield return new WaitForSeconds(0.01f);

        if (inHitReaction || isstunned) yield break;
        FindObjectOfType<test>().canPunch = false;
        missHitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        missHitbox.SetActive(false);
        FindObjectOfType<test>().canPunch = true;
    }

    IEnumerator KickStepBack()
    {
        float dashDistance = 2f;        // Smaller movement for polish
        float dashDuration = 0.35f;        // Duration of step back
        float returnDuration = 0.2f;      // Slower return

        Vector3 backPosition = enemyOriginalPosition + new Vector3(dashDistance, 0f, 0f); // RIGHT movement

        float t = 0f;
        Vector3 start = tutorialManagerObject.transform.position;

        // Step back
        while (t < 1f)
        {
            t += Time.deltaTime / dashDuration;
            tutorialManagerObject.transform.position = Vector3.Lerp(start, backPosition, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // small pause

        // Return
        t = 0f;
        start = tutorialManagerObject.transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime / returnDuration;
            tutorialManagerObject.transform.position = Vector3.Lerp(start, enemyOriginalPosition, t);
            yield return null;
        }

        // Snap to original just in case
        tutorialManagerObject.transform.position = enemyOriginalPosition;
    }


    public float GetSongBeatPosition()
    {
        float songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        return songPosition / secPerBeat;
    }
}
