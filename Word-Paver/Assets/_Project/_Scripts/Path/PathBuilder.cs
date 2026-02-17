using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _pathBlockPrefab;
    [SerializeField] private float _blockLength = 5f;

    private float _currentZ;
    private float _distanceTravelled;

    public float DistanceTravelled => _distanceTravelled;

    public void SpawnBlock()
    {
        Instantiate(_pathBlockPrefab, new Vector3(0, 0, _currentZ), Quaternion.identity);

        _currentZ += _blockLength;
        _distanceTravelled += _blockLength;
    }
}
