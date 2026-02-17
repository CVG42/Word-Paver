using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 5f;
    [SerializeField] private float _moveDuration = 0.25f;

    public void MoveForward()
    {
        transform.DOMoveZ(transform.position.z + _moveDistance, _moveDuration)
            .SetEase(Ease.OutQuad);
    }
}
