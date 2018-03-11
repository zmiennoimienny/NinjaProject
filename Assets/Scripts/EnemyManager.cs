using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    EnemyAI[] Enemies;
    Transform player;

    private void Awake()
    {
        GetPlayerTransform();
        GetEnemies();
    }

    private void Update()
    {
        UpdateEnemies();   
    }


    void UpdateEnemies()
    {
        foreach(EnemyAI enemy in Enemies)
        {
            enemy.UpdateEnemy(player);
        }
    }

    void GetPlayerTransform()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	void GetEnemies()
    {
        GameObject[] EnemiesGameobjects = GameObject.FindGameObjectsWithTag("Enemy");
        Enemies = new EnemyAI[EnemiesGameobjects.Length];
        int i = 0;
        foreach(GameObject ob in EnemiesGameobjects)
        {
            Enemies[i] = ob.GetComponent<EnemyAI>();
            i++;
        }
    }
}
