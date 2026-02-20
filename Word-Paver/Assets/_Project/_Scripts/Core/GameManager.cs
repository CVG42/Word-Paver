using System;
using UnityEngine;

public class GameManager : Singleton<IGameSource>, IGameSource
{
    public float DistanceTravelled { get; private set; }

    public event Action<float> OnDistanceChanged; public void AddDistance(float amount)
    {
        DistanceTravelled += amount;
        OnDistanceChanged?.Invoke(DistanceTravelled);
    }

    public void ResetRun()
    {
        DistanceTravelled = 0;
        OnDistanceChanged?.Invoke(DistanceTravelled);
    }
}

public interface IGameSource
{
    event Action<float> OnDistanceChanged;

    float DistanceTravelled { get; }

    void AddDistance(float amount);
    void ResetRun();
}
