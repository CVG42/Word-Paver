using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<ITransitionSource>, ITransitionSource
{
    [SerializeField] private CanvasGroup _levelTransitionCanvasGroup;

    [Header("Loading Bar")]
    [SerializeField] private CanvasGroup _loadingBarCanvasGroup;
    [SerializeField] private Animator _loadingBarAnimator;
    [SerializeField] private int _loadingTime = 3000;

    private async UniTask LoadSceneAsync(string sceneName)
    {
        _levelTransitionCanvasGroup.gameObject.SetActive(true);
        await _levelTransitionCanvasGroup.DOFade(1, 0.4f).AsyncWaitForCompletion();
        _loadingBarCanvasGroup.gameObject.SetActive(true);
        _loadingBarCanvasGroup.alpha = 1;
        _loadingBarAnimator.Play("loading");
        await UniTask.Delay(_loadingTime);
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).ToUniTask();
        await _levelTransitionCanvasGroup.DOFade(0, 0.35f).AsyncWaitForCompletion();
        _loadingBarCanvasGroup.gameObject.SetActive(false);
        _levelTransitionCanvasGroup.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        LoadSceneAsync(sceneName).Forget();
    }
}

public interface ITransitionSource
{
    void LoadScene(string sceneName);
}
