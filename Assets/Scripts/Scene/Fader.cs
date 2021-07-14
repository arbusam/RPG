using System.Collections;
using UnityEngine;

namespace RPG.Scene
{
    public class Fader : MonoBehaviour {

        Coroutine activeFadeOut = null;
        Coroutine activeFadeIn = null;

        public void FadeOutImmediate()
        {
            GetComponent<CanvasGroup>().alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            if (activeFadeOut != null)
            {
                StopCoroutine(activeFadeOut);
            }
            activeFadeOut = StartCoroutine(FadeOutRoutine(time));
            yield return activeFadeOut;
        }

        private IEnumerator FadeOutRoutine(float time)
        {
            while (GetComponent<CanvasGroup>().alpha < 1)
            {
                GetComponent<CanvasGroup>().alpha += Time.unscaledDeltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            if (activeFadeIn != null)
            {
                StopCoroutine(activeFadeIn);
            }
            activeFadeOut = StartCoroutine(FadeInRoutine(time));
            yield return activeFadeIn;
        }

        private IEnumerator FadeInRoutine(float time)
        {
            while (GetComponent<CanvasGroup>().alpha > 0)
            {
                GetComponent<CanvasGroup>().alpha -= Time.unscaledDeltaTime / time;
                yield return null;
            }
        }

    }
}
