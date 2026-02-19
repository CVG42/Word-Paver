using TMPro;
using UnityEngine;

public class WordView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _correctLetterColor = Color.green;

    private string _word;
    private int _currentIndex;

    private string _colorHex;

    private void Awake()
    {
        _colorHex = ColorUtility.ToHtmlStringRGB(_correctLetterColor);
    }

    private void Start()
    {
        TypingController.Source.OnWordChanged += SetWord;
        TypingController.Source.OnLetterCorrect += HandleLetterCorrect;
    }

    public void SetWord(string word)
    {
        _word = word;
        _currentIndex = 0;

        Redraw();
    }

    private void HandleLetterCorrect(int index)
    {
        _currentIndex = index + 1;
        Redraw();
    }

    private void Redraw()
    {
        if (string.IsNullOrEmpty(_word))
        {
            _text.text = "";
            return;
        }

        string result = "";

        for (int i = 0; i < _word.Length; i++)
        {
            if (i < _currentIndex)
            {
                result += $"<color=#{_colorHex}>{_word[i]}</color>";
            }
            else
            {
                result += _word[i];
            }
        }

        _text.text = result;
    }
}
