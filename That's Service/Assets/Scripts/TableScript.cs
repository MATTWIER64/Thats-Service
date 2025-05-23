using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TableScript : MonoBehaviour
{
    // add a timer that counts up whener the table is dirty, and de-activates when it is clean.
    // then send that "score" to a tip tracker on the player, to decide how much money will be made from that table.
    // have it take into account how many seats were at the table, and a bit of rng to determing how much they spent. 
    // 6 tops take most of the bus tub, but cleaning them quickly will award a bigger tip than cleaning up a few 2 tops.

    public bool dirty = false;
    public bool reset = false;
    public bool clean = true;
    public bool sat = false;
    public float dirtyTimer = 0f;
    public bool barSeat;
    public int tableNum;
    public int maxHeads = 10;
    public int minHeads = 1;
    private int headsAtTableStatic = 0;
    public int headsAtTable = 0;
    public Clock _clock;
    private PlayerScore _playerScore;
    private SpriteRenderer sR;
    public int endTime;
    public seatingScript _customers;
    private hostStand _hostStand;
    [SerializeField]
    private GameObject tableTipBoard;
    [SerializeField]
    Color myBrown = new Color(79f, 52f, 15f,1f);
    [SerializeField]
    Color myGreen = new Color (57,113,15);
    [SerializeField]
    Color myYellow = new Color(207, 181, 0);
    [SerializeField]
    Color myRed = new Color(188, 77, 26);
    void Awake()
    {
        _hostStand = GameObject.FindWithTag("RestaurantManager").GetComponent<hostStand>();
        _clock = GameObject.FindWithTag("Player").GetComponent<Clock>();
        sR = (SpriteRenderer)GetComponent<SpriteRenderer>();
        _customers = GameObject.FindWithTag("RestaurantManager").GetComponent<seatingScript>();
        _playerScore = GameObject.FindWithTag("Player").GetComponent<PlayerScore>();
    }

    int GetEndTime(int hours, int mins)
    {
        System.Random rng = new System.Random();
        int campTime = rng.Next(45, 90);
        mins += campTime;
        while (mins >= 60)
        {
            hours++;
            mins -= 60;           
        }
        int endTime = (hours * 100) + mins;
        return endTime;
    }


    void Update()
    {
        if (_clock.realTime >= endTime && sat == true)
        {
            FinishTable();
        }
        if (dirty == true || reset == true)
        {
            dirtyTimer += Time.deltaTime;
        }
        if (tableTipBoard != null && tableTipBoard.activeSelf)
        {
            dirtyTimer += Time.deltaTime;
            if (dirtyTimer >= 2.0f)
            {
                dirtyTimer = 0f;
            }
        }
    }

    public void ProgressTable()
    { 
        if (dirty == true)
        {
            ClearTable();
        }
        else if (reset == true)
        {
            ResetTable();
        }
    }
    public void SitTable(int heads)
    {
        if(clean == true)
        {
            headsAtTableStatic = heads;
            headsAtTable = heads;
            sat = true;
            clean = false;
            sR.color = myGreen;
        }
        endTime = GetEndTime(_clock.hours, _clock.mins);
    }
    void ClearTable()
    {
        dirty = false;
        reset = true;
        sR.color = myYellow;
    }
    void ResetTable()
    {
        reset = false;
        clean = true;
        sR.color = myBrown;
        if (tableNum < 100)
        {
            _hostStand.tableOpen = true;
        }
        else if (tableNum >= 100)
        {
            _hostStand.barOpen = true;
        }
        _customers.allOpen = _customers.CheckForAllOpen();
        if (_hostStand.autoClean != true)
        {
            int tipEarned = _playerScore.CalculateTipEarned(headsAtTableStatic, dirtyTimer);
            GameObject tipBoard = Instantiate(tableTipBoard, gameObject.transform);
            tipBoard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "$" + tipEarned.ToString();
            Destroy(tipBoard, 3.0f);
        }
        dirtyTimer = 0;
    }
    public void FinishTable()
    {
        sat = false;
        dirty = true;
        endTime = 0;
        sR.color = myRed;
        UnityEngine.Debug.Log("Table " + tableNum + " got up.");
    }
}
