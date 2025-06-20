using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Conductor : MonoBehaviour
{

    public float playerDamageMultiplier = 1f;
    public float enemyDamageMultiplier = 1f;
    public enum DifficultyLevel { Easy, Medium, Hard }
    public DifficultyLevel currentDifficulty = DifficultyLevel.Medium;
    private bool kickBeatsGenerated = false;

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
    public GameObject player;
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

    private bool hasDied = false;
    private bool hasRevived = false;

    public GameObject gameOverScreen; // Assign in Inspector
    public GameObject gameWinScreen; // Assign in Inspector
    private bool gameOverTriggered = false;

    private bool isGameOverScreenActive = false;
    private bool isGameWinScreenActive = false;

    public bool isFinalStage = false;

    void Start()
    {
        enemyOriginalPosition = tutorialManagerObject.transform.position;

        if (SceneManager.GetActiveScene().name == "CustomLvl")
        {
            AddBeatRangeToMiss(0, 30);
            AddBeatRangeToMiss(33, 36);
            AddBeatRangeToMiss(39, 40);
            AddBeatRangeToMiss(43, 44);
            AddBeatRangeToMiss(47, 48);
            AddBeatRangeToMiss(51, 52);
            AddBeatRangeToMiss(55, 56);
            AddBeatRangeToMiss(59, 60);
            AddBeatRangeToMiss(63, 64);
            AddBeatRangeToMiss(67, 68);
            AddBeatRangeToMiss(71, 72);
            AddBeatRangeToMiss(75, 76);
            AddBeatRangeToMiss(79, 80);
            AddBeatRangeToMiss(83, 84);
            AddBeatRangeToMiss(87, 88);
            AddBeatRangeToMiss(91, 92);
            AddBeatRangeToMiss(109, 109);
            AddBeatRangeToMiss(115, 115);


            
            AddBeatRangeToMiss(164, 164);
            AddBeatRangeToMiss(165, 165);
            AddBeatRangeToMiss(166, 166);
            AddBeatRangeToMiss(167, 167);

            

            AddBeatRangeToMiss(173, 174);
            AddBeatRangeToMiss(180, 181);
            AddBeatRangeToMiss(192, 193);
            AddBeatRangeToMiss(200, 200);

            kickBeats.Add(255);
            AddBeatRangeToMiss(256, 259);
            AddBeatRangeToMiss(259, 260);
            kickBeats.Add(261);
            AddBeatRangeToMiss(262, 263);

            AddBeatRangeToMiss(264, 265);
            AddBeatRangeToMiss(268, 269);
            AddBeatRangeToMiss(272, 273);
            AddBeatRangeToMiss(276, 277);
            AddBeatRangeToMiss(280, 281);
            AddBeatRangeToMiss(284, 284);
            AddBeatRangeToMiss(287, 288);
            AddBeatRangeToMiss(291, 292);
            AddBeatRangeToMiss(295, 296);
            AddBeatRangeToMiss(299, 300);
            AddBeatRangeToMiss(303, 304);
            AddBeatRangeToMiss(307, 308);
            AddBeatRangeToMiss(311, 312);
            AddBeatRangeToMiss(315, 316);
            AddBeatRangeToMiss(319, 319);
            AddBeatRangeToMiss(322, 322);
            AddBeatRangeToMiss(330, 330);
            AddBeatRangeToMiss(350, 352);

            AddBeatRangeToMiss(358, 360);
            kickBeats.Add(361);
            AddBeatRangeToMiss(362, 366);

            kickBeats.Add(367);
            AddBeatRangeToMiss(368, 369);

            AddBeatRangeToMiss(372, 372);
            AddBeatRangeToMiss(375, 376);
            AddBeatRangeToMiss(379, 380);
            AddBeatRangeToMiss(383, 384);
            AddBeatRangeToMiss(387, 388);
            AddBeatRangeToMiss(391, 392);
            AddBeatRangeToMiss(395, 396);
            AddBeatRangeToMiss(399, 400);
            AddBeatRangeToMiss(403, 404);
            AddBeatRangeToMiss(407, 409);
            AddBeatRangeToMiss(413, 413);
            AddBeatRangeToMiss(417, 419);
            AddBeatRangeToMiss(422, 423);
            AddBeatRangeToMiss(426, 426);
            AddBeatRangeToMiss(429, 431);
            AddBeatRangeToMiss(434, 434);
            AddBeatRangeToMiss(437, 439);
            AddBeatRangeToMiss(442, 446);
            AddBeatRangeToMiss(449, 449);
            AddBeatRangeToMiss(459, 459);
            AddBeatRangeToMiss(471, 471);
            AddBeatRangeToMiss(475, 475);

            kickBeats.Add(505);
            AddBeatRangeToMiss(506, 507);
            kickBeats.Add(508);
            AddBeatRangeToMiss(509, 510);
            kickBeats.Add(511);
            AddBeatRangeToMiss(512, 514);
            AddBeatRangeToMiss(515, 516);

            AddBeatRangeToMiss(517, 518);
            AddBeatRangeToMiss(521, 522);
            AddBeatRangeToMiss(525, 526);
            AddBeatRangeToMiss(529, 530);
            AddBeatRangeToMiss(533, 534);
            AddBeatRangeToMiss(537, 538);
            AddBeatRangeToMiss(541, 542);
            AddBeatRangeToMiss(571, 571);
            AddBeatRangeToMiss(575, 592);
            AddBeatRangeToMiss(592, 700);
        }

        AddBeatRangeToMiss(0, 2);
        AddBeatRangeToMiss(6, 8);
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                playerDamageMultiplier = 1.5f;
                enemyDamageMultiplier = 0.75f;
                break;
            case DifficultyLevel.Medium:
                playerDamageMultiplier = 1f;
                enemyDamageMultiplier = 1f;
                break;
            case DifficultyLevel.Hard:
                playerDamageMultiplier = 0.75f;
                enemyDamageMultiplier = 1.5f;
                break;
        }
        musicSource.Play();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "CustomLvl")
        {
            if (songPositionInBeats >= 575f)
            {
                StartCoroutine(TriggerGameWinAfterDelay());
                gameOverTriggered = true; 
            }
        }

        if (isGameOverScreenActive && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        if (isGameWinScreenActive && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            // Replace with the actual name or build index of your next scene
            if (isFinalStage)
            {
                SceneManager.LoadScene("MM");
            }
            else
            {
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            
        }

        if (pausedExternally) return;

        if (player.GetComponent<test>().enemyhealth <= 0 && !hasDied || Input.GetKeyDown(KeyCode.M))
        {
            hasDied = true;

            StartCoroutine(HandleEnemyDeath());
            return; // Prevent further animation this frame
        }

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

        if (pausedExternally) return;

        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        songPositionInBeats = songPosition / secPerBeat;

        //anim.Play("idledone");

        // Stop attacks when the song is nearly over (e.g., 1 second before the end)
        if (songPosition >= musicSource.clip.length - 1f)
        {
            Debug.Log("Song is ending - disabling further enemy attacks");
            anim.SetTrigger("Done");
            StartCoroutine(TriggerGameWinAfterDelay());
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idledone"))
            {
                anim.Play("idledone", 0, 0f);
            }

            return;
        }

        if(player.GetComponent<test>().playerhealth <= 0)
        {
            anim.SetTrigger("Done");
            StartCoroutine(TriggerGameOverAfterDelay());
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("idledone"))
            {
                anim.Play("idledone", 0, 0f);
            }
            return;
        }



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

        if (SceneManager.GetActiveScene().name == "CustomLvl")
        {

        }
        else
        {
            if (!kickBeatsGenerated && beatRounded >= 20)
            {
                GenerateKickBeats();
                kickBeatsGenerated = true;
            }
        }

        

        lastBeat = beatRounded;

        beatRoundedUp = Mathf.RoundToInt(lastBeat + 1f);
        GameObject.Find("Player").GetComponent<test>().StartCoroutine("HeartPulse");

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

        yield return new WaitForSeconds(0.35f); // delay before hitbox appears
        hitboxKick.SetActive(true);
        yield return new WaitForSeconds(0.15f); // active time
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
        if (pausedExternally) yield break;

        if (SceneManager.GetActiveScene().name == "CustomLvl")
        {
            if (!isstunned)
            {
                isAttacking = true;

                if (kickBeats.Contains(beat)) // Check for kick beats first
                {
                    isKicking = true;
                    anim.Play("kick", 0, 0f);
                    StartCoroutine(ActivateHitboxKick());
                    StartCoroutine(KickStepBack());
                }
                else // If not a kick beat, it's a regular attack or startup
                {
                    // This part needs to align with your desired CustomLvl rhythm for non-kicks.
                    // Assuming you want regular hits on odd beats and startups on even beats still:
                    if (beat % 2 == 0) // Even beat (for Startup animation)
                    {
                        anim.Play("Startup1", 0, 0f);
                        isAttacking = false; // Startup isn't a direct hit
                    }
                    else // Odd beat (for actual hit)
                    {
                        isKicking = false;
                        anim.Play("hit1", 0, 0f);
                        StartCoroutine(ActivateJudgmentHitboxes());
                    }
                }
            }
        }
        else
        {
            if (beat % 2 != 0)
            {
                anim.Play("Startup1", 0, 0f);
            }
            else if (!isstunned)
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

        
    }

    IEnumerator ActivateJudgmentHitboxes()
    {
        // Delay before early
        yield return new WaitForSeconds(0.1f);

        if (inHitReaction || isstunned) yield break;
        earlyHitbox.SetActive(true);
        yield return new WaitForSeconds(0.08f);
        earlyHitbox.SetActive(false);


        if (inHitReaction || isstunned) yield break;
        perfectHitbox.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        perfectHitbox.SetActive(false);


        if (inHitReaction || isstunned) yield break;
        lateHitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        lateHitbox.SetActive(false);


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

    IEnumerator DeathStepBack()
    {
        float dashDistance = 5f;        // Distance to knockback
        float dashDuration = 0.5f;      // Faster knockback movement
        float pauseDuration = 1.75f;       // Stay back for a moment
        float returnDuration = 1f;      // Smooth return

        Vector3 backPosition = enemyOriginalPosition + new Vector3(dashDistance, 0f, 0f); // Knockback to the right

        float t = 0f;
        Vector3 start = tutorialManagerObject.transform.position;

        // Step back
        while (t < 1f)
        {
            t += Time.deltaTime / dashDuration;
            tutorialManagerObject.transform.position = Vector3.Lerp(start, backPosition, t);
            yield return null;
        }

        // Stay at back position for a bit
        yield return new WaitForSeconds(pauseDuration);

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

    IEnumerator HandleEnemyDeath()
    {
        pausedExternally = true;
        //StartCoroutine(DeathStepBack());
        anim.SetTrigger("Die");
        NPCReactionEvents.OnPlayerHit?.Invoke();

        yield return new WaitForSeconds(2f); // Let death play out

        if (songPosition < musicSource.clip.length - 1f && !hasRevived)
        {
            anim.Play("getup", 0, 0f);
            yield return new WaitForSeconds(1.2f); // Adjust to match getup anim length

            player.GetComponent<test>().enemyhealth = 100f;
            player.GetComponent<test>().playerhealth = 100f;
            hasRevived = true;
            pausedExternally = false;
            hasDied = false;
        }
        else
        {
            // End the game here if music is done
            Debug.Log("Enemy defeated for good!");
            StartCoroutine(TriggerGameWinAfterDelay());
        }

        
    }


    public float GetSongBeatPosition()
    {
        float songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        return songPosition / secPerBeat;
    }

    void GenerateKickBeats()
    {
        int totalBeats = Mathf.FloorToInt((float)musicSource.clip.samples / musicSource.clip.frequency / secPerBeat);
        int startBeat = 22;

        int step = 2; // Ensure we only check even beats
        int maxKicks = 0;

        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                maxKicks = 5;
                break;
            case DifficultyLevel.Medium:
                maxKicks = 10;
                break;
            case DifficultyLevel.Hard:
                maxKicks = 18;
                break;
        }

        List<int> potentialKicks = new List<int>();
        for (int i = startBeat; i < totalBeats; i += step)
        {
            potentialKicks.Add(i);
        }

        // Shuffle and select random even beats
        for (int i = 0; i < potentialKicks.Count; i++)
        {
            int rnd = Random.Range(i, potentialKicks.Count);
            int temp = potentialKicks[i];
            potentialKicks[i] = potentialKicks[rnd];
            potentialKicks[rnd] = temp;
        }

        for (int i = 0; i < Mathf.Min(maxKicks, potentialKicks.Count); i++)
        {
            if (!kickBeats.Contains(potentialKicks[i]))
            {
                kickBeats.Add(potentialKicks[i]);
            }
        }

        Debug.Log($"Kick beats generated for difficulty {currentDifficulty}: {string.Join(", ", kickBeats)}");
    }

    IEnumerator TriggerGameOverAfterDelay()
    {
        if (gameOverTriggered) yield break;
        gameOverTriggered = true;

        yield return new WaitForSeconds(2f);

        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        isGameOverScreenActive = true;
    }

    IEnumerator TriggerGameWinAfterDelay()
    {
        if (gameOverTriggered) yield break;
        gameOverTriggered = true;

        yield return new WaitForSeconds(2f);

        gameWinScreen.SetActive(true);
        Time.timeScale = 0f;
        isGameWinScreenActive = true;
    }
}
