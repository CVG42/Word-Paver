using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] private string _musicName;

    private void Start()
    {
        AudioManager.Source.PlayLevelMusic(_musicName);
    }
}
