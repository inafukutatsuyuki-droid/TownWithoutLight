using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TownWithoutLight.UI
{
    /// <summary>
    /// Handles fade-in/out transitions by animating a CanvasGroup.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private UnityEvent onFadeInCompleted;
        [SerializeField] private UnityEvent onFadeOutCompleted;

        private CanvasGroup _canvasGroup;
        private Coroutine _routine;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1f;
        }

        public void FadeIn()
        {
            StartFade(0f, onFadeInCompleted);
        }

        public void FadeOut()
        {
            StartFade(1f, onFadeOutCompleted);
        }

        private void StartFade(float target, UnityEvent callback)
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
            }

            _routine = StartCoroutine(FadeRoutine(target, callback));
        }

        private IEnumerator FadeRoutine(float target, UnityEvent callback)
        {
            float start = _canvasGroup.alpha;
            float t = 0f;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / fadeDuration);
                _canvasGroup.alpha = Mathf.Lerp(start, target, normalized);
                yield return null;
            }

            _canvasGroup.alpha = target;
            callback?.Invoke();
        }
    }
}
