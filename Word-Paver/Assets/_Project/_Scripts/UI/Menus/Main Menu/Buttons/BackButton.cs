using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    [SerializeField] private MenuTypingController _typing;
    [SerializeField] private GameObject _panelToClose;
    [SerializeField] private GameObject _panelToOpen;

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
        EventSystem.current.SetSelectedGameObject(null);
        _typing.ResetAll();
        _panelToOpen.SetActive(true);
        _panelToClose.SetActive(false);
    }
}
