using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Destructible : MonoBehaviour
{
    [SerializeField] private float _destructionTime = 1f;
    
    [Range(0f, 1f)]
    [SerializeField] private float _spawnChance = 0.2f;
    [SerializeField] GameObject [] _spawnableItems;
    private void Start()
    {
        Destroy(gameObject, _destructionTime);
    }

    private void OnDestroy()
    {
        if (_spawnableItems.Length > 0 && Random.value < _spawnChance)
        {
            int randomIndex = Random.Range(0, _spawnableItems.Length);
            Instantiate(_spawnableItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
}
