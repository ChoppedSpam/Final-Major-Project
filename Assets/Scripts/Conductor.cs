using System.Collections;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public Transform starSpawnPoint;
    public float stunduration = 0f;
    public GameObject hitbox2;
    public GameObject StarFlyPrefab;

    public Animator anim;
    public GameObject hitbox1;
    public float songBpm;
    public AudioSource musicSource;

    private float secPerBeat;
    private float dspSongTime;
    private float songPosition;
    private float songPositionInBeats;
    private float lastBeat = -1;

    public bool isAttacking = false;
    public bool inHitReaction = false;

    void Start()
    {
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    void Update()
    {
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

        int beatRounded = Mathf.FloorToInt(songPositionInBeats);
        if (beatRounded == lastBeat) return;
        lastBeat = beatRounded;

        // DEBUG INFO
        Debug.Log($"[Conductor] Beat: {beatRounded} | isAttacking: {isAttacking} | inHitReaction: {inHitReaction}");

        // START ATTACK LOGIC
        if (!isAttacking && !inHitReaction)
        {
            bool isIdle = state.IsName("idle");

            if (isIdle)
            {
                isAttacking = true;

                // Play attack animation
                anim.Play("hit1", 0, 0f); // Change to Startup1 if preferred

                // Start hitbox coroutine
                StartCoroutine(ActivateHitbox());

                // Reset attack flag after short delay
                StartCoroutine(ResetAttackFlag(secPerBeat * 0.8f));
            }
        }
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

    IEnumerator ResetAttackFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
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
}
