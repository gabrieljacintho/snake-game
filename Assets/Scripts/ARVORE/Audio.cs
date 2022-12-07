using ARVORE.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ARVORE
{
    [System.Serializable]
    public class Audio
    {
        public List<AudioClip> samples;
        public float volumeScale = 1f;


        public void Play()
        {
            if (samples != null && samples.Count > 0 && AudioManager.Instance != null)
                AudioManager.Instance.PlayOneShot(samples[Random.Range(0, samples.Count)], volumeScale);
        }
    }
}
