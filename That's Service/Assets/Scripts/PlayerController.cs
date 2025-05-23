using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Specialized;
using System;

public class PlayerController : MonoBehaviour
{
    public int cashOnHand = 0;
    public bool canLeave = true;
    public GameObject unloadTimerUI;
    [SerializeField]
    private GameObject TextBubble;
    [SerializeField]
    private TextMeshProUGUI speechText;
    public float moveSpeed;
    public float bussingLoadDelay = 1f;
    public float bussingUnloadDelay = 1f;
    public float resetingLoadDelay = 1f;
    public float resetingUnloadDelay = 1f;
    private float unloadDelay = 5f;
    private float loadDelay = 5f;
    public float timer;
    private Rigidbody2D rb;
    private UIManager _uiManager;
    private bussing _bussing;
    public int atTable;
    private bool bussing;
    private bool reseting;
    [SerializeField]
    private sectionsScript _sectionsScript;
    [SerializeField]
    private seatingScript _seatingScript;
    [SerializeField]
    private hostStand _hostStand;
    public GameObject busBar;
    public Image actionBar;
    public SpriteRenderer sR;
    private List<GameObject> tablesInReach = new List<GameObject>();
    private bool busInReach;
    private bool resetInReach;
    private bool inDish;

    [SerializeField]
    Color myRed = new Color(188, 77, 26);
    [SerializeField]
    Color myYellow = new Color(207, 181, 0);
    [SerializeField]
    public Color myWhite = new Color(255, 255, 255);

