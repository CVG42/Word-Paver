using System;
using UnityEngine;

public class TypingController : MonoBehaviour
{
    public event Action OnWordCompleted;
    public event Action OnLetterFailed;

    public int CurrentWordLength => _currentWord.Length;

    private string _currentWord;
    private int _currentIndex;

    public void SetWord(string word)
    {
        _currentWord = word;
        _currentIndex = 0;

        Debug.Log($"Current word: {_currentWord}");
    }

    public void ProcessInput(char input)
    {
        if (_currentWord[_currentIndex] == input)
        {
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
