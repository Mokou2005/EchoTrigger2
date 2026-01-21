using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    [Header("ターゲットプレイヤー")]
    public Transform m_TargetGun;
    [Header("外す場所")]
    public Transform m_RemoveGun;
    [Header("銃口の位置")]
    public Transform m_Gun;
    [Header("プレイヤーに当たる確率")]
    [Range(0f, 1f)]
    public float m_Probability = 0.8f;
    public GameObject m_bulletPrefab;
    public float m_BulletSpeed;
    public AudioClip m_sound;

    [Header("この銃を使う敵本体")]
    public GameObject m_Owner;

    private Parameta m_Parameta;

    private void Start()
    {

        m_Parameta = m_Owner.GetComponent<Parameta>();

        // ★ PlayerをTagから取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 子から探す
            if (m_TargetGun == null)
                m_TargetGun = player.transform.Find("TargetGun");

            if (m_RemoveGun == null)
                m_RemoveGun = player.transform.Find("RemoveGun");
        }


    }

    public void Shot()
    {
        if (m_Parameta.m_Hp <= 0) return;

        Transform targetPos = (Random.value <= m_Probability) ? m_TargetGun : m_RemoveGun;

        GameObject bullet = Instantiate(m_bulletPrefab, m_Gun.position, Quaternion.identity);

        Vector3 dir = (targetPos.position - m_Gun.position).normalized;

        bullet.GetComponent<DamegeSystem>().m_Parameta = m_Parameta;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(dir * m_BulletSpeed, ForceMode.Impulse);

        AudioSource.PlayClipAtPoint(m_sound, transform.position);
        Destroy(bullet, 5f);
    }
}
