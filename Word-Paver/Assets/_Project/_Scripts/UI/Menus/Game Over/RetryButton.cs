using UnityEngine;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(Retry);
    }

    private void Retry()
    {
        GameManager.Source.ChangeState(GameState.OnPlay);

        GameManager.Source.ResetRun();

        LevelController.Source.RestartRun();
    }
}
