using StateMachineAI;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Stone : MonoBehaviour
{
    [Header("（自動）MasterEnemySystemをアタッチ")]
    [SerializeField]private MasterEnemySystem m_EnemySystem;
    [Header("（自動）EnemyPatrol_Waypointをアタッチ")]
    [SerializeField] private EnemyPatrol_Waypoint[] m_waypoint;
    [Header("転がるSE")]
    public AudioClip m_RollSE;
    [SerializeField]private AudioSource m_AudioSource;
    //音フラグ
    private bool m_PlayAudio=false;


    void Update()
     {
        if (m_EnemySystem == null)
        {
            // Tag で対象オブジェクトを探す
            GameObject target = GameObject.FindGameObjectWithTag("MasterEnemySystemGetTag");

            if (target != null)
            {
                // 既に MasterEnemySystem が付いているか確認
                m_EnemySystem = target.GetComponent<MasterEnemySystem>();
            }
            else
            {
                Debug.LogError("Tag 'MasterEnemySystemGetTag' のオブジェクトが見つかりません！");
            }
        }
        else
        {
            Debug.Log("すでにscriptはついています");
        }

        List<EnemyPatrol_Waypoint> temp = new List<EnemyPatrol_Waypoint>();

        foreach (GameObject enemy in m_EnemySystem.m_Enemys)
        {
            if (enemy == null) continue;  // 破壊済みの敵をスキップ
            
            EnemyPatrol_Waypoint wp = enemy.GetComponent<EnemyPatrol_Waypoint>();
            if (wp != null)
            {
                temp.Add(wp);
            }
        }
        m_waypoint = temp.ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        // HitAreaMarker を持っているか？
        if (other.GetComponent<HitAreaMarker>() != null)
        {
            Debug.Log("障害物にヒット！");
            if (!m_PlayAudio)
            {
                m_AudioSource.PlayOneShot(m_RollSE);
                m_PlayAudio = true;
            }
            Hit();
        }
    }
    void Hit()
    {
        
        //敵とぶつかった石との距離を計算
        foreach (GameObject enmay in m_EnemySystem.m_Enemys)
        {
            if (enmay == null) continue;  // 破壊済みの敵をスキップ
            
            float dist = Vector3.Distance(transform.position, enmay.transform.position);

            Vector3 dir = (transform.position - enmay.transform.position).normalized;
            RaycastHit hitInfo;
       

            //  Ray が何かに当たった場合
            if (Physics.Raycast(enmay.transform.position, dir, out hitInfo, dist))
            {
                // 壁か床にヒットする無効
                if (hitInfo.collider.GetComponent<HitAreaMarker>() != null)
                {
                    Debug.Log($"壁/床が遮っている！敵 {enmay.name} は石に反応しない");
                    continue;
                }

            }

            //投げた間の距離が15以下ならその石に移動
            if (dist<=15)
            {
                Debug.Log("敵が石の方に移動");
                foreach (EnemyPatrol_Waypoint wp in m_waypoint)
                {
                    wp.StonePatrol();
                }
            }
            if (dist <= 30)
            {
                Debug.Log("敵の頭に？だけ出す");
                foreach (EnemyPatrol_Waypoint wp in m_waypoint)
                {
                    //まだ未完成
                    wp.StonePatrol();
                }
            }
        }
        
    }
}



