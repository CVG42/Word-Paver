using TMPro;
using UnityEngine;

public class DistanceView : MonoBehaviour
{
    [SerializeField] private TMP_Text _distanceText;

    private void Start()
    {
        GameManager.Source.OnDistanceChanged += UpdateDistance;

        UpdateDistance(GameManager.Source.DistanceTravelled);
    }

    private void UpdateDistance(float distance)
    {
        _distanceText.text = $"{Mathf.FloorToInt(distance)} M";
    }

    private void OnDestroy()
    {
        GameManager.Source.OnDistanceChanged -= UpdateDistance;
    }
}
