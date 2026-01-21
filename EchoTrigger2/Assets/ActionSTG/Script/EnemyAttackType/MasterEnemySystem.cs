using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 今シーン上にいる敵をリストで把握する処理
/// </summary>
public class MasterEnemySystem : MonoBehaviour
{
    [Header("敵を出現させる地点"),SerializeField]
    public List<Respon> m_Respawn = new List<Respon>();

    [Header("今現在いる敵"), SerializeField]
    public List<GameObject> m_Enemys = new List<GameObject>();


    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //繰り返してResponがついてるキャラをリストに格納
        foreach (Transform Dummy in transform)
        {
            if (Dummy.GetComponent<Respon>())
            {
                Dummy.GetComponent<Respon>().m_MES = this;
                m_Respawn.Add(Dummy.GetComponent<Respon>());
            }
        }

        //敵リストを初期化
        m_Enemys.Clear();
    }

    //敵をスポーン
    public void EnemyyAdd(GameObject Dummy)
    {
        m_Enemys.Add(Dummy);
    }

    /// <summary>
    /// //敵をすべてリセット（消す）
    /// </summary>
    public void EnemyAllDestroy()
    {
        //敵を一体ずつ消す
        foreach (var enemy in m_Enemys)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        m_Enemys.Clear();
    }

    /// <summary>
    /// 敵をスポーンの処理
    /// </summary>
    public void EnemySpawn()
    {
        foreach (var respon in m_Respawn)
        {
            if (respon != null)
            {
                respon.SetUp();   // ← ここが重要
            }
        }
    }
}
