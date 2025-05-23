using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject helpMenu;
    private PauseMenu _pauseMenuScript;
    public GameObject popUp;
    [SerializeField]
    private TextMeshProUGUI popUpText;
    private float timer;
    private float popUpEnd;
    void Awake()
    {
        timer = 0f;
        _pauseMenuScript = pauseMenu.GetComponent<PauseMenu>();
        PopUp(3f, "Press right shift to pause");
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= popUpEnd)
        {
            PopUp(0f);
            popUpEnd = 0f;
        }
    }
    public void PopUp(float duration = 3f, string txt = "")
    {
        if (duration != 0f)
        {
            popUpText.text = txt;
            popUp.SetActive(true);
            popUpEnd = duration;
            timer = 0f;
        }
        else
        {
            popUp.SetActive(false);
        }
    }
    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        UnityEngine.Debug.Log("Open bus bar");
        pauseMenu.SetActive(true);
        _pauseMenuScript.MenuOpened();
    }
    public void ClosePauseMenu() 
    {
        Time.timeScale = 1f;
        UnityEngine.Debug.Log("Close bus bar");
        pauseMenu.SetActive(false);
        helpMenu.SetActive(false);
    }
    public void OpenHelpMenu()
    {
        helpMenu.SetActive(true);
    }
}
