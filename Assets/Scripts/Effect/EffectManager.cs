using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EffectManager : MonoBehaviour
    {
        private static EffectManager instance = null;

        public List<Effect> preloadEffectList;
        public List<int> nums;

        private static List<List<Effect>> effectList = new List<List<Effect>>();

        PlayerStats test;

        void Awake()
        {
            if (null == instance)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void Start()
        {
            for(int i = 0; i < preloadEffectList.Count && i < nums.Count; i++)
            {
                List<Effect> tempList = new List<Effect>();
                for (int j = 0; j < nums[i]; j++)
                {
                    var effect = Instantiate(preloadEffectList[i]);
                    effect.transform.parent = this.transform;
                    effect.gameObject.SetActive(false);
                    tempList.Add(effect);
                }
                effectList.Add(tempList);
            }
        }

        public static EffectManager Instance
        {
            get
            {
                if (null == instance)
                {
                    return null;
                }
                return instance;
            }
        }

        public static void PlayEffect(int idx, Vector3 position)
        {
            for (int i = 0; i < effectList[idx].Count; i++)
            {
                if (effectList[idx][i].gameObject.activeInHierarchy) continue;

                effectList[idx][i].transform.position = position;
                effectList[idx][i].gameObject.SetActive(true);
                break;
            }
        }

        public static Effect PlayEffect(int idx, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            for(int i = 0; i < effectList[idx].Count; i++)
            {
                if (effectList[idx][i].gameObject.activeInHierarchy) continue;

                effectList[idx][i].transform.position = position;
                effectList[idx][i].transform.rotation = rotation;
                effectList[idx][i].transform.localScale = scale;
                effectList[idx][i].gameObject.SetActive(true);
                return effectList[idx][i];
            }

            return null;
        }

    }
}