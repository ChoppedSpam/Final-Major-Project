using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class hurtbox : MonoBehaviour
{
    public GameObject EnemyTracker;
    public GameObject OOBTracker;
    public GameObject Enemy;
    public GameObject EnemyHealthSlider;
    public GameObject PlayerHealthSlider;
    public float enemyhealth = 100;
    public float playerhealth = 100;
    public float step = 1f;
    public float mult = 1f;

    public float late;
    public float early;
    public float latemiss;
    public float earlymiss;

    public GameObject Conductor;

    public bool counter = false;
    //public Rigidbody2D rb;
    public GameObject Player;

    public float timepressed;

    public Transform thispos;
    void Start()
    {
        playerhealth = 100f;
        enemyhealth = 100f;  
    }

    // Update is called once per frame
    void Update()
    {



        if (EnemyTracker.GetComponent<EnemyTracker>().EnemyPresent == false && OOBTracker.GetComponent<OOB>().EnemyOutofBounds == true)
        {
            Debug.Log("Reset");
            enemyhealth = 100f;
            //Enemy.transform.position = Vector3.MoveTowards(transform.position, thispos.position, step);
            Enemy.transform.position = thispos.position;
        }

        EnemyHealthSlider.GetComponent<Slider>().value = enemyhealth * 0.01f;
        PlayerHealthSlider.GetComponent<Slider>().value = playerhealth * 0.01f;

        early = (Conductor.GetComponent<Conductor>().BeatRounded - 0.5f);
        late = (Conductor.GetComponent<Conductor>().BeatRounded - 0.4f);
        /*latemiss = (Conductor.GetComponent<Conductor>().BeatRounded - 0.3f);
        earlymiss = (Conductor.GetComponent<Conductor>().BeatRounded - 0.6f);*/



        timepressed = Conductor.GetComponent<Conductor>().timepressed;

        if (counter == true)
        {
            if (timepressed < early)
            {
                Player.GetComponent<test>().hitearly++;
                Player.GetComponent<test>().score = Player.GetComponent<test>().score + (100 * mult);
                enemyhealth = enemyhealth - 10f;
                counter = false;
                
            }
            else if (timepressed > late)
            {
                Player.GetComponent<test>().hitlate++;
                Player.GetComponent<test>().score = Player.GetComponent<test>().score + (100* mult);
                enemyhealth = enemyhealth - 10f;
                counter = false;
                
            }
            else
            {
                Player.GetComponent<test>().hitperfect++;
                Player.GetComponent<test>().score = Player.GetComponent<test>().score + (300 * mult);
                enemyhealth = enemyhealth - 15f;
                counter = false;
                
            }

            /*if(timepressed <= earlymiss)
            {
                Player.GetComponent<test>().miss++;
            }

            if (timepressed >= latemiss)
            {
                Player.GetComponent<test>().miss++;
            }*/

            if (enemyhealth <= 0f)
            {
                ResetEnemy();
                EnemyHealthSlider.SetActive(false);
            }
            else
            {
                EnemyHealthSlider.SetActive(true);
            }

            

            /*if (timepressed > (Conductor.GetComponent<Conductor>().songPositionInBeats - 0.1f) && timepressed < (Conductor.GetComponent<Conductor>().songPositionInBeats + 0.1f))
            {
                Player.GetComponent<test>().hitperfect++;
                counter = false;
            } 
            
            if (timepressed > (Conductor.GetComponent<Conductor>().songPositionInBeats + 0.1f) && timepressed < (Conductor.GetComponent<Conductor>().songPositionInBeats + 0.5f))
            {
                Player.GetComponent<test>().hitlate++;
                counter = false;
            }

            if (timepressed < (Conductor.GetComponent<Conductor>().songPositionInBeats - 0.1f) && timepressed > (Conductor.GetComponent<Conductor>().songPositionInBeats - 0.5f))
            {
                Player.GetComponent<test>().hitearly++;
                counter = false;
            }*/

            /*if(timepressed > (Conductor.GetComponent<Conductor>().BeatRoundedDown + 0.4)  && (timepressed > Conductor.GetComponent<Conductor>().BeatRounded - 0.4))
            {
                Player.GetComponent<test>().hitperfect++;
                counter = false;
            }

            if (timepressed < (Conductor.GetComponent<Conductor>().BeatRoundedDown + 0.4))
            {
                Player.GetComponent<test>().hitlate++;
                counter = false;
            }

            if (timepressed > (Conductor.GetComponent<Conductor>().BeatRoundedDown + 0.6))
            {
                Player.GetComponent<test>().hitearly++;
                counter = false;
            }*/
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hit" && counter == false)
        {
            Debug.Log("hit");
            counter = true;
            
        }
    }

    public void ResetEnemy()
    {
        Enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(70, 70), ForceMode2D.Impulse); 
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Hit")
        {
            Debug.Log("hit");
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }
        
    }*/
}
