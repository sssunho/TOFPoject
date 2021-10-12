using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class SEPlayer : MonoBehaviour
    {
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SetAudioClip(AudioClip clip)
        {
            audioSource.clip = clip;
        }

        public void Play()
        {
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length + 0.1f);
        }
    }
}