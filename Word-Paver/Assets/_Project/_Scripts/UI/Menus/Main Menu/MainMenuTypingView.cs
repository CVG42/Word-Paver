using TMPro;
using UnityEngine;

public class MainMenuTypingView : MonoBehaviour
{
    [SerializeField] private MainMenuTyping _option;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _progressColor = Color.green;

    private string _colorHex;

    private void Awake()
    {
        _colorHex = ColorUtility.ToHtmlStringRGB(_progressColor);
    }

    private void Update()
    {
        Redraw();
    }

    private void Redraw()
    {
        string word = _option.Word;

        if (string.IsNullOrEmpty(word))
        {
            _text.text = "";
            return;
        }

        int index = _option.CurrentIndex;

        string result = "";

        for (int i = 0; i < word.Length; i++)
        {
            if (i < index)
                result += $"<color=#{_colorHex}>{word[i]}</color>";
            else
                result += word[i];
        }

        _text.text = result;
    }
}
