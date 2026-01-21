using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class SuitMove : State<EnemyAI>
    {
        public SuitMove(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {

        }

        public override void Stay()
        {

        }

        public override void Exit()
        {
        }
    }

}