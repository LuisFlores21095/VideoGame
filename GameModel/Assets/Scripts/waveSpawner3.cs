using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveSpawner3 : MonoBehaviour
{

    [System.Serializable]// array of waves
    public class Wave //wave class and properties for Unity
    {
        public string name;
        public Transform[] enemies;
        public int count;
        public float delay;

    }

    public enum SpawnState { spawning, waiting, counting, boss, end };
    public Wave[] waves;
    public float timeBetween = 5f;
    public SpawnState state = SpawnState.counting;
    public Transform[] spawnPoints;
    public Transform bossSpawnPoint;
    public Transform miniBossSP1;
    public Transform miniBossSP2;
    public Transform bossEnemy;
    public Transform miniBEnemy1;
    public Transform miniBEnemy2;

    public GameObject nextArrow;
    public GameObject textBox;
    public Animator animator;

    int nextWave = 0;
    float waveCountdown;
    float searchCountdown = 1f;
    float textBoxCountdown = 5f;
    bool cutscene = false;

    void Start()
    {
        waveCountdown = timeBetween;
        nextArrow.SetActive(false);
        textBox.SetActive(true);
    }


    void Update()
    {
        if (textBoxCountdown > 0)
        {
            textBoxCountdown -= Time.deltaTime;
        }
        else
        {
            textBox.SetActive(false);
        }



        if (state == SpawnState.boss)
        {
            if (!EnemyIsAlive())
            {
                nextArrow.SetActive(true);
            }
            else
            {
                return;
            }
        }
        if (state == SpawnState.waiting)// waiting for player to kill enemies
        {
            if (!EnemyIsAlive()) //if no enemies alive
            {
                Debug.Log("Wave Complete.");
                state = SpawnState.counting;
                waveCountdown = timeBetween;
                if (nextWave + 1 > waves.Length - 1)// waves ended, spawn boss
                {
                    textBoxCountdown = 10f;
                    textBox.SetActive(true);
                    animator.SetTrigger("Switch");
                    SpawnBoss(bossEnemy, miniBEnemy1, miniBEnemy2);
                    state = SpawnState.boss;
                    nextWave = 0;
                }
                else
                {
                    nextWave++;
                }
                return;
            }
            else
            {
                return; //do nothing if enemies still alive
            }
        }
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.spawning)//if hasnt spawned yet
            {
                StartCoroutine(SpawnWave(waves[nextWave]));

            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave w)
    {
        Debug.Log("Spawning Wave: " + w.name);
        state = SpawnState.spawning; // enemy spawning begins
        for (int i = 0; i < w.count; i++) // for count in wave w
        {
            Transform en = w.enemies[Random.Range(0, w.enemies.Length)];
            SpawnEnemy(en);
            yield return new WaitForSeconds(w.delay); //spawns enemy and delays
        }
        state = SpawnState.waiting; //start waiting
        yield break;
    }

    void SpawnEnemy(Transform enemy)
    {
        Debug.Log("Spawning Enemy: " + enemy.name);
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, sp.position, sp.rotation);
    }

    void SpawnBoss(Transform boss, Transform minib1, Transform minib2)
    {
        Debug.Log("Spawning Enemy: " + boss.name);
        Transform sp = bossSpawnPoint;
        Instantiate(boss, sp.position, sp.rotation);

        Debug.Log("Spawning Enemy: " + minib1.name);
        Transform sp2 = miniBossSP1;
        Instantiate(minib1, sp2.position, sp2.rotation);

        Debug.Log("Spawning Enemy: " + minib2.name);
        Transform sp3 = miniBossSP2;
        Instantiate(minib2, sp3.position, sp3.rotation);
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0) //searching for enemy after a timer
        {
            searchCountdown = 1f; //reset timer to 1 sec
            if (GameObject.FindGameObjectWithTag("Enemy") == null) //if objects with tag enemy exist
            {
                return false;
            }
        }
        return true;
    }
}
