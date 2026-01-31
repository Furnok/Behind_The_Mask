using System.Collections.Generic;
using UnityEngine;

public class S_RandomSpawnKeyPoint : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField] GameObject _keyPointPrefab;

    //[Header("Inputs")]

    //[Header("Outputs")]

    void Start()
    {
        if (_spawnPoints.Count == 0 || _keyPointPrefab == null)
        {
            Debug.LogWarning("No spawn points or key point prefab assigned.");
            return;
        }
        int randomIndex = Random.Range(0, _spawnPoints.Count);
        Transform spawnPoint = _spawnPoints[randomIndex];
        Instantiate(_keyPointPrefab, spawnPoint.position, spawnPoint.rotation, transform);
    }
}