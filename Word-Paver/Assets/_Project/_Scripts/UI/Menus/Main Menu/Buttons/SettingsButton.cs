using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private MenuTypingController _typing;
    [SerializeField] private GameObject _settingsPanel;
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
        EventSystem.current.SetSelectedGameObject(null);
        _typing.ResetAll();
        _settingsPanel.SetActive(true);
        _currentPanel.SetActive(false);
    }
}
