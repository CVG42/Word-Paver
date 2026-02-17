using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private TypingController _typing;
    [SerializeField] private TimerSystem _timer;
    [SerializeField] private PathBuilder _path;
    [SerializeField] private CameraController _camera;

    [Header("Balance")]
    [SerializeField] private float _timePerLetter = 0.3f;
    [SerializeField] private float _penaltyPerError = 1f;

    private bool _wordCompleted;

    private async void Start()
    {
        SubscribeEvents();

        await StartGameLoop();
    }

    private void SubscribeEvents()
    {
        _typing.OnWordCompleted += HandleWordCompleted;
        _typing.OnLetterFailed += HandleLetterFailed;
        _timer.OnTimerEnded += HandleGameOver;
    }

    private async UniTask StartGameLoop()
    {
        while (_timer.TimeRemaining > 0)
        {
            _wordCompleted = false;

            string word = WordManager.Source.GetWord(_path.DistanceTravelled);
            _typing.SetWord(word);

            await UniTask.WaitUntil(() => _wordCompleted || _timer.TimeRemaining <= 0);
        }
    }

    private void HandleWordCompleted()
    {
        _wordCompleted = true;

        _path.SpawnBlock();

        float bonus = _typing.CurrentWordLength * _timePerLetter;
        _timer.AddTime(bonus);
        _camera.MoveForward();
    }

    private void HandleLetterFailed()
    {
        _timer.RemoveTime(_penaltyPerError);
    }

    private void HandleGameOver()
    {
        Debug.Log($"Distance travelled: {_path.DistanceTravelled}");
    }

    private int _typingWordLength()
    {
        return 5;
    }
}
