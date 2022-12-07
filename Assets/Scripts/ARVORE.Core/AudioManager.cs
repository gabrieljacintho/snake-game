using UnityEngine;

namespace ARVORE.Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioSource audioSource;


        public void PlayOneShot(AudioClip audioClip, float volumeScale = 1f)
        {
            if (audioSource != null) audioSource.PlayOneShot(audioClip, volumeScale);
        }
    }
}
