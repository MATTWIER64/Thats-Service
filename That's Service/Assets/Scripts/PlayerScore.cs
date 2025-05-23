using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI tipOut;
    public TextMeshProUGUI tablesCleaned;
    public TextMeshProUGUI avgCleanTime;
    public TextMeshProUGUI tipsLost;
    public GameObject tableTip;

    public int inTime = 0;
    [SerializeField]
    private GameObject diningRoomCanvas;
    private int tablesCleanedCount;
    private float tipsLostCount;
    [SerializeField]
    private GameObject endOfDayReport;
    private List<float> busTimes;
    public int playerScore;
    private float tipScored;
    public int[] fancyOdds;
    public PlayerController _playerController;
    private float popUpTimer;
    private List<GameObject> tipBoards;
    System.Random rng = new System.Random();

    void Awake()
    {
        tipBoards = new List<GameObject>();
        playerScore = 0;
        fancyOdds = new int[] { 10, 20, 25, 30, 35, 40, 50, 60 };
        busTimes = new List<float>();
        _playerController = this.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (endOfDayReport.activeSelf == true && Input.GetKeyDown("space"))
        {
            CloseEndOfDayReport();
        }
    }
    public int CalculateTipEarned(int heads, float dirtyTimer)
    {
        float penalty;
        if (dirtyTimer > 120f)
        {
            penalty = 1f + ((dirtyTimer - 120f) * .0083f);
        }
        else
        {
            penalty = 0f;
        }
        for (int i = 0; i < fancyOdds.Length; i++)
        {
            if (i == (heads - 1) && rng.Next(1,101) <= fancyOdds[i])
            {
                tipScored = ((Convert.ToSingle(heads * rng.Next(50, 66))* .02f));
                UnityEngine.Debug.Log("They were heavy spenders!");
                break;
            }
            else if (i == (heads - 1))
            {
                tipScored = (Convert.ToSingle(heads * rng.Next(35,46) * .02f));
                break;
            }
        }
        int tip = Convert.ToInt32(tipScored - penalty);
        playerScore += tip;
        busTimes.Add(dirtyTimer);
        tipsLostCount += penalty;
        if (tip > 0) {TipBoardPopUp("+" + Convert.ToString(tip)); }
        else{TipBoardPopUp("-" + Convert.ToString(tip)); }
        return playerScore;
    }

    private void TipBoardPopUp(string text) {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject tipBoard = Instantiate(tableTip, diningRoomCanvas.transform);
        tipBoard.transform.position = pos;
        tipBoard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        Destroy(tipBoard, 2f);
    }

    public void EndOfShift()
    {
        Time.timeScale = 0f;
        float total = 0f;
        for (int i = 0; i < busTimes.Count; i++)
        {
            total += busTimes[i];
        }
        avgCleanTime.text = $"{Math.Round(total / busTimes.Count, 2).ToString()} seconds";
        tablesCleaned.text = busTimes.Count.ToString();
        tipOut.text = $"${playerScore.ToString()}";
        tipsLost.text = $"${Math.Round(tipsLostCount, 2).ToString()}";
        endOfDayReport.SetActive(true);
    }
    public void CloseEndOfDayReport()
    {
        endOfDayReport.SetActive(false);
        Time.timeScale = 1f;
        _playerController.ClockOut(playerScore);
        playerScore = 0;
    }
}
