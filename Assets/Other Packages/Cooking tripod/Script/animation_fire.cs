using UnityEngine;
using System.Collections;

namespace TOF
{
    public class animation_fire : MonoBehaviour
    {
        void Update()
        {
            GetComponent<Animation>().Play("Ligth Fire Animation");
        }
    }
}
