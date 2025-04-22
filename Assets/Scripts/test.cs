using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static hurtbox;

public class test : MonoBehaviour
{

    public float inputOffset = 0.05f;
    public float timingDiff;
    public float timepressed = -1f;

    public Animator anim;

    public int parryCharges = 3;
    public int maxParryCharges = 3;
    private int perfectChain = 0;
    public GameObject parryTick1;
    public GameObject parryTick2;
    public GameObject parryTick3;
    public GameObject EnemyHealthSlider;
    public GameObject playerHealthSlider;
    public GameObject PlayerPortrait;
    public GameObject PlayerPortrait2;
    public GameObject PlayerPortrait3;
    public GameObject EnemyPortrait;
    public GameObject EnemyPortrait2;
    public GameObject EnemyPortrait3;
    public RectTransform tugOfWarFill;
    public RectTransform tugOfWarFillHeart;
    public float lastTugX = 0f;
    public float barFlashThreshold = 30f; // How far must the bar jump to flash
    public RectTransform tugFlashTarget;
    public float maxOffset = 870f;
    private float targetX = 0f;
    private float lerpSpeed = 8f;
    public float enemyhealth = 100f;
    public float playerhealth = 100f;

    public TMP_Text Hits;
    public TMP_Text Hitslate;
    public TMP_Text HitsPerfect;
    public TMP_Text Miss;
    public TMP_Text Score;
    public TMP_Text Combo;
    public TMP_Text AccuracyText;

    public int hitearly;
    public int hitperfect;
    public int hitlate;
    public int miss;
    public float score;
    public float oldscore;
    public int combo;
    public int oldcombo;

    public GameObject htbox1;
    public GameObject htbox2;
    public GameObject Player;
    public GameObject conductor;

    public float htboxtimer = 0;
    public float htboxtimer2 = 0;
    public float htboxstartup = 0;

    public float htboxstun;
    public Rigidbody2D rb;

    public float xinput;
    public float yinput;
    public float speed = 5f;

    public bool hitting;
    public bool dead = false;
    public Sprite deadasl;
    public SpriteRenderer deadone;

    private bool isHit = false;
    public bool guardcounter = false;
    public float dashCooldown = 0.6f;
    private float dashTimer = 0f;
    private bool isDashing = false;
    public bool isInvincible = false;
    public bool canPunch = true;


    private Vector3 originalPosition;

    public GameObject[] dashCloudPrefabs;
    public Transform dashCloudSpawnPoint;
    private int currentCloudIndex = 0;



    private CameraShake cameraShake; // Reference to CameraShake script

    public TMP_Text ScoreSpriteText;
    public TMP_Text ComboSpriteText;
    public TMP_SpriteAsset numberSpriteAsset;
    public DigitDisplay scoreDisplay;
    public DigitDisplay comboDisplay;

    private bool hasRevived = false;


    public string GetSpriteScoreText(int value)
    {
        string result = "";
        foreach (char c in value.ToString())
        {
            int index = int.Parse(c.ToString());
            result += $"<sprite index={index}>";  // Use index here
        }
        return result;
    }

    public string GetSpriteComboText(int value)
    {
        string result = "";
        foreach (char c in value.ToString())
        {
            int index = int.Parse(c.ToString());
            result += $"<sprite index={index}>";  // Use index here
        }
        return result;
    }


