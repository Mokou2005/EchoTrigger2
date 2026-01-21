using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace StateMachineAI
{

    public class DogSearch : State<EnemyAI>
    {
        private AlertLevel m_AlertLevel;
        private Sensor m_Sensor;
        public DogSearch(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            m_AlertLevel = owner.GetComponent<AlertLevel>();
            m_Sensor = owner.GetComponent<Sensor>();

        }

        public override void Stay()
        {
            if (m_Sensor.m_Look == true)
            {
                owner.m_Animator.SetBool("Search", true);
                m_AlertLevel?.IncreaseVigilance(m_Sensor.m_LastDistance);
            }
  
            if (m_Sensor.m_Look == false)
            {
                owner.m_Animator.SetBool("Search", false);
                //åxâ˙ìxå∏è≠
                m_AlertLevel?.DecreaseVigilance();
            }
        }

        public override void Exit()
        {
        }
    }

}

