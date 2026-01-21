using UnityEngine;

public class Reload : MonoBehaviour
{
    //リロードアニメーター
    private Animator m_ReloadAnimator;
    //リロード効果音
    public AudioClip m_ReloadSound;
    private AudioSource m_Sound;
    //リロード中は動きを停止（public staticは他のscriptに連動）
    public static bool m_Reloading=false;
    public static int m_GunBullet;
    //scriptのPlayerAttackを参照
    private PlayerAttack m_PlayerAttack;
    private bool m_Reload=false;
    private void Start()
    {
        m_ReloadAnimator = GetComponent<Animator>();
        m_Sound = GetComponent<AudioSource>();
        //プレイヤーオブジェクトから PlayerAttack を探す
        m_PlayerAttack = FindObjectOfType<PlayerAttack>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !m_Reload)
        {
            // 弾がフルの場合、または予備弾薬(Index)がない場合はリロードしない
            if (m_PlayerAttack.m_BulletCount < m_PlayerAttack.m_InitialBulletCount && m_PlayerAttack.m_BulletIndex > 0)
            {
                m_ReloadAnimator.SetBool("Reload", true);
                m_Sound.PlayOneShot(m_ReloadSound);
                m_Reloading = true;
                m_Reload = true;
                GunReload();
            }
        }

    }
    //アニメーションが終了したら動く
    public void ReloadEnd()
    {
        m_ReloadAnimator.SetBool("Reload", false);
        m_Reloading=false;
        m_Reload=false;
    }
    void GunReload()
    {
        if (m_PlayerAttack != null)
        {
            m_PlayerAttack.ReloadAmmo();
        }
    }
}
