using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private UIActions uiActions;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject scoreText;

    public bool MenuOpen { get; set; }

    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        menu.SetActive(false);
    }

    private void Awake()
    {
        uiActions = new UIActions();
        uiActions.Actions.OpenMenu.performed += cxt => OpenCloseMenu();         //To assign function when escape is pressed
    }

    private void OpenCloseMenu()
    {
        menu.SetActive(!menu.activeSelf);
        MenuOpen = menu.activeSelf;
        if (MenuOpen)
        {
            scoreText.SetActive(false);
            Time.timeScale = 0f;                   //To pause the scene and audio when menu open
            AudioListener.pause = true;
        }
        else
        {
            scoreText.SetActive(true);
            Time.timeScale = 1f;                  //To unpause(resume) the scene and audio when menu closed
            AudioListener.pause = false;
        }

    }

    public void Continue()
    {
        menu.SetActive(false);
        MenuOpen = menu.activeSelf;
        scoreText.SetActive(true);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void Restart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        uiActions.Enable();
    }
    private void OnDisable()
    {
        uiActions.Disable();
    }
}
