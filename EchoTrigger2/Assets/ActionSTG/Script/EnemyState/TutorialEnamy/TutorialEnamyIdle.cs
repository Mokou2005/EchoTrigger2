using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class TutorialEnamyIdle : State<EnemyAI>
    {
        public TutorialEnamyIdle(EnemyAI owner) : base(owner) { }
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