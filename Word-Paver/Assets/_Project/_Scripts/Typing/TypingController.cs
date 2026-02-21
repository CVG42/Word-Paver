using System;
using UnityEngine;

public class TypingController : Singleton<ITypingSource>, ITypingSource
{
    public event Action OnWordCompleted;
    public event Action OnLetterFailed;
    public event Action<int> OnLetterCorrect;

    public event Action<string> OnWordChanged;

    public int CurrentWordLength => string.IsNullOrEmpty(_currentWord) ? 0 : _currentWord.Length;
    public string CurrentWord => _currentWord ?? string.Empty;

    private string _currentWord;
    private int _currentIndex;

    private void Update()
    {
        if (GameManager.Source.CurrentGameState != GameState.OnPlay) return;

        foreach (char c in Input.inputString)
        {
            if (!char.IsLetter(c)) continue;

            ProcessInput(char.ToLowerInvariant(c));
        }
    }

    public void SetWord(string word)
    {
        _currentWord = word;
        _currentIndex = 0;

        OnWordChanged?.Invoke(word);
    }

    public void ProcessInput(char input)
    {
        if (string.IsNullOrEmpty(_currentWord)) return;
        if (_currentIndex >= _currentWord.Length) return;

        if (_currentWord[_currentIndex] == input)
        {
            OnLetterCorrect?.Invoke(_currentIndex);

            _currentIndex++;

            if (_currentIndex >= _currentWord.Length)
            {
                OnWordCompleted?.Invoke();
            }
        }
        else
        {
            OnLetterFailed?.Invoke();
        }
    }
}

public interface ITypingSource
{
    event Action OnWordCompleted;
    event Action OnLetterFailed;
    event Action<int> OnLetterCorrect;
    event Action<string> OnWordChanged;

    int CurrentWordLength {  get; }
    string CurrentWord { get; }

    void SetWord(string word);
    void ProcessInput(char input);
}
