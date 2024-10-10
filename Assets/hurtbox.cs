using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtbox : MonoBehaviour
{
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollision2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Hit")
        {
            Debug.Log("hit");
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }
        
    }
}
