using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int EnemyCount;
    public GameObject enemy;
    void Start()
    {

        for(int i=0;i<EnemyCount;i++){
        Instantiate(enemy,new Vector3(Random.Range(-100,100),Random.Range(-100,100),Random.Range(-100,100)),Quaternion.identity);
    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
