using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bullets;

public class EnemyBullet : MonoBehaviour
{
    float speed;
    Rigidbody2D rb;

    SoundManager soundManager;
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        rb = GetComponent<Rigidbody2D>();

        speed = 20;
        rb.velocity = new Vector2(transform.up.x * speed, transform.up.y * speed);
        StartCoroutine(Desaparicion(10));
    }

    // Update is called once per frame
    IEnumerator Desaparicion(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
                soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 10);

                Destroy(gameObject);
        }
    }
}
