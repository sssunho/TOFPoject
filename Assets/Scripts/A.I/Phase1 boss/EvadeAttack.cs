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
        private class NormalAttack : ActionBase
        {
            AnimatorManager animatorManager;
            EnemyStats stats;
            Boss1AI ai;

            protected override void OnInit()
            {
                animatorManager = Owner.GetComponentInChildren<AnimatorManager>();
                stats = Owner.GetComponent<EnemyStats>();
                ai = Owner.GetComponent<Boss1AI>();
            }

            protected override void OnStart()
            {
                animatorManager.anim.SetTrigger("attackTrigger");
                animatorManager.anim.SetInteger("attackID", 2);
                animatorManager.SetInteraction(true);

                Damage damage = new Damage();
                damage.teamID = stats.teamIDNumber;
                damage.value = 20;
                damage.poiseDamage = 100;
                damage.reaction = HitReaction.NORMAL;

                stats.currentDamage = damage;
                ai.isAttacking = true;
            }

            protected override TaskStatus OnUpdate()
            {
                if (ai.isHit) return TaskStatus.Failure;

                if (!ai.isAttacking && !animatorManager.anim.IsInTransition(5))
                    return TaskStatus.Success;

                if(ai.isAttacking)
                {
                    float normalizedTime = ai.fullbodyInfo.normalizedTime % 1.0f;
                    if (normalizedTime > 0.0f && normalizedTime < 0.2f)
                        ai.LookTargetWithLerp(ai.currentTarget.transform.position, 8.0f);

                    if (normalizedTime > 0.1f && normalizedTime < 0.3f)
                        ai.MoveToDirection(Owner.transform.forward, 5.0f);
                }


                return TaskStatus.Continue;
            }

            protected override void OnExit()
            {
                ai.delayTimer += 0.5f;
                ai.stareTimer += 1.1f;
            }
        }

        private class MagicAttack1 : ActionBase
        {
            AnimatorManager animatorManager;
            EnemyStats stats;
            Boss1AI ai;

            protected override void OnInit()
            {
                animatorManager = Owner.GetComponentInChildren<AnimatorManager>();
                stats = Owner.GetComponent<EnemyStats>();
                ai = Owner.GetComponent<Boss1AI>();
            }

            protected override TaskStatus OnUpdate()
            {
                
            }

            protected override void OnExit()
            {
                ai.delayTimer += 0.5f;
                ai.stareTimer += 0.3f;
            }
        }

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
                    return TaskStatus.Failure;
                }

                if (!ai.isEvadeAttack && !animatorManager.anim.IsInTransition(5))
                {
                    return TaskStatus.Success;
                }

                float normalizedTime = animatorManager.anim.GetCurrentAnimatorStateInfo(5).normalizedTime % 1.0f;
                float speed = 8.0f * Mathf.Sin(Mathf.PI * normalizedTime);
                ai.MoveToDirection(-Owner.transform.forward, speed);

                return TaskStatus.Continue;
            }

            protected override void OnExit()
            {
                ai.delayTimer += 0.7f;
                ai.stareTimer += 0.3f;
            }
        }

        private class HammerFall : ActionBase
        {
            AnimatorManager animatorManager;
            EnemyStats stats;
            EnemyLocomotionManager enemyLocomotion;
            Boss1AI ai;
            float attackRange = 5.0f;
            bool triggered = false;
            bool waitOneTick = true;

            protected override void OnInit()
            {
                animatorManager = Owner.GetComponentInChildren<AnimatorManager>();
                stats = Owner.GetComponent<EnemyStats>();
                enemyLocomotion = Owner.GetComponent<EnemyLocomotionManager>();
                ai = Owner.GetComponent<Boss1AI>();

            }

            protected override void OnStart()
            {
                animatorManager.anim.SetTrigger("attackTrigger");
                animatorManager.anim.SetInteger("attackID", 1);
                stats.currentDamage.value = 50;
                stats.currentDamage.poiseDamage = 100;
                stats.currentDamage.reaction = HitReaction.DOWN;
                stats.currentDamage.teamID = stats.teamIDNumber;
                ai.isHammerFall = true;
            }

            protected override TaskStatus OnUpdate()
            {
                if(waitOneTick)
                {
                    waitOneTick = false;
                    return TaskStatus.Continue;
                }

                if (ai.isHit)
                {
                    return TaskStatus.Failure;
                }

                if (!triggered)
                {
                    Vector3 rel = ai.currentTarget.transform.position - Owner.transform.position;

                    if(rel.magnitude < attackRange)
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
                        else
                            enemyLocomotion.ignoreGravity = false;
                        ai.MoveToDirection(Owner.transform.forward, speed);
                        return TaskStatus.Continue;
                    }

                    return TaskStatus.Success;
                }
            }

            protected override void OnExit()
            {
                enemyLocomotion.ignoreGravity = false;
                triggered = false;
                waitOneTick = true;
                ai.hammerfallDelay += 20.0f;
                ai.delayTimer += 1.0f;
                ai.stareTimer += 1.1f;
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
