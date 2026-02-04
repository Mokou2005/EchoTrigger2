using UnityEngine;

/// <summary>
/// Mキーでアニメーションを開閉する
/// </summary>
public class OpenOperation : MonoBehaviour
{
    [Header("Animatorを自動アタッチ")]
    [SerializeField] private Animator m_Animator;

    [Header("開閉状態")]
    [SerializeField] private bool m_IsOpen = false;

    [Header("Push音")]
    [SerializeField] private AudioSource m_PushSE;
    /// <summary>
    /// 開始
    /// </summary>
    void Start()
    {
        // Animatorを自動取得
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }

        if (m_Animator == null)
        {
            Debug.LogError("Animatorがアタッチされていません");
        }

        if (m_PushSE==null)
        {
            m_PushSE = GetComponent<AudioSource>();
        }

        if (m_PushSE == null)
        {
            Debug.LogError("AudioSourceがアタッチされてません");
        }
       
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // Mキーで開閉をトグル
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleOpen();
        }
    }

    /// <summary>
    /// 開閉をトグルする
    /// </summary>
    public void ToggleOpen()
    {
        if (m_Animator == null) return;

        m_IsOpen = !m_IsOpen;
        m_Animator.SetBool("Open", m_IsOpen);
        Debug.Log(m_IsOpen ? "開きました" : "閉じました");
        m_PushSE.Play();
    }

    /// <summary>
    /// 外部から開く
    /// </summary>
    public void Open()
    {
        if (m_Animator == null) return;

        m_IsOpen = true;
        m_Animator.SetBool("Open", m_IsOpen);
    }

    /// <summary>
    /// 外部から閉じる
    /// </summary>
    public void Close()
    {
        if (m_Animator == null) return;

        m_IsOpen = false;
        m_Animator.SetBool("Open", m_IsOpen);
    }
}
