using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 敵のパトロールパターン（全敵共通）
/// </summary>
public class EnemyPatrol_Waypoint : MonoBehaviour
{
    [Header("WaypointManagerのscriptを自動アタッチ")]
    public WaypointManager m_Manager;

    [Header("Sensorを自動アタッチ")]
    public Sensor m_Sensor;

    [Header("AlertLevelを自動アタッチ")]
    public AlertLevel m_AlertLevel;

    [Header("Responを自動アタッチ")]
    [SerializeField] private Respon m_Respon;

    [Header("stoneを自動アタッチ")]
    [SerializeField] private Stone m_stone;

    [Header("参照"),SerializeField]
    private NavMeshAgent m_agent;

    //どのWaypointに向かっているか
    private int m_currentIndex = 0;

    /// <summary>
    /// 開始
    /// </summary>
    [System.Obsolete]
    private void Start()
    {
        //参照
        m_Sensor = GetComponent<Sensor>();
        m_agent = GetComponent<NavMeshAgent>();
        m_AlertLevel = GetComponent<AlertLevel>();
        //NavMeshの移動速度を設定
        switch (m_Respon.m_UnitType)
        {
            case 0:              
                m_agent.speed = 0f;
                Debug.Log("スーツの速度を設定:" + m_agent.speed);
                break;
            case 1:
                m_agent.speed = 1.5f;
                Debug.Log("警備員の速度を設定:" + m_agent.speed);
                break;
           case 2:
               m_agent.speed = 3f;
                Debug.Log("Dogの速度を設定:" + m_agent.speed);
                break;
            default:
                Debug.LogError("未知のUnityTyoeが来ました。caseを確認してください。");
                break;
        }


        //マネージャーが未設定なら、近くのものを自動検索
        if (m_Manager == null)
        {
            m_Manager = FindClosestManager();
        }
        if (m_Sensor == null)
        {
            Debug.LogError("センサーが入ってません");
        }

        ////次のPointへ
        MoveToNextPoint();
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // AttackModeの時はShootingスクリプトが制御するため、ここでは何もしない
        if (m_AlertLevel != null && m_AlertLevel.m_AttackMode) return;

        //センサーが反応したら（攻撃モードでない時のみ）
        if (m_Sensor.m_Look == true)
        {
            m_agent.enabled = false;
            // センサー反応中は巡回しない
            return;
        }
        else
        {
            m_agent.enabled = true;
        }

        if (m_agent.enabled)
        {
            //NavMeshAgentがまだ経路を計算中ではなく現在の目的地に到着したら
            if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
                //次のPointへ
                MoveToNextPoint();
        }
    }

    /// <summary>
    /// 次へのポイントの処理
    /// </summary>
    void MoveToNextPoint()
    {
        if (m_Manager == null || m_Manager.m_Waypoints.Length == 0)
        {
            Debug.Log("WaypointManagerのscriptが原因です。");
            return;
        }
        //次の移動ポイントをセット
        m_agent.destination = m_Manager.m_Waypoints[m_currentIndex].position;
        //配列の最後まで行ったら最初に戻る
        m_currentIndex = (m_currentIndex + 1) % m_Manager.m_Waypoints.Length;
    }

    /// <summary>
    /// 石に投げられた方に行く処理
    /// </summary>
    /// <param name="stonePos"></param>
    public void OnDetectStone(Vector3 stonePos)
    {
        //攻撃中など
        if (m_agent.enabled == false) return; 

        m_agent.SetDestination(stonePos);
        //パトロール再開時に最初から
        m_currentIndex = 0; 
    }

    /// <summary>
    /// リスポーン処理
    /// </summary>
    /// <param name="respon"></param>
    public void SetRespon(Respon respon)
    {
        m_Respon = respon;
        if (m_Respon == null)
        {
            Debug.LogError("Responが入ってません");
        }
    }
    //石の方に向かう関数
    public void StonePatrol()
    {
        GameObject target = GameObject.FindGameObjectWithTag("StoneGetTag");
        if (target!=null)
        {
            // 既に MasterEnemySystem が付いているか確認
            m_stone = target.GetComponent<Stone>();
        }
        if (m_stone!=null)
        {
            //石の方に向かう
            m_agent.SetDestination(m_stone.transform.position);
            
        }
        else
        {
            Debug.Log("石がありません。");
        }
        
    }
    //石の方を向いて停止
    public void StonePatrolStop()
    {

    }
    // 最も近いManagerを探す
    [System.Obsolete]
    WaypointManager FindClosestManager()
    {
        //シーンにある全てのWaypointManagerを獲得
        WaypointManager[] managers = FindObjectsOfType<WaypointManager>();
        //最も近いものを保存する物
        WaypointManager closest = null;
        //最小距離の初期値
        float minDist = Mathf.Infinity;
        //獲得したWaypointManagerを一つずつ調べる
        foreach (var m in managers)
        {
            //距離を計算
            float dist = Vector3.Distance(transform.position, m.transform.position);
            //今までよりも近いものがあれば更新
            if (dist < minDist)
            {
                minDist = dist;
                closest = m;
            }
        }
        //最終的に近かったものをいれる
        return closest;
    }
   

}
