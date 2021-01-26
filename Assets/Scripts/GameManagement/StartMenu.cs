using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public CanvasGroup logoShow;
    public CanvasGroup mainMenu;
    public CanvasGroup loadingScreen;

    //system vars
    Image logo;
    CanvasRenderer alsoLogo;
    Scrollbar loadbar;
    Text loadText;
    bool logoActive = true;
    bool menuActive = false;
    bool loadingActive = false;
    float timer;

     void Start()
    {
        logoShow.gameObject.SetActive(true);
        logo = logoShow.GetComponent<Image>();
        alsoLogo = logo.GetComponent<CanvasRenderer>();

        mainMenu.gameObject.SetActive(false);

        loadbar = loadingScreen.GetComponentInChildren<Scrollbar>();
        loadText = GameObject.FindGameObjectWithTag("Player").GetComponent<Text>();
        loadingScreen.gameObject.SetActive(false);

        logoShow.alpha = 1f; //sets logo to full opacity
        alsoLogo.SetAlpha(0); //sets opacity of bg to 0
    }

    void Update()
    {
        //activates and deactivates logo
        if (!menuActive && !loadingActive)
        {
            if (logoActive)
                alsoLogo.SetAlpha(alsoLogo.GetAlpha() + 0.01f);
            else
                logoShow.alpha -= 0.01f;

            //scales until high enough val then goes back down
            if (alsoLogo.GetAlpha() >= 1f && logoActive)
                logoActive = false;
            else if (logoShow.alpha <= 0 && !logoActive)
            {
                menuActive = true;
                logoShow.gameObject.SetActive(false);
                mainMenu.gameObject.SetActive(true);
            }

        }

        //main menu
        else if (menuActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //resets time scale
                Time.timeScale = 1;
                //stops music
                GetComponent<AudioSource>().Stop();

                menuActive = false;
                loadingActive = true;
                mainMenu.gameObject.SetActive(false);
                loadingScreen.gameObject.SetActive(true);

            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit(); //exits application
            }
        }
        //loading screen
        else
        {
            //increases loading bar size for 10 seconds
            if (timer < 10)
            {
                timer += Time.deltaTime;
            }

            loadbar.size = timer / 10;

            //then allows player to progress when L is clicked
            if (loadbar.size == 1f)
            {
                loadText.text = "Press Mouse L";
                if (Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene("GAME-WORLD");
                }
            }

        }

    }
}
