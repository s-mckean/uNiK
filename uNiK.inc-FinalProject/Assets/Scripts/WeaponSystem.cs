using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour {

    [SerializeField] private MonoBehaviour m_AimScript;
    [SerializeField] private GameObject m_Crosshair;
    [SerializeField] private List<Rigidbody2D> weaponsList;
    [SerializeField] private Canvas weaponMenu;
    private bool weaponMenuIsOpen;

    private void Start()
    {
        weaponMenu.enabled = false;
        weaponMenuIsOpen = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            OpenWeaponMenu();
        }
    }

    public void ActivateSystem(bool activate)
    {
        m_AimScript.enabled = activate;
        m_Crosshair.SetActive(activate);
    }

    public void OpenWeaponMenu()
    {
        if (!weaponMenuIsOpen)
        {
            weaponMenuIsOpen = true;
            m_AimScript.enabled = false;
            weaponMenu.enabled = true;
        }
        else
        {
            weaponMenuIsOpen = false;
            m_AimScript.enabled = true;
            weaponMenu.enabled = false;
        }
    }
}
