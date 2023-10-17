using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GCMenu : MonoBehaviour
{

    public GameObject mandoC, pCC;
    private void Awake()
    {
        InputManager.ToggleActionMap(InputManager.inputActions.UI);
        
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
    public void Salir()
    {
        Application.Quit();
    }

    public void NuevaPartida()
    {
        LevelLoader.LoadLevel("Prologo");
    }
    public void Creditos()
    {
        LevelLoader.LoadLevel("Creditos");
    }
}
