using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public seatingScript _pops;

    public int mins = 00;
    public int hours = 4;

    public int realTime;
    private bool pop1Active;
    private bool pop2Active;
    private bool pop3Active;
    private float time;
    public bool am;

    System.Random rng = new System.Random();


    public int pop1;//4:30 - 5:00
    public int pop2;//6:00 - 7:00
    public int pop3;//8:00 - 9:00
    public int minStrngth = 5;
    public int maxStrngth = 13;
    private int pop1End;
    private int pop2End;
    private int pop3End;

    void Awake()//connect to a clock here with the hours and then a single digit to find the minutes. (114 = 11:40)
    {
        _pops = GameObject.FindWithTag("RestaurantManager").GetComponent<seatingScript>();
        if (mins < 10)
        {
            timeText.text = (hours + ":" + "0" + mins);
        }
        else
        {
            timeText.text = (hours + ":" + mins);
        }
        pop1 = rng.Next(43, 47) * 10;
        pop2 = rng.Next(60, 64) * 10;
        pop3 = rng.Next(80, 84) * 10;
        //pop1End = _pops.PopCalc(pop1);
        //pop2End = _pops.PopCalc(pop2);
        //pop3End = _pops.PopCalc(pop3);
        //UnityEngine.Debug.Log("Pop1:" + pop1 + " - " + pop1End + "\nPop2:" + pop2 + " - " + pop2End + "\nPop3:" + pop3 + " - " + pop3End);

    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 10)
        {
            time = 0;
            mins += 5;
            if (mins >= 60)
            {
                hours++;
                if (hours > 11 && am == true)
                {
                    am = false;
                }
                else if (hours > 11 && am == false)
                {
                    am = true;
                }
                mins = 0;
            }
            if (mins < 10)
            {
                timeText.text = (hours + ":" + "0" + mins);
            }
            else
            {
                timeText.text = (hours + ":" + mins);
            }
        }
        if (hours > 12)
        {
            hours -= 12;
        }
        realTime = (hours*100 )+ mins;
        if (realTime >= pop1 && pop1Active != true)
        {
            pop1Active = true;
            _pops.Pop(rng.Next(minStrngth, maxStrngth));
        }
        if (realTime >= pop2 && pop2Active != true)
        {
            pop2Active = true;
            _pops.Pop(rng.Next(minStrngth, maxStrngth));
        }
        if(realTime >= pop3 && pop3Active != true)
        {
            pop3Active = true;
            _pops.Pop(rng.Next(minStrngth, maxStrngth));
        }

    }
    public int GetTimerEnd((int lengthHours, int lengthMins) timer)
    {
        int current = realTime;
        int mins = current % 100;
        int minsFinal = mins + timer.lengthMins;
        int hoursFinal = ((current - mins) / 100) + timer.lengthHours;
        UnityEngine.Debug.Log(hoursFinal.ToString());
        UnityEngine.Debug.Log(minsFinal.ToString());
        if (minsFinal >= 60)
        {
            for (int i = minsFinal; i >= 60; i -= 60)
            {
                hoursFinal++;
                if (hoursFinal > 12)
                {
                    hoursFinal -= 12;
                }
                minsFinal -= 60;
            }
        }
        int final = (hoursFinal * 100) + minsFinal;
        UnityEngine.Debug.Log("End time is: " + final.ToString());
        return final;
    }
}
