using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused;
    public GameObject pausePanel;
    public GameObject text;
    public string mainMenu;
    [SerializeField] private GameObject resumeButton;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        text = GameObject.FindGameObjectWithTag("Player UI").transform.Find("PlaceText").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("pause"))
        {
            ChangePause();
        }
        if (Input.GetKeyDown(KeyCode.I))
            pausePanel.SetActive(false);
    }

    public void ChangePause()
    {
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        isPaused = !isPaused;
        if (isPaused)
        {
            if (text.activeInHierarchy)
                text.SetActive(false);
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }

        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitToMain()
    {
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }
}