    // Start is called before the first frame update
    void Start()
    {
        

        originalPosition = transform.position;
        enemyhealth = 100f;
        playerhealth = 100f;

        oldcombo = 0;
        htboxtimer = 2;
        rb = GetComponent<Rigidbody2D>();
        htbox1.SetActive(false);
        htbox2.SetActive(false);

        // Get CameraShake component from the main camera
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (enemyhealth <= 0 && !hasRevived)
        {
            hasRevived = true;
            conductor.GetComponent<Conductor>().anim.Play("death");
            StartCoroutine(HandleEnemyRevival());
        }*/

        if (ScoreSpriteText.spriteAsset != numberSpriteAsset)
        {
            ScoreSpriteText.spriteAsset = numberSpriteAsset;
        }
        if (ComboSpriteText.spriteAsset != numberSpriteAsset)
        {
            ComboSpriteText.spriteAsset = numberSpriteAsset;
        }
        ScoreSpriteText.spriteAsset = numberSpriteAsset;
        ScoreSpriteText.text = GetSpriteScoreText((int)score);
        ComboSpriteText.spriteAsset = numberSpriteAsset;
        ComboSpriteText.text = GetSpriteComboText((int)score);
        dashTimer += Time.deltaTime;

        scoreDisplay.SetNumber((int)score);
        comboDisplay.SetNumber((int)combo);


        if (playerhealth >= 50)
        {
            PlayerPortrait.SetActive(true);
            PlayerPortrait2.SetActive(false);
            PlayerPortrait3.SetActive(false);
        }
        else if (playerhealth <= 50 && playerhealth >= 30)
        {
            PlayerPortrait.SetActive(false);
            PlayerPortrait2.SetActive(true);
            PlayerPortrait3.SetActive(false);
        }
        else if (playerhealth <= 30)
        {
            PlayerPortrait.SetActive(false);
            PlayerPortrait2.SetActive(false);
            PlayerPortrait3.SetActive(true);
        }


        if (playerhealth <= 0 && dead == false)
        {
            anim.Play("stardie");
            StartCoroutine(FreezeOnDeath());
            dead = true;
        }

        if (dead == true)
        {
            deadone.GetComponent<SpriteRenderer>().sprite = deadasl;
        }

        // ENEMY PORTRAITS
        if (enemyhealth >= 50)
        {
            EnemyPortrait.SetActive(true);
            EnemyPortrait2.SetActive(false);
            EnemyPortrait3.SetActive(false);
        }
        else if (enemyhealth <= 50 && enemyhealth >= 30)
        {
            EnemyPortrait.SetActive(false);
            EnemyPortrait2.SetActive(true);
            EnemyPortrait3.SetActive(false);
        }
        else if (enemyhealth <= 30)
        {
            EnemyPortrait.SetActive(false);
            EnemyPortrait2.SetActive(false);
            EnemyPortrait3.SetActive(true);
        }


        float totalHealth = Mathf.Max(playerhealth + enemyhealth, 1f); // Prevent divide by zero
        float balance = Mathf.Clamp01(playerhealth / totalHealth);
        targetX = Mathf.Lerp(-maxOffset, maxOffset, balance);
        Vector2 anchoredPos = tugOfWarFill.anchoredPosition;

        float deltaX = Mathf.Abs(targetX - lastTugX);

        if (deltaX > barFlashThreshold)
        {
            StartCoroutine(TugFlashPop());
        }

        anchoredPos.x = Mathf.Lerp(anchoredPos.x, targetX, Time.deltaTime * lerpSpeed);
        lastTugX = anchoredPos.x;

        lastTugX = anchoredPos.x;
        tugOfWarFill.anchoredPosition = anchoredPos;
        tugOfWarFillHeart.anchoredPosition = anchoredPos;

        if (combo >= oldcombo + 25)
        {
            htbox1.GetComponent<hurtbox>().mult = htbox1.GetComponent<hurtbox>().mult + .25f;
            oldcombo = combo;
        }

        // Update UI
        Hits.text = hitearly.ToString();
        Hitslate.text = hitlate.ToString();
        HitsPerfect.text = hitperfect.ToString();
        score = (int)score;
        Score.text = score.ToString();
        Miss.text = miss.ToString();
        Combo.text = combo.ToString();

        // **Calculate accuracy as a percentage**
        int totalHits = hitearly + hitlate + hitperfect + miss;
        float accuracy = 100f; // Default to 100% if no hits yet

        if (totalHits > 0)
        {
            float weightedHits = (hitperfect * 100f) + (hitearly * 50f) + (hitlate * 50f); // Perfect = 100%, Early/Late = 50%
            accuracy = weightedHits / totalHits; 
        }

        AccuracyText.text = $"{accuracy:F2}%"; // Display accuracy with 2 decimal places

        htboxtimer += Time.deltaTime;
        htboxtimer2 += Time.deltaTime;

        Animator enemyAnim = conductor.GetComponent<Conductor>().anim;
        AnimatorStateInfo enemyState = enemyAnim.GetCurrentAnimatorStateInfo(0);

        // Attack input
        if (Input.GetKeyDown(KeyCode.E) && canPunch && htboxtimer >= 0.22f && !anim.GetCurrentAnimatorStateInfo(0).IsName("StarGetHurt") && !enemyState.IsName("kick"))
        {
            float currentBeat = conductor.GetComponent<Conductor>().GetSongBeatPosition();
            timepressed = currentBeat + inputOffset;

            // Compare to the actual beat immediately
            timingDiff = currentBeat - Mathf.Round(currentBeat); // how far off from the closest full beat

            anim.Play("Punch");
            htboxtimer = 0;
            htbox1.SetActive(true);
            StartCoroutine(DisableHitboxAfterDelay(htbox1, 0.025f));
            oldscore = score;
        }


        if (Input.GetKeyDown(KeyCode.W) && htboxtimer2 >= 0.11f && !anim.GetCurrentAnimatorStateInfo(0).IsName("StarGetHurt"))
        {
            anim.Play("Guard");
            
            htboxtimer2 = 0;
            htbox2.SetActive(true);
            StartCoroutine(DisableHitboxAfterDelay(htbox2, 0.15f));
        }

        if (Input.GetKeyDown(KeyCode.A) && dashTimer >= dashCooldown && !isDashing)
        {
            StartCoroutine(DashBack());
        }



        // Check if attack missed or landed
        if (htboxtimer > 0.2f)
        {
            if (score == oldscore && score != 0) // If no score change, player missed
            {
                miss++;
                playerhealth -= 2.5f;
                combo = 0;
                oldscore = 0;
                htbox1.SetActive(false);
            }
            else if (oldscore != 0) // If score changed, player landed a hit
            {
                combo++;
                oldscore = 0;
                htbox1.SetActive(false);

                //conductor.GetComponent<Conductor>().anim.Play("blocked");

                // **Trigger Camera Shake on a successful hit**
                if (cameraShake != null)
                {
                    
                }
            }

            htbox1.SetActive(false);
        }

        if (htboxtimer2 > 0.11f)
        {
            htbox2.SetActive(false);
        }

        if (htbox2.GetComponent<TestParry>().guardcounter && Input.GetKeyDown(KeyCode.E))
        {
            if (cameraShake != null)
            {
                
            }
            //conductor.GetComponent<Conductor>().anim.Play("blocked");
            htbox2.GetComponent<TestParry>().guardcounter = false;
            conductor.GetComponent<Conductor>().stunduration = 0;
            //Debug.Log("Player attacks stunned enemy!");
            //score += 500; // **Bonus points for attacking stunned enemy**
            //combo += 1; // **Increase combo**
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hit" && !isHit && !isInvincible)
        {
            Animator playerAnim = Player.GetComponent<test>().anim;


            isHit = true;
            playerAnim.Play("StarGetHit", 0, 0f);
            Player.GetComponent<test>().ResetPerfectChain();
            combo = 0;
            Player.GetComponent<test>().miss++;
            StartCoroutine(ResetHit());

            // Check if player is not punching
            if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
            {
                
            }
        }
    }

    IEnumerator ResetHit()
    {
        playerhealth -= 5;
        yield return new WaitForSeconds(0.5f); // Cooldown to prevent multiple triggers
        isHit = false;
        
    }

    IEnumerator DisableHitboxAfterDelay(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    IEnumerator HandleParryStun()
    {
        if (parryCharges <= 0)
            yield break; // No charges available
        conductor.GetComponent<ParryEffectManager>().ShowParryPopup();

        parryCharges--;
        UpdateParryUI();

        guardcounter = true;

        // Apply stun to enemy
        conductor.GetComponent<Conductor>().isstunned = true;
        conductor.GetComponent<Conductor>().parryHitbox.SetActive(true);
        conductor.GetComponent<Conductor>().StartHitReaction();

        yield return new WaitForSeconds(0.5f);

        // Clear stun
        conductor.GetComponent<Conductor>().isstunned = false;
        conductor.GetComponent<Conductor>().parryHitbox.SetActive(false);
        guardcounter = false;
    }

    void UpdateParryUI()
    {
        parryTick1.SetActive(parryCharges >= 1);
        parryTick2.SetActive(parryCharges >= 2);
        parryTick3.SetActive(parryCharges >= 3);
    }

    public void RegisterPerfect()
    {
        perfectChain++;

        if (perfectChain >= 3 && parryCharges < maxParryCharges)
        {
            parryCharges++;
            UpdateParryUI();
            perfectChain = 0;

            Debug.Log("Parry charge restored!");
        }
    }

    public void ResetPerfectChain()
    {
        perfectChain = 0;
    }

    IEnumerator DashBack()
    {
        isDashing = true;
        dashTimer = 0f;
        isInvincible = true;

        float invincibilityTime = 0.6f;
        StartCoroutine(ResetInvincibility(invincibilityTime));

        if (dashCloudPrefabs.Length > 0)
        {
            GameObject fx = Instantiate(
                dashCloudPrefabs[currentCloudIndex],
                dashCloudSpawnPoint.position,
                Quaternion.identity
            );

            // Scale up the cloud (adjust size to taste)
            fx.transform.localScale = Vector3.one * 0.9f;

            // Add a bit of random rotation (e.g. -15 to +15 degrees)
            float randomZRotation = Random.Range(-15f, 15f);
            fx.transform.Rotate(0f, 0f, randomZRotation);

            Destroy(fx, 0.2f);

            // Move to next cloud in cycle
            currentCloudIndex = (currentCloudIndex + 1) % dashCloudPrefabs.Length;
        }
        anim.Play("Guard");

        float dashDistance = 1.5f;
        float dashSpeed = 10f;
        Vector3 dashTarget = originalPosition - new Vector3(dashDistance, 0, 0);

        float t = 0f;
        Vector3 start = transform.position;

        // Dash backward
        while (t < 1f)
        {
            t += Time.deltaTime * dashSpeed;
            transform.position = Vector3.Lerp(start, dashTarget, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        // Return to original position
        t = 0f;
        start = transform.position;

        while (t < 1f)
        {
            t += Time.deltaTime * (dashSpeed / 2f);
            transform.position = Vector3.Lerp(start, originalPosition, t);
            yield return null;
        }

        isDashing = false;
    }
    IEnumerator ResetInvincibility(float delay)
    {
        yield return new WaitForSeconds(delay);
        isInvincible = false;
    }

    IEnumerator TugFlashPop()
    {
        if (tugFlashTarget == null) yield break;

        Vector3 originalScale = tugFlashTarget.localScale;
        Vector3 popScale = Vector3.Min(originalScale * 1.15f, new Vector3(1.2f, 1.2f, 1f)); // Cap max

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * 10f;
            tugFlashTarget.localScale = Vector3.Lerp(popScale, originalScale, t);
            yield return null;
        }


        tugFlashTarget.localScale = originalScale;
    }

    IEnumerator FreezeOnDeath()
    {
        // Wait for the animation to finish
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Then freeze the pose
        anim.enabled = false;
    }

    IEnumerator HandleEnemyRevival()
    {
        // 2. Check if song is still playing
        AudioSource music = conductor.GetComponent<Conductor>().musicSource;
        if (music != null && music.time < music.clip.length - 1f)
        {
            conductor.GetComponent<Conductor>().anim.Play("death");
            yield return new WaitForSeconds(1f);

            // Disable hitboxes and attacks
            conductor.GetComponent<Conductor>().pausedExternally = true;



            yield return new WaitForSeconds(2.5f); // Wait before revival

            // Play get-up animation
            conductor.GetComponent<Conductor>().anim.Play("getup");

            yield return new WaitForSeconds(1.2f); // Time for getup animation

            // Reset health and resume
            enemyhealth = 100f;
            playerhealth = 100f;
            conductor.GetComponent<Conductor>().pausedExternally = false;
            
        }
    }

    IEnumerator HeartPulse()
    {
        Vector3 originalScale = tugOfWarFillHeart.localScale;
        Vector3 pulsedScale = originalScale * 1.2f;

        float t = 0f;
        float pulseDuration = 0.2f;

        // Scale up
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / pulseDuration;
            tugOfWarFillHeart.localScale = Vector3.Lerp(originalScale, pulsedScale, t);
            yield return null;
        }

        // Scale down
        t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / pulseDuration;
            tugOfWarFillHeart.localScale = Vector3.Lerp(pulsedScale, originalScale, t);
            yield return null;
        }

        tugOfWarFillHeart.localScale = originalScale;
    }

}