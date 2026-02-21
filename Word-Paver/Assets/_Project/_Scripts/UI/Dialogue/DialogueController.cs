using Cysharp.Threading.Tasks;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueDatabase _database;

    private DialogueSequence _currentSequence;
    private int _messageIndex;
    private bool _waitingInput;

    private async void Start()
    {
        await UniTask.NextFrame();

        GameManager.Source.OnGameStateChanged += HandleGameStateChanged;
        LevelController.Source.OnRunRestarted += HandleRunRestarted;

        HandleInitialFlow();
    }

    private void Update()
    {
        if (!_waitingInput) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceDialogue();
        }
    }

    private void PreparePreviewWord()
    {
        float distance = GameManager.Source.DistanceTravelled;

        string word = WordManager.Source.GetWord(distance);

        TypingController.Source.SetWord(word);
    }

    private void HandleInitialFlow()
    {
        if (IsFirstLaunch())
        {
            StartSequence(DialogueCategory.Tutorial);
        }
        else
        {
            StartSequence(DialogueCategory.RegularStart);
        }
    }

    private void HandleRunRestarted()
    {
        StartSequence(DialogueCategory.Retry);
    }

    private void StartSequence(DialogueCategory category)
    {
        _currentSequence = _database.GetRandom(category);

        PreparePreviewWord();

        if (_currentSequence == null)
        {
            LevelController.Source.StartRun();
            return;
        }

        _messageIndex = 0;

        GameManager.Source.ChangeState(GameState.OnIntro);

        ShowCurrentMessage();
    }

    private void ShowCurrentMessage()
    {
        if (_messageIndex >= _currentSequence.Messages.Count)
        {
            EndSequence();
            return;
        }

        UIManager.Source.ShowDialogue(_currentSequence.Messages[_messageIndex]);

        _waitingInput = true;
    }

    private void AdvanceDialogue()
    {
        _waitingInput = false;

        _messageIndex++;

        ShowCurrentMessage();
    }

    private void EndSequence()
    {
        UIManager.Source.HideDialogue();

        LevelController.Source.StartRun();
    }

    private bool IsFirstLaunch()
    {
        if (!PlayerPrefs.HasKey("FIRST_LAUNCH"))
        {
            PlayerPrefs.SetInt("FIRST_LAUNCH", 1);
            PlayerPrefs.Save();
            return true;
        }

        return false;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.OnPause || state == GameState.OnGameOver)
        {
            _waitingInput = false;
        }
    }

    private void OnDestroy()
    {
        GameManager.Source.OnGameStateChanged -= HandleGameStateChanged;
        LevelController.Source.OnRunRestarted -= HandleRunRestarted;
    }
}
