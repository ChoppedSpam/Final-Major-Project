using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public GameObject hurtbox;
    public bool EnemyPresent;  
    public bool EnemyOutofBounds;  


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            EnemyPresent = true;
            if(hurtbox.GetComponent<hurtbox>().enemyhealth <= 0f)
            {
                
                Debug.Log("AHHHH");
            }
        }
        else
        {
            EnemyPresent = false;
        }
    }
}
