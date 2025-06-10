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
        // �����̤��ƽ̨ʱ�����ƶ�
        if (collision.gameObject.name == "Player")
        {
            isActive = true;
        }
    }


    private void Update()
    {
        // ֻ�м���״̬���ƶ�
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

