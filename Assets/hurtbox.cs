using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class hurtbox : MonoBehaviour
{
    public GameObject EnemyTracker;
    public GameObject OOBTracker;
    public GameObject Enemy;
    public GameObject EnemyHealthSlider;
    public float enemyhealth = 100;

    public float late;
    public float early;

    public GameObject Conductor;

    public bool counter = false;
    //public Rigidbody2D rb;
    public GameObject Player;

    public float timepressed;

    public Transform thispos;
    void Start()
    {
        
        enemyhealth = 100f;  
    }

    // Update is called once per frame
    void Update()
    {
        EnemyHealthSlider.GetComponent<Slider>().value = enemyhealth * 0.01f;

        early = (Conductor.GetComponent<Conductor>().BeatRounded - 0.6f);
        late = (Conductor.GetComponent<Conductor>().BeatRounded - 0.4f);



        timepressed = Conductor.GetComponent<Conductor>().timepressed;

        if (counter == true)
        {
            if (timepressed < early)
            {
                Player.GetComponent<test>().hitearly++;
                enemyhealth = enemyhealth - 10f;
                counter = false;
                
            }
            else if (timepressed > late)
            {
                Player.GetComponent<test>().hitlate++;
                enemyhealth = enemyhealth - 10f;
                counter = false;
                
            }
            else
            {
                Player.GetComponent<test>().hitperfect++;
                enemyhealth = enemyhealth - 15f;
                counter = false;
                
            }

            if (enemyhealth <= 0f)
            {
                ResetEnemy();
                EnemyHealthSlider.SetActive(false);
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
        Enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 50), ForceMode2D.Impulse);

        if (EnemyTracker.GetComponent<EnemyTracker>().EnemyPresent == false && OOBTracker.GetComponent<OOB>().EnemyOutofBounds == false)
        {
            enemyhealth = 100f;
            Enemy.transform.position = thispos.position;

        }
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
