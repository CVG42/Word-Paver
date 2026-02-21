using System;
using UnityEngine;

public class GameManager : Singleton<IGameSource>, IGameSource
{
    public float DistanceTravelled { get; private set; }
    public GameState CurrentGameState { get; private set; }

    public event Action OnGamePaused;
    public event Action OnGameUnpaused;
    public event Action<GameState> OnGameStateChanged;
    public event Action<float> OnDistanceChanged;

    private GameState _previousState;

    public void ChangeState(GameState state)
    {
        if (CurrentGameState == state) return;

        if (state == GameState.OnPause)
        {
            _previousState = CurrentGameState;
        }

        CurrentGameState = state;
        OnGameStateChanged?.Invoke(CurrentGameState);

        CheckPauseState(state);
    }

    public void ResumePreviousState()
    {
        if (_previousState == GameState.OnPause)
        {
            _previousState = GameState.OnPlay;
        }

        ChangeState(_previousState);
    }

    public void AddDistance(float amount)
    {
        DistanceTravelled += amount;
        OnDistanceChanged?.Invoke(DistanceTravelled);
    }

    public void ResetRun()
    {
        DistanceTravelled = 0;
        OnDistanceChanged?.Invoke(DistanceTravelled);
    }

    private void CheckPauseState(GameState state)
    {
        if (state == GameState.OnPause || state == GameState.OnGameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OnGamePaused?.Invoke();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            OnGameUnpaused?.Invoke();
        }
    }
}

public interface IGameSource
{
    event Action<float> OnDistanceChanged;
    event Action<GameState> OnGameStateChanged;
    event Action OnGamePaused;
    event Action OnGameUnpaused;
    
    GameState CurrentGameState { get; }

    float DistanceTravelled { get; }

    void ChangeState(GameState state);
    void ResumePreviousState();
    void AddDistance(float amount);
    void ResetRun();
}

public enum GameState
{
    OnIntro,
    OnPlay,
    OnPause,
    OnGameOver
}
