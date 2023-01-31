using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{



    [SerializeField]
    GameObject fadeInPanel;

    AsyncOperation loadSceneAsync;
    CanvasGroup cg;
    float targetAlpha = 0.0f;


    static public SceneController Get()
    {
        return GameObject.Find("Canvas").GetComponent<SceneController>();
    }

    private void Start()
    {
        fadeInPanel.SetActive(true);
        cg = fadeInPanel.GetComponent<CanvasGroup>();
        cg.alpha = 0.99f;
        
    }

    private void Update()
    {

        cg.alpha = Mathf.Lerp(cg.alpha, targetAlpha, 0.1f);


        if (Mathf.Abs(cg.alpha - targetAlpha) < 0.001f)
            cg.alpha = targetAlpha;



        if (cg.alpha == 1.0f) 
            StartCoroutine(BeginSceneLoading());

        if(cg.alpha == 0.0f)
            fadeInPanel.SetActive(false);

    }


    public void ToGame()
    {
        MainController.Get().PauseAllAudio();
        loadSceneAsync = SceneManager.LoadSceneAsync("GameScene");
        targetAlpha = 1.0f;
        fadeInPanel.SetActive(true);
    }

    public void ToTutorial()
    {
        MainController.Get().PauseAllAudio();
        loadSceneAsync = SceneManager.LoadSceneAsync("TutorialScene");
        targetAlpha = 1.0f;
        fadeInPanel.SetActive(true);
    }

    public void ToMainMenu()
    {
        MainController.Get().PauseAllAudio();
        loadSceneAsync = SceneManager.LoadSceneAsync("MainMenu");
        targetAlpha = 1.0f;
        fadeInPanel.SetActive(true);
    }

    public void Quit()
    {
        MainController.Get().PauseAllAudio();
        Application.Quit();
    }

    IEnumerator BeginSceneLoading()
    {
        if(loadSceneAsync != null)
        {
            while (!loadSceneAsync.isDone)
                yield return null;
        }
    }

}
