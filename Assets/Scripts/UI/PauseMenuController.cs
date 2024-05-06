using Patterns.Singletons;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : SingletonMonoBehaviour<PauseMenuController>
{
    [SerializeField]
    private GameObject _mainPanel;
    
    [SerializeField]
    private Button _resumeButton, _configurationButton,
        _restartButton, _exitButton;

    protected override void OnAwake()
    {
        base.OnAwake();
        _mainPanel.SetActive(false);
    }

    protected override void OnStart()
    {
        base.OnStart();
        _resumeButton.onClick.AddListener(GameManager.Instance.ResumeGame);
        _configurationButton.onClick.AddListener(GameManager.Instance.ResumeGame);
        _restartButton.onClick.AddListener(GameManager.Instance.RestartGame);
        _exitButton.onClick.AddListener(GameManager.Instance.ExitGame);

        GameManager.Instance.OnPauseGame.AddListener(Enable);
        GameManager.Instance.OnResumeGame.AddListener(Disable);
        GameManager.Instance.OnResetScene.AddListener(OnChangeScene);
    }

    public void OnChangeScene()
    {
        GameManager.Instance.OnPauseGame.RemoveListener(Disable);
        GameManager.Instance.OnResumeGame.RemoveListener(Enable);
        GameManager.Instance.OnResetScene.RemoveListener(OnChangeScene);
    }

    public void Enable()
    {
        _mainPanel.SetActive(true);
    }

    public void Disable()
    {
        _mainPanel.SetActive(false);
    }

    public void PlayerDie()
    {
        Enable();
        _resumeButton.gameObject.SetActive(false);
        _configurationButton.gameObject.SetActive(false);
    }
}

