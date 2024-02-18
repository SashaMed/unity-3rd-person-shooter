using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject[] enemies;
    private PlayerHealth player;

    private int enemiesCount;
    private int deadEnemies;

    private int victoriesCount;
    private int defeatsCount;

    private void Awake()
    {
        victoriesCount = PlayerPrefs.GetInt(nameof(victoriesCount), 0);
        defeatsCount = PlayerPrefs.GetInt(nameof(defeatsCount), defeatsCount);
        player = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        player.OnDeath += OnPlayerDeath;
        var enemies = (EnemyHealth[])FindObjectsOfType(typeof(EnemyHealth));
        SubscribeToEnemiesDeaths(enemies);
    }

    private void SubscribeToEnemiesDeaths(EnemyHealth[] enemies)
    {
        enemiesCount = enemies.Length;
        foreach ( var enemy in enemies)
        {
            enemy.OnDeath += OnEnemyDeath;
        }
    }

    private void OnPlayerDeath(Vector3 pos)
    {
        defeatsCount++;
        SaveData();
        LevelUIController.Instance.ShowLevelEndUI("you lose", victoriesCount, defeatsCount);
    }

    private void OnEnemyDeath(Vector3 pos)
    {
        deadEnemies++;
        if (deadEnemies == enemiesCount)
        {
            victoriesCount++;
            SaveData();
            StartCoroutine(TimeDisablerCoroutine());
            LevelUIController.Instance.ShowLevelEndUI("you win", victoriesCount, defeatsCount);
        }
    }

    private IEnumerator TimeDisablerCoroutine()
    {

        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(nameof(victoriesCount), victoriesCount);
        PlayerPrefs.SetInt(nameof(defeatsCount), defeatsCount);
        PlayerPrefs.Save();
    }




}
