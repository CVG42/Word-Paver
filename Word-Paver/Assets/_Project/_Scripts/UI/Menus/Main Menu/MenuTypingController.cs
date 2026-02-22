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

            AudioManager.Source.PlayTypeSFX();
            ProcessInput(char.ToLowerInvariant(c));
        }
    }

    private void ProcessInput(char c)
    {
        if (_activeOption == null)
        {
            foreach (var option in _options)
            {
                if (!option.isActiveAndEnabled) continue;

                if (option.Word.StartsWith(c.ToString(), System.StringComparison.OrdinalIgnoreCase))
                {
                    _activeOption = option;
                    break;
                }
            }
        }

        if (_activeOption == null) return;

        bool success = _activeOption.ProcessInput(c);

        if (!success)
        {
            _activeOption.ResetProgress();
            _activeOption = null;
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
