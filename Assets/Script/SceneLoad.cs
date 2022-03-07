using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoad : MonoBehaviour
{
    
    void Start()
    {
        
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
    void Update()
    {
        
    }
}
