using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
public class ParticleSystemController : MonoBehaviour
{
    public UnityAction OnStop;

    [SerializeField]
    private ParticleSystem _particleSystem;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioClip;

    public ParticleSystem ThisParticleSystem { get => _particleSystem; }

    private void Awake()
    {
        if(_particleSystem == null)
            _particleSystem = GetComponent<ParticleSystem>();  
        
        if(_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        if(_audioClip != null)
            _audioSource.clip = _audioClip;

        if (_audioSource.clip == null)
            Debug.LogWarning("ParticleSystemDestroy must has an Audio Clip references in this Script or in his AudioSource component.");
    }

    public void PlayAnimation()
    {
        if(_particleSystem != null)
            _particleSystem.Play();
        if( _audioSource != null)
            _audioSource.Play();

        StartCoroutine(StopParticleSystem());
    }

    private IEnumerator StopParticleSystem()
    {
        float dur = 0;
        if (_particleSystem != null)
            dur = _particleSystem.main.duration;
        if (_audioSource.clip != null)
            dur = _audioSource.clip.length < _particleSystem.main.duration ? _particleSystem.main.duration : _audioSource.clip.length;

        yield return new WaitForSeconds(dur);
        if(_particleSystem != null)
        {
            _particleSystem.Stop();
            OnStop?.Invoke();
        }
    }
}
