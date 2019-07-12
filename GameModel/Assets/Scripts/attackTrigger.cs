using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) //create attack hitbox
    {
        if (col.isTrigger != true && col.CompareTag("Enemy")) //hitbox only detects enemies for now
        {
            col.SendMessageUpwards("Damage", 1); //sends message to enemy that damage was taken
        }

        if(col.isTrigger !=true && col.CompareTag("Player"))
        {
            col.SendMessageUpwards("DamagePlayer", 1);
            print("player hit");
        }
    }
}
