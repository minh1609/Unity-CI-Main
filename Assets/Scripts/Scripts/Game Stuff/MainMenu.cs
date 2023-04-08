using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private StringValue currentScene;
    private string tempCurrentScene;
    // Start is called before the first frame update
    void Start()
    {
        if (currentScene.element == null || currentScene.element == "" || currentScene.element == "StartMenu")
        {
            currentScene.element = "Home";
        }
        tempCurrentScene = currentScene.element;
        currentScene.element = "StartMenu";
    }


    public void NewGame()
    {
        currentScene.element = tempCurrentScene;
        FindObjectOfType<AudioManager>().stopAllThemes();
        FindObjectOfType<AudioManager>().Play(currentScene.element + " Theme");
        SceneManager.LoadScene(currentScene.element);
    }

    public void QuitToDesktop()
    {
        currentScene.element = tempCurrentScene;
        Application.Quit();
    }
}
