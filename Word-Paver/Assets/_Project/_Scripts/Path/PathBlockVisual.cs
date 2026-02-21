using DG.Tweening;
using UnityEngine;

public class PathBlockVisual : MonoBehaviour
{
    private Vector3 _baseScale;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = _baseScale;
        transform.DOKill();
    }

    public Vector3 GetBaseScale() => _baseScale;
}
