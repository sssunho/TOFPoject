using UnityEngine;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using TOF;

public partial class FirstBT
{
    public GameObject hammerWindFx;

    float hammerWindCooltime = 0.0f;
    bool doingHammerWind = false;

    TaskStatus TriggerHammerWind()
    {
        if (hammerWindCooltime > 0.0f) return TaskStatus.Failure;
        if ((enemyManager.currentTarget.transform.position - transform.position).magnitude > enemyManager.maximumAggroRadius) return TaskStatus.Failure;
        anim.anim.SetBool("isChanneling", true);
        anim.PlayTargetAnimation("HammerWind Casting", true);
        SeeTarget();
        return TaskStatus.Success;
    }

    TaskStatus ChannelingHammerWind()
    {
        if (anim.anim.GetBool("isChanneling")) return TaskStatus.Continue;

        var inst = Instantiate(hammerWindFx);
        inst.transform.position = transform.position + Vector3.up * 0.1f;
        inst.transform.rotation = transform.rotation;
        inst.GetComponent<HammerWindFx>().target = enemyManager.currentTarget;

        hammerWindCooltime = 10.0f;
        curPatternDelay = 1.0f;
        strafingTimer = 2.0f;

        return TaskStatus.Success;
    }

    void SeeTarget()
    {
        if (enemyManager.currentTarget)
        {
            Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
            rel.y = 0;
            transform.rotation = Quaternion.LookRotation(rel);
        }
    }
}
