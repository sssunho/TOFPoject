using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorID
{
    static int isInteracting;
    static int isAttacking;
    static int isBlocking;

    public static void init()
    {
        isInteracting = Animator.StringToHash("isInteracting");
        isAttacking = Animator.StringToHash("isAttacking");
        isBlocking = Animator.StringToHash("isBlocking");
    }
}
