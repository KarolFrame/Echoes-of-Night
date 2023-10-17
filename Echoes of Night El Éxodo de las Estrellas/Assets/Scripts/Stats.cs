using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats instance;
    int nivel;
    int balizas;
    float oxigeno;
    float polvoEstelar;

    private void Awake()
    {
        instance = this;

        balizas = 3; 
        oxigeno = 100;
        polvoEstelar = 0;
    }

    public float GetOxigeno() { return oxigeno; }
    public float GetPolvoEstelar() { return polvoEstelar; }
    public int GetNivel() { return nivel; }

    public void SetOxigeno(float a)
    {
        oxigeno = a;   
    }
    public void SetPolvo(float a)
    {
        polvoEstelar = a;
    }

    public void SetNivel(int a)
    { nivel = a; }

    public int GetBalizas() { return balizas; }
    public void SetBalizas(int a) { balizas = a; }
}
