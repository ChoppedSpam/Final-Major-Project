using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class hurtbox : MonoBehaviour
{
    private bool isReacting = false;
    private bool reactionQueued = false;
    

    public GameObject Enemy;
    public Animator EnemyAnims;
    
    public GameObject Player;
    public Animator PlayerAnims;
    public GameObject Conductor;
    public GameObject htbox1;
    public GameObject htbox2;
    public GameObject HitVFXPrefab;
    public GameObject ParryFXPrefab;
    public GameObject StarFlyPrefab;
    public GameObject BasedPrefab;

    
    public float mult = 1f;
    public float timepressed;
    public float early = 0.06f;
    public float late = 0.15f;
    public float playerhealthCalc;
    public bool counter = false;

    


    void Start()
    {
        
        playerhealthCalc = 100f;
        
    }

    void Update()
    {

        float multiplier = Conductor.GetComponent<Conductor>().playerDamageMultiplier;

        if (counter == true)
        {
            StartCoroutine(DelayedHitCheck());
            counter = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.inTutorial)
        {
            Debug.Log("Paused for tutorial � skipping hit logic");
            return; // Or return, depending on the method
        }

        if (collision.gameObject.tag == "Hit" && counter == false)
        {
            Conductor.GetComponent<Conductor>().stunduration = 0f;
            counter = true;
        }
    }

    IEnumerator DelayedHitCheck()
    {

        if (TutorialManager.Instance != null && TutorialManager.Instance.inTutorial)
            yield break;

        yield return new WaitForSeconds(0.01f);

        timepressed = Player.GetComponent<test>().timepressed;
        GameObject htbox1 = Player.GetComponent<test>().htbox1;
        GameObject htbox2 = Player.GetComponent<test>().htbox2;

        /*if (htbox1.activeSelf || htbox2.activeSelf)
        {
            //if(timepressed > Conductor.GetComponent<Conductor>().beatRoundedUp - 0.4f && timepressed < Conductor.GetComponent<Conductor>().beatRoundedUp - 0.1f)
            //{
            //    Debug.Log("EARLY");
            //    Player.GetComponent<test>().hitearly++;
            //    Player.GetComponent<test>().score += 100 * mult;
            //    Player.GetComponent<test>().enemyhealth -= 1;
            //}

            //if(timepressed < Conductor.GetComponent<Conductor>().lastBeat + 0.4f)
            //{
            //    Debug.Log("LATE");
            //    Player.GetComponent<test>().hitlate++;
            //    Player.GetComponent<test>().score += 100 * mult;
            //    Player.GetComponent<test>().enemyhealth -= 1;
            //}

            //if(timepressed > Conductor.GetComponent<Conductor>().beatRoundedUp - 0.1f && timepressed < Conductor.GetComponent<Conductor>().beatRoundedUp)
            //{
            //    Debug.Log("EARLY PERFECT");
            //    Player.GetComponent<test>().hitperfect++;
            //    Player.GetComponent<test>().score += 300 * mult;
            //    Player.GetComponent<test>().enemyhealth -= 5;
            //}

            //if (timepressed > Conductor.GetComponent<Conductor>().lastBeat && timepressed < Conductor.GetComponent<Conductor>().lastBeat + 0.1f)
            //{
            //    Debug.Log("LATE PERFECT");
            //    Player.GetComponent<test>().hitperfect++;
            //    Player.GetComponent<test>().score += 300 * mult;
            //    Player.GetComponent<test>().enemyhealth -= 5;
            //}

            float time = timepressed;
            float beatUp = Conductor.GetComponent<Conductor>().beatRoundedUp;
            float beatDown = Conductor.GetComponent<Conductor>().lastBeat;

            // DEBUG
            Debug.Log($"[HIT DEBUG] Pressed: {time:F3}, beatUp: {beatUp}, beatDown: {beatDown}");

            /*if (time > beatUp - 0.1f && time < beatUp)
            {
                Debug.Log("EARLY PERFECT");
                Player.GetComponent<test>().hitperfect++;
                Player.GetComponent<test>().score += 300 * mult;
                Player.GetComponent<test>().enemyhealth -= 5;
            }
            else if (time > beatDown && time < beatDown + 0.1f)
            {
                Debug.Log("LATE PERFECT");
                Player.GetComponent<test>().hitperfect++;
                Player.GetComponent<test>().score += 300 * mult;
                Player.GetComponent<test>().enemyhealth -= 5;
            }
            else if (time > beatUp - 0.4f && time < beatUp - 0.1f)
            {
                
            }
            else if (time > beatDown + 0.1f && time < beatDown + 0.4f)
            {
                
            }
            else
            {
                Debug.Log("MISS");
            }*/

            /*Debug.Log("PERFECT");
            Player.GetComponent<test>().hitperfect++;
            Player.GetComponent<test>().score += 300 * mult;
            Player.GetComponent<test>().enemyhealth -= 5;

            Debug.Log("EARLY");
            Player.GetComponent<test>().hitearly++;

            Debug.Log("LATE");
            Player.GetComponent<test>().hitlate++;

            Player.GetComponent<test>().score += 100 * mult;
            Player.GetComponent<test>().enemyhealth -= 1;

            Debug.Log("MISS");
            Player.GetComponent<test>().miss++;
            playerhealthCalc -= 10;

            Conductor.GetComponent<Conductor>().StartHitReaction();
            Vector3 vfxPos = Enemy.transform.position + new Vector3(0, 1.5f, 0);
            GameObject vfx = Instantiate(HitVFXPrefab, vfxPos, Quaternion.identity);
            Destroy(vfx, 0.3f);
        }
        else
        {
            Player.GetComponent<test>().playerhealth -= 5;
            PlayerAnims.Play("PlayerHit", 0, 0f);
        }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (TutorialManager.Instance != null && TutorialManager.Instance.inTutorial)
        {
            Debug.Log("Paused for tutorial � skipping hit logic");
            return; // Or return, depending on the method
        }

        if (other.gameObject.tag == "Parry" && Player.GetComponent<test>().guardcounter)
        {
            NPCReactionEvents.OnPlayerHit?.Invoke();
            Debug.Log("PARRY");
            Player.GetComponent<test>().hitperfect++;
            Player.GetComponent<test>().hitSoundSource.Play();
            Player.GetComponent<test>().score += 300 * mult;
            float multiplier = Conductor.GetComponent<Conductor>().playerDamageMultiplier;
            if (SceneManager.GetActiveScene().name == "CustomLvl")
            {
                Player.GetComponent<test>().enemyhealth -= 1f * multiplier;

            }
            else
            {
                Player.GetComponent<test>().enemyhealth -= 2.5f * multiplier;
            }
            Conductor.GetComponent<Conductor>().StartHitReaction();
            FindObjectOfType<CameraShake>().Shake(1f, 0.2f);
            for (int i = 0; i < 4; i++)
            {
                Vector3 spawnPos = transform.position;
                GameObject fx;

                if (i % 2 == 0)
                    fx = Instantiate(StarFlyPrefab, spawnPos, Quaternion.identity); // Star
                else
                    fx = Instantiate(ParryFXPrefab, spawnPos, Quaternion.identity); // New effect

                Rigidbody2D rb = fx.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    float angle = Random.Range(20f, 60f) * Mathf.Deg2Rad;
                    float speed = Random.Range(15f, 20f);
                    Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    rb.velocity = dir * speed;
                    rb.angularVelocity = Random.Range(-360f, 360f);
                }

                Destroy(fx, 1.5f);
            }
            Vector3 vfxPos = Enemy.transform.position + new Vector3(0, 1.5f, 0);
            GameObject vfx = Instantiate(HitVFXPrefab, vfxPos, Quaternion.identity);
            Destroy(vfx, 0.3f);
            this.gameObject.SetActive(false);

        }

        if (Player.GetComponent<test>().guardcounter)
        {
            Debug.Log("Parry success � ignore enemy damage");
            return;
        }

        if (other.gameObject.tag == "Early")
        {
            NPCReactionEvents.OnPlayerHit?.Invoke();
            Debug.Log("EARLY");
            Player.GetComponent<test>().hitearly++;
            Player.GetComponent<test>().hitSoundSource.Play();
            Player.GetComponent<test>().score += 100 * mult;
            float multiplier = Conductor.GetComponent<Conductor>().playerDamageMultiplier;
            if (SceneManager.GetActiveScene().name == "CustomLvl")
            {
                Player.GetComponent<test>().enemyhealth -= 0.5f * multiplier;

            }
            else
            {
                Player.GetComponent<test>().enemyhealth -= 2.5f * multiplier;
            }
            Player.GetComponent<test>().ResetPerfectChain();
            Conductor.GetComponent<Conductor>().StartHitReaction();
            FindObjectOfType<CameraShake>().Shake(1f, 0.2f);
            Vector3 vfxPos = Enemy.transform.position + new Vector3(0, 1.5f, 0);
            GameObject vfx = Instantiate(HitVFXPrefab, vfxPos, Quaternion.identity);
            Destroy(vfx, 0.3f);
            this.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Late")
        {
            NPCReactionEvents.OnPlayerHit?.Invoke();
            Debug.Log("LATE");
            Player.GetComponent<test>().hitlate++;
            Player.GetComponent<test>().hitSoundSource.Play();
            Player.GetComponent<test>().score += 100 * mult;
            float multiplier = Conductor.GetComponent<Conductor>().playerDamageMultiplier;
            if (SceneManager.GetActiveScene().name == "CustomLvl")
            {
                Player.GetComponent<test>().enemyhealth -= 0.5f * multiplier;

            }
            else
            {
                Player.GetComponent<test>().enemyhealth -= 2.5f * multiplier;
            }
            Player.GetComponent<test>().ResetPerfectChain();
            Conductor.GetComponent<Conductor>().StartHitReaction();
            FindObjectOfType<CameraShake>().Shake(1f, 0.2f);
            Vector3 vfxPos = Enemy.transform.position + new Vector3(0, 1.5f, 0);
            GameObject vfx = Instantiate(HitVFXPrefab, vfxPos, Quaternion.identity);
            Destroy(vfx, 0.3f);
            this.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Perfect")
        {
            NPCReactionEvents.OnPlayerHit?.Invoke();
            Debug.Log("PERFECT");
            Player.GetComponent<test>().hitperfect++;
            Player.GetComponent<test>().hitSoundSource.Play();
            Player.GetComponent<test>().score += 300 * mult;
            float multiplier = Conductor.GetComponent<Conductor>().playerDamageMultiplier;
            if (SceneManager.GetActiveScene().name == "CustomLvl")
            {
                Player.GetComponent<test>().enemyhealth -= 0.5f * multiplier;

            }
            else
            {
                Player.GetComponent<test>().enemyhealth -= 2.5f * multiplier;
            }
            
            Player.GetComponent<test>().RegisterPerfect();
            Conductor.GetComponent<Conductor>().StartHitReaction();
            FindObjectOfType<CameraShake>().Shake(1f, 0.2f);
            Vector3 vfxPos = Enemy.transform.position + new Vector3(0, 1.5f, 0);
            GameObject vfx = Instantiate(HitVFXPrefab, vfxPos, Quaternion.identity);
            Destroy(vfx, 0.3f);
            this.gameObject.SetActive(false);
        }
    }

    



    /*IEnumerator PlayHitReaction()
    {
        if (isReacting)
        {
            reactionQueued = true;
            yield break;
        }

        isReacting = true;
        reactionQueued = false;

        var cond = Conductor.GetComponent<Conductor>();
        cond.inHitReaction = true;
        cond.stunduration = 0f; //  Reset duration right here

        // Force animation to reset (even if already playing)
        EnemyAnims.Play("idle", 0, 0f);
        yield return null;
        EnemyAnims.Play("Empty", 0, 0f);

        SpawnFlyingStars(4);

        yield return new WaitForSeconds(0.2f); // give the reaction time to breathe

        cond.inHitReaction = false;
        isReacting = false;

        if (reactionQueued)
        {
            Debug.Log(" Queued reaction running now");
            StartCoroutine(PlayHitReaction());
        }
    }

    void SpawnFlyingStars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = starSpawnPoint.position;
            GameObject star = Instantiate(StarFlyPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = star.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float angle = Random.Range(20f, 60f) * Mathf.Deg2Rad;
                float speed = Random.Range(6f, 10f);
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                rb.velocity = dir * speed;
                rb.angularVelocity = Random.Range(-360f, 360f);
            }

            Destroy(star, 1.5f);
        }
    }*/
}
