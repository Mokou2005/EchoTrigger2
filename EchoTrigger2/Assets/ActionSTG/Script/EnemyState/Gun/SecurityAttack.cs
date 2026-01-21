using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace StateMachineAI
{

    public class SecurityAttack : State<EnemyAI>
    {
        private AlertLevel m_AlertLevel;
        private Sensor m_Sensor;
        private Shooting m_Shooting;
        private EnemyPatrol_Waypoint m_patrol;
        public SecurityAttack(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            m_Shooting = owner.GetComponent<Shooting>();
            if (m_Shooting == null)
            {
                Debug.Log("Shootingがなかったので自動追加します");
                m_Shooting = owner.gameObject.AddComponent<Shooting>();
            }

            m_patrol = owner.gameObject.GetComponent<EnemyPatrol_Waypoint>();

            m_AlertLevel = owner.GetComponent<AlertLevel>();
            m_Sensor = owner.GetComponent<Sensor>();
        }

        /// <summary>
        /// センサーの判定
        /// </summary>
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
                //警戒度減少
                m_AlertLevel?.DecreaseVigilance();
            }
        }

        public override void Exit()
        {
        }
    }

}