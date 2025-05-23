using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private bussing _bussing;
    [SerializeField]
    private TextMeshProUGUI slotsFull;
    private int busIndex;

    public void MenuOpened()
    {
        busIndex = _bussing.busTubIndex;
        slotsFull.text = busIndex.ToString();
    }
}

