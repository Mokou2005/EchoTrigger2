using UnityEngine;

public class Throw : MonoBehaviour
{
    [Header("（自動）アニメーター")]
    [SerializeField] private Animator m_animator;
    [Header("ItemCountをアタッチ")]
    public ItemCount m_ItemCount;
    [Header("投げるSE")]
    [SerializeField] private AudioClip m_ThrowSE;
    [Header("構えるSE")]
    [SerializeField] private AudioClip m_BraceSE;
    [Header("投げる石")]
    public GameObject m_ThrowStone;
    [Header("出現する場所")]
    public Transform m_SpawnStone;
    [Header("放出する角度")]
    public float m_ThrowAngle=60f;
    [SerializeField]private AudioSource m_AudioSource;
    //構えた状態かどうか
    [SerializeField] private bool m_IsBrace = false;
    //投げたフラグ
    [SerializeField] public bool m_IsThrow = false;
    //手に持っている石を保存
    private GameObject m_HoldingStone = null;


    private void Start()
    {
        if (m_AudioSource==null)
        {
            m_AudioSource = GetComponent<AudioSource>();
            if (m_AudioSource==null)
            {
                Debug.LogError("AudioSourceがアタッチされてません");
            }
        }
        if (m_ItemCount == null)
        {
            Debug.LogError("ItemCountのscriptが入ってません。");
        }

        if (m_animator == null)
        {
            m_animator = GetComponent<Animator>();
            if (m_animator == null)
            {
                Debug.LogError("アニメーターが入ってません。");
            }
        }
    }

    private void Update()
    {
        //オプション中は操作禁止
        if (Options.m_IsOptionsOpen)
            return;

        // Qボタンは構え
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //投げるアイテムがあるなら
            if (m_ItemCount.m_ItemCount > 0)
            {
                m_IsBrace = !m_IsBrace;
                if (m_IsBrace)
                {
                    Debug.Log("構えた");
                    m_AudioSource.PlayOneShot(m_BraceSE);
                    m_animator.SetBool("Brace", m_IsBrace);
                    m_IsThrow = true;
                    // 持ち石を生成（まだ持っていなければ）
                    if (m_HoldingStone == null)
                    {
                        m_HoldingStone = Instantiate(m_ThrowStone, m_SpawnStone.position, m_SpawnStone.rotation);
                        //m_HoldingStoneにコンポーネントしてなかったら
                        if (m_HoldingStone.GetComponent<MasterEnemySystem>()==null)
                        {
                            m_HoldingStone.AddComponent<MasterEnemySystem>();
                            Debug.Log("Stone に MasterEnemySystem を自動追加しました！");
                        }
                        // 手の位置の子にする（位置ずれ防止）
                        m_HoldingStone.transform.SetParent(m_SpawnStone);
                        // 重力オフにして落ちないように
                        var rd = m_HoldingStone.GetComponent<Rigidbody>();
                        if (rd != null)
                        {
                            rd.isKinematic = true;
                        }
                        else
                        {
                            Debug.LogError("石にRigidbodyがついてません。");
                        }
                        
                    }
                }
                else
                {                  
                    Debug.Log("しまった");
                    m_AudioSource.PlayOneShot(m_BraceSE);
                    m_animator.SetBool("Brace", m_IsBrace);
                    m_IsThrow = false;
                }

            }
           
        }
        // 左クリックで投げる
        if (Input.GetMouseButtonDown(0) && m_IsThrow)
        {
            Debug.Log("投げた");
            m_AudioSource.PlayOneShot(m_ThrowSE);
            m_IsBrace = false;
            // Throw アニメーション再生
            m_animator.SetTrigger("Throw");
            //アイテム消費
            m_ItemCount.m_UseItem = true;
            m_ItemCount.UseStoneItem();
        }

    }
    //投げるの時のアニメーションで制御
    void ThrowShoot()
    {
        // 手から離す
        m_HoldingStone.transform.SetParent(null);
        //Rigidbodyを取得
        Rigidbody rd= m_HoldingStone.GetComponent<Rigidbody>();
        //Rigidbodyがついていなければ
        if (rd!=null)
        {
            //手から離すように
            rd.isKinematic=false;
            //プレイヤーが向いてる方向に角度を求める
            Vector3 dir=Quaternion.AngleAxis(m_ThrowAngle,transform.right*-1) * transform.forward;
            //投げる力
            float throwPowar = 10f;           
            //力を加える
            rd.AddForce(dir * throwPowar, ForceMode.Impulse);
            #region　ForceMode.Impulseの意味
            //一瞬だけ力を加えて、その瞬間にRigidbody の速度を変える
            #endregion
        }
        else
        {
            Debug.LogError("石にRigidbodyがついてません。");
        }
        //投げた後７秒後消す
        Destroy(m_HoldingStone, 7f);
        // 投げたので手の石をリセット
        m_HoldingStone = null;
    }
    //アニメーションイベントで制御
    void ThrowEnd()
    {
        m_IsThrow = false;
        m_animator.SetBool("Brace", false);
        // 構え解除 → 手の石を消す
        if (m_HoldingStone != null)
        {
            Destroy(m_HoldingStone);
            m_HoldingStone = null;
        }
    }
}
