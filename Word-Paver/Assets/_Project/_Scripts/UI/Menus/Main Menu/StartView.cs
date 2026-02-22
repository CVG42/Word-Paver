using TMPro;
using UnityEngine;

public class StartView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _progressColor = Color.green;
    [SerializeField] private StartTypingGate _gate;

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
        string word = "START";

        int index = _gate.CurrentIndex;

        string result = "";

        for (int i = 0; i < word.Length; i++)
        {
            if (i < index)
            {
                result += $"<color=#{_colorHex}>{word[i]}</color>";
            }
            else
            {
                result += word[i];
            }
        }

        _text.text = result;
    }
}
