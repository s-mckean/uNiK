using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour {

    [SerializeField] private GameObject[] menuObjects;
    [SerializeField] private Button[] menuButtons;
    [SerializeField] private RawImage splashScreen;
    [SerializeField] private RawImage whiteScreen;

	// Use this for initialization
	void Start () {
        ActivateMenuObjects(false);
        ActivateButtons(false);
        splashScreen.color = new Color(255, 255, 255, 0);

        StartCoroutine(DisplaySplashScreen());
	}
	
	private IEnumerator DisplaySplashScreen()
    {
        while (splashScreen.color.a < 1)
        {
            float alpha = splashScreen.color.a + 0.01f;
            splashScreen.color = new Color(255, 255, 255, alpha);

            yield return new WaitForFixedUpdate();
        }
        
        Invoke("StartDisplayingMenu", 2.0f);
    }

    private void StartDisplayingMenu()
    {
        ActivateMenuObjects(true);
        StartCoroutine(DisplayMenu());
    }

    private IEnumerator DisplayMenu()
    {
        
        while (splashScreen.color.a > 0)
        {
            float alpha = splashScreen.color.a - 0.01f;
            splashScreen.color = new Color(255, 255, 255, alpha);
            whiteScreen.color = new Color(255, 255, 255, alpha);

            yield return new WaitForFixedUpdate();
        }

        ActivateButtons(true);
    }

    private void ActivateMenuObjects(bool activate)
    {
        foreach (GameObject gObj in menuObjects)
        {
            gObj.SetActive(activate);
        }
    }

    private void ActivateButtons(bool activate)
    {
        foreach(Button b in menuButtons)
        {
            b.interactable = activate;
            b.GetComponentInChildren<Text>().enabled = activate;
        }
    }
}
