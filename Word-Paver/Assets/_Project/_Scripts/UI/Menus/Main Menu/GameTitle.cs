using DG.Tweening;
using UnityEngine;

public class GameTitle : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float _popScale = 1.15f;
    [SerializeField] private float _duration = 0.25f;

    [SerializeField] private GameObject _startText;

    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _startText.SetActive(false);
    }

    private void Start()
    {
        PlayPop();
    }

    public void PlayPop()
    {
        transform.localScale = Vector3.zero;

        transform
            .DOScale(_originalScale * _popScale, _duration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                transform
                    .DOScale(_originalScale, _duration * 0.6f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                     {
                        _startText.SetActive(true);
                     });
            });
    }
}
