using TMPro;
using UnityEngine;

public class UIManager : Singleton<IUISource>, IUISource
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;

    [Header("Dialogue")]
    [SerializeField] private GameObject _dialogPanel;
    [SerializeField] private TMP_Text _dialogText;

    private void Start()
    {
        HideAllMenus();
        GameManager.Source.OnGameStateChanged += HandleGameStateChanged;
    }

    public void ShowDialogue(string message)
    {
        _dialogPanel.SetActive(true);
        _dialogText.text = message;
    }

    public void HideDialogue()
    {
        _dialogPanel.SetActive(false);
    }

    private void HandleGameStateChanged(GameState state)
    {
        HideAllMenus();

        switch (state)
        {
            case GameState.OnPause:
                _pauseMenu.SetActive(true);
                break;

            case GameState.OnGameOver:
                _gameOverMenu.SetActive(true);
                break;
        }
    }

    private void HideAllMenus()
    {
        _pauseMenu.SetActive(false);
        _gameOverMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.Source.OnGameStateChanged -= HandleGameStateChanged;
    }
}

public interface IUISource
{
    void ShowDialogue(string message);
    void HideDialogue();
}

public enum DialogueCategory
{
    Tutorial,
    RegularStart,
    Retry
}
