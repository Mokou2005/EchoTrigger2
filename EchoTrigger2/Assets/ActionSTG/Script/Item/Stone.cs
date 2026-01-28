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
        // 最も近い敵を見つけるための変数
        GameObject closestEnemy = null;
        float closestDist = Mathf.Infinity;
        
        // 敵とぶつかった石との距離を計算し、最も近い敵を見つける
        foreach (GameObject enemy in m_EnemySystem.m_Enemys)
        {
            if (enemy == null) continue;  // 破壊済みの敵をスキップ
            
            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            Vector3 dir = (transform.position - enemy.transform.position).normalized;
            RaycastHit hitInfo;

            // Ray が何かに当たった場合（壁で遮られているかチェック）
            if (Physics.Raycast(enemy.transform.position, dir, out hitInfo, dist))
            {
                // 壁か床にヒットしていたらこの敵はスキップ
                if (hitInfo.collider.GetComponent<HitAreaMarker>() != null)
                {
                    Debug.Log($"壁/床が遮っている！敵 {enemy.name} は石に反応しない");
                    continue;
                }
            }

            // 距離30以内で、今までの最小距離より近い敵を記録
            if (dist <= 30 && dist < closestDist)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        // 最も近い敵がいる場合のみ処理
        if (closestEnemy != null)
        {
            EnemyPatrol_Waypoint wp = closestEnemy.GetComponent<EnemyPatrol_Waypoint>();
            if (wp != null)
            {
                if (closestDist <= 15)
                {
                    Debug.Log($"敵 {closestEnemy.name} が石の方に移動（距離: {closestDist}）");
                    wp.StonePatrol();
                }
                else if (closestDist <= 30)
                {
                    Debug.Log($"敵 {closestEnemy.name} の頭に？だけ出す（距離: {closestDist}）");
                    // まだ未完成 - 必要に応じて処理を追加
                    wp.StonePatrol();
                }
            }
        }
    }
}



