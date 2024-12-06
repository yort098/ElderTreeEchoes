using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        float fadeDuration = 2f;
        AudioManager.Instance.StartFadeOut(fadeDuration);
        StartCoroutine(SwitchToLevel(fadeDuration));
    }

    private IEnumerator SwitchToLevel(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;

            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
