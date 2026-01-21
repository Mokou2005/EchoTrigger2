using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace StateMachineAI
{

    public class DogAttack : State<EnemyAI>
    {
        private Bite m_Bite;
        private AlertLevel m_AlertLevel;
        private Sensor m_Sensor;
        private NavMeshAgent m_Agent; 
        public DogAttack(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            m_Agent = owner.GetComponent<NavMeshAgent>();
            Bite Bi = owner.GetComponent<Bite>();
            if (Bi == null)
            {
                Debug.Log("Biteがなかったので自動追加します。");
                Bi = owner.gameObject.AddComponent<Bite>();
            }
            m_AlertLevel = owner.GetComponent<AlertLevel>();
            m_Sensor = owner.GetComponent<Sensor>();
        }

        public override void Stay()
        {
            if (m_Sensor.m_Look == true)
            {
                owner.m_Animator.SetBool("Search", true);
                m_AlertLevel?.IncreaseVigilance(m_Sensor.m_LastDistance);
                // ターゲットが存在するなら、そこへ向かって移動させる
                if (m_Sensor.m_Target != null && m_Agent != null)
                {
                    // 停止フラグを解除して動けるようにする
                    m_Agent.isStopped = false;
                    // 目的地をセットする
                    m_Agent.SetDestination(m_Sensor.m_Target.position);
                }
            }

            if (m_Sensor.m_Look == false)
            {
                owner.m_Animator.SetBool("Search", false);
                //警戒度減少
                m_AlertLevel?.DecreaseVigilance();
            }


        }

        public override void Exit()
        {
        }


    }

}
