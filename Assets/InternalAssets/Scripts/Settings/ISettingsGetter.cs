using System;

public interface ISettingsGetter 
{
    public event Action SettingsChanged;

    public float SpawnCircleRadiusOffset { get; }
    public float DestinationPercent { get; }
    public int WavesCount { get; }
    public float WavesInterval { get; }
    public int EnemiesCountPerWave { get; }
    public float EnemiesInterval { get; }
    public float DestinationTime { get; }
}
