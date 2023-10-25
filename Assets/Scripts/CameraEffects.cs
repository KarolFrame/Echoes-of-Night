using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
using System;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance;

    CinemachineVirtualCamera virtualCamera;
    float tiempoMovimiento, tiempoMoviminetoTotal;
    float intensidadInicial;
    CinemachineBasicMultiChannelPerlin cmbmcp;

    private void Awake()
    {
        Instance= this;

        virtualCamera= GetComponent<CinemachineVirtualCamera>();
        cmbmcp = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    public void ShakeCamera(float intensidad, float frecuencia, float tiempo)
    {
        cmbmcp.m_AmplitudeGain= intensidad;
        cmbmcp.m_FrequencyGain= frecuencia;

        intensidadInicial = intensidad;
        tiempoMoviminetoTotal = tiempo;
        tiempoMovimiento= tiempo;

    }

    void StopShakeCamera()
    {
        /*cmbmcp = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cmbmcp.m_AmplitudeGain = 0;

        timer = 0;*/
    }

    void Update()
    {
        if(tiempoMovimiento>0)
        {
            tiempoMovimiento-= Time.deltaTime;
            cmbmcp.m_AmplitudeGain = Mathf.Lerp(intensidadInicial, 0, 1 - (tiempoMovimiento / tiempoMoviminetoTotal));
        }
        if(tiempoMovimiento<=0) 
        {
            Camera.main.transform.eulerAngles= new Vector3(0, 0, 0);
        }
    }
}
