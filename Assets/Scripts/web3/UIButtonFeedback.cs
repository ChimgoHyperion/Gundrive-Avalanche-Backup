using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class UIButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Scale")]
    public float pressScale = 0.92f;
    public float scaleDuration = 0.08f;

    [Header("Tint (optional)")]
    public bool enableTint = true; // set false for connectButton (because you update its color every frame)
    public Color clickTint = new Color(0.2f, 0.6f, 1f, 1f);
    public float tintDuration = 0.18f;
    public float tintHold = 0.10f;

    Vector3 originalScale;
    Coroutine scaleCoroutine;
    Coroutine tintCoroutine;
    Image targetImage;
    Color originalColor;

    void Awake()
    {
        originalScale = transform.localScale;
        targetImage = GetComponent<Image>() ?? GetComponentInChildren<Image>();
        if (targetImage != null) originalColor = targetImage.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartScale(originalScale * pressScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartScale(originalScale);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (enableTint && targetImage != null)
        {
            if (tintCoroutine != null) StopCoroutine(tintCoroutine);
            tintCoroutine = StartCoroutine(TintPulseRoutine());
        }
    }

    void StartScale(Vector3 target)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleRoutine(target));
    }

    IEnumerator ScaleRoutine(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float t = 0f;
        while (t < scaleDuration)
        {
            t += Time.unscaledDeltaTime;
            float f = Mathf.Clamp01(t / scaleDuration);
            // smoothstep easing
            f = f * f * (3f - 2f * f);
            transform.localScale = Vector3.Lerp(start, target, f);
            yield return null;
        }
        transform.localScale = target;
        scaleCoroutine = null;
    }

    IEnumerator TintPulseRoutine()
    {
        // lerp to tint
        float t = 0f;
        Color start = targetImage.color;
        while (t < tintDuration)
        {
            t += Time.unscaledDeltaTime;
            float f = Mathf.Clamp01(t / tintDuration);
            f = f * f * (3f - 2f * f);
            targetImage.color = Color.Lerp(start, clickTint, f);
            yield return null;
        }
        targetImage.color = clickTint;

        // hold
        yield return new WaitForSecondsRealtime(tintHold);

        // lerp back
        t = 0f;
        start = targetImage.color;
        while (t < tintDuration)
        {
            t += Time.unscaledDeltaTime;
            float f = Mathf.Clamp01(t / tintDuration);
            f = f * f * (3f - 2f * f);
            targetImage.color = Color.Lerp(start, originalColor, f);
            yield return null;
        }
        targetImage.color = originalColor;
        tintCoroutine = null;
    }
}
