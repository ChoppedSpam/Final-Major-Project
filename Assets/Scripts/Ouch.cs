using System.Collections;
using UnityEngine;

public class Ouch : MonoBehaviour
{
    public GameObject htbox1;
    public GameObject Player;
    private Animator playerAnim;
    private bool isHit = false;

    void Start()
    {
        playerAnim = Player.GetComponent<test>().anim;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hit" && !isHit)
        {
            Animator playerAnim = Player.GetComponent<test>().anim;

            isHit = true;
            playerAnim.Play("StarGetHit", 0, 0f);
            Player.GetComponent<test>().playerhealth -= 5;
            Player.GetComponent<test>().miss++;
            Debug.Log("MISSS");
            StartCoroutine(ResetHit());


        }
    }


    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(0.5f); // Cooldown to prevent multiple triggers
        isHit = false;
    }
}