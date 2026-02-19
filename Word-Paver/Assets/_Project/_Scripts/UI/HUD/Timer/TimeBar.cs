using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class TimeBar : MonoBehaviour
{
    [SerializeField] private Image _fill;

    private void Start()
    {
        LevelController.Source.OnTimerChanged += UpdateBar;
        LevelController.Source.OnTimerFinished += HandleFinished;
    }

    private void UpdateBar(float current, float max)
    {
        _fill.fillAmount = current / max;
    }

    private void HandleFinished()
    {
        Debug.Log("Game Over UI");
    }

    private void OnDestroy()
    {
        LevelController.Source.OnTimerChanged -= UpdateBar;
        LevelController.Source.OnTimerFinished -= HandleFinished;
    }
}
