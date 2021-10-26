using UnityEngine;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using TOF;

namespace TOF
{
    public partial class Boss1AI
    {
        private class EvadeAttack : ActionBase
        {
            AnimatorManager animatorManager;
            Boss1AI ai;
            
            protected override void OnInit()
            {
                animatorManager = Owner.GetComponentInChildren<AnimatorManager>();
                ai = Owner.GetComponent<Boss1AI>();
            }

            protected override void OnStart()
            {
                animatorManager.anim.SetTrigger("attackTrigger");
                animatorManager.anim.SetInteger("attackID", 0);
                animatorManager.SetInteraction(true);
                ai.isEvadeAttack = true;
            }

            protected override TaskStatus OnUpdate()
            {
                if (ai.isHit)
                {
                    ai.delayTimer += 10.0f;
                    return TaskStatus.Failure;
                }

                if (!ai.isEvadeAttack && !animatorManager.anim.IsInTransition(5))
                {
                    ai.delayTimer += 10.0f;
                    return TaskStatus.Success;
                }

                float normalizedTime = animatorManager.anim.GetCurrentAnimatorStateInfo(5).normalizedTime % 1.0f;
                float speed = 8.0f * Mathf.Sin(Mathf.PI * normalizedTime);
                ai.MoveToDirection(-Owner.transform.forward, speed);

                return TaskStatus.Continue;
            }

        }

        private class HammerFall : ActionBase
        {
            AnimatorManager animatorManager;
            EnemyLocomotionManager enemyLocomotion;
            Boss1AI ai;
            bool triggered = false;
            bool waitOneTick = true;

            protected override void OnInit()
            {
                animatorManager = Owner.GetComponentInChildren<AnimatorManager>();
                enemyLocomotion = Owner.GetComponent<EnemyLocomotionManager>();
                ai = Owner.GetComponent<Boss1AI>();

            }

            protected override void OnStart()
            {
                animatorManager.anim.SetTrigger("attackTrigger");
                animatorManager.anim.SetInteger("attackID", 1);
                ai.isHammerFall = true;
            }

            protected override TaskStatus OnUpdate()
            {
                if(waitOneTick)
                {
                    waitOneTick = false;
                    return TaskStatus.Continue;
                }

                if (!triggered)
                {
                    Vector3 rel = ai.currentTarget.transform.position - Owner.transform.position;

                    if(rel.magnitude < ai.meleeAttackRange)
                    {
                        triggered = true;
                        ai.LookTarget(ai.currentTarget.transform.position);
                        animatorManager.anim.SetTrigger("attackTrigger");
                        animatorManager.SetInteraction(true);
                        enemyLocomotion.ignoreGravity = true;
                    }

                    ai.ChasePosition(ai.currentTarget.transform.position);

                    return TaskStatus.Continue;
                }
                else
                {
                    if (ai.isHammerFall || animatorManager.anim.IsInTransition(5))
                    {
                        float normalizedTime = animatorManager.anim.GetCurrentAnimatorStateInfo(5).normalizedTime % 1.0f;
                        float speed = SpeedProfile(normalizedTime);
                        if (normalizedTime < 0.4f)
                            ai.LookTargetWithLerp(ai.currentTarget.transform.position);
                        ai.MoveToDirection(Owner.transform.forward, speed);
                        return TaskStatus.Continue;
                    }

                    ai.delayTimer += 3.0f;
                    return TaskStatus.Success;
                }
            }

            protected override void OnExit()
            {
                enemyLocomotion.ignoreGravity = false;
                triggered = false;
                waitOneTick = true;
                ai.StopMoveAnimation();
            }

            private float SpeedProfile(float t)
            {
                float offset = 0.4f;
                float speed = 3.0f;

                if (t < offset) return speed;
                else return 0;
            }
        }

    }
}
