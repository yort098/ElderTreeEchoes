using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip menuTrack;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            musicSource.clip = menuTrack;
            musicSource.Play();
            //musicSource.loop = true;
        }
        
    }

    private IEnumerator FadeOut(float duration)
    {
        float time = 0f;
        float startVolume = musicSource.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }
    }

    public void StartFadeOut(float duration)
    {
        StartCoroutine(FadeOut(duration));
    }
}
