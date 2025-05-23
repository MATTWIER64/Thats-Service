using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class door : MonoBehaviour
{
    public string doorTo;
    public string loadPos;

    void Awake()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.GetComponent<PlayerController>().canLeave == true)
            {
                UnityEngine.Debug.Log("Loading: " + doorTo);
                GameObject player = GameObject.FindWithTag("Player");
                DontDestroyOnLoad(player);
                SceneManager.LoadScene("Assets/Scenes/ThatsService_Scene_Ship.unity");
            }
            else
            {
                UnityEngine.Debug.Log("You're in the middle of a shift!");
            }
            
            // Send the entrance direction to a manager, and call it loadPos
            // /\ Do this to save what door I used and where I should spawn in the new scene.
        }
    }

}
