using UnityEngine;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using TOF;

public class FirstBT : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree _tree;
    public GameObject magicPrefab;
    public GameObject magicMissilePrefab;

    int detectionCounter = 0;
    EnemyManager enemyManager;
    EnemyAnimationManager anim;

    float curPatternDelay = 0.0f;
    float curRecoveryRotationDelay = 0.0f;
    float castTimer = 0.0f;
    int normalAttackCounter = 0;
    int normalMagicCounter = 0;
    int AttackComboCount = 0;
    List<Projectile> projectiles = new List<Projectile>();
    MagicMissile magicMissile;

    private void OnDrawGizmos()
    {
        if (enemyManager)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(transform.position, enemyManager.detectionRadius);
            if(enemyManager.navMeshAgent.enabled)
            {
                var corners = enemyManager.navMeshAgent.path.corners;
                foreach (var corner in corners)
                    Gizmos.DrawSphere(corner, 0.05f);
            }
        }
    }

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        anim = GetComponentInChildren<EnemyAnimationManager>();

        _tree = new BehaviorTreeBuilder(gameObject)
            .Selector()
                .Sequence("Idle")
                    .Condition("Have a target", () => enemyManager.currentTarget == null)
                    .Condition("Detecting", DetectPlayer)
                .End()
                .Selector("Combat")
                    .Sequence("Pattern Delay")
                        .Condition("Delay", () => curPatternDelay > 0)
                        .Do("Recovery", Recovery)
                        .Do("RotateWithRootmotion", RotateWithRootmotion)
                    .End()
                    .SelectorRandom("Special")
                        .Do("Special1", () => TaskStatus.Failure)
                        .Do("Special2", () => TaskStatus.Failure)
                        .Do("Special3", () => TaskStatus.Failure)
                        .Do("Special4", () => TaskStatus.Failure)
                    .End()
                    .SelectorRandom("Normal")
                        .Selector("Phase")
                            .SelectorRandom("Phase1")
                                .Selector("Normal Attack")
                                    .Do("Chase", ChaseTarget)
                                    .Do("Attack target", AttackTarget)
                                .End()
                                .SelectorRandom("Magic")
                                    .Do("Magic1", SpawnProjectiles)
                                    .Do("Magic2", Magic2)
                                .End()
                            .End()
                            .SelectorRandom("Phase2")
                                .Do("Normal1", () => TaskStatus.Failure)
                                .Do("Normal2", () => TaskStatus.Failure)
                                .Do("Normal3", () => TaskStatus.Failure)
                            .End()
                        .End()
                    .End()
                .End()
            .End()
            .Build();
    }
    private void Start()
    {
        anim.PlayTargetAnimation("Sleep", false);
    }

    private void Update()
    {
        // Update our tree every frame
        _tree.Tick();
    }

    private bool AlwaysTrue()
    {
        return true;
    }

    private bool DetectPlayer()
    {
        if (detectionCounter++ % 10 != 0) return true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, 1 << 11);
        foreach(Collider collider in colliders)
        {
            CharacterStats stats = collider.GetComponent<CharacterStats>();
            if (stats)
            {
                enemyManager.currentTarget = stats;
                anim.PlayTargetAnimation("Get Up", true);
                return false;
            }
        }

        return true;
    }

    private TaskStatus AttackTarget()
    {
        if (enemyManager.isInteracting) return TaskStatus.Continue;

        Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
        Quaternion look = Quaternion.LookRotation(rel);

        switch (AttackComboCount)
        {
            case 0:
                anim.anim.SetFloat("Vertical", 0);
                anim.PlayTargetAnimation("Onehanded Light 1", true);
                transform.rotation = look;
                AttackComboCount++;
                return TaskStatus.Continue;
            case 1:
                anim.PlayTargetAnimation("Onehanded Light 2", true);
                AttackComboCount++;
                return TaskStatus.Continue;
            default:
                AttackComboCount = 0;
                curPatternDelay += 2.0f;
                return TaskStatus.Success;
        }
    }

    private TaskStatus ChaseTarget()
    {
        if (enemyManager.isInteracting) return TaskStatus.Continue;

        Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;

        if (rel.magnitude < enemyManager.maximumAggroRadius) return TaskStatus.Failure;

        anim.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

        enemyManager.navMeshAgent.enabled = true;
        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

        enemyManager.navMeshAgent.updateRotation = false;
        enemyManager.navMeshAgent.updatePosition = false;

        Vector3 targetVelocity = enemyManager.navMeshAgent.desiredVelocity;
        Vector3 lookPos = enemyManager.currentTarget.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(lookPos);
        enemyManager.controller.gameObject.transform.rotation = Quaternion.Slerp(enemyManager.controller.gameObject.transform.rotation, targetRot, Time.deltaTime * 10.0f);

        enemyManager.controller.Move(targetVelocity * Time.deltaTime);
        enemyManager.navMeshAgent.velocity = enemyManager.controller.velocity;

        return TaskStatus.Continue;
    }

    private TaskStatus RotateToTarget()
    {
        if (enemyManager.canRotate && enemyManager.currentTarget != null)
        {
            Quaternion look = Quaternion.LookRotation(enemyManager.currentTarget.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, 100 * Time.deltaTime);
        }

        return TaskStatus.Success;
    }

    private TaskStatus Recovery()
    {
        if(curPatternDelay < 0)
        {
            curPatternDelay = 0.0f;
            curRecoveryRotationDelay = 0.0f;
            return TaskStatus.Failure;
        }

        curPatternDelay -= Time.deltaTime;

        return TaskStatus.Success;
    }

    private TaskStatus RotateWithRootmotion()
    {
        if (curRecoveryRotationDelay < 0)
        {
            curRecoveryRotationDelay = 1.0f;
            Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
            rel.y = 0;
            float angle = Vector3.Angle(rel, transform.forward);
            Vector3 cross = Vector3.Cross(rel, transform.forward);
            if (cross.y < 0) angle *= -1;

            if (angle > 45 && angle < 135)
                anim.PlayTargetAnimation("Turn Left", true);
            else if (angle < -45 && angle > -135)
                anim.PlayTargetAnimation("Turn Right", true);
            else if (angle <= -135 || angle >= 135)
                anim.PlayTargetAnimation("Turn Behind", true);
        }

        curRecoveryRotationDelay -= Time.deltaTime;

        return TaskStatus.Success;
    }

    private TaskStatus Magic2()
    {
        if(magicMissile == null)
        {
            var inst = Instantiate(magicMissilePrefab);
            anim.anim.ResetTrigger("endOfCasting");
            castTimer = 0;
            inst.transform.position = transform.position + 3.5f * Vector3.up;
            inst.transform.rotation = transform.rotation;
            magicMissile = inst.GetComponent<MagicMissile>();
            magicMissile.target = enemyManager.currentTarget.gameObject;
            magicMissile.owner = gameObject;
            anim.PlayTargetAnimation("Cast Start", true);
            return TaskStatus.Continue;
        }
        else
        {
            castTimer += Time.deltaTime;
            if (castTimer > 4.0f)
            {
                magicMissile.Shoot();
                curPatternDelay += 2.0f;
                anim.anim.SetTrigger("endOfCasting");
                return TaskStatus.Success;
            }
            else
                return TaskStatus.Continue;
        }
    }

    private TaskStatus SpawnProjectiles()
    {
        if (projectiles.Count > 0) return TaskStatus.Failure;

        float projAngle = 60;

        for (int i = 0; i < 5; i++)
        {
            var inst = Instantiate(magicPrefab);
            inst.transform.position = transform.position;
            inst.transform.Rotate(transform.forward, (projAngle / 2.0f) - ((projAngle / 4.0f) * (float)i));
            inst.transform.parent = gameObject.transform;
            var proj = inst.GetComponent<Projectile>();
            proj.owner = gameObject;
            proj.velocity = new Vector3(0, 8, 0);
            proj.running = true;
            projectiles.Add(proj);
        }

        Invoke("StopProjectiles", 0.3f);
        Invoke("ShootProjectiles", 2.0f);

        curPatternDelay = 3.0f;

        return TaskStatus.Success;
    }

    private void StopProjectiles()
    {
        foreach(Projectile proj in projectiles)
            proj.running = false;
    }

    private void ShootProjectiles()
    {
        foreach (Projectile proj in projectiles)
        {
            if(enemyManager.currentTarget)
                proj.transform.rotation = Quaternion.LookRotation(enemyManager.currentTarget.transform.position - proj.transform.position + 0.5f * Vector3.up);
            proj.running = true;
            proj.velocity = 20.0f * Vector3.forward;
            proj.EnableDamageCollider();
            Destroy(proj.gameObject, 3.0f);
        }
        projectiles.Clear();
    }
}