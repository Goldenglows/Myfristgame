using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypointnew : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float speed = 2f;
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当玩家踏上平台时激活移动
        if (collision.gameObject.name == "Player")
        {
            isActive = true;
        }
    }


    private void Update()
    {
        // 只有激活状态才移动
        if (!isActive) return;

        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }


}

