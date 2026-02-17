using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private PathBuilder _pathBuilder;
    [SerializeField] private GameObject[] _easyProps;
    [SerializeField] private GameObject[] _hardProps;

    public void TrySpawnProps()
    {
        float distance = _pathBuilder.DistanceTravelled;

        GameObject prop = distance < 100 ? _easyProps[Random.Range(0, _easyProps.Length)] : _hardProps[Random.Range(0, _hardProps.Length)];

        Instantiate(prop, GetSpawnPosition(), Quaternion.identity);
    }

    private Vector3 GetSpawnPosition()
    {
        float side = Random.value > 0.5f ? 1 : -1;
        return new Vector3(5 * side, 0, _pathBuilder.DistanceTravelled + 20);
    }
}
