using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bussing : MonoBehaviour
{
    public TextMeshProUGUI busTubCountUI;
    public bool[] busTub = new bool[8];
    public int busTubIndex = 0;


    public bool ClearSeat(TableScript table)
    {
        busTub[busTubIndex] = true;
        busTubIndex++;
        table.headsAtTable--;
        busTubCountUI.text = busTubIndex.ToString();
        if (table.headsAtTable == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void EmptyBusTub(int times = 1)
    {
        for (int i = times; i > 0; i--) {
            if (busTub[0] == false)
            {
                return;
            }
            busTub[busTubIndex - 1] = false;
            busTubIndex--;
            busTubCountUI.text = busTubIndex.ToString();
        }
    }
}
