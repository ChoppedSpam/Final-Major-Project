using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TestParry : MonoBehaviour
{
    public GameObject player;
    public bool guardcounter = false;
    public GameObject Enemy;

    public float stunduration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(guardcounter)
        {
            stunduration += Time.deltaTime;

            Enemy.GetComponent<Conductor>().anim.speed = 0;
            if(Enemy.GetComponent<Conductor>().hitbox1.activeSelf)
            {
                Enemy.GetComponent<Conductor>().hitbox1.SetActive(true);
            }
            if (Enemy.GetComponent<Conductor>().hitbox2.activeSelf)
            {
                Enemy.GetComponent<Conductor>().hitbox2.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E)) 
            {
                player.GetComponent<test>().score++;
                guardcounter = false;
            }
            else if (stunduration >= 1)
            {
                stunduration = 0;
                guardcounter = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hit" && guardcounter == false)
        {
            Debug.Log("hit");
            guardcounter = true;

        }
    }
}
