using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Parameta : MonoBehaviour
{
    [Header("このオブジェクトのチーム")]
    public string m_Team;

    [Header("HPと最大HP")]
    public int m_Hp = 100;
    public int m_MaxHp = 100;

    [Header("死亡アニメーター")]
    public Animator m_Die;

    //HPのUI
    public HPUI m_HpUI;
    //死んだかどうか
    public bool m_IsDie = false;

    /// <summary>
    /// ダメージ処理ここから死亡処理へ
    /// </summary>
    /// <param name="DamegePoint"></param>
    public void TakeDamege(int DamegePoint)
    {
        //HPが0なら何もしない（二度死なないように）
        if (m_Hp <= 0)
            return;

        m_Hp -= DamegePoint;
        //HPが０以下ならかつTagdeでEnemyなら
        if (m_Hp <= 0)
        {
            m_IsDie = true;
            m_Hp = 0;
            
            // NavMeshAgentを停止して移動を止める
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            if (agent != null && agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.enabled = false;
            }
            
            // 他のスクリプトをすべて停止
            MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                // Parameta 自身は止めない
                if (script != this) 
                {
                    script.enabled = false;
                }
            }
            //死亡アニメーション（五秒後消える）
            if (m_Die != null)
            {
                m_Die.SetTrigger("Die");
            }
            Destroy(gameObject, 5f);
        }
        if (m_HpUI != null)
        {
            m_HpUI.UpdateHp(m_Hp, m_MaxHp);
        }

    }

}
