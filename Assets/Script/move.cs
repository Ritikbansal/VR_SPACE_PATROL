using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
 public Vector3 MousePosition;
 public float position;
 public float curShipSpeed = 0.0f;
 public int step = 2;
 public int current_speed = 0;
 public int maxSpeed =100;
  
 public float mouse_rotate = 0.5f;
 public int agility = 30;
 public float dist_fr_ship = 2.85f;
 public float rotate_speed = 2.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey ("d")) 
         {
             transform.Rotate(0,0,-rotate_speed);
         }
 
         if (Input.GetKey ("a")) 
         {
             transform.Rotate(0,0,rotate_speed);
         }
         if ((Input.GetKey("up") || Input.GetKey("w")) && (current_speed < maxSpeed)) {
         current_speed += step;
     } else if ((Input.GetKey("down") || Input.GetKey("s"))  && (current_speed > 0)) {
         current_speed -= step;
     }
 
         MousePosition = Input.mousePosition;
         MousePosition.x = (Screen.height/2) - Input.mousePosition.y;
         MousePosition.y = -(Screen.width/2) + Input.mousePosition.x;
         transform.Rotate(MousePosition * Time.deltaTime * mouse_rotate, Space.Self);
 
     curShipSpeed = Mathf.Lerp(curShipSpeed, current_speed, Time.deltaTime * step);
     transform.position += transform.TransformDirection(Vector3.forward) * curShipSpeed * Time.deltaTime;
    }
}
