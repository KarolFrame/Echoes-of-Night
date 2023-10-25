using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerMiniGame : MonoBehaviour
{
    public GameObject piecesHolder;
    GameObject[] pieces;
    public Animator fade;
    public string siguienteEEscena;

    [SerializeField]
    int totalPieces = 0;
    int correct = 0;

    Color negro = new Color (0, 0, 0, 1);
    Color blanco = new Color(1, 1, 1, 1);

    void Start()
    {
        InputManager.ToggleActionMap(InputManager.inputActions.UI);

        totalPieces= piecesHolder.transform.childCount;

        pieces = new GameObject[totalPieces];

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i] = piecesHolder.transform.GetChild(i).gameObject; 
        }

        StartCoroutine(InicioColor());
    }

    public void ComprobarPosiciones()
    {
        correct= 0;

        for (int a = 0; a < totalPieces; a++)
        {
            if (pieces[a].GetComponent<PiecesMiniG>().isPlaced)
            {
                pieces[a].GetComponent<Image>().color = blanco;
                correct++;
            }
            else
            {
                pieces[a].GetComponent<Image>().color = negro;
            }
        }
        if(correct==totalPieces) 
        {
            Ganas(siguienteEEscena);
        }
    }

    public void Ganas(string siguienteEscena)
    {
        StartCoroutine(CambiarEscenaConDelay(1.5f, siguienteEscena));
    }

    public void CambiarDeEscena(string name)
    {
        LevelLoader.LoadLevel(name);
    }

    public void CambiarDeEscenaTelescopio(string name)
    {
        if (Stats.instance.GetPolvoEstelar() == 3)
        {
            StartCoroutine(CambiarEscenaConDelay(1, name));
        }
    }
    IEnumerator CambiarEscenaConDelay(float t, string a)
    {
        fade.Play("FadeIn_anim");
        yield return new WaitForSeconds(t);
        LevelLoader.LoadLevel(a);
    }
    IEnumerator InicioColor()
    {
        yield return new WaitForSeconds(1);
        ComprobarPosiciones();
    }
}
