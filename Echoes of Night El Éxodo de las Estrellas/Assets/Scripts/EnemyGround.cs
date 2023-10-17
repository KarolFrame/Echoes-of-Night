using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    public float speed = 5;
    Rigidbody2D rb;

    public int vida;

    public GameObject o2;

    SpriteRenderer sprite;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(Movement());
    }


    IEnumerator Movement()
    {
        speed = speed * -1;
        sprite.flipX = !sprite.flipX;
        rb.velocity = new Vector2(speed, 0);
        yield return new WaitForSeconds(3);
        StartCoroutine(Movement());
    }

    public int GetVida() { return vida; }

    public void SetVida(int a) { vida = a; }

    private void OnDestroy()
    {
        GameObject loot = Instantiate(o2);
        loot.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ColiderLuz")
        {
            sprite.sortingLayerName = "Enemigo";

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ColiderLuz")
        {
            sprite.sortingLayerName = "EnemigoOscuro";

        }
    }
}
