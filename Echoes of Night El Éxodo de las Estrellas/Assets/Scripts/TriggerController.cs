using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerController : MonoBehaviour
{
	/// <summary>
	/// Evento de entrada
	/// </summary>
	public UnityEvent @enter;
	/// <summary>
	/// Evento de salida
	/// </summary>
	public UnityEvent @exit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
            enter.Invoke();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            @exit.Invoke();
    }
}
