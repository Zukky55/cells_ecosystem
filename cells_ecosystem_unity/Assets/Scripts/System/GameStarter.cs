using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CellsEcosystem
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] GameObject starterObj;
        [SerializeField] TypefaceAnimator titleText;
        [SerializeField] TypefaceAnimator buttonText;
        [SerializeField] Button button;
        [SerializeField] iTween.EaseType easeType;
        [SerializeField] Vector3 from = Vector3.zero;
        [SerializeField] Vector3 to = Vector3.one;
        [SerializeField] float time = 1.5f;

        Ecosystem ecosystem;

        private void Awake()
        {
            ecosystem = Ecosystem.Instance;

            button.interactable = false;
        }
        public void DisplayStarter()
        {
            var hash = new Hashtable()
            {
                { "from",from },
                { "to",to},
                { "time",time},
                {"onupdate","ScaleAnimation" },
                {"onupdatetarget",gameObject },
                {"ignoretimescale",true },
                {"easetype",easeType },
                {"oncomplete","TextAnimation"},
                {"oncompletetarget",gameObject},
            };
            Time.timeScale = 0f;
            iTween.ValueTo(starterObj, hash);
        }
        public void GameStart()
        {
            Time.timeScale = 1f;
            starterObj.SetActive(false);
        }
        void ScaleAnimation(Vector3 nextVec)
        {
            starterObj.transform.localScale = nextVec;
        }
        void TextAnimation()
        {
            titleText.Play();
            titleText.onComplete.AddListener(() => buttonText.Play());
            buttonText.onComplete.AddListener(() => button.interactable = true);
        }
    }
}
