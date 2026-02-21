using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(ResumeGame);
    }

    private void ResumeGame()
    {
        if (GameManager.Source.CurrentGameState != GameState.OnPause) return;

        GameManager.Source.ResumePreviousState();
    }
}
