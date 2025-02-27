using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Conductor : MonoBehaviour
{
    public Animator anim;
    public GameObject hurtbox;
    public GameObject player;
    public int[] beatstomiss;
    public int[] SecondHit;

    [Range(0.1f, 0.4f)]
    public float Early;
    [Range(0.1f, 0.4f)]
    public float Late;

    public float oldscore;
    public int misscount;
    public float timepressed;
    public bool even;
    public bool SecondHitAct = false;
    public GameObject hitbox1;
    public GameObject hitbox2;

    public float songBpm;
    public float secPerBeat;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTime;
    public AudioSource musicSource;

    public float currentBeat;
    public float BeatRounded;
    public float BeatRoundedDown;

    public bool offline = false;
    public float timer;

    public bool isAttacking = false;
    public float lastBeat = -1; // Keeps track of last processed beat

    public float stunduration = 0f;

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    void Update()
    {
        stunduration += Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
        {
            timepressed = songPositionInBeats;
        }

        SecondHitAct = false;
        offline = false;

        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        songPositionInBeats = songPosition / secPerBeat;
        currentBeat = songPositionInBeats;
        BeatRounded = Mathf.CeilToInt(currentBeat);
        BeatRoundedDown = Mathf.FloorToInt(currentBeat);

        even = (BeatRounded % 2 == 0);

        // If it's the same beat as last frame, do nothing to prevent retriggering
        if (BeatRounded == lastBeat)
            return;

        lastBeat = BeatRounded; // Update last processed beat

        

        foreach (var item in beatstomiss)
        {
            if (item == BeatRounded)
            {
                offline = true;
                hitbox1.SetActive(false);
                hitbox2.SetActive(false);
                return; // Skip attack logic if offline
            }
        }

        foreach (var item in SecondHit)
        {
            if (item == BeatRounded)
            {
                SecondHitAct = true;
                break;
            }
        }

        if (hurtbox.GetComponent<hurtbox>().enemyhealth <= 0f)
        {
            offline = true;
            hitbox1.SetActive(false);
            hitbox2.SetActive(false);
            return;
        }

        // **Only allow attacks when a new beat is detected**
        if (!isAttacking && !anim.GetCurrentAnimatorStateInfo(0).IsName("blocked"))
        {
            isAttacking = true; // Lock attack state

            if (SecondHitAct) // Variation attack
            {
                if (even)
                {
                    anim.Play("SecondHitStart");
                    StartCoroutine(HitboxTiming(hitbox2, 0.1f, 0.4f));
                }
                else
                {
                    anim.Play("SecondHit");
                    StartCoroutine(HitboxTiming(hitbox2, 0.1f, 0.4f));
                }
            }
            else // Normal attack
            {
                if (even )
                {
                    anim.Play("Startup1");
                }
                else
                {
                    anim.Play("hit1");

                    StartCoroutine(HitboxTiming(hitbox2, 0.1f, 0.4f));
                }
            }

            StartCoroutine(ResetAttackFlag(secPerBeat * 0.8f)); // Ensure attack doesn't retrigger too quickly
        }
    }

    IEnumerator HitboxTiming(GameObject hitbox, float delayBefore, float activeTime)
    {
        yield return new WaitForSeconds(delayBefore);
        hitbox.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        hitbox.SetActive(false);
    }

    IEnumerator ResetAttackFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false; // Allow new attack after the delay
    }
}