using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private MenuTypingController _typing;
    [SerializeField] private string _sceneName = "Prototype";

    private Button _button;
    private bool _isLoading;


    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(Execute);
    }

    public void Execute()
    {
        if (_isLoading) return;

        LoadScene().Forget();
    }

    private async UniTaskVoid LoadScene()
    {
        _isLoading = true;

        AudioManager.Source.PlaySelectedSFX();
        _typing.ResetAll();

        await UniTask.Delay(1500);

        SceneTransitionManager.Source.LoadScene(_sceneName);
    }
}
