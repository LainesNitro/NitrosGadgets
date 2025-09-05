using UnityEngine;
using UnityEngine.Audio;

namespace NitrosGadgets.Unity
{
    public class LampControlProxy : MonoBehaviour
    {
        public enum LampState
        {
            Off = 0,
            On = 1,
            Blinking = 2,
            None = 3
        }

        public IndicatorEmissionProxy lampInd;

        public LampState lampState;

        public AudioClip warningAudio;

        public AudioClip onStateBuzzingLoopAudio;

        public AudioClip blinkStateBuzzingLoopAudio;

        public AudioMixerGroup lampAudioMixerGroup;
    }
}
