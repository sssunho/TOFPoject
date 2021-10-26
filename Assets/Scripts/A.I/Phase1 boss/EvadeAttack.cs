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
            int evadeCount = 0;

            protected override void OnStart()
            {
                evadeCount = Random.Range(1, 3);
            }

            protected override TaskStatus OnUpdate()
            {
                return TaskStatus.Success;
            }

            protected override void OnExit()
            {
                evadeCount = 0;
            }
        }
    }
}
