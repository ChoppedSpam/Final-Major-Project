using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TestParry : MonoBehaviour
{
    public GameObject player;
    public bool guardcounter = false;
    public GameObject Enemy;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Enemy.GetComponent<Conductor>().stunduration >= 0.1f)
        {
            Enemy.GetComponent<Conductor>().stunduration = 0f;
            guardcounter = false;
        }


        if (guardcounter)
        {
            

            Enemy.GetComponent<Conductor>().anim.Play("Idle");

            if(Enemy.GetComponent<Conductor>().hitbox1.activeSelf)
            {
                Enemy.GetComponent<Conductor>().hitbox1.SetActive(true);
            }

            if (Enemy.GetComponent<Conductor>().hitbox2.activeSelf)
            {
                Enemy.GetComponent<Conductor>().hitbox2.SetActive(true);
            }

            

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hit" && guardcounter == false)
        {
            Debug.Log("hit");
            Enemy.GetComponent<Conductor>().stunduration = 0f;
            guardcounter = true;
        }
    }
}
