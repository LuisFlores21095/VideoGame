using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour
{
    public int damage = 1; //setting damage to 1, doesnt do anything right now

    void OnTriggerEnter2D(Collider2D col) //create attack hitbox
    {
        if (col.isTrigger != true && col.CompareTag("Enemy")) //hitbox only detects enemies for now
        {
            col.SendMessageUpwards("Damage", damage); //sends message to enemy that damage was taken
        }
    }
}
