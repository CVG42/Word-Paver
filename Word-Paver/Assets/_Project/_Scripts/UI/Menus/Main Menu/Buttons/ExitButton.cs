using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private MenuTypingController _typing;

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
        _typing.ResetAll();
        Application.Quit();
    }
}
