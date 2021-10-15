using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using TOF;

public class FirstBT : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree _tree;

    int detectionCounter = 0;
    EnemyManager enemyManager;
    EnemyAnimationManager anim;
    Quaternion lookTarget;

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
                .Do("Rotate To Target", RotateToTarget)
                .Condition("Interacting", () => {
                    return enemyManager.isInteracting; })
                .Condition("Idle", DetectPlayer)
                .Do("Chase", ChaseTarget)
                .Condition("Always True", () => true)
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
        if (enemyManager.currentTarget != null) return false;
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

    private TaskStatus ChaseTarget()
    {
        Vector3 rel = enemyManager.currentTarget.transform.position - transform.position;
        Debug.DrawRay(transform.position, rel * enemyManager.detectionRadius * 1.3f);
        if (rel.magnitude < enemyManager.maximumAggroRadius)
        {
            if (Vector3.Angle(rel, transform.forward) < 45.0f)
            {
                anim.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                anim.PlayTargetAnimation("Onehanded Light 1", true);
                lookTarget = Quaternion.LookRotation(rel.normalized);
                return TaskStatus.Success;
            }
        }

        if(rel.magnitude > enemyManager.detectionRadius * 1.3f)
        {
            anim.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            anim.PlayTargetAnimation("Sleep", false);
            enemyManager.currentTarget = null;
            enemyManager.navMeshAgent.enabled = false;
            return TaskStatus.Success;
        }

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

        return TaskStatus.Success;
    }

    private TaskStatus RotateToTarget()
    {
        if (enemyManager.canRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookTarget, 100 * Time.deltaTime);
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}