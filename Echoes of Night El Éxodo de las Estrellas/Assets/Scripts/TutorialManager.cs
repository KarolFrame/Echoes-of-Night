using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public GameObject fondoTuto;
    public GameObject[] PC;
    public GameObject[] MANDO;

    public void TutorialMostrar(int a)
    {
        fondoTuto.SetActive(true);

        var binding = InputManager.inputActions.Player.Move.GetBindingForControl(InputManager.inputActions.Player.Move.activeControl).Value;

        if (binding.groups == "Keyboard&Mouse")
        {
            PC[a].gameObject.SetActive(true);
        }


        if (binding.groups == "Joystick")
        {
            MANDO[a].gameObject.SetActive(true);
        }

    }

     public void OcultarTodos()
    {
        fondoTuto.SetActive(false);

        for (int i = 0; i < PC.Length; i++)
        {
            PC[i].SetActive(false);
        }
        for (int o = 0;o < PC.Length; o++)
        {
            MANDO[o].SetActive(false);
        }
    }
}
