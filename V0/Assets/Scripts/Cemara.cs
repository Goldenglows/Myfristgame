using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cemara : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x,player.position.y,transform.position.z);
    }
}
