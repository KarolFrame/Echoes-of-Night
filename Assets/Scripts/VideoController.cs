using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VideoController : MonoBehaviour
{
    public Animator fade;
    public string siguienteEscena;

    public float duracionVideo, duracionFade;
    void Start()
    {
        StartCoroutine(CuentaAtras());
    }

    IEnumerator CuentaAtras()
    {
        yield return new WaitForSeconds(duracionVideo);
        fade.Play("FinVideo");
        yield return new WaitForSeconds(duracionFade);
        LevelLoader.LoadLevel(siguienteEscena);
    }
}
