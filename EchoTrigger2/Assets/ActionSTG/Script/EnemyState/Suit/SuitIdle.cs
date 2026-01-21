using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class SuitIdle : State<EnemyAI>
    {
        public SuitIdle(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
           
            //センサーを導入
            Sensor Sen = owner.GetComponent<Sensor>();
            //警戒度を導入
            AlertLevel level = owner.GetComponent<AlertLevel>();
            if (Sen == null)
            {
                Debug.Log("Sensorがなかったので自動追加します。");
                Sen = owner.gameObject.AddComponent<Sensor>();
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