    void Awake()
    {
        sR = gameObject.GetComponent<SpriteRenderer>();
        _seatingScript = GameObject.FindWithTag("RestaurantManager").GetComponent<seatingScript>();
        _bussing = GetComponent<bussing>();
        rb = GetComponent<Rigidbody2D>();
        _uiManager = GetComponent<UIManager>();
        _sectionsScript = GameObject.FindWithTag("RestaurantManager").GetComponent<sectionsScript>();
        timer = unloadDelay;
        actionBar = busBar.transform.GetChild(2).GetComponent<Image>();
    }
    void Update()
    {
        if (inDish == true && Input.GetKey("space") && _bussing.busTubIndex != 0)
        {
            if (busBar.activeSelf != true)
            {
                busBar.SetActive(true);
            }
            timer -= Time.deltaTime;
            actionBar.fillAmount = timer / unloadDelay;
            if (timer < 0f)
            {
                timer = unloadDelay;
                UnityEngine.Debug.Log("Emptied busTubIndex " + _bussing.busTubIndex);
                _bussing.EmptyBusTub();
            }

        }
        if (Input.GetKeyUp("space") && inDish == true)
        {
            busBar.SetActive(false);
            timer = unloadDelay;
        }
        if (Input.GetKey("space"))
        {
            if (atTable != -1)
            {
                if (bussing == true && _sectionsScript.tables[atTable].GetComponent<TableScript>().dirty == true && _bussing.busTubIndex <8)
                {
                    BusSeat();
                }
                else if (reseting == true && _sectionsScript.tables[atTable].GetComponent<TableScript>().reset == true)
                {
                    BusSeat();
                }
            }
            if (atTable == -1 && inDish == false)
            {
                timer = 0f;
                busBar.SetActive(false);
            }
        }    
        if (Input.GetKeyUp("space") && inDish != true)
        {
            timer = 0f;
            busBar.SetActive(false);
        }
        if (Input.GetKeyDown("space") && canLeave == false)
        {
            if (busInReach == true)
            {
                StartBussing();
            }
            else if (resetInReach == true)
            {
                if (_bussing.busTubIndex != 0)
                {
                    UnityEngine.Debug.Log("You can't start reseting tables before emptying your bustub!");
                    _uiManager.PopUp(3f, "You can't start reseting tables before emptying your bustub!");
                }
                else
                {
                    StartReseting();
                }
            }
        }
        if (Input.GetKeyDown("right shift"))
        {
            if (_uiManager.pauseMenu.activeSelf == false)
            {
                _uiManager.OpenPauseMenu();
            }
            else if (_uiManager.pauseMenu.activeSelf == true)
            {
                _uiManager.ClosePauseMenu();
            }
        }
    }
    void FixedUpdate()
    {
        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        Vector3 moveInput = rb.position + (new Vector2(horiInput, vertInput) * moveSpeed * Time.deltaTime);
        rb.MovePosition(moveInput);
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Item")
        {
            string itemName = col.gameObject.GetComponent<item>().itemName;
            if (itemName == "ClockIn" && Input.GetKey("space") && TextBubble.activeSelf == true && canLeave == true)
            {
                canLeave = false;
                TextBubble.SetActive(false);
                _hostStand.StartShift();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Table")
        {
            tablesInReach.Add(col.gameObject);
            atTable = CheckClosestObject(tablesInReach);
        }

        if (col.gameObject.tag == "Item")
        {
            string itemName = col.gameObject.GetComponent<item>().itemName;
            if (itemName == "bus")
            {
                busInReach = true;
            }
            else if (itemName == "ClockIn" && canLeave == true)
            {
                TextBubble.SetActive(true);
            }
            else if (itemName == "reset")
            {
                resetInReach = true;
            }

            else if (itemName == "dish")
            {
                inDish = true;
                timer = unloadDelay;
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Table")
        {
            tablesInReach.Remove(col.gameObject);
            atTable = CheckClosestObject(tablesInReach);
        }
        if (col.gameObject.tag == "Item")
        {
            string itemName = col.gameObject.GetComponent<item>().itemName;
            if (itemName == "ClockIn")
            {
                TextBubble.SetActive(false);
            }
            if (itemName == "bus")
            {
                busInReach = false;
            }
            else if (itemName == "reset")
            {
                resetInReach = false;
            }

            else if (itemName == "dish")
            {
                inDish = false;
                timer = 0f;
            }
        }

    }
    void StartBussing()
    {
        bussing = true;
        reseting = false;
        sR.color = myRed;
        loadDelay = bussingLoadDelay;
        unloadDelay = bussingUnloadDelay;
    }
    void StartReseting()
    {
        reseting = true;
        bussing = false;
        sR.color = myYellow;
        loadDelay = resetingLoadDelay;
        unloadDelay = resetingUnloadDelay;
    }
    void BusSeat()
    {
        if (busBar.activeSelf != true && bussing == true)
        {
            busBar.SetActive(true);
            actionBar.color = myRed;
        }
        else if (busBar.activeSelf != true && reseting == true)
        {
            busBar.SetActive(true);
            actionBar.color = myYellow;
        }
        bool complete = false;
        TableScript table = _sectionsScript.tables[atTable].GetComponent<TableScript>();
        timer += Time.deltaTime;
        actionBar.fillAmount = timer / loadDelay;
        if (timer >= loadDelay && bussing == true)
        {
            complete = _bussing.ClearSeat(table);
            timer = 0f;
        }
        else if (timer >= loadDelay && reseting == true)
        {
            timer = 0f;
            complete = true;
        }
        if (complete == true)
        {
            _sectionsScript.tables[atTable].gameObject.GetComponent<TableScript>().ProgressTable();
        }
    }
    int CheckClosestObject(List<GameObject> objects)
    {
        if (objects.Count > 1)
        {
            Vector3 myPos = this.transform.position;
            float dist = Vector3.Distance(myPos, objects[0].transform.position);
            GameObject closest = objects[0];
            for (int i = 0; i < objects.Count; i++)
            {
                float newDist = Vector3.Distance(myPos, objects[i].transform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    closest = objects[i];
                }
            }
            return closest.GetComponent<TableScript>().tableNum;
        }
        else if (objects.Count == 1) { return objects[0].GetComponent<TableScript>().tableNum; }
        else
        {
            return -1;
        }
    }
    public void ClockOut(int pay) {
        reseting = false;
        bussing = false;
        canLeave = true;
        _bussing.EmptyBusTub(_bussing.busTubIndex);
        cashOnHand += pay;
        sR.color = myWhite;
        _hostStand.autoClean = true;
    }
}
