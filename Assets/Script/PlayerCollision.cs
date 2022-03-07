using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerCollision : MonoBehaviour
{
    public GameObject Explosion;
    public AudioSource hit;
    public Image shield;
    public Image health;
    public static float Health=1.0f;
    public static float Shield=1.0f;
    public static int score=0;
    void Start()
    {
        if(shield==null)
        {
            print("null");
        }
        shield.fillAmount = 1.0f;
        health.fillAmount = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag=="asteoroid")
        {
            Destroy(this.gameObject);
            Instantiate(Explosion);
            return;
        }else if(other.gameObject.tag=="enemy")
        {
             Destroy(this.gameObject);
            Instantiate(Explosion);
            return;
            
        }
        if(Shield>0)
        {
            Shield-=0.0025f;
            shield.fillAmount = Shield;
        }else
        {
            Health-=0.0025f;
            health.fillAmount = Health;
            

            if(Health<0)
            {
                Destroy(this.gameObject);
                Instantiate(Explosion,transform.position+new Vector3(0,0,5.0f),Quaternion.identity);
                 SceneManager.LoadScene(3);
                 //Invoke("loadScene", 2.0f);
            }
        }
        
        print(Health);
        hit.Play();
    }
    void loadScene()
    {
        
       
       
    }
}
