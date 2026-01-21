using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class DogIdle : State<EnemyAI>
    {
        private Animator m_animator;
        private Sensor m_sensor;
        public DogIdle(EnemyAI owner) : base(owner) { }
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
