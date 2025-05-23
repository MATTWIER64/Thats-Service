using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class seatingScript : MonoBehaviour
{
    public sectionsScript _sections;
    private Transform[] tables;
    private Transform[] bar;
    public int[] sat;
    public int seatOdds = 15;
    private float seatTimerDelay = 1f;
    public hostStand _hostStand;
    private float seatTimer;
    private Clock _clock;
    public bool allOpen;

    System.Random rng = new System.Random();
    void Awake()
    {
        _clock = GetComponent<Clock>();
        tables = _sections.tables;
        bar = _sections.bar;
        _sections = GetComponent<sectionsScript>();
        sat = new int[_sections.tableSection.transform.childCount + _sections.barSection.transform.childCount];
        for (int i = 0; i <= rng.Next(7, 16); i++)
        {
            int heads = rng.Next(1, 5);
            int tableNum = CheckForOpenTable(heads);
            SeatTable(heads, tableNum);
            int status = rng.Next(0, 1);
            TableScript table = _sections.tables[tableNum].GetComponent<TableScript>();
            table.endTime -= 100;
            if (status == 1)
            {
                table.ProgressTable();
            }
            if (status == 2) {
                table.ProgressTable();
                table.ProgressTable();
            }
        }
    }
    public void Pop(int popStrngth)
    {
        int totalSat = 0;
        int heads;
        System.Random rng = new System.Random();
        for (int i = 0; i <= popStrngth; i++) 
        {
            heads = rng.Next(1, 5);
            _hostStand.NewTable(heads);
            totalSat++;
        }
    }

    public void SeatTable(int heads, int tableNum)
    {
        TableScript currentTable = _sections.tables[tableNum].GetComponent<TableScript>();
        currentTable.SitTable(heads);
        sat[currentTable.tableNum] = heads;
    }
    //public void SeatBar()
    //{
    //    int seating;
    //    while (true)
    //    {
    //        seating = (rng.Next(0, _sections.bar.Length));
    //        TableScript currentBar = _sections.bar[seating].GetComponent<TableScript>();
    //        if (currentBar.clean == false)
    //        {
    //            continue;
    //        }
    //        else
    //        {
    //            currentBar.SitTable(1);
    //            satBar[seating] = 1;
    //            break;
    //        }
    //    }
    //}

    public int CheckForOpenTable(int heads)
    {
        if (heads == 1) {
            for (int i = 0; i <= bar.Length - 1; i++) {
                TableScript current = bar[i].gameObject.GetComponent<TableScript>();
                if (current.clean == true) { return current.tableNum; }
            }
        }
        for (int i = 0; i <= tables.Length -1; i++)
        {
            TableScript currentTable = tables[i].gameObject.GetComponent<TableScript>();
            if (currentTable.clean == true && currentTable.maxHeads >= heads && currentTable.minHeads <= heads)
            {
                return currentTable.tableNum;
            }
        }
        return -1;
    }
    //public bool CheckForOpenBar()
    //{
    //    Transform[] seatedBar = _sections.bar;
    //    for (int i = 0; i <= seatedBar.Length - 1; i++)
    //    {
    //        if (seatedBar[i].gameObject.GetComponent<TableScript>().clean == true)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    void Update()
    {
        seatTimer = seatTimer + Time.deltaTime;
        if (seatTimer >= seatTimerDelay && rng.Next(1, 101) <= seatOdds)
        {
            _hostStand.NewTable(rng.Next(1, 5));
            seatTimer = 0f;
            UnityEngine.Debug.Log("Sent to host");
        }
        else if (seatTimer >= seatTimerDelay)
        {
            seatTimer = 0f;
        }
    }

    public bool CheckForAllOpen()
    {
        if (_hostStand._clock.realTime >= 915)
        {
            for (int i = 0; i <= sat.Length - 1; i++)
            {
                if (sat[i] != 0)
                {
                    return false;
                }
            }
            //for (int i = 0; i <= satBar.Length - 1; i++)
            //{
            //    if (satBar[i] != 0)
            //    {
            //        return false;
            //    }
            //}
            return true;
        }
        else // this function is to check to see if I can run end of day, so if it isn't past 9:15, it doesn't matter if all the tables are open
        {
            return false;
        }
    }


}
