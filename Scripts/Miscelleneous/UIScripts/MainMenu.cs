using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    [SerializeField]
    Button startButton;

    [SerializeField]
    Button tutorialButton;

    [SerializeField]
    Button quitButton;

    // Start is called before the first frame update
    void Start()
    {

        startButton.onClick.AddListener(SceneController.Get().ToGame);
        tutorialButton.onClick.AddListener(SceneController.Get().ToTutorial);
        quitButton.onClick.AddListener(SceneController.Get().Quit);

    }




}
