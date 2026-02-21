using UnityEngine;

public class EnvironmentTile : MonoBehaviour
{
    [SerializeField] private float _length = 20f;
    
    public float Length => _length;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
