using UnityEngine;

public class DamegeSystem : MonoBehaviour
{
    public int m_DamegePoint = 1; // 攻撃力
    public Parameta m_Parameta;   // 攻撃者の情報

    private bool m_HitOnce = false;

    private void OnEnable()
    {
        // 新しく生成されたときにリセット
        m_HitOnce = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_HitOnce) return;

        Parameta target = other.GetComponent<Parameta>();
        if (target == null || m_Parameta == null) return;

        if (target.m_Team == m_Parameta.m_Team) return;

        target.TakeDamege(m_DamegePoint);
        m_HitOnce = true;
    }
}
