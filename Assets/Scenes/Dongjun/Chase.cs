using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;

public class Chase : ActionBase
{
    protected override TaskStatus OnUpdate()
    {
        Debug.Log(Owner.name);
        return TaskStatus.Success;
    }
}

public static partial class BehaviorTreeBuilderExtensions
{
    public static BehaviorTreeBuilder Chase(this BehaviorTreeBuilder builder, string name = "My Action")
    {
        return builder.AddNode(new Chase { Name = name });
    }
}

public class ExampleUsage : MonoBehaviour
{
    public void Awake()
    {
        var bt = new BehaviorTreeBuilder(gameObject)
            .Sequence()
                .Chase()
            .End();
    }
}