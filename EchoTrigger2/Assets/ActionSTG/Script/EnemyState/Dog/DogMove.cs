using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{
    public class DogMove : State<EnemyAI>
    {
        

        public DogMove(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
            //移動scriptを導入
            EnemyPatrol_Waypoint col = owner.GetComponent<EnemyPatrol_Waypoint>();
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

