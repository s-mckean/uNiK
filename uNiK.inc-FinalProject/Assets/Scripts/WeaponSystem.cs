using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSystem : MonoBehaviour {

    [SerializeField] private MonoBehaviour m_AimScript;
    [SerializeField] private GameObject m_Crosshair;
    [SerializeField] private List<Rigidbody2D> weaponsList;
    [SerializeField] private GameObject weaponMenu;
    [SerializeField] private Button buttonPrefab;
    private List<Button> buttons;
    private bool weaponMenuIsOpen;
    public float weaponImageScaling = 75f;

    private void Start()
    {
        buttons = new List<Button>();
        CreateButtons();
        weaponMenu.SetActive(false);
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
            weaponMenu.SetActive(true);
        }
        else
        {
            weaponMenuIsOpen = false;
            m_AimScript.enabled = true;
            weaponMenu.SetActive(false);
        }
    }

    public void CreateButtons()
    {
        int weaponCount = 0;
        float buttonY = 0.0f;
        foreach (Rigidbody2D weapon in weaponsList)
        {
            float buttonX = -2.0f + ((weaponCount % 4) * 3);
            if (weaponCount % 4 == 0 && weaponCount != 0) buttonY -= 3;

            Sprite weaponSprite = weapon.GetComponent<SpriteRenderer>().sprite;

            Button newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(weaponMenu.transform, false);
            newButton.transform.position = new Vector2(buttonX, buttonY);

            GameObject weaponImage = new GameObject();
            weaponImage.transform.SetParent(newButton.transform, false);
            weaponImage.transform.position = new Vector2(buttonX, buttonY);
            weaponImage.AddComponent<SpriteRenderer>();
            weaponImage.GetComponent<SpriteRenderer>().sprite = weaponSprite;
            weaponImage.GetComponent<SpriteRenderer>().sortingOrder = 1;
            weaponImage.transform.localScale = new Vector2(weaponImageScaling, weaponImageScaling);

            newButton.onClick.AddListener(delegate { ButtonPress(weapon); });

            buttons.Add(newButton);

            weaponCount++;
        }
    }

    public void ButtonPress(Rigidbody2D weaponSelected)
    {
        GetComponent<GenericAim>().SetProjectile(weaponSelected);
        OpenWeaponMenu();
    }
}
