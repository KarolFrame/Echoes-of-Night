using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorEnemigos : MonoBehaviour
{
    public Transform player;
    Transform tr;
    private void Awake()
    {
        tr = transform;
    }
    private void Update()
    {
        tr.position= player.position;
    }
}
