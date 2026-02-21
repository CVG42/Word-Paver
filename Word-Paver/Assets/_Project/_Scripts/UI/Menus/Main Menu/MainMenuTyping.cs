using UnityEngine;
using UnityEngine.Events;

public class MainMenuTyping : MonoBehaviour
{
    public string Word;
    public UnityEvent OnSelected;
    public int CurrentIndex => _currentIndex;

    private int _currentIndex;

    public void ResetProgress()
    {
        _currentIndex = 0;
    }

    public bool ProcessInput(char c)
    {
        if (string.IsNullOrEmpty(Word)) return false;

        c = char.ToLowerInvariant(c);

        if (Word[_currentIndex] == c)
        {
            _currentIndex++;

            if (_currentIndex >= Word.Length)
            {
                OnSelected?.Invoke();
                _currentIndex = 0;
            }

            return true;
        }

        return false;
    }
}
