using StateMachineAI;
using UnityEngine;
/// <summary>
/// 敵のセンサー（全敵共通）
/// </summary>
public class Sensor : MonoBehaviour
{
    [Header("検知する対象")]
    public string m_targetTag = "Player";

    [Header("感知距離")]
    public float m_viewDistance = 10f;

    [Header("視野角（左右）")]
    public float m_viewAngle = 90f;
    
    [Header("センサー外でパトロールに戻るまでの時間（秒）")]
    public float m_lostTimeToPatrol = 3.0f;
    
    //外部では変更できない用の変数
    public float m_LastDistance { get; private set; }

    //プレイヤーを見たかどうか
    public bool m_Look;

    [Header("参照")]
    [SerializeField] private EnemyAI m_EnemyAI;
    [SerializeField] private AlertLevel m_AlertLevel;
    [SerializeField] public Transform m_Target;

    // センサー外にいる時間をカウント
    private float m_LostTimer = 0f;

    private void Awake()
    {
        //EnemyAIを格納
        m_EnemyAI = GetComponent<EnemyAI>();
        if (m_EnemyAI == null) m_EnemyAI = GetComponentInParent<EnemyAI>();
        if (m_EnemyAI == null) m_EnemyAI = GetComponentInChildren<EnemyAI>();
        if (m_EnemyAI == null) m_EnemyAI = transform.root.GetComponentInChildren<EnemyAI>();
    }

    /// <summary>
    /// AlertLevelを動的に検索
    /// </summary>
    private void TryGetAlertLevel()
    {
        // 既に取得済みならスキップ
        if (m_AlertLevel != null) return;  
        
        m_AlertLevel = GetComponent<AlertLevel>();
        if (m_AlertLevel == null) m_AlertLevel = GetComponentInParent<AlertLevel>();
        if (m_AlertLevel == null) m_AlertLevel = GetComponentInChildren<AlertLevel>();
        if (m_AlertLevel == null) m_AlertLevel = transform.root.GetComponentInChildren<AlertLevel>();
    }

    private void Update()
    {
        //ターゲット検知処理
        DetectTarget();
        
        // AlertLevelを動的に取得（後から追加される場合があるため）
        TryGetAlertLevel();
        
        // 攻撃モード中にセンサー外にいる時間をカウント
        if (m_AlertLevel != null && m_AlertLevel.m_AttackMode)
        {
            if (!m_Look)
            {
                // センサー外にいる時間を加算
                m_LostTimer += Time.deltaTime;
                
                // 指定時間経過したらパトロールに戻る
                if (m_LostTimer >= m_lostTimeToPatrol)
                {
                    ReturnToPatrol();
                }
            }
            else
            {
                // センサー内ならタイマーリセット
                m_LostTimer = 0f;
            }
        }
    }

    /// <summary>
    /// パトロールモードに戻る
    /// </summary>
    private void ReturnToPatrol()
    {
        m_LostTimer = 0f;
        m_AlertLevel.m_AttackMode = false;
        // 警戒度もリセット
        m_AlertLevel.m_currentLevel = 0f;  
        
        if (m_EnemyAI != null)
        {
            m_EnemyAI.ChangeState(AIState.Move);
        }
    }

    /// <summary>
    /// センサーの処理（条件によって警戒度の方に行く）
    /// </summary>
    private void DetectTarget()
    {
        //Playerのタグを探す
        GameObject targetObj = GameObject.FindGameObjectWithTag(m_targetTag);
        //無ければ発見をないことにする
        if (targetObj == null)
        {
            m_Look = false;
            m_Target = null;
            return;
        }
        //プレイヤーにtransformをつける
        m_Target = targetObj.transform;
        //自分から敵のベクトルを計算
        Vector3 dirToTarget = (m_Target.position - transform.position).normalized;
        //ターゲットまでの距離を取得
        float distance = Vector3.Distance(transform.position, m_Target.position);
        m_LastDistance = distance;
        //センサーの高さを2mに調整
        float heightDifference = Mathf.Abs(m_Target.position.y - transform.position.y);
        float maxHeightDifference = 2f;
        //forward方向との角度を計算
        float dot = Vector3.Dot(transform.forward, dirToTarget);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // 視野角＆距離＆高さチェック
        if (angle < m_viewAngle * 0.5f && distance < m_viewDistance && heightDifference <= maxHeightDifference)
        {
            //頭の高さに調整
            Vector3 rayOrigin = transform.position + Vector3.up * 1.5f; 
            // 視界に遮蔽物がないことをチェック
            if (Physics.Raycast(rayOrigin, dirToTarget, out RaycastHit hit, distance))
            {
                //ヒットしたのがプレイヤーなら
                if (hit.collider.CompareTag(m_targetTag))
                {
                    m_Look = true;
                    //距離をAlertLevelに渡す
                    m_LastDistance = distance;
                    m_EnemyAI.ChangeState(AIState.Search);
                    return;
                }
            }
        }

        m_Look = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_viewDistance);

        Vector3 leftDir = Quaternion.Euler(0, -m_viewAngle / 2, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, m_viewAngle / 2, 0) * transform.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftDir * m_viewDistance);
        Gizmos.DrawRay(transform.position, rightDir * m_viewDistance);
    }
}
