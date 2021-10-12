using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class AnimatorSE : StateMachineBehaviour
    {
        public GameObject SoundSourceObject;
        public AudioClip audioClip;

        [Range(0, 1)]
        public float startTime;

        bool active = false;

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!active)
            {
                if (stateInfo.normalizedTime % 1 >= startTime)
                {
                    active = true;
                    var obj = Instantiate(SoundSourceObject);
                    var source  = obj.GetComponent<SEPlayer>();
                    obj.transform.position = animator.gameObject.transform.position;
                    source.SetAudioClip(audioClip);
                    source.Play();
                }
            }

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            active = false;
        }
    }
}