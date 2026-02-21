using UnityEngine;

public class PuaseController : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Source.CurrentGameState == GameState.OnGameOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (GameManager.Source.CurrentGameState == GameState.OnPlay)
        {
            GameManager.Source.ChangeState(GameState.OnPause);
        }
        else if (GameManager.Source.CurrentGameState == GameState.OnPause)
        {
            GameManager.Source.ResumePreviousState();
        }
    }
}
