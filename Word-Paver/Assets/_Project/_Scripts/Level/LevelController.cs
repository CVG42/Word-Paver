using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;

public class LevelController : Singleton<ILevelSource>, ILevelSource
{
    [SerializeField] private TypingController _typing;
    [SerializeField] private PathBuilder _path;
    [SerializeField] private CameraController _camera;

    [Header("Balance")]
    [SerializeField] private float _timePerLetter = 0.3f;
    [SerializeField] private float _penaltyPerError = 1f;
    [SerializeField] private float _initialTime = 10f;

    public event Action<float, float> OnTimerChanged;
    public event Action OnTimerFinished;

    private bool _wordCompleted;
    private float _maxTime;
    private CountdownTimer _timer;

    private async void Start()
    {
        _maxTime = _initialTime;
        _timer = new CountdownTimer(_maxTime);
        _timer.Start();

        SubscribeEvents();

        NotifyTimer();

        await StartGameLoop();
    }

    private void Update()
    {
        _timer?.Tick(Time.deltaTime);

        NotifyTimer();

        if (_timer != null && _timer.IsFinished)
        {
            HandleGameOver();
        }
    }

    private void SubscribeEvents()
    {
        _typing.OnWordCompleted += HandleWordCompleted;
        _typing.OnLetterFailed += HandleLetterFailed;

        _path.OnDistanceChanged += HandleDistanceChanged;
        WorldManager.Source.OnWorldChanged += HandleWorldChanged;
    }

    private async UniTask StartGameLoop()
    {
        while (!_timer.IsFinished)
        {
            _wordCompleted = false;

            string word = WordManager.Source.GetWord(_path.DistanceTravelled);
            _typing.SetWord(word);

            await UniTask.WaitUntil(() => _wordCompleted || _timer.IsFinished);
        }
    }

    private void HandleDistanceChanged(float distance)
    {
        WorldManager.Source.EvaluateWorld(distance);
    }

    private void HandleWorldChanged(WorldDefinition world)
    {
        _path.SetWorld(world);

        Debug.Log($"World changed to: {world.Name}");
    }

    private void HandleWordCompleted()
    {
        if (_timer.IsFinished) return;

        _wordCompleted = true;

        _path.SpawnBlock();

        float bonus = _typing.CurrentWordLength * _timePerLetter;
        _timer.AddTime(bonus);
        ClampTimer();
        _camera.MoveForward();
    }

    private void HandleLetterFailed()
    {
        if (_timer.IsFinished) return;

        _timer.RemoveTime(_penaltyPerError);
    }

    private void HandleGameOver()
    {
        OnTimerFinished?.Invoke();
        Debug.Log($"Distance travelled: {_path.DistanceTravelled}");
        enabled = false;
    }

    private void NotifyTimer()
    {
        OnTimerChanged?.Invoke(_timer.GetRemainingTime(), _maxTime);
    }

    public void AddTime(float value)
    {
        _timer.AddTime(value);
        NotifyTimer();
    }

    public void RemoveTime(float value)
    {
        _timer.RemoveTime(value);
        NotifyTimer();
    }

    private void ClampTimer()
    {
        float remaining = _timer.GetRemainingTime();

        if (remaining > _maxTime)
        {
            _timer.RemoveTime(remaining - _maxTime);
        }

        if (remaining < 0)
        {
            _timer.RemoveTime(remaining);
        }
    }

    private void OnDestroy()
    {
        _typing.OnWordCompleted -= HandleWordCompleted;
        _typing.OnLetterFailed -= HandleLetterFailed;
        _path.OnDistanceChanged -= HandleDistanceChanged;
        WorldManager.Source.OnWorldChanged -= HandleWorldChanged;
    }
}

public interface ILevelSource
{
    event Action<float, float> OnTimerChanged;
    event Action OnTimerFinished;
}
