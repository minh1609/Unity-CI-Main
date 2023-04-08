using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOver : MonoBehaviour
{
    [SerializeField] private FloatValue playerHealth;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private StringValue currentScene;
    private Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        anim = fadePanel.GetComponent<Animator>();
    }

    public void gameOver()
    {
        fadePanel.SetActive(true);
        anim.SetTrigger("activate");
        FindObjectOfType<AudioManager>().stopAllThemes();
        FindObjectOfType<AudioManager>().Play("Game Over Theme");
        playerHealth.RuntimeValue = playerHealth.initialValue;
        playerInventory.currentMagic = playerInventory.maxMagic;
        StartCoroutine(gameOverCo());
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            FindObjectOfType<AudioManager>().stopAllThemes();
            FindObjectOfType<AudioManager>().Play(currentScene.element + " Theme");
            SceneManager.LoadSceneAsync(currentScene.element);
        }
    }

    private IEnumerator gameOverCo()
    {
        yield return new WaitForSeconds(12f);
        FindObjectOfType<AudioManager>().stopAllThemes();
        FindObjectOfType<AudioManager>().Play(currentScene.element + " Theme");
        SceneManager.LoadSceneAsync(currentScene.element);
    }

}
