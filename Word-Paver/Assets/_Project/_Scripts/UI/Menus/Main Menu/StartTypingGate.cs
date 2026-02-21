using UnityEngine;

public class StartTypingGate : MonoBehaviour
{
    [SerializeField] private string _word = "start";
    [SerializeField] private GameObject _startPrompt;
    [SerializeField] private GameObject _mainMenu;

    public int CurrentIndex => _index;

    private int _index;

    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (!char.IsLetter(c)) continue;

            ProcessInput(char.ToLowerInvariant(c));
        }
    }

    private void ProcessInput(char c)
    {
        if (_word[_index] == c)
        {
            _index++;

            if (_index >= _word.Length)
            {
                UnlockMenu();
                _index = 0;
            }
        }
        else
        {
            _index = 0;
        }
    }

    private void UnlockMenu()
    {
        _startPrompt.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
