using UnityEngine;
using System.Collections.Generic;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using TOF;

public partial class FirstBT : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree _tree;
    public GameObject magicPrefab;
    public GameObject magicMissilePrefab;

    int detectionCounter = 0;
    EnemyManager enemyManager;
    EnemyStats enemyStats;
    EnemyAnimationManager anim;

    float curPatternDelay = 0.0f;
    float curRecoveryRotationDelay = 0.0f;
    float castTimer = 0.0f;
    float strafingTimer = 0.0f;

    [Header("Special Cool Time")]
    public float sp_CoolTime1 = 15;
    public float sp_CoolTime2 = 7;
    public float sp_CoolTime3 = 10;
    public float sp_CoolTime4 = 15;

    float switchStrafingDirectionDelay = 0.0f;
    int normalAttackCounter = 0;
    int normalMagicCounter = 0;
    int AttackComboCount = 0;
    bool isStrafingRight = true;

    List<Projectile> projectiles = new List<Projectile>();
    MagicMissile magicMissile;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyStats = GetComponent<EnemyStats>();
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
                    .Do("Strafing", Strafing)
                    .Selector("Special")
                        .Selector("Special1")
                            .Do("Chasing", () => SP_Chasing(sp_CoolTime1))
                            .Do("Attack", SP_Attack1)
                        .End()
                        .Sequence("test")
                            .Do("trigger", TriggerHammerWind)
                            .Do("channeling", ChannelingHammerWind)
                        .End()
                        .Selector("Special3")
                            .Do("Chasing", () => SP_Chasing(sp_CoolTime3))
                            .Do("Attack", SP_Attack3)
                        .End()
                        .Selector("Special4")
                            //.Condition("Special Attack 4 Condition", () => sp_CurTime4 >= sp_CoolTime4 && enemyStats.currentHealth <= enemyStats.maxHealth / 2)
                            .Do("Special4", () => TaskStatus.Failure)
                        .End()
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
        CoolTimer();
    }
    
    private void CoolTimer()
    {
        sp_CoolTime1 -= Time.deltaTime;
        if (sp_CoolTime1 < 0) sp_CoolTime1 = 0;
        sp_CoolTime2 -= Time.deltaTime;
        if (sp_CoolTime2 < 0) sp_CoolTime2 = 0;
        sp_CoolTime3 -= Time.deltaTime;
        if (sp_CoolTime3 < 0) sp_CoolTime3 = 0;
        sp_CoolTime4 -= Time.deltaTime;
        if (sp_CoolTime4 < 0) sp_CoolTime4 = 0;

        hammerWindCooltime -= Time.deltaTime;
        if (hammerWindCooltime < 0) hammerWindCooltime = 0;
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

    private bool ChasingPlayer()
    {
        Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;

        if (rel.magnitude < enemyManager.maximumAggroRadius) return false;

        anim.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

        enemyManager.navMeshAgent.enabled = true;
        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

        enemyManager.navMeshAgent.updateRotation = false;
        enemyManager.navMeshAgent.updatePosition = false;

        Vector3 targetVelocity = enemyManager.navMeshAgent.desiredVelocity;
        Vector3 lookPos = enemyManager.currentTarget.transform.position - transform.position;
        targetVelocity.y -= 9.81f;
        lookPos.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(lookPos);
        enemyManager.controller.gameObject.transform.rotation = Quaternion.Slerp(enemyManager.controller.gameObject.transform.rotation, targetRot, Time.deltaTime * 10.0f);

        enemyManager.controller.Move(targetVelocity * Time.deltaTime);
        enemyManager.navMeshAgent.velocity = enemyManager.controller.velocity;

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

        if (!ChasingPlayer()) return TaskStatus.Failure;

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

    private TaskStatus Strafing()
    {
        if (strafingTimer <= 0)
        {
            anim.anim.SetFloat("isStrafing", 0);
            strafingTimer = 0;
            return TaskStatus.Failure;
        }
        strafingTimer -= Time.deltaTime;
        anim.anim.SetFloat("isStrafing", 1);

        if (enemyManager.isInteracting) return TaskStatus.Continue;

        float strafingSpeed = 1.0f;
        float strafingRotSpeed = 10.0f;
        Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
        rel.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(rel), strafingRotSpeed * Time.deltaTime);

        if (switchStrafingDirectionDelay <= 0)
        {
            switchStrafingDirectionDelay = 3.0f;
            if (Random.Range(0, 2) == 0) isStrafingRight = !isStrafingRight;

            if (isStrafingRight) anim.anim.SetFloat("Horizontal", 0.4f);
            else anim.anim.SetFloat("Horizontal", -0.4f);

        }

        switchStrafingDirectionDelay -= Time.deltaTime;

        Vector3 moveDirection = transform.right * (isStrafingRight ? 1 : -1);
        moveDirection.y -= 9.81f;
        anim.anim.SetFloat("Vertical", 0);
        enemyManager.controller.Move(strafingSpeed * moveDirection * Time.deltaTime);
        enemyManager.navMeshAgent.velocity = enemyManager.controller.velocity;

        return TaskStatus.Continue;
    }

    #region Special Attack

    // #. 1 

    private bool SP_ChasingTarget()
    {
        Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
        if (rel.magnitude < enemyManager.maximumAggroRadius) return false;

        anim.anim.SetFloat("Vertical", 2, 0.1f, Time.deltaTime);

        enemyManager.navMeshAgent.enabled = true;
        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

        enemyManager.navMeshAgent.updateRotation = false;
        enemyManager.navMeshAgent.updatePosition = false;

        Vector3 targetVelocity = enemyManager.navMeshAgent.desiredVelocity;
        Vector3 lookPos = enemyManager.currentTarget.transform.position - transform.position;
        lookPos.y = 0; 
        targetVelocity.y -= 9.81f;
        Quaternion targetRot = Quaternion.LookRotation(lookPos);
        enemyManager.controller.gameObject.transform.rotation = Quaternion.Slerp(enemyManager.controller.gameObject.transform.rotation, targetRot, Time.deltaTime * 10.0f);

        enemyManager.controller.Move(targetVelocity * Time.deltaTime);
        enemyManager.navMeshAgent.velocity = enemyManager.controller.velocity;
        return true;
    }

    private TaskStatus SP_Chasing(float coolTime)
    {
        // #. 1 스킬 쿨타임인 경우 fail
        if (coolTime > 0)
            return TaskStatus.Failure;

        if (!SP_ChasingTarget()) return TaskStatus.Failure;

        return TaskStatus.Continue;
    }

    private TaskStatus SP_Attack1()
    {
        // #. 1 스킬 쿨타임인 경우 fail
        if (sp_CoolTime1 > 0)
            return TaskStatus.Failure;

        // #. 2 스킬 사용 가능하면 내려찍기
        Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
        Quaternion look = Quaternion.LookRotation(rel);
        transform.rotation = look;
        anim.anim.SetFloat("Vertical", 0);
        anim.PlayTargetAnimation("Special Attack 1", true);
        sp_CoolTime1 = 15;
        curPatternDelay = 1.5f;
        return TaskStatus.Success;
    }

    private TaskStatus Special2_Debuff()
    {
        return TaskStatus.Success;
    }

    private TaskStatus SP_GuardCheck(float coolTime)
    {
        if (coolTime > 0)
            return TaskStatus.Failure;
        // #. 변경 부탁
        var player = FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        if(player != null)
        {
            if (player.isBlocking)
                return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    private TaskStatus SP_Attack3()
    {
        if (enemyManager.isInteracting) return TaskStatus.Continue;

        if (SP_GuardCheck(sp_CoolTime3) == TaskStatus.Success)
        {
            Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
            Quaternion look = Quaternion.LookRotation(rel);
            transform.rotation = look;
            anim.anim.SetFloat("Vertical", 0);
            anim.PlayTargetAnimation("Special Attack 3", true);
            sp_CoolTime3 = 10;
            curPatternDelay += 2.0f;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    private TaskStatus Special4_Anger()
    {
        return TaskStatus.Success;
    }

    #endregion
}