using UnityEngine;

namespace CellsEcosystem
{
    class SoundEvent : MonoBehaviour
    {
        public void PlaySE(int id)
        {
            AudioManager.Instance.PlaySE_2D((SE_ID)id);
        }

        public void PlaySE(SE_ID id)
        {
            AudioManager.Instance.PlaySE_2D(id);
        }

        public void PlayBGM(int id)
        {
            AudioManager.Instance.PlayBGM((BGM_ID)id);
        }

        public void PlayBGM(BGM_ID id)
        {
            AudioManager.Instance.PlayBGM(id);
        }

        public void StopBGM(float outTime)
        {
            AudioManager.Instance.StopBGM(outTime);
        }
    }
}
