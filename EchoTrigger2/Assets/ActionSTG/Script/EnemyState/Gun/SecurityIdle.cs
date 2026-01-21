using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class SecurityIdle : State<EnemyAI>
    {
        public SecurityIdle(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            owner.ChangeState(AIState.Move);
        }

        public override void Stay()
        {

        }

        public override void Exit()
        {
        }
    }

}
