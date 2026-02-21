using UnityEngine;
using UnityEngine.UI;

public class CreditsButton : MonoBehaviour
{
    [SerializeField] private MenuTypingController _typing;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _currentPanel;

    private Button _button;

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
        AudioManager.Source.PlaySelectedSFX();
        _typing.ResetAll();
        _creditsPanel.SetActive(true);
        _currentPanel.SetActive(false);
    }
}
