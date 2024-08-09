using UnityEngine;
using UnityEngine.UI;

namespace Assets.Helpers
{
    public class SoundVolume : MonoBehaviour
    {
        public float speedBack;
        public AudioSource source;
        public Slider volumeSlider;


        public void Start()
        {
            volumeSlider = volumeSlider == null ? GetComponent<Slider>() : volumeSlider;
            volumeSlider.onValueChanged.AddListener(Change);
        }

        public void Change(float value)
        {
            source.volume = Mathf.Clamp01(value);
        }

        private void Update()
        {
            var temp = source.volume + speedBack;
            source.volume = Mathf.Clamp01(temp);

            volumeSlider.value = source.volume;
        }
    }
}
