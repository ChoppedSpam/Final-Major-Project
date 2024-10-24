using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class hurtbox : MonoBehaviour
{
    public GameObject Conductor;

    public bool counter = false;
    //public Rigidbody2D rb;
    public GameObject Player;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == true)
        {
            if (Conductor.GetComponent<Conductor>().timepressed > (Conductor.GetComponent<Conductor>().songPositionInBeats - 0.1f) && Conductor.GetComponent<Conductor>().timepressed < (Conductor.GetComponent<Conductor>().songPositionInBeats + 0.1f))
            {
                Player.GetComponent<test>().hitperfect++;
                counter = false;
            } 
            
            if (Conductor.GetComponent<Conductor>().timepressed > (Conductor.GetComponent<Conductor>().songPositionInBeats + 0.1f) && Conductor.GetComponent<Conductor>().timepressed < (Conductor.GetComponent<Conductor>().songPositionInBeats + 0.5f))
            {
                Player.GetComponent<test>().hitlate++;
                counter = false;
            }

            if (Conductor.GetComponent<Conductor>().timepressed < (Conductor.GetComponent<Conductor>().songPositionInBeats - 0.1f) && Conductor.GetComponent<Conductor>().timepressed > (Conductor.GetComponent<Conductor>().songPositionInBeats - 0.5f))
            {
                Player.GetComponent<test>().hitearly++;
                counter = false;
            }

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
