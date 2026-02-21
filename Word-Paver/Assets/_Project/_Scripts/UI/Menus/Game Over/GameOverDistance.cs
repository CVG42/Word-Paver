using TMPro;
using UnityEngine;

public class GameOverDistance : MonoBehaviour
{
    [SerializeField] private TMP_Text _distanceText;

    private void OnEnable()
    {
        float distance = GameManager.Source.DistanceTravelled;
        _distanceText.text = $"{distance} M";
    }
}
