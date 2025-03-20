using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouch : MonoBehaviour
{
    public GameObject htbox1;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hit")
        {
            Player.GetComponent<test>().anim.Play("StarGetHit");
            htbox1.GetComponent<hurtbox>().playerhealth -= 10;
        }
    }*/
}
