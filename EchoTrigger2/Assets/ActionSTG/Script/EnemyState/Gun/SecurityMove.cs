
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace StateMachineAI
{

    public class SecurityMove : State<EnemyAI>
    {
        public SecurityMove(EnemyAI owner) : base(owner) { }

        private EnemyPatrol_Waypoint col;

        public override void Enter()
        {
            //移動scriptを導入
            col = owner.GetComponent<EnemyPatrol_Waypoint>();
            //センサーを導入
            Sensor Sen = owner.GetComponent<Sensor>();
            //警戒度を導入
            AlertLevel level = owner.GetComponent<AlertLevel>();

            if (Sen == null)
            {
                Debug.Log("Sensorがなかったので自動追加します。");
                Sen = owner.gameObject.AddComponent<Sensor>();
            }
            if (col == null)
            {
                Debug.Log("EnemyPatrol_Waypointがなかったので自動追加します。");
                col = owner.gameObject.AddComponent<EnemyPatrol_Waypoint>();
            }
            if (level == null)
            {
                Debug.Log("AlertLevelがなかったので自動追加します。");
                level = owner.gameObject.AddComponent<AlertLevel>();
            }
        }

        public override void Stay()
        {

        }

        public override void Exit()
        {
        }
    }

}

