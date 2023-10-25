using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed = 5;
    Rigidbody2D rb;
    Transform tr;


    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Movement());
    }


    IEnumerator Movement()
    {
        speed = speed * -1;

        rb.velocity = new Vector2(speed, 0);
        yield return new WaitForSeconds(3);
        StartCoroutine(Movement());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            collision.transform.SetParent(tr);
            collision.GetComponent<PlayerController>().enPlataforma = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
            collision.GetComponent<PlayerController>().enPlataforma = false;
        }
    }
}
