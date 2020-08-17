using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Game2D
{
    public sealed class SoundPlayer : MonoBehaviour
    {

        private static float MIN_VOLUME = -80.00f;
        private static float DEFAULT_VOLUME = 0.00f;
        private static float MAX_VOLUME = 20.00f;
        private static float MIN_SLIDER_VALUE = 0.0f;
        private static float DEFAULT_SLIDER_VALUE = 0.8f;


        [SerializeField] private List<AudioSource> _sources;
        [SerializeField] private AudioMixer _audioMixer;

        [SerializeField] private Toggle _toggle;
        [SerializeField] private Slider _slider;


        public void MuteAllSound()
        {
            AudioMixerGroup masterGroup = _audioMixer.FindMatchingGroups("Master")[0];

            if (_toggle.isOn == true)
            {
                masterGroup.audioMixer.SetFloat("MasterVolume", MIN_VOLUME);
                _slider.value = MIN_SLIDER_VALUE;
            }

            if (_toggle.isOn == false)
            {
                masterGroup.audioMixer.SetFloat("MasterVolume", DEFAULT_VOLUME);
                _slider.value = DEFAULT_SLIDER_VALUE;
            }
        }

        public void SetMasterSoundLevel()
        {
            var sliderValue = _slider.value;
            AudioMixerGroup masterGroup = _audioMixer.FindMatchingGroups("Master")[0];
            masterGroup.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, Mathf.Clamp01(sliderValue)));
        }

        public void PlaySound(SoundEffectType soundEffect, bool active)
        {
            AudioSource audioInit = _sources.Find(e => e.clip.name.Equals(soundEffect.ToString()));

            if (audioInit != null)
            {
                if (active)
                {
                    audioInit.Play();
                }
                else
                {
                    audioInit.Stop();
                }
            }          
        }


    }
}