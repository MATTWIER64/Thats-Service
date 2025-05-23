using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageHolder : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> pages = new List<GameObject>();
    int pageNum = 0;
    [SerializeField]
    private GameObject arrowR;
    [SerializeField]
    private GameObject arrowL;
    public void NextPage()
    {
        pages[pageNum].SetActive(false);
        pageNum++;
        pages[pageNum].SetActive(true);
        arrowL.SetActive(true);
        if (pageNum >= pages.Count - 1)
        {
            arrowR.SetActive(false);
        }
    }
    public void PrevPage()
    {
        pages[pageNum].SetActive(false);
        pageNum--;
        pages[pageNum].SetActive(true);
        arrowR.SetActive(true);
        if (pageNum <= 0)
        {
            arrowL.SetActive(false);
        }
    }
}
