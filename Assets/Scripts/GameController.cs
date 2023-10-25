using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    SoundManager soundManager;
    [Space]
    [Header("References")]
    public GameObject menuPausa;
    public UnityEngine.UI.Image polvosHUD, bolaOxigenoHUD;
    public Animator fade;
    public AudioSource alarma;
    public GameObject Player;

    public GameObject mandoC, pCC;

    [Space]
    [Header("Confing")]
    public int NivelActual;
    public string currentScene;
    ///public PlayerInput playerInput;
    string nivelACargar;


    private void Awake()
    {
        instance = this;

        soundManager= GetComponent<SoundManager>();
        InputManager.ToggleActionMap(InputManager.inputActions.Cinematica);
        
        StartCoroutine(RestarOxigeno());
        StartCoroutine(EmpiezaJuego());
    }
    private void Start()
    {
        Stats.instance.SetNivel(NivelActual);
    }

    public void ActivarMenuPausa()
    {
        //playerInput.SwitchCurrentActionMap("UI");
        InputManager.ToggleActionMap(InputManager.inputActions.UI);


        Time.timeScale = 0;
        menuPausa.SetActive(true);
    }
    public void DesactivarMenuPausa()
    {
        //playerInput.SwitchCurrentActionMap("Player");
        InputManager.ToggleActionMap(InputManager.inputActions.Player);

        Time.timeScale = 1;
        menuPausa.SetActive(false);
    }
    IEnumerator RestarOxigeno()
    {
        yield return new WaitForSeconds(10);
        Stats.instance.SetOxigeno(Stats.instance.GetOxigeno() - 5);
        ActualizarTexto();
        if(Stats.instance.GetOxigeno()<20)
            alarma.mute= false;
        else
            alarma.mute= true;

        if(Stats.instance.GetOxigeno() <= 0)
        {
            Muerte();
        }
        else
        {
            StartCoroutine(RestarOxigeno());
        }
    }
    public void ActualizarPolvos()
    {
        polvosHUD.fillAmount = (0.33f * Stats.instance.GetPolvoEstelar());
    }

    public void ActualizarTexto()
    {
        bolaOxigenoHUD.fillAmount = Stats.instance.GetOxigeno() / 100;
    }

    public void CambiarDeEscena(string name)
    {
        LevelLoader.LoadLevel(name);
    }

    public void CambiarDeEscenaTelescopio(string name)
    {
        if(Stats.instance.GetPolvoEstelar() == 3) 
        {
            StartCoroutine(CambiarEscenaConDelay(1, name));
        }
    }

    public void BotonSalir()
    {
        Application.Quit();
    }

    public void HacerFundidaANegro()
    {
        InputManager.ToggleActionMap(InputManager.inputActions.Cinematica);
        fade.Play("FadeIn_anim");
        StartCoroutine(EmpiezaJuego());
    }
    public void HacerFundidaANegroLava()
    {
        InputManager.ToggleActionMap(InputManager.inputActions.Cinematica);
        fade.Play("FadeInLava_anim");
        StartCoroutine(EmpiezaJuego());
    }

    public void TeletrasportPorMuerte(Transform target)
    {
        soundManager.PlayAudio(Player.GetComponent<AudioSource>(),20);
        HacerFundidaANegro();
        StartCoroutine(TpPlayer(target));

    }

    public void TeletrasportPorLava(Transform target)
    {
        HacerFundidaANegroLava();
        StartCoroutine(TpPlayer(target));

    }

    IEnumerator TpPlayer(Transform a)
    {
        yield return new WaitForSeconds(1);
        Player.transform.position = a.position;
    }

    IEnumerator CambiarEscenaConDelay(float t, string a)
    {
        HacerFundidaANegro();
        yield return new WaitForSeconds(t);
        LevelLoader.LoadLevel(a);
    }

    IEnumerator EmpiezaJuego() 
    {
        yield return new WaitForSeconds(5);
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
    }

    void Muerte()
    {
        fade.Play("Muerte");
        StartCoroutine(ReinicioEscena());
    }
    IEnumerator ReinicioEscena() 
    {
        yield return new WaitForSeconds(3.5f);
        LevelLoader.LoadLevel(currentScene);
    }

    public void controlesBoton()
    {
        var binding = InputManager.inputActions.UI.Click.GetBindingForControl(InputManager.inputActions.UI.Click.activeControl).Value;

        if (binding.groups == ";Keyboard&Mouse")
        {
            pCC.SetActive(true);
        }
        if (binding.groups == "Joystick")
        {
            mandoC.SetActive(true);
        }
    }


}
