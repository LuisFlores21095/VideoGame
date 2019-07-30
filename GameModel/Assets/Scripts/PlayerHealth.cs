using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Slider HealthBar;
    public float Health = 100;

    private float currentHealth;

    private void Start()
    {
        currentHealth = Health;
    }

    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
        HealthBar.value = currentHealth;
        if(currentHealth <= 0)
        {
            SceneManager.LoadScene("mainMenu");
        }

    }
}
