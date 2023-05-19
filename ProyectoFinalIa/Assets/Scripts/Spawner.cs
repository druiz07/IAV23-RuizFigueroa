using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{

    public List<GameObject> enemies;


    public float minGeneration = 0.13f;
    public float maxGeneration = 0.2f;
    public float minStartingGeneration=2f;
    public float maxStartingGeneration=5f;

    float contador = 0f;
    float minTime;
    public float timeResta=2f;
    private void Awake()
    {
        minTime = Random.Range(minStartingGeneration, maxStartingGeneration);
        InvokeRepeating("RestaTime", 2f, 2f);
    }

    private void Update()
    {
        if (contador >= minTime)
        {
            SpawnEnemy();
            contador = 0f;
            minTime = Random.Range(minStartingGeneration, maxStartingGeneration);
        }
        else contador += Time.deltaTime;
    }

    void RestaTime()
    {
        if (minStartingGeneration > minGeneration) minStartingGeneration -= 0.015f;
        if (maxStartingGeneration > maxGeneration) maxStartingGeneration -= 0.03f;
    }

    void SpawnEnemy()
    {
        int x = Random.Range(0, 10);
        if (x < 3) GameManager.enemies.Add(Instantiate(enemies[(int)TipoEnemigo.RAPIDO], transform.position, transform.rotation));
        else if (x < 6) GameManager.enemies.Add(Instantiate(enemies[(int)TipoEnemigo.FUERTE], transform.position, transform.rotation));
        else GameManager.enemies.Add(Instantiate(enemies[(int)TipoEnemigo.DEFAULT], transform.position, transform.rotation));
    }
}
