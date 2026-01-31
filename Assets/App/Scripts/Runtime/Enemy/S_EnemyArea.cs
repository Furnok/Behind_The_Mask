using System.Collections.Generic;
using UnityEngine;

public class S_EnemyArea : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] List<S_EnemyPerception> _enemies = new List<S_EnemyPerception>();

    //[Header("Inputs")]

    //[Header("Outputs")]

    Transform _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform;
            foreach (var enemy in _enemies)
            {
                enemy.SetPlayer(_player);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in _enemies)
            {
                enemy.SetPlayer(null);
            }
            _player = null;
        }
    }
}