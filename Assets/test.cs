using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    public TMP_Text Hits;
    public TMP_Text Hitslate;
    public TMP_Text HitsPerfect;
    public TMP_Text Miss;
    public TMP_Text Score;
    public TMP_Text Combo;


    public int hitearly;
    public int hitperfect;
    public int hitlate;
    public int miss;
    public int score;
    public int oldscore;
    public int combo;

    public GameObject htbox1;
    //public GameObject htbox2;
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
        //htbox2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

        Hits.text = hitearly.ToString();
        Hitslate.text = hitlate.ToString();
        HitsPerfect.text = hitperfect.ToString();
        Score.text = score.ToString();
        Miss.text = miss.ToString();
        Combo.text = combo.ToString();

        htboxtimer += Time.deltaTime;

        /*if (htboxtimer < 0.35f)
        {
            hitting = true;
        }
        else
        {
            hitting=false;
        }

        xinput = Input.GetAxis("Horizontal");


        

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
        */
        if (Input.GetKeyDown(KeyCode.E))
        {
            htboxtimer = 0;
            htbox1.SetActive(true);
            oldscore = score;
        }

        if (htboxtimer > 0.2f)
        {
            if(score == oldscore && score != 0)
            {
                miss++;
                combo = 0;
                oldscore = 0;
                htboxtimer = 0;
                htbox1.SetActive(false);
            }
            else if (oldscore !=0)
            {
                combo++;
                oldscore = 0;
                htboxtimer = 0;
                htbox1.SetActive(false); 
            }
            
            htbox1.SetActive(false);
        }
    }

    void FixedUpdate()
    {

        /*if (hitting != true)
        {
            rb.velocity = new Vector2(xinput * speed,yinput * speed);
        }*/
    }

    /*void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hit")
        {
            Debug.Log("hit");
        }

    }*/

    
}
