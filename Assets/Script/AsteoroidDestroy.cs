using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteoroidDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Explosion;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        //print("exploded");
        if(other.gameObject.tag=="asteoroid")
        {
            return;
        }
        Destroy(this.gameObject);
        Instantiate(Explosion,transform.position,Quaternion.identity);
    }
}
