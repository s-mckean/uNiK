using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private MonoBehaviour m_AimScript;
    [SerializeField] private GameObject m_Crosshair;

    public void ActivateSystem(bool activate)
    {
        m_AimScript.enabled = activate;
        m_Crosshair.SetActive(activate);
    }
}