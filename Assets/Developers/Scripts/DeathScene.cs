using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Developers.Scripts
{
    public class DeathScene : MonoBehaviour
    {
        [SerializeField]
        private Image fadeImage;

        [FormerlySerializedAs("fadeSpeed")]
        [SerializeField]
        private float fadeTime;

        [SerializeField]
        private bool triggerFadeIn;

        [SerializeField]
        private bool triggerFadeOut;

        private IEnumerator FadeIn()
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
            float currentTime = 0;
            while (fadeImage.color.a < 1)
            {
                currentTime = (currentTime + Time.deltaTime) / fadeTime;
                fadeImage.color = new Color(
                    fadeImage.color.r,
                    fadeImage.color.g,
                    fadeImage.color.b,
                    currentTime * 255
                );
                yield return null;
            }
        }

        private IEnumerator FadeOut()
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            while (fadeImage.color.a > 0)
            {
                fadeImage.color = new Color(
                    fadeImage.color.r,
                    fadeImage.color.g,
                    fadeImage.color.b,
                    fadeImage.color.a - fadeTime * Time.deltaTime
                );
                yield return null;
            }
            fadeImage.gameObject.SetActive(false);
        }

        private void Update()
        {
            // testing purposes
            if (triggerFadeIn)
            {
                StartCoroutine(FadeIn());
                triggerFadeIn = false;
            }
            if (triggerFadeOut)
            {
                StartCoroutine(FadeOut());
                triggerFadeOut = false;
            }
        }
    }
}
