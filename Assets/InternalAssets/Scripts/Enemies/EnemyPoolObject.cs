using UnityEngine;

using Spawners;
using EventBusSystem;
using System.Collections.Generic;

public class EnemyPoolObject : PoolObject, IShootingTarget
{
    public LinkedListNode<EnemyPoolObject> EnemyNode { get; private set; }
    private Vector3 _spawnPosition;
    private Vector3 _destinationPosition;
    private float _destinationTime;
    private float _destinationPercent;
    private float _maxHP = 10f;

    private float _percentTraveled = 0f;
    private float _currentHP = 10f;
    
    
    public void Setup
    (
        LinkedListNode<EnemyPoolObject> enemyNode, 
        Vector3 spawnPosition, 
        Vector3 destinationPosition, 
        float destinationTime, 
        float destinationPercent
    )
    {
        EnemyNode = enemyNode;
        _spawnPosition = spawnPosition;
        _destinationPosition = destinationPosition;
        _destinationTime = destinationTime;
        _destinationPercent = destinationPercent;
    }
    
    public void MoveToBase()
    {
        _percentTraveled += Time.deltaTime / _destinationTime;
        
        if(_percentTraveled < _destinationPercent)
        {
            transform.position = Vector3.Lerp(_spawnPosition, _destinationPosition, _percentTraveled);
        }
        else
        {
            EventBus.RaiseEvent<IBaseDamageSubscriber>(h => h.HandleBaseDamage(0.01f));
        }
    }

    public Vector3 GetTargetPosition()
    {
        return transform.position;
    }

    public void TakeDamage(float damage)
    {
        _currentHP -= damage;

        if (_currentHP <= 0f)
        {
            Debug.Log("Враг уничтожен !!!");
            enabled = false;
            
            // Событие уничтожения врага
        }
        else
        {
            Debug.Log($"Враг получил урон !!!");
        }
    }
}