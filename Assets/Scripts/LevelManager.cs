using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float timeToDisableTime = 0.5f;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    [SerializeField] private GameObject cinemachine1;
    [SerializeField] private GameObject cinemachine2;

    private EnemyHealth[] enemies;
    private PlayerHealth playerHealth;

    private int enemiesCount;
    private int deadEnemies;

    private int victoriesCount;
    private int defeatsCount;

    private void Awake()
    {
        victoriesCount = PlayerPrefs.GetInt(nameof(victoriesCount), 0);
        defeatsCount = PlayerPrefs.GetInt(nameof(defeatsCount), defeatsCount);
        //playerHealth = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        //enemies = (EnemyHealth[])FindObjectsOfType(typeof(EnemyHealth));
    }


    public void SetPlayer(GameObject player)
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            this.playerHealth = playerHealth;
            this.playerHealth.OnDeath += OnPlayerDeath;
        }
    }

    public void SetEnemies(EnemyHealth[] en)
    {
        if (en != null)
        {
            enemies = en;
            SubscribeToEnemiesDeaths(enemies);
        }
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath += OnPlayerDeath;
        }
        if (enemies != null)
        {
            SubscribeToEnemiesDeaths(enemies);
        }
    }

    private void OnDisable()
    {
        playerHealth.OnDeath -= OnPlayerDeath;
        UnsubscribeFromEnemiesDeaths(enemies);
    }

    private void SubscribeToEnemiesDeaths(EnemyHealth[] enemies)
    {
        enemiesCount = enemies.Length;
        foreach ( var enemy in enemies)
        {
            enemy.OnDeath += OnEnemyDeath;
        }
    }

    private void UnsubscribeFromEnemiesDeaths(EnemyHealth[] enemies)
    {
        enemiesCount = enemies.Length;
        foreach (var enemy in enemies)
        {
            enemy.OnDeath -= OnEnemyDeath;
        }
    }

    private void OnPlayerDeath(Vector3 pos)
    {
        defeatsCount++;
        SaveData();
        StartCoroutine(TimeDisablerCoroutine());
        SoundPool.SoundInstance.PlayVFXSound(loseSound, playerHealth.gameObject.transform.position);
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
            SoundPool.SoundInstance.PlayVFXSound(winSound, playerHealth.gameObject.transform.position);
            LevelUIController.Instance.ShowLevelEndUI("you win", victoriesCount, defeatsCount);
        }
    }

    private IEnumerator TimeDisablerCoroutine()
    {
        //Debug.Log("TimeDisablerCoroutine start");
        yield return new WaitForSeconds(timeToDisableTime);
        cinemachine1.SetActive(false);
        cinemachine2.SetActive(false);
        var playerController = playerHealth.GetComponent<PlayerController>();
        playerController.OnLevelEnd();
        //Debug.Log("TimeDisablerCoroutine end");
        Time.timeScale = 0;
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(nameof(victoriesCount), victoriesCount);
        PlayerPrefs.SetInt(nameof(defeatsCount), defeatsCount);
        PlayerPrefs.Save();
    }




}
