using Patterns.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : SingletonMonoBehaviour<HudController>
{
    [SerializeField]
    private GameObject _mainPanel;


    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();

        GameManager.Instance.OnPauseGame.AddListener(Disable);
        GameManager.Instance.OnResumeGame.AddListener(Enable);
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

    
}
