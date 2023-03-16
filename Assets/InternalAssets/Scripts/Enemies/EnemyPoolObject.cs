using UnityEngine;

using Spawners;


public class EnemyPoolObject : PoolObject
{
    private Vector3 _spawnPosition;
    private Vector3 _destinationPosition;
    private float _destinationTime;
    private float _destinationPercent;

    private float _percentTraveled = 0f;
   
    
    public void Setup(Vector3 spawnPosition, Vector3 destinationPosition, float destinationTime, float destinationPercent)
    {
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
            Debug.Log($"Атакую базу !!!");
        }
    }
}