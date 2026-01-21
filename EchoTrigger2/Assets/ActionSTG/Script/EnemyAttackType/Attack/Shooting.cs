using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// プレイヤーに向けて銃を撃つ処理
/// </summary>
public class Shooting : MonoBehaviour
{
    [Header("ターゲット"), SerializeField]
    private Transform m_Target;

    [Header("射撃距離（この距離以内で射撃）")]
    [SerializeField] private float m_ShootingRange = 5.0f;

    [Header("追跡速度")]
    [SerializeField] private float m_ChaseSpeed = 3.5f;

    [Header("射撃間隔（秒）")]
    [SerializeField] private float m_ShootInterval = 1.0f;
   
    [Header("自動で参照されます。")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private AssaultRifle m_Rifle;
    [SerializeField] private NavMeshAgent m_Agent;
    [SerializeField] private AlertLevel m_AlertLevel;
    [SerializeField] private Sensor m_Sensor;

    //撃つごとの間の時間帯
    private float m_ShootTimer = 0f;

    // 前回のターゲット位置
    private Vector3 m_LastTargetPosition;

    void Start()
    {
        //自動で格納
        if (m_Animator == null) m_Animator = GetComponent<Animator>();
        if (m_Rifle == null) m_Rifle = GetComponentInChildren<AssaultRifle>();
        if (m_Agent == null) m_Agent = GetComponent<NavMeshAgent>();
        if (m_AlertLevel == null) m_AlertLevel = GetComponent<AlertLevel>();
        if (m_Sensor == null) m_Sensor = GetComponent<Sensor>();
        if (m_Target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                m_Target = player.transform;
            }
        }

        // 最初は移動をオンに
        m_Agent.enabled = true;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // AlertLevelのm_AttackModeがfalseなら何もしない
        if (m_AlertLevel == null || !m_AlertLevel.m_AttackMode) return;

        // ターゲットがなければ何もしない
        if (m_Target == null) return;

        // NavMeshAgentが無効なら有効にする
        if (!m_Agent.enabled) m_Agent.enabled = true;

        // プレイヤーとの距離を計算
        float distance = Vector3.Distance(transform.position, m_Target.position);

        // 射撃タイマーを更新
        m_ShootTimer -= Time.deltaTime;

        // 射撃距離以内ならば
        if (distance <= m_ShootingRange)
        {
            // 移動を完全に停止
            m_Agent.speed = 0f;
            m_Agent.velocity = Vector3.zero;

            // プレイヤーの方を向く
            LookAtTarget();

            // 射撃間隔ごとに撃つ
            if (m_ShootTimer <= 0f)
            {
                Shoot();
                m_ShootTimer = m_ShootInterval;
            }
        }
        else
        {
            // 射撃距離外ならプレイヤーを追跡
            m_Agent.speed = m_ChaseSpeed;
            m_Agent.isStopped = false;

            // ターゲットが1m以上移動したか、経路がない場合のみ再計算
            float targetMoveDistance = Vector3.Distance(m_Target.position, m_LastTargetPosition);
            if (!m_Agent.hasPath || targetMoveDistance > 1.0f)
            {
                m_Agent.SetDestination(m_Target.position);
                m_LastTargetPosition = m_Target.position;
            }
        }
    }

    /// <summary>
    /// ターゲットの方向を向く
    /// </summary>
    private void LookAtTarget()
    {
        Vector3 direction = (m_Target.position - transform.position).normalized;
        // Y軸の回転だけにする
        direction.y = 0; 
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    /// <summary>
    /// 撃つ処理
    /// </summary>
    private void Shoot()
    {
        m_Animator.SetTrigger("Attack");

        if (m_Rifle != null)
        {
            m_Rifle.Shot();
            Debug.Log("弾を発射しました！");
        }
        else
        {
            Debug.LogWarning("AssaultRifleが設定されていません");
        }
    }

}
