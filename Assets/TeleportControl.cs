using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class TeleportControl : MonoBehaviour
    {
        Text text;
        Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<Text>();
        }
    }
}

