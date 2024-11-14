using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public int[] beatstomiss;

    [Range(0.1f, 0.4f)]
    public float Early;

    [Range(0.1f, 0.4f)]
    public float Late;


    public float timepressed;

    public bool even;

    public GameObject hitbox1;
    public GameObject hitbox2;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    public float currentBeat;
    public float BeatRounded;
    public float BeatRoundedDown;

    public bool offline = false;


    // Start is called before the first frame update
    void Start()
    {


        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in beatstomiss)
        {
            offline = false;

            if (item == BeatRounded)
            {
                offline = true;
                hitbox1.SetActive(false);
                hitbox2.SetActive(false);
            }
        }


        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        currentBeat = songPositionInBeats;
        BeatRounded = Mathf.CeilToInt(currentBeat);
        BeatRoundedDown = Mathf.FloorToInt(currentBeat);

        if (BeatRounded%2 == 0)
        {
            even = true;
        }
        else
        {
            even = false;
        }


        if (even == true && offline == false)
        {
            
            hitbox1.SetActive(true);
            hitbox2.SetActive(false);
            
        }
        
        if (even == false && offline == false)
        {
            hitbox1.SetActive(false);
            hitbox2.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            timepressed = songPositionInBeats;
        }
    }
}
