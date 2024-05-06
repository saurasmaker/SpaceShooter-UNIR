using UnityEngine;
using Cinemachine;
using System.Collections;

/// <summary>
/// An add-on module for Cinemachine to shake the camera
/// </summary>

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera _vcam;

    [SerializeField]
    float _durationSecs, _amplitudeGain, _frequencyGain;

    CinemachineBasicMultiChannelPerlin _cbmp;

    private void Awake()
    {
        _cbmp = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void Shake()
    {
        if (_shakeCameraRoutine != null)
        {
            StopCoroutine( _shakeCameraRoutine );
            _shakeCameraRoutine = null;
        }
        _shakeCameraRoutine = StartCoroutine(ShakeCameraRoutine());
    }

    private Coroutine _shakeCameraRoutine = null;
    private IEnumerator ShakeCameraRoutine()
    {
        _cbmp.m_AmplitudeGain = _amplitudeGain;
        _cbmp.m_FrequencyGain = _frequencyGain;

        yield return new WaitForSeconds(_durationSecs);

        _cbmp.m_AmplitudeGain = 0f;
        _cbmp.m_FrequencyGain = 0f;
    }
}