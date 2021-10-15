using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class FirstBT : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree _tree;

    private void Awake()
    {
        _tree = new BehaviorTreeBuilder(gameObject)
            .Sequence()
                .Condition("Custom Condition", () => {
                    return true;
                })
                .Do("Custom Action", () => {
                    return TaskStatus.Success;
                })
            .End()
            .Build();
    }

    private void Update()
    {
        // Update our tree every frame
        _tree.Tick();
    }
}