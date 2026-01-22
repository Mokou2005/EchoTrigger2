using UnityEngine;
/// <summary>
/// 敵をBattleManagerに登録するスクリプト
/// 警戒レベルに関係なく常に表示される
/// </summary>
public class EnemyBattleRegister : MonoBehaviour
{
    //BattleManagerに登録されているか
    bool m_IsRegistered = false;

    /// <summary>
    /// 開始時に関数へ
    /// </summary>
    void Start()
    {
        // 開始時に即座にBattleManagerに登録
        RegisterToBattle();
    }

    /// <summary>
    /// BattleManagerに敵を登録
    /// </summary>
    public void RegisterToBattle()
    {
        if (BattleManager.m_BattleInstance != null && !m_IsRegistered)
        {
            BattleManager.m_BattleInstance.EnemyFoundPlayer(transform);
            m_IsRegistered = true;
            Debug.Log($"{gameObject.name} がBattleManagerに登録されました");
        }
    }

    /// <summary>
    /// BattleManagerから敵を解除
    /// </summary>
    public void UnregisterFromBattle()
    {
        if (BattleManager.m_BattleInstance != null && m_IsRegistered)
        {
            BattleManager.m_BattleInstance.EnemyLostPlayer(transform);
            m_IsRegistered = false;
            Debug.Log($"{gameObject.name} がBattleManagerから解除されました");
        }
    }

    /// <summary>
    /// 敵が死亡した時に呼び出す
    /// </summary>
    public void OnEnemyDeath()
    {
        if (BattleManager.m_BattleInstance != null && m_IsRegistered)
        {
            BattleManager.m_BattleInstance.EnemyDeath(transform);
            m_IsRegistered = false;
            Debug.Log($"{gameObject.name} が死亡によりBattleManagerから削除されました");
        }
    }

    void OnDestroy()
    {
        // オブジェクト破棄時に自動解除
        UnregisterFromBattle();
    }
}
