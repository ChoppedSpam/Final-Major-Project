using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public float maxOffset = 870f;
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

    

    private CameraShake cameraShake; // Reference to CameraShake script


    // Start is called before the first frame update
    void Start()
    {
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
            anim.Play("stardie", 1);
            dead = true;
        }

        if(dead == true)
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


        float totalHealth = playerhealth + enemyhealth;
        float balance = playerhealth / totalHealth; // value from 0.0 to 1.0

        // Apply this to tug-of-war UI
        tugOfWarFill.anchoredPosition = new Vector2(Mathf.Lerp(-maxOffset, maxOffset, balance), 0f);
        tugOfWarFillHeart.anchoredPosition = new Vector2(Mathf.Lerp(-maxOffset, maxOffset, balance), 0f);

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


        // Attack input
        if (Input.GetKeyDown(KeyCode.E) && htboxtimer >= 0.22f)
        {
            float currentBeat = conductor.GetComponent<Conductor>().GetSongBeatPosition();
            timepressed = currentBeat + inputOffset;

            // Compare to the actual beat immediately
            timingDiff = currentBeat - Mathf.Round(currentBeat); // how far off from the closest full beat

            anim.Play("Punch");
            htboxtimer = 0;
            htbox1.SetActive(true);
            StartCoroutine(DisableHitboxAfterDelay(htbox1, 0.05f));
            oldscore = score;
        }


        if (Input.GetKeyDown(KeyCode.W) && htboxtimer2 >= 0.11f)
        {
            anim.Play("Guard");
            htboxtimer2 = 0;
            htbox2.SetActive(true);
            StartCoroutine(DisableHitboxAfterDelay(htbox2, 0.15f));
        }



        // Check if attack missed or landed
        if (htboxtimer > 0.2f)
        {
            if (score == oldscore && score != 0) // If no score change, player missed
            {
                miss++;
                playerhealth -= 5;
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
                    cameraShake.ShakeCamera(0.05f, 0.05f);
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
                cameraShake.ShakeCamera(0.05f, 0.05f);
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
        if (collision.gameObject.tag == "Hit" && !isHit)
        {
            Animator playerAnim = Player.GetComponent<test>().anim;

            // Check if player is not punching
            if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
            {  
                isHit = true;
                playerAnim.Play("StarGetHit", 0, 0f);
                Player.GetComponent<test>().ResetPerfectChain();
                Player.GetComponent<test>().miss++;
                StartCoroutine(ResetHit());
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
}