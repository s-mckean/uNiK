using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuEvents : MonoBehaviour {

    [SerializeField] private RawImage whiteScreen;
    [SerializeField] private Canvas canvas;

    private int option;

	public void ExitButton()
    {
        StartCoroutine(FadeToWhite());
        option = 1;
    }

    public void PlayButton()
    {
        option = 2;
    }

    public void TutorialButton()
    {
        StartCoroutine(FadeToWhite());
        option = 3;
    }

    private IEnumerator FadeToWhite()
    {
        canvas.sortingOrder = 5;
        while (whiteScreen.color.a < 1)
        {
            float alpha = whiteScreen.color.a + 0.01f;
            whiteScreen.color = new Color(255, 255, 255, alpha);

            yield return new WaitForFixedUpdate();
        }

        switch(option)
        {
            case 1: Application.Quit(); break;
            case 2: break;
            case 3: SceneManager.LoadScene(1); break;
            default: break;
        }
    }
}
