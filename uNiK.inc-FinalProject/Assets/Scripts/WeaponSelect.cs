﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{

    private MonoBehaviour m_AimScript;
    private GameObject m_Crosshair;
    private Rigidbody2D[] weaponsList;
    private Rigidbody2D defaultWeapon;
    private Dictionary<string, int> weaponCosts;
    [SerializeField] private GameObject weaponMenu;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private string defaultWeaponName = "Rocket";
    [SerializeField] private GameObject RightScroll;
    [SerializeField] private GameObject LeftScroll;
    [SerializeField] private GameObject turnSystem;

    private List<Button> buttons;
    private bool weaponMenuIsOpen, scrollLShow, scrollRShow, interactableMenu;
    public float weaponImageScaling = 75f;
    private int showingButtons = 12;
    private TankController m_Controller;
    private Stats m_Stats;

    private void Start()
    {
        CreateHashtable();
        weaponsList = Resources.LoadAll<Rigidbody2D>("Projectile Prefabs");
        SetDefaultWeapon(defaultWeaponName);
        buttons = new List<Button>();
        CreateButtons();
        weaponMenu.SetActive(false);
        weaponMenuIsOpen = false;
        interactableMenu = false;
        HideScrollButtons();
    }
    private void Update()
    {
        if (Input.GetKeyDown("e") && interactableMenu)
        {
            OpenWeaponMenu();
            DisableWeaponButtons();
        }
    }

    private void SetDefaultWeapon(string name)
    {
        foreach (Rigidbody2D weapon in weaponsList)
        {
            if (weapon.name == name)
            {
                defaultWeapon = weapon;
            }
        }
    }

    public void Interactable(bool active)
    {
        interactableMenu = active;
    }

    public void OpenWeaponMenu()
    {
        if (!weaponMenuIsOpen)
        {
            weaponMenuIsOpen = true;
            m_AimScript.enabled = false;
            m_Controller.IsActive = false;
            weaponMenu.SetActive(true);
        }
        else
        {
            CloseMenu();
        }

        foreach(Button b in buttons)
        {
            b.gameObject.SetActive(weaponMenuIsOpen);
        }
    }

    public void CloseMenu()
    {
        weaponMenuIsOpen = false;
        m_AimScript.enabled = true;
        m_Controller.IsActive = true;
        weaponMenu.SetActive(false);
    }

    public void CreateButtons()
    {
        float offset = 112.0f;
        float buttonY = 537.0f;
        for (int i = 0; i < 12; i++)
        {
            float buttonX = 150 + offset + ((i % 4) * 169);
            if (i % 4 == 0 && i != 0) buttonY -= 153;

            Button newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(weaponMenu.transform, false);
            newButton.transform.position = new Vector2(buttonX, buttonY);

            GameObject weaponImage = new GameObject();
            weaponImage.transform.SetParent(newButton.transform, false);
            weaponImage.transform.position = new Vector2(buttonX, buttonY);
            weaponImage.AddComponent<SpriteRenderer>();
            weaponImage.GetComponent<SpriteRenderer>().sortingOrder = 3;
            weaponImage.transform.localScale = new Vector2(weaponImageScaling, weaponImageScaling);

            if (i <= weaponsList.Length - 1)
            {
                Rigidbody2D weapon = weaponsList[i];
                newButton.transform.GetChild(0).GetComponent<Text>().text = weapon.name;
                foreach (KeyValuePair<string, int> kvp in weaponCosts)
                {
                    if(kvp.Key == weapon.name)
                    {
                        newButton.transform.GetChild(1).GetComponent<Text>().text = kvp.Value.ToString();
                    }
                }
                newButton.onClick.AddListener(delegate { ButtonPress(weapon, newButton); });
            }

            buttons.Add(newButton);
        }
    }

    private void HideScrollButtons()
    {
        if (showingButtons < weaponsList.Length)
        {
            RightScroll.SetActive(true);
            scrollRShow = true;
        }
        else
        {
            RightScroll.SetActive(false);
            scrollRShow = false;
        }

        if (showingButtons - 12 > 0)
        {
            LeftScroll.SetActive(true);
            scrollLShow = true;
        }
        else
        {
            LeftScroll.SetActive(false);
            scrollLShow = false;
        }
    }

    public void ButtonPress(Rigidbody2D weaponSelected, Button weaponButton)
    {
        int pointCost = 0;
        int.TryParse(weaponButton.transform.GetChild(1).GetComponent<Text>().text, out pointCost);
        m_AimScript.GetComponent<GenericAim>().SetProjectile(weaponSelected);
        m_Stats.points -= pointCost;
        OpenWeaponMenu();
    }

    public void ScrollingButtonRight()
    {
        showingButtons += 12;
        HideScrollButtons();
        for (int i = 0; i < 12; i++)
        {
            if (i + (showingButtons - 12) <= weaponsList.Length - 1)
            {
                Rigidbody2D weapon = weaponsList[i + (showingButtons - 12)];
                buttons[i].transform.GetChild(0).GetComponent<Text>().text = weapon.name;
                foreach (KeyValuePair<string, int> kvp in weaponCosts)
                {
                    if (kvp.Key == weapon.name)
                    {
                        buttons[i].transform.GetChild(1).GetComponent<Text>().text = kvp.Value.ToString();
                    }
                }
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(delegate { ButtonPress(weapon, buttons[i]); });
            }
            else
            {
                buttons[i].GetComponentInChildren<SpriteRenderer>().sprite = null;
                buttons[i].onClick.RemoveAllListeners();
            }
        }
        DisableWeaponButtons();
    }

    public void ScrollingButtonLeft()
    {
        showingButtons -= 12;
        HideScrollButtons();
        for (int i = 0; i < 12; i++)
        {
            Rigidbody2D weapon = weaponsList[i + (showingButtons - 12)];
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = weapon.name;
            foreach (KeyValuePair<string, int> kvp in weaponCosts)
            {
                if (kvp.Key == weapon.name)
                {
                    buttons[i].transform.GetChild(1).GetComponent<Text>().text = kvp.Value.ToString();
                }
            }
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(delegate { ButtonPress(weapon, buttons[i]); });
        }
        DisableWeaponButtons();
    }

    public void ChangeActiveTank()
    {
        var aiming = turnSystem.GetComponent<TurnSystem>().m_ActiveCharacter;
        m_AimScript = aiming.gameObject.GetComponentInChildren<GenericAim>();
        m_Crosshair = aiming.gameObject.transform.GetChild(0).gameObject;
        m_Controller = aiming.gameObject.GetComponentInChildren<TankController>();
        m_Stats = aiming.gameObject.GetComponentInChildren<Stats>();
        m_AimScript.GetComponent<GenericAim>().SetProjectile(defaultWeapon);
    }

    private void CreateHashtable()
    {
        //each weapon needs to be added here
        weaponCosts = new Dictionary<string, int>
        {
            //weapon name (as it is writen in resource folder) , point value
            { "BouncyBall", 5 },
            { "FireworkBomb", 15 },
            { "Grenade", 3 },
            { "Railgun", 1 },
            { "Rocket", 0 },
            { "spikyBall", 2 },
            { "EnergyBall", 10 },
            { "PlasmaRocket", 15 }
        };
    }

    public void DisableWeaponButtons()
    {
        foreach (Button weaponButton in buttons)
        {
            int parsedInt = 0;
            int.TryParse(weaponButton.transform.GetChild(1).GetComponent<Text>().text, out parsedInt);
            if(parsedInt > m_Stats.points)
            {
                weaponButton.interactable = false;
            }
            else
            {
                weaponButton.interactable = true;
            }
        }
    }

    public void AddPoints(int value)
    {
        m_Stats.points += value;
    }
}
