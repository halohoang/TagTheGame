using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Variables
    [SerializeField] private float _maximumHealth;
    [SerializeField] internal float _currentHealth;
    [SerializeField] private float _takenDamage;
    // Functions


    void Start()
    {
        _currentHealth = _maximumHealth;
    }


    void Update()
    {
        
    }

    internal void GetDamage()
    {
        _currentHealth -= _takenDamage;
    }
}
