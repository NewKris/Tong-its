using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NordicBibo.Runtime {
    public class BlackScreen : MonoBehaviour {
        public float fadeTime;
        public float padding;
        public Image image;

        public IEnumerator FadeIn() {
            yield return new WaitForSeconds(padding);
            yield return Fade(Color.black, Color.clear);
        }

        public IEnumerator FadeOut() {
            yield return Fade(Color.clear, Color.black);
            yield return new WaitForSeconds(padding);
        }

        private IEnumerator Fade(Color from, Color to) {
            float t = 0;
            while (t < fadeTime) {
                image.color = Color.Lerp(from, to, t / fadeTime);
                
                t += Time.deltaTime;
                yield return null;
            }

            image.color = to;
        }
    }
}
