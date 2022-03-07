using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform player;
    public GameObject projectile;
    public Transform firePoint;
    public GameObject Explosion;
    public Transform playerFirePoint;
    public float seeAngle;
    public int count=10;
    public bool move;
    private float hSliderValue = 0.5f;
    private float fireCountdown = 0f;
    private List<Vector3> patrolPoints=new List<Vector3>();
    private int currentPatrolPos=0;
    public TMP_Text scoreTXT;
    //Double-click protection
    private float buttonSaver = 0f;
    void Start()
    {
        scoreTXT=GameObject.FindWithTag("Score").GetComponent<TMP_Text>();
        if(player==null)
        {
            player=Camera.main.transform;
        }
        if(playerFirePoint==null)
        {
           playerFirePoint=GameObject.FindWithTag("playerFirePoint").transform;
        }
        for(int i=0;i<10;i++)
        {
     patrolPoints.Add(new Vector3(Random.Range(-200,200),Random.Range(-200,200),Random.Range(-200,200)));
        }
    }
    public bool CanSeePlayer()
	{
		Vector3 direction = player.position - transform.position;
		float angle = Vector3.Angle(direction, transform.forward);

		if (angle < seeAngle)
		{
            Vector3 relativePos = player.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotation, Time.time * 0.1f);
            
			return true;
		}

		return false;
	}
    void Update()
    {
        if(CanSeePlayer())
        {
            Shoot();
        }
        fireCountdown -= Time.deltaTime;
        if (Vector3.Distance(transform.position,patrolPoints[currentPatrolPos]) < 0.01f)
        {
            currentPatrolPos++;
            if(currentPatrolPos>=patrolPoints.Count)
            {
                currentPatrolPos=0;
            }
             Vector3 relativePos =patrolPoints[currentPatrolPos] - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
         transform.rotation = rotation;
        }
        

        
       
       if(move)
       {
            transform.position = Vector3.MoveTowards(transform.position,patrolPoints[currentPatrolPos], 25.0f * Time.deltaTime);
        
           }//Instantiate(enemyPrefab);
    }
    public void Shoot()
    {
        if(playerFirePoint==null)
        {
            print("null");
            return;
        }
        Instantiate(projectile,firePoint.position,Quaternion.LookRotation(playerFirePoint.position - transform.position, Vector3.up));
        fireCountdown = 0;
        fireCountdown += hSliderValue;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="p")
        {
            PlayerCollision.score++;
            UpdateScore();
        }
        Destroy(this.gameObject);
        Instantiate(Explosion,transform.position,Quaternion.identity);
        Instantiate(enemy,new Vector3(Random.Range(-100,100),Random.Range(-100,100),Random.Range(-100,100)),Quaternion.identity);
    }
    private void UpdateScore()
    {
        scoreTXT.text="Score : "+PlayerCollision.score;
    }
}
