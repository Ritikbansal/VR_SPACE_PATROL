using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CycleShips : MonoBehaviour
{
    public static int planeSelect=0;
    public GameObject[] Prefabs;
    public Image[] Fill;
    public int Prefab=0;
    void Start()
    {
        Counter(0);
        
    }
    public void OnClickIncrease()
    {
        Counter(1);
    }
    public void OnClickDecrease()
    {
        Counter(-1);
    }
    void SetActive()
    {
        foreach(GameObject g in Prefabs)
        {
            g.SetActive(false);
        }
        Prefabs[Prefab].SetActive(true);
        if(Prefab==0)
        {
            Fill[0].fillAmount=0.25f;
            Fill[1].fillAmount=0.50f;
            Fill[2].fillAmount=0.75f;
            Fill[3].fillAmount=0.0f;
        }else if(Prefab==1)
        {
            Fill[0].fillAmount=0.75f;
            Fill[1].fillAmount=0.20f;
            Fill[2].fillAmount=1.00f;
            Fill[3].fillAmount=0.50f;
        }else if(Prefab==2)
        {
            Fill[0].fillAmount=1.00f;
            Fill[1].fillAmount=0.00f;
            Fill[2].fillAmount=0.75f;
            Fill[3].fillAmount=1.0f;
        }else if(Prefab==3)
        {
            Fill[0].fillAmount=1.00f;
            Fill[1].fillAmount=1.00f;
            Fill[2].fillAmount=1.00f;
            Fill[3].fillAmount=1.00f;
        }
        planeSelect=Prefab;
    }
    void Counter(int count)
    {
        Prefab += count;
        if (Prefab > Prefabs.Length - 1)
        {
            Prefab = 0;
        }
        else if (Prefab < 0)
        {
            Prefab = Prefabs.Length - 1;
        } SetActive();
    }
    void Update()
    {
        
    }
}
