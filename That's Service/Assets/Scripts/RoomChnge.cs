using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChnge : MonoBehaviour
{
    public GameObject mainCam;
    public string roomCode;
    
    void OnTriggerExit2D(Collider2D col)
    {
       if (col.gameObject.tag == "Player" && roomCode == "DiningRoom")
        {
            UnityEngine.Debug.Log("In kitchen");
            mainCam.transform.position = new Vector3(0, 0, -10);

        }
       if (col.gameObject.tag == "Player" && roomCode == "Kitchen")
        {
            UnityEngine.Debug.Log("In dining room");
            mainCam.transform.position = new Vector3(0, 10, -10);
        }
    }
}
