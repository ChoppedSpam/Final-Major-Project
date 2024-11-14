using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class hurtbox : MonoBehaviour
{
    public float late;
    public float early;

    public GameObject Conductor;

    public bool counter = false;
    //public Rigidbody2D rb;
    public GameObject Player;

    public float timepressed;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        early = (Conductor.GetComponent<Conductor>().BeatRounded - 0.6f);
        late = (Conductor.GetComponent<Conductor>().BeatRounded - 0.4f);



        timepressed = Conductor.GetComponent<Conductor>().timepressed;

        if (counter == true)
        {
            if (timepressed < early)
            {
                Player.GetComponent<test>().hitearly++;
                counter = false;
            }
            else if (timepressed > late)
            {
                Player.GetComponent<test>().hitlate++;
                counter = false;
            }
            else
            {
                Player.GetComponent<test>().hitperfect++;
                counter = false;
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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Hit")
        {
            Debug.Log("hit");
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }
        
    }*/
}
