using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;

    private void Start()
    {
        HideAllMenus();
        GameManager.Source.OnGameStateChanged += HandleGameStateChanged;
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
