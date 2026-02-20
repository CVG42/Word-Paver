using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 5f;
    [SerializeField] private float _moveDuration = 0.25f;

    [Header("Shake")]
    [SerializeField] private float _shakeDuration = 0.15f;
    [SerializeField] private float _shakeStrength = 0.15f;
    [SerializeField] private int _shakeVibrato = 12;

    private Tween _shakeTween;

    private void Start()
    {
        LevelController.Source.OnTypingError += ShakeOnError;
    }

    public void MoveForward()
    {
        transform.DOMoveZ(transform.position.z + _moveDistance, _moveDuration)
            .SetEase(Ease.OutQuad);
    }

    private void ShakeOnError()
    {
        _shakeTween?.Kill();

        _shakeTween = transform.DOShakePosition(
            _shakeDuration,
            _shakeStrength,
            _shakeVibrato,
            randomness: 90,
            fadeOut: true
        );
    }
}
