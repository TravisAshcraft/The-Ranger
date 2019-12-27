using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> wayPoints;
   

    int waypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        wayPoints = waveConfig.GetWaypoints();
        transform.position = wayPoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
       
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        //tells the enemy where to go when game is started and how fast to move
        if (waypointIndex <= wayPoints.Count - 1)
        {
            var targetPostition = wayPoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position,
                targetPostition, movementThisFrame);

            //tells the enemy when target waypoint is reached moved to the next waypoint. 
            if (transform.position == targetPostition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
