using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullets : MonoBehaviour
{
    public enum TypeBullet {ball, ray}
    TypeBullet type;

    SoundManager soundManager;

    Rigidbody2D rb;
    float speed = 10;
    bool chocaPared;

    PlayerController jugador;
    
    void Start()
    {

        jugador = FindObjectOfType<PlayerController>();

        if(type == TypeBullet.ray)
        {
            Stats.instance.SetBalizas(Stats.instance.GetBalizas() - 1);
            jugador.ActualizarRestanteRayosText();
        }


        soundManager = FindObjectOfType<SoundManager>();

        rb= GetComponent<Rigidbody2D>();

        if(type == TypeBullet.ray)
        {
            speed = 10;
            rb.velocity = new Vector2(transform.up.x * speed, transform.up.y * speed);
            StartCoroutine(Desaparicion(10));
        }
        if (type == TypeBullet.ball)
        {
            speed = 20;
            rb.velocity = new Vector2(transform.up.x * speed, transform.up.y * speed);
            StartCoroutine(Desaparicion(10));
        }

    }

    public void SetType(int a)
    {
        if(a==0)
            type= TypeBullet.ball;
        if(a==1)
            type= TypeBullet.ray;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer== 8) 
        {
            if(type==TypeBullet.ray)
            {
                soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 0);

                chocaPared = true;
                rb.velocity = Vector2.zero;
            }

            if (type == TypeBullet.ball)
            {
                soundManager.PlayAudio(gameObject.GetComponent<AudioSource>(), 10);

                Destroy(gameObject);
            }
        }


        if (collision.gameObject.tag == "Enemy")
        {
            if (type == TypeBullet.ball)
            {
                EnemyGround enemy = collision.GetComponent<EnemyGround>();
                if (enemy != null)
                {
                    enemy.SetVida(enemy.GetVida() - 1);

                    if (enemy.GetVida() <= 0)
                    {
                        soundManager.PlayAudio(FindObjectOfType<PlayerController>().GetComponent<AudioSource>(), 16);
                        Destroy(collision.gameObject);
                    }
                    else
                        soundManager.PlayAudio(collision.GetComponent<AudioSource>(), 14);

                }
                else
                {
                    EnemyWall enemywall = collision.GetComponent<EnemyWall>();
                    if(enemywall!=null) 
                    {
                        enemywall.SetVida(enemywall.GetVida() - 1);

                        if (enemywall.GetVida() <= 0)
                        {
                            soundManager.PlayAudio(FindObjectOfType<PlayerController>().GetComponent<AudioSource>(), 16);
                            Destroy(collision.gameObject);
                        }
                        else
                            soundManager.PlayAudio(collision.GetComponent<AudioSource>(), 14);
                    }

                }

                Destroy(gameObject);
            }
        }

    }
    IEnumerator Desaparicion(float time) 
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(type == TypeBullet.ray)
        {
            Stats.instance.SetBalizas(Stats.instance.GetBalizas() + 1);
            jugador.ActualizarRestanteRayosText();
        }
    }

}
