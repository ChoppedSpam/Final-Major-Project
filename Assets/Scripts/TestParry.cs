using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TestParry : MonoBehaviour
{
    public GameObject player;
    public bool guardcounter = false;
    public GameObject Enemy;

    public float stunDuration; 


    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Early") || other.CompareTag("Perfect")) && !player.GetComponent<test>().guardcounter)
        {
            Debug.Log("PARRY SUCCESS");

            player.GetComponent<test>().StartCoroutine("HandleParryStun");
        }
    }
}
