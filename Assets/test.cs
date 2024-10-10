using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject htbox1;
    public GameObject htbox2;
    public GameObject Player;

    public float htboxtimer= 0;
    public float htboxstartup = 0;

    public float htboxstun;

    public Rigidbody2D rb;

    public float xinput;
    public float yinput;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        htboxtimer = 2;
        rb = GetComponent<Rigidbody2D>();
        htbox1.SetActive(false);
        htbox2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        htboxtimer += Time.deltaTime;

        xinput = Input.GetAxis("Horizontal");


        if (Input.GetKeyDown(KeyCode.E)) 
        {
            htboxtimer = 0;
            htbox1.SetActive(true);
        }

        if (htboxtimer > 0.25f)
        {
            htbox2.SetActive(true);
        }
        if (htboxtimer > 0.75f)
        {
            htbox1.SetActive(false);
            htbox2.SetActive(false);
        }

        

        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(xinput * speed, yinput * speed);
    }
}
