using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWall : MonoBehaviour
{
    public enum DirDisparo{derecha, izquirda};
    public DirDisparo dir;

    public Transform[] spawnBullet;
    public GameObject enemybullets;

    public int vida;

    public GameObject o2;

    void Start()
    {
        StartCoroutine(Disparar());
    }
    
    IEnumerator Disparar() 
    {
        yield return new WaitForSeconds(3);
        GameObject bullet = Instantiate(enemybullets);
        if (dir == DirDisparo.derecha) 
        {
            bullet.transform.position = spawnBullet[0].position;
            bullet.transform.rotation = spawnBullet[0].rotation;
        }
        if (dir == DirDisparo.izquirda)
        {
            bullet.transform.position = spawnBullet[1].position;
            bullet.transform.rotation = spawnBullet[1].rotation;
        }
        StartCoroutine(Disparar());
    }
    public int GetVida() { return vida; }

    public void SetVida(int a) { vida = a; }

    private void OnDestroy()
    {
        GameObject loot = Instantiate(o2);
        loot.transform.position = transform.position;
    }

}
