using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesMiniG : MonoBehaviour
{
    float[] rotations = { 0, 90, 180, 270 };

    public bool isPlaced = false;

    private void Start()
    {
        int random = Random.Range(0, rotations.Length);
        transform.eulerAngles = new Vector3(0, 0, rotations[random]);

        if (transform.eulerAngles.z < 0.01f && transform.eulerAngles.z > -0.01f)
        {
            isPlaced = true;
        }
        else if (isPlaced == true)
        {
            isPlaced = false;
        }
    }

    public void ChangeRotation()
    {
        transform.Rotate(new Vector3(0,0, 90));

        if(transform.eulerAngles.z < 0.01f&& transform.eulerAngles.z > -0.01f && isPlaced==false) 
        {
            isPlaced= true;
        }
        else if(isPlaced==true) 
        {
            isPlaced= false;
        }
    }
}
