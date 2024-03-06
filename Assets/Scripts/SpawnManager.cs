using Assets.Scripts.Interfaces;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera defaultCamera;
    [SerializeField] private CinemachineVirtualCamera aimCamera;


    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnTransform;
    private Transform playerTransform;

    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private Transform[] turretsSpawnTransform;

    [SerializeField] private GameObject bugPrefab;
    [SerializeField] private Transform[] bugsSpawnTransform;


    [SerializeField] private LevelManager levelManager;

    private List<EnemyHealth> enemiesHealth;

    void Start()
    {
        enemiesHealth = new List<EnemyHealth>();
        PlayerSpawn();
        SpawnEnemy(turretPrefab, turretsSpawnTransform);
        SpawnEnemy(bugPrefab, bugsSpawnTransform);
        if (levelManager != null)
        {
            levelManager.SetEnemies(enemiesHealth.ToArray());
        }
    }

    private void PlayerSpawn()
    {
        if (playerPrefab == null || playerSpawnTransform == null)
        {
            return;
        }
        var player = Instantiate(playerPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);
        defaultCamera.Follow = player.transform;
        defaultCamera.LookAt= player.transform;
        aimCamera.Follow = player.transform;
        aimCamera.LookAt = player.transform;
        levelManager.SetPlayer(player);
        playerTransform = player.transform;
    }

    private void SpawnEnemy(GameObject prefab, Transform[] spawnTransforms )
    {
        if (prefab == null || spawnTransforms == null)
        {
            return;
        }
        foreach (var transform in spawnTransforms)
        {
            var turret = Instantiate(prefab, transform.position, transform.rotation);
            var health = turret.GetComponentInChildren<EnemyHealth>();
            if (health != null)
            {
                enemiesHealth.Add(health);
            }
            var detector = turret.GetComponentInChildren<IPlayerDetector>();
            if (detector != null && playerSpawnTransform != null)
            {
                detector.SetPlayer(playerTransform);
            }
        }
    }

}
