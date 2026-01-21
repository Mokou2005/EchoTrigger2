using StateMachineAI;
using UnityEngine;

public class AlertLevel : MonoBehaviour
{

    [Header("警戒度の上昇速度(1秒あたり)")]
    public float m_increaseRate = 90f;

    [Header("警戒度の減少速度(1秒あたり)")]
    public float m_decreaseRate = 5f;

    [Header("警戒度MAX")]
    public float m_maxLevel = 50f;

    [Header("現在の警戒度")]
    public float m_currentLevel = 0f;

    [Header("警戒MAX時にAIへ通知するか")]
    public bool m_autoAlert = true;

    private EnemyAI enemyAI;
    //攻撃モードに入ったかどうか
    public bool m_AttackMode=false;



    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    //distanceは敵とプレイヤーの距離（Sensorのscriptpに引き継いでます）
    public void IncreaseVigilance(float distance)
    {
        //距離が近いほど急上昇させる
        float k = 0.1f;
        float factor = Mathf.Exp(-distance * k);
        // 上昇速度を距離係数で調整
        float rate = m_increaseRate * factor;
        m_currentLevel += rate * Time.deltaTime;
        //警戒度の上限と下限を設定
        m_currentLevel = Mathf.Clamp(m_currentLevel, 0, m_maxLevel);
        Debug.Log($"警戒度上昇中: {m_currentLevel:F1}/{m_maxLevel}");
        // 警戒度MAXなら攻撃モードへ
        if (m_autoAlert && m_currentLevel >= m_maxLevel)
        {
            Debug.Log("警戒度MAX → 攻撃モードへ");
            //攻撃モードに入る
            m_AttackMode = true;
            if (enemyAI != null)
                enemyAI.ChangeState(AIState.Attack);
        }
    }


    [Header("警戒度低下時に巡回へ戻る閾値")]
    public float m_returnToPatrolThreshold = 10f;

    public void DecreaseVigilance()
    {
        //現在の警戒度を減少
        m_currentLevel -= m_decreaseRate * Time.deltaTime;
        //警戒度の上限と下限を設定
        m_currentLevel = Mathf.Clamp(m_currentLevel, 0, m_maxLevel);
        Debug.Log($"警戒度上昇中: {m_currentLevel:F1}/{m_maxLevel}");

        // 警戒度が閾値以下に下がったら巡回モードへ戻る
        if (m_AttackMode && m_currentLevel <= m_returnToPatrolThreshold)
        {
            Debug.Log("警戒度低下 → 巡回モードへ");
           // m_AttackMode = false;
            // 敵AIの状態をMove（巡回）に変更
            if (enemyAI != null)
             enemyAI.ChangeState(AIState.Move);
        }
    }

    //警戒度MAX判定
    public bool IsMax() => m_currentLevel >= m_maxLevel;
}


