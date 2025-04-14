using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBurst : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 2f;

    void Start()
    {
        Destroy(gameObject, 1.0f); // Clean up after 1 sec
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
