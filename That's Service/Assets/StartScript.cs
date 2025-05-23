using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public GameObject player; 
    public void loadGame() {
        Time.timeScale = 1f;
        //GameObject playerCharacter = Instantiate(player);
        //Object.DontDestroyOnLoad(playerCharacter);
        SceneManager.LoadScene("ThatsService_Scene_Restaurant");
    }
    public void loadHome() {
        SceneManager.LoadScene("Menu");

    }
    public void QuitGame() {
        Application.Quit();
    }
}
