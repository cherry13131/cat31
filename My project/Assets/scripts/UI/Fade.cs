using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Image fadeImage;
    [SerializeField] float Duration = 0.5f;

    private Coroutine coroutine;

    private void Awake()
    {
        fadeImage = GetComponentInChildren<Image>();
    }
       
    public void FadeIn(float duration = -1, Action onComplete = null)
    {
        
    }
    
    public void FadeOut(float duration = -1, Action onComplete = null)
    {

    }

    private void StartFade(float from, float to, float duration, Action onComplete)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(FadeRoutine(from, to, duration < 0? Duration: duration, onComplete));
    }

    private IEnumerator FadeRoutine(float from, float to, float duration, Action onComplete)
    {
        float time = 0f;
        SetAlpha(from);

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, time / duration);
            SetAlpha(alpha);
            yield return null;
        }
        
        SetAlpha(to);
        coroutine = null;
        onComplete?.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
