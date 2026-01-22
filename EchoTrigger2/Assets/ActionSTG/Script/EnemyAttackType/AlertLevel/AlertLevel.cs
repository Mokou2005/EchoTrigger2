using StateMachineAI;
using UnityEngine;
/// <summary>
/// 警戒度処理
/// </summary>
public class AlertLevel : MonoBehaviour
{
    [Header("警戒度の上昇速度(1秒あたり)"), SerializeField]
    private float m_IncreaseRate = 90f;

    [Header("警戒度の減少速度(1秒あたり)"), SerializeField]
    private float m_DecreaseRate = 5f;

    [Header("警戒度MAX"), SerializeField]
    private float m_MaxLevel = 50f;

    [Header("現在の警戒度")]
    public float m_CurrentLevel = 0f;

    [Header("警戒MAX時にAIへ通知するか"), SerializeField]
    private bool m_AutoAlert = true;

    [Header("参照"), SerializeField]
    private EnemyAI m_EnemyAI;

    //攻撃モードに入ったかどうか
    public bool m_AttackMode = false;

    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        m_EnemyAI = GetComponent<EnemyAI>();
    }

    /// <summary>
    /// 距離関係で警戒度の上昇処理を変更(警戒度Maxなら攻撃へ)
    /// </summary>
    /// <param name="distance">敵とプレイヤーの距離</param>
    public void IncreaseVigilance(float distance)
    {
        //距離が近いほど急上昇させる
        float k = 0.1f;
        float factor = Mathf.Exp(-distance * k);

        // 上昇速度を距離係数で調整
        float rate = m_IncreaseRate * factor;
        m_CurrentLevel += rate * Time.deltaTime;

        //警戒度の上限と下限を設定
        m_CurrentLevel = Mathf.Clamp(m_CurrentLevel, 0, m_MaxLevel);
        Debug.Log($"警戒度上昇中: {m_CurrentLevel:F1}/{m_MaxLevel}");

        // 警戒度MAXなら攻撃モードへ
        if (m_AutoAlert && m_CurrentLevel >= m_MaxLevel)
        {
            Debug.Log("警戒度MAX → 攻撃モードへ");
            //攻撃モードに入る
            m_AttackMode = true;
            if (m_EnemyAI != null)
                m_EnemyAI.ChangeState(AIState.Attack);
        }
    }
    [Header("警戒度低下時に巡回へ戻る閾値")]
    private float m_ReturnToPatrolThreshold = 10f;

    /// <summary>
    /// 警戒度を下げる処理
    /// </summary>
    public void DecreaseVigilance()
    {
        //現在の警戒度を減少
        m_CurrentLevel -= m_DecreaseRate * Time.deltaTime;

        //警戒度の上限と下限を設定
        m_CurrentLevel = Mathf.Clamp(m_CurrentLevel, 0, m_MaxLevel);
        Debug.Log($"警戒度上昇中: {m_CurrentLevel:F1}/{m_MaxLevel}");

        // 警戒度が閾値以下に下がったら巡回モードへ戻る
        if (m_AttackMode && m_CurrentLevel <= m_ReturnToPatrolThreshold)
        {
            Debug.Log("警戒度低下 → 巡回モードへ");
            if (m_EnemyAI != null)
                m_EnemyAI.ChangeState(AIState.Move);
        }
    }
    //警戒度MAX判定
    public bool IsMax() => m_CurrentLevel >= m_MaxLevel;
}


