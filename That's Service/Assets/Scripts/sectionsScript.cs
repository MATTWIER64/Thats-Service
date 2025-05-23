using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sectionsScript : MonoBehaviour
{
    public Transform[] tables;
    public Transform[] bar;
    public GameObject tableSection;
    public GameObject barSection;
    public float cleanTime = 0f;
    public float resetTime = 0f;
    [SerializeField]
    private hostStand _hostStand;
    void Awake()
    {
        tables = new Transform[tableSection.transform.childCount];
        for (int i = 0; i < tables.Length; i++)
        {
            tables[i] = tableSection.transform.GetChild(i);
            tables[i].gameObject.GetComponent<TableScript>().tableNum = i;
        }
        bar = new Transform[barSection.transform.childCount];
        for (int j = 0; j < bar.Length;j++)
        {
            bar[j] = barSection.transform.GetChild(j);
            bar[j].gameObject.GetComponent<TableScript>().tableNum = j + 100;
        }
    }

    void Update()
    {
       if (_hostStand.autoClean == true)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                TableScript table = tables[i].gameObject.GetComponent<TableScript>();
                if (table.dirtyTimer > cleanTime && table.dirty == true)
                {
                    table.ProgressTable();
                }
                else if (table.reset == true && table.dirtyTimer > resetTime)
                {
                    table.ProgressTable();
                }
            }
        }
    }

}

