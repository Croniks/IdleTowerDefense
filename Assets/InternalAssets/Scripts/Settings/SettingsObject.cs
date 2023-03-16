using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
public class SettingsObject : ScriptableObject, ISettingsGetter, ISettingsSetter
{
    public event Action SettingsChanged;
    
    public float SpawnCircleRadiusOffset { get => _spawnCircleRadiusOffset; set => _spawnCircleRadiusOffset = value; }
    [SerializeField, Space, Header("EnemiesSettings")] private float _spawnCircleRadiusOffset = 0.05f;


    public int WavesCount { get => _wavesCount; set => _wavesCount = value; }
    [SerializeField, Space] private int _wavesCount = 3;

    public float WavesInterval { get => _wavesInterval; set => _wavesInterval = value; }
    [SerializeField] private float _wavesInterval = 2f;

    public int EnemiesCountPerWave { get => _enemiesCountPerWave; set => _enemiesCountPerWave = value; }
    [SerializeField] private int _enemiesCountPerWave = 10;

    public float EnemiesInterval { get => _enemiesInterval; set => _enemiesInterval = value; }
    [SerializeField] private float _enemiesInterval = 0.5f;

    public float DestinationTime { get => _destinationTime; set => _destinationTime = value; }
    [SerializeField, Range(0.1f, 30f)] private float _destinationTime = 3f;
    public float DestinationPercent { get => _destinationPercent; set => _destinationPercent = value; }
    [SerializeField, Range(0f, 1f)] private float _destinationPercent = 0.9f;

    public Vector3 DestinationPosition { get => _destinationPosition; set => _destinationPosition = value; }
    [SerializeField] private Vector3 _destinationPosition = Vector3.zero;

    [SerializeField, Space] private bool _overridePlayerPrefs = false;

    
    public void SaveSettings()
    {
        if (_overridePlayerPrefs == false)
        {
            PlayerPrefs.Save();
        }
        
        SettingsChanged?.Invoke();
    }

    public void LoadSettings()
    {
        if (_overridePlayerPrefs == true)
        {
            return;
        }
    }
}

public class PlayerPrefsSettingsNames
{
    
}