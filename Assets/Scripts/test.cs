using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    public float inputOffset = 0.07f;
    public float timingDiff;
    public float timepressed = -1f;

    public Animator anim;

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

    private CameraShake cameraShake; // Reference to CameraShake script

    // Start is called before the first frame update
    void Start()
    {
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
            StartCoroutine(DisableHitboxAfterDelay(htbox1, 0.2f));
            oldscore = score;
        }


        if (Input.GetKeyDown(KeyCode.W) && htboxtimer2 >= 0.11f)
        {
            anim.Play("Guard");
            htboxtimer2 = 0;
            htbox2.SetActive(true);
        }



        // Check if attack missed or landed
        if (htboxtimer > 0.2f)
        {
            if (score == oldscore && score != 0) // If no score change, player missed
            {
                miss++;
                htbox1.GetComponent<hurtbox>().playerhealth -= 10;
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
            Debug.Log("Player attacks stunned enemy!");
            score += 500; // **Bonus points for attacking stunned enemy**
            combo += 1; // **Increase combo**
        }
    }

    IEnumerator DisableHitboxAfterDelay(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}