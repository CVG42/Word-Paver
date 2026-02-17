using System;
using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    public event Action OnTimerEnded;

    [SerializeField] private float _timeRemaining;

    public float TimeRemaining => _timeRemaining;

    public void AddTime(float amount)
    {
        _timeRemaining += amount;
    }

    public void RemoveTime(float amount)
    {
        _timeRemaining -= amount;

        if (_timeRemaining <= 0)
        {
            OnTimerEnded?.Invoke();
        }
    }

    private void Update()
    {
        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0)
        {
            OnTimerEnded?.Invoke();
        }
    }
}
