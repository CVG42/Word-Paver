using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities;

public class LevelController : Singleton<ILevelSource>, ILevelSource
{
    [SerializeField] private PathBuilder _path;
    [SerializeField] private CameraController _camera;
    [SerializeField] private EnvironmentManager _environment;

    [Header("Balance")]
    [SerializeField] private float _timePerLetter = 0.3f;
    [SerializeField] private float _penaltyPerError = 1f;
    [SerializeField] private float _initialTime = 10f;

    public event Action<float, float> OnTimerChanged;
    public event Action OnTimerFinished;
    public event Action OnTypingError;

    private bool _wordCompleted;
    private float _maxTime;
    private CountdownTimer _timer;

    private async void Start()
    {
        _maxTime = _initialTime;
        _timer = new CountdownTimer(_maxTime);
        _timer.Start();

        SubscribeEvents();
        InitializeWorld();
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

    private void InitializeWorld()
    {
        var world = WorldManager.Source.GetWorld(0);

        if (world == null) return;

        _path.SetWorld(world);
        _environment.SetWorld(world.Environment);

        Debug.Log($"Initial world: {world.Name}");
    }

    private void SubscribeEvents()
    {
        TypingController.Source.OnWordCompleted += HandleWordCompleted;
        TypingController.Source.OnLetterFailed += HandleLetterFailed;

        GameManager.Source.OnDistanceChanged += HandleDistanceChanged;
        WorldManager.Source.OnWorldChanged += HandleWorldChanged;
    }

    private async UniTask StartGameLoop()
    {
        while (!_timer.IsFinished)
        {
            _wordCompleted = false;

            string word = WordManager.Source.GetWord(GameManager.Source.DistanceTravelled);
            TypingController.Source.SetWord(word);

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
        _environment.SetWorld(world.Environment);

        Debug.Log($"World changed to: {world.Name}");
    }

    private void HandleWordCompleted()
    {
        if (_timer.IsFinished) return;

        _wordCompleted = true;

        if (ObstacleManager.Source.HasActiveObstacle)
        {
            ObstacleManager.Source.NotifyWordCompleted();
            return;
        }

        if (ObstacleManager.Source.TrySpawnObstacle(GameManager.Source.DistanceTravelled)) return;

        _path.SpawnBlock();

        GameManager.Source.AddDistance(5);

        float bonus = TypingController.Source.CurrentWordLength * _timePerLetter;
        _timer.AddTime(bonus);

        ClampTimer();

        _camera.MoveForward();
    }

    private void HandleLetterFailed()
    {
        if (_timer.IsFinished) return;

        OnTypingError?.Invoke();
        _timer.RemoveTime(_penaltyPerError);
    }

    private void HandleGameOver()
    {
        OnTimerFinished?.Invoke();
        Debug.Log($"Distance travelled: {GameManager.Source.DistanceTravelled}");
        enabled = false;
    }

    private void NotifyTimer()
    {
        OnTimerChanged?.Invoke(_timer.GetRemainingTime(), _maxTime);
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
        TypingController.Source.OnWordCompleted -= HandleWordCompleted;
        TypingController.Source.OnLetterFailed -= HandleLetterFailed;
        GameManager.Source.OnDistanceChanged -= HandleDistanceChanged;
        WorldManager.Source.OnWorldChanged -= HandleWorldChanged;
    }
}

public interface ILevelSource
{
    event Action<float, float> OnTimerChanged;
    event Action OnTimerFinished;
    event Action OnTypingError;
}
