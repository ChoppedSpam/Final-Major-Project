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

    public bool hitting;

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

        if (htboxtimer < 0.35f)
        {
            hitting = true;
        }
        else
        {
            hitting=false;
        }

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
        if (htboxtimer > 0.35)
        {
            htbox1.SetActive(false);
            htbox2.SetActive(false);
        }

        if(hitting == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {

        if (hitting != true)
        {
            rb.velocity = new Vector2(xinput * speed,yinput * speed);
        }
    }
}
