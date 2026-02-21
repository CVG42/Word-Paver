using System.Collections.Generic;
using UnityEngine;

public class MenuTypingController : MonoBehaviour
{
    [SerializeField] private List<MainMenuTyping> _options;

    private MainMenuTyping _activeOption;

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
        foreach (var option in _options)
        {
            if (option.Word.StartsWith(c.ToString(), System.StringComparison.OrdinalIgnoreCase))
            {
                if (_activeOption != option)
                {
                    _activeOption?.ResetProgress();
                    _activeOption = option;
                }

                break;
            }
        }

        if (_activeOption == null) return;

        bool success = _activeOption.ProcessInput(c);

        if (!success)
        {
            _activeOption.ResetProgress();
        }
    }

    public void SelectOption(MainMenuTyping option)
    {
        if (_activeOption != option)
        {
            ResetAll();
            _activeOption = option;
        }

        option.OnSelected?.Invoke();
    }

    public void ResetAll()
    {
        _activeOption = null;

        foreach (var option in _options)
        {
            option.ResetProgress();
        }
    }
}
