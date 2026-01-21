using System;
using System.Reflection;
using UnityEngine;
namespace StateMachineAI
{
    public enum AIState
    {
        Idle,
        Move,
        Search,
        Attack,
    }
    public class EnemyAI : StatefulObjectBase<EnemyAI, AIState>
    {
        public Transform m_Player;
        public Animator m_Animator;
        public Respon m_RSP;
        public bool m_StartFlag = false;
       

        public void AISetUp()
        {

            //ステートマシーンを自身として設定
            stateMachine = new StateMachine<EnemyAI>();
            m_Animator = GetComponent<Animator>();
             ChangeState(AIState.Idle);
        }





        public bool AddStateByName(string className)
        {
            try
            {
                // 現在のアセンブリからクラスを取得
                Type stateType = Assembly.GetExecutingAssembly().GetType($"StateMachineAI.{className}");

                // クラスが見つからなかった場合の対処
                if (stateType == null)
                {
                    Debug.LogError($"{className} クラスが見つかりませんでした。");
                    return false;
                }

                // 型チェック
                if (!typeof(State<EnemyAI>).IsAssignableFrom(stateType))
                {
                    Debug.LogError($"{className} は State<EnemySuitAI> 型ではありません。");
                    return false;
                }

                // コンストラクタ取得
                ConstructorInfo constructor = stateType.GetConstructor(new[] { typeof(EnemyAI) });
                if (constructor == null)
                {
                    Debug.LogError($"{className} のコンストラクタが見つかりませんでした。");
                    return false;
                }

                // インスタンス生成
                State<EnemyAI> stateInstance =
                    constructor.Invoke(new object[] { this }) as State<EnemyAI>;

                if (stateInstance != null)
                {
                    // ステートリストに追加
                    stateList.Add(stateInstance);
                    Debug.Log($"{className} をステートリストに追加しました。");
                    return true;
                }
                else
                {
                    Debug.LogError($"{className} のインスタンス生成に失敗しました。");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"エラーが発生しました。: {ex.Message}");
                return false;
            }
        }
     
    }
}




