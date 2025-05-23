using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class hostStand : MonoBehaviour
{
    private Color clockBlue = new Color(82, 108, 168, 164);
    private Color clockYellow = new Color(255, 248, 0, 164);
    public List<int> waitList;
    public List<int> barWaitList;
    public seatingScript _seating;
    public PlayerScore _playerScore;
    public UIManager _ui;
    public Clock _clock;
    public bool tableOpen = true;
    public bool barOpen = true;
    public int bigTopOdds = 10;
    public int toBarOdds = 10;
    public float seatDelay;
    private float seatTimer;
    public bool autoClean = true;
    [SerializeField]
    private int closeTime = 845;
    [SerializeField]
    private GameObject clockPanel;
    private float timer;
    private bool firstRun;
    [SerializeField]
    private int shiftEndTime;
    [SerializeField]
    private int shiftEndDelay;

    System.Random rng = new System.Random();
    void Awake()
    {
        waitList = new List<int>();
        barWaitList = new List<int>();
        _clock = GameObject.FindWithTag("Player").GetComponent<Clock>();
        seatTimer = seatDelay;
    }
    void Update()
    {
        if (tableOpen == true || barOpen == true)
        {
            seatTimer += Time.deltaTime;
        }
        else { seatTimer = 0f; }
        if (seatTimer >= seatDelay && waitList.Count > 0 && tableOpen == true)
        {
            for (int i = 0; i < waitList.Count; i++)
            {
                int seating = _seating.CheckForOpenTable(waitList[i]);
                if (seating != -1)
                {
                    _seating.SeatTable(waitList[i], seating);
                    waitList.RemoveAt(i);
                    seatTimer = 0f;
                    UnityEngine.Debug.Log("Sat Table");
                    break;
                }
            }
        }
        //if (seatTimer >= seatDelay && barWaitList.Count > 0 && barOpen == true)
        //{
        //    UnityEngine.Debug.Log("Sat bar");
        //    _seating.SeatBar();
        //    barWaitList.Remove(barWaitList[0]);
        //    seatTimer = 0f;
        //    barOpen = _seating.CheckForOpenBar();
        //}
        if (_clock.realTime >= closeTime && waitList.Any() == false)
        {
            if (firstRun == true)
            {
                shiftEndTime = _clock.GetTimerEnd((0, 45));
                firstRun = false;
                clockPanel.GetComponent<Image>().color = clockYellow;
                _ui.PopUp(3f, "Shift ends at " + (shiftEndTime / 100).ToString() + ":" + (shiftEndTime % 100).ToString());
                //      FIXME
                //      Anything else
            }
            else if (_clock.realTime >= shiftEndTime && _playerScore._playerController.canLeave == false)
            {
                _playerScore.EndOfShift();
                clockPanel.GetComponent<Image>().color = clockBlue;
                shiftEndTime = 0;
            }
        }
        
    }

    public void NewTable(int heads)
    {
        if (_clock.realTime >= 400 && _clock.realTime <= closeTime)
        {
            if (rng.Next(1, 101) <= bigTopOdds)
            {
                waitList.Add(heads + 4);
            }
            else
            {
                waitList.Add(heads);
            }
        }
    }
    public void StartShift()
    {
        UnityEngine.Debug.Log("Shift started");
        _playerScore.inTime = _clock.realTime;
        autoClean = false;
        firstRun = true;
        shiftEndTime = 0;
        if (_clock.realTime < 845 && _clock.realTime >= 400 && _clock.am == false)
        {
            closeTime = 845;
        }
        else if (_clock.realTime >= 800 && _clock.realTime <= 230 && _clock.am == true)
        {
            closeTime = 230;
        }
    }
}
