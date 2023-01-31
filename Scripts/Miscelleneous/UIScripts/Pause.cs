using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{

    

    [SerializeField]
    GameObject pauseUIObject;

    [SerializeField]
    Button continueButton;

    [SerializeField]
    Button quitButton;

    [SerializeField]
    Button exitToDesktopButton;

    [HideInInspector]
    public bool isPaused = false;


    CanvasGroup canvasGroup;
    // Start is called before the first frame update
    private void Start()
    {
        pauseUIObject.SetActive(false);
        canvasGroup = pauseUIObject.GetComponent<CanvasGroup>();
        continueButton.onClick.AddListener(ResumeApp);
        quitButton.onClick.AddListener(MainController.Get().QuitToMainMenu);
        exitToDesktopButton.onClick.AddListener(MainController.Get().Shutdown);

    }

    public void Initialize()
    {
        if (isPaused)
            return;
        pauseUIObject.SetActive(true);
        canvasGroup.alpha = 0.0f;
        isPaused = true;
        Time.timeScale = 0.0f;
        SceneController.Get().ShowCursor();
    }


    private void Update()
    {
        switch (isPaused)
        {
            case true:
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1.0f, 0.1f);
                break;
            case false:
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0.0f, 0.1f);
                break;
        
        
        }

        if(!isPaused && canvasGroup.alpha < 0.01f)
            Exit();
        if (isPaused && canvasGroup.alpha > 0.99f)
            canvasGroup.alpha = 1.0f;



    }

    void Exit()
    {
        if (!isPaused)
            return;
        canvasGroup.alpha = 0.0f;
        pauseUIObject.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

        SceneController.Get().HideCursor();

    }

    public void ResumeApp()
    {
        Exit();
    }

    public void PauseApp()
    {
        Initialize();
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    static public Pause Get()
    {
        return GameObject.Find("Canvas").GetComponent<Pause>();
    }

}
