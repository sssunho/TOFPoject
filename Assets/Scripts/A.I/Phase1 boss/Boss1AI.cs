using UnityEngine;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using TOF;


namespace TOF
{
    public partial class Boss1AI : MonoBehaviour
    {
        [SerializeField]
        BehaviorTree _tree;

        AnimatorStateInfo upperbodyInfo;
        AnimatorStateInfo fullbodyInfo;

        EnemyManager enemyManager;
        EnemyStats enemyStats;
        EnemyAnimationManager animatorManager;

        CharacterManager currentTarget;

        float delayTimer = 0.0f;
        float stareTimer = 0.0f;
        float strafingTimer = 0.0f;
        float turnDelay = 0.0f;

        bool IsDelayed
        {
            get => delayTimer > 0 || stareTimer > 0 || strafingTimer > 0;
        }

        public bool isHit = false;

        public float speed = 10.0f;
        public float strafingSpeed = 10.0f;
        public float angularSpeed = 20.0f;

        public float meleeAttackRange = 5.0f;
        public float rangeAttackRange = 10.0f;
        public float detectRange = 20.0f;

        public bool debug = false;

        // 매 틱마다 Overlap 함수 호출을 방지하기 위해
        // detectCounter % 10 == 0  일때만 Detect를 수행합니다.
        int detectCounter = 0;

        private void OnDrawGizmos()
        {
            if (debug)
            {
                UnityEditor.Handles.color = new Color(1, 0, 0, 0.3f);
                UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, detectRange);
                UnityEditor.Handles.color = new Color(0, 1, 0, 0.3f);
                UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, rangeAttackRange);
                UnityEditor.Handles.color = new Color(0, 0, 1, 0.3f);
                UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.up, meleeAttackRange);
            }
        }

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyStats = GetComponent<EnemyStats>();
            animatorManager = GetComponentInChildren<EnemyAnimationManager>();

            var MeleeAttackNode = new BehaviorTreeBuilder(gameObject)
                .Selector("Melee Attack")
                    .Do("test", ()=> TaskStatus.Failure)
                .End();

            var RangeAttackNode = new BehaviorTreeBuilder(gameObject)
                .Selector("Range Attack")
                    .Do("test", () => TaskStatus.Failure)
                .End();

            _tree = new BehaviorTreeBuilder(gameObject)
                .Selector()
                    .Condition("Dead", () => enemyStats.isDead)
                    .Selector("Combat")
                        .Selector("Cooltime Phase")
                            .Do("Stare", StareBehaviour)
                            .Do("Delay", DelayBehaviour)
                        .End()
                        .Selector("Special Attack")
                            .Do("null", () => TaskStatus.Failure)
                        .End()
                        .Selector("Normal Attack")
                            .Splice(MeleeAttackNode.Build())
                            .Splice(RangeAttackNode.Build())
                        .End()
                    .End()
                    .Selector("Idle State")
                        .Do("Detect", DetectPlayer)
                        .Do("Idle", () => TaskStatus.Success)
                    .End()
                .End()
                .Build();
        }

        private void Update()
        {
            // Update our tree every frame
            _tree.Tick();
            UpdateAnimatorInfo();
            CoolTimer();
        }

        private void CoolTimer()
        {
            float delta = Time.deltaTime;
            delayTimer = Mathf.Clamp(delayTimer - delta, 0.0f, 30.0f);
            stareTimer = Mathf.Clamp(stareTimer - delta, 0.0f, 30.0f);
            strafingTimer = Mathf.Clamp(strafingTimer - delta, 0.0f, 30.0f);

            turnDelay = Mathf.Clamp(strafingTimer - delta, 0.0f, 30.0f);
        }

        private void UpdateAnimatorInfo()
        {
            fullbodyInfo = animatorManager.anim.GetCurrentAnimatorStateInfo(5);
            upperbodyInfo = animatorManager.anim.GetCurrentAnimatorStateInfo(4);
            isHit = animatorManager.anim.GetBool("isHit");
        }

        #region General Methods

        private void LookTarget(Vector3 target)
        {
            Vector3 rel = target - transform.position;
            rel.y = 0;
            Quaternion look = Quaternion.LookRotation(rel);
            transform.rotation = look;
        }

        private void LookTargetWithLerp(Vector3 target)
        {
            Vector3 rel = target - transform.position;
            rel.y = 0;
            Quaternion look = Quaternion.LookRotation(rel);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, angularSpeed * Time.deltaTime);
        }

        private void RotateWithRootmotion()
        {
            Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
            rel.y = 0;
            float angle = Vector3.Angle(rel, transform.forward);
            Vector3 cross = Vector3.Cross(rel, transform.forward);
            if (cross.y < 0) angle *= -1;

            if (angle > 45 && angle < 135)
                animatorManager.PlayTargetAnimation("Turn Left", true);
            else if (angle < -45 && angle > -135)
                animatorManager.PlayTargetAnimation("Turn Right", true);
            else if (angle <= -135 || angle >= 135)
                animatorManager.PlayTargetAnimation("Turn Behind", true);
        }

        private bool IsInRange(Vector3 target, float range)
        {
            Vector3 rel = target - transform.position;
            rel.y = 0;
            return rel.magnitude < range;
        }

        private bool IsInAngle(Vector3 target, float angle)
        {
            Vector3 rel = target - transform.position;
            rel.y = 0;
            return Vector3.Angle(rel, transform.forward) < angle;
        }

        #endregion

        #region Idle State Behaviours

        private TaskStatus DetectPlayer()
        {
            if(detectCounter++ % 10 == 0)
            {
                var colliders = Physics.OverlapSphere(transform.position, detectRange);
                
                for(int i = 0; i < colliders.Length; i++)
                {
                    currentTarget = colliders[i].GetComponent<PlayerManager>();

                    if (currentTarget != null) return TaskStatus.Success;
                }
            }

            return TaskStatus.Failure;
        }

        #endregion

        #region Cooltime Phase Behaviours

        private TaskStatus StrafingBehaviour()
        {
            if (currentTarget == null) return TaskStatus.Failure;
            if (strafingTimer <= 0.0f) return TaskStatus.Failure;

            LookTargetWithLerp(currentTarget.transform.position);

            return TaskStatus.Success;
        }

        private TaskStatus StareBehaviour()
        {
            if (currentTarget == null) return TaskStatus.Failure;
            if (stareTimer <= 0.0f) return TaskStatus.Failure;

            if(turnDelay <= 0.0f)
            {
                RotateWithRootmotion();
                turnDelay += 1.0f;
            }

            return TaskStatus.Continue;
        }

        private TaskStatus DelayBehaviour()
        {
            if (delayTimer <= 0.0f) return TaskStatus.Failure;

            return TaskStatus.Success;
        }

        #endregion

        #region Normal Attack Behaviours

        private TaskStatus MeleeCombo1()
        {
            return TaskStatus.Failure;
        }

        #endregion
    }
}