using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
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

    public int beatRoundedUp;

    public GameObject earlyHitbox;
    public GameObject perfectHitbox;
    public GameObject lateHitbox;
    public GameObject missHitbox;

    void Start()
    {
        AddBeatRangeToMiss(1, 2);
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    void Update()
    {
        Time.timeScale = 1f;
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if (inHitReaction && !isAttacking && state.IsName("idle") && stunduration > 2.5f)
        {
            Debug.LogWarning("SafetyNet triggered: reset full enemy state");

            inHitReaction = false;
            isAttacking = false;
            stunduration = 0f;

            // Force reset animator to idle, in case it's still mid-blend or stuck
            anim.Play("idle", 0, 0f);

            
        }

        stunduration += Time.deltaTime;

        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        songPositionInBeats = songPosition / secPerBeat;

        

        isAttacking = false;

        
        int beatRounded = Mathf.FloorToInt(songPositionInBeats);
        if (beatRounded == lastBeat) return;
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
        yield return new WaitForSeconds(0.2f); // delay before hitbox appears
        hitboxKick.SetActive(true);
        yield return new WaitForSeconds(0.3f); // active time
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

    public void StartHitReaction()
    {
        StartCoroutine(HitReactionRoutine());
    }



    IEnumerator HitReactionRoutine()
    {
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
        else
        {
            isAttacking = true;

            if (kickBeats.Contains(beat))
            {
                anim.Play("kick", 0, 0f);
                StartCoroutine(ActivateHitboxKick());
            }
            else
            {
                anim.Play("hit1", 0, 0f);
                StartCoroutine(ActivateJudgmentHitboxes());
            }
        }
    }

    IEnumerator ActivateJudgmentHitboxes()
    {
        yield return new WaitForSeconds(0.15f);
        earlyHitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f); // ~early window
        earlyHitbox.SetActive(false);
        yield return new WaitForSeconds(0.01f);

        perfectHitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f); // ~perfect window
        perfectHitbox.SetActive(false);
        yield return new WaitForSeconds(0.01f);

        lateHitbox.SetActive(true);
        yield return new WaitForSeconds(0.1f); // ~late window
        lateHitbox.SetActive(false);
        yield return new WaitForSeconds(0.01f);

        missHitbox.SetActive(true); // only if player didn’t respond in time
        yield return new WaitForSeconds(0.2f);
        missHitbox.SetActive(false);
        yield return new WaitForSeconds(0.01f);
    }

    public float GetSongBeatPosition()
    {
        float songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        return songPosition / secPerBeat;
    }
}
