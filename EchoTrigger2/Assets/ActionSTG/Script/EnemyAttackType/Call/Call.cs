using UnityEngine;

public class Call : MonoBehaviour
{
    [Header("非表示→表示にする敵をここに登録")]
    public GameObject m_EnemySpon;  
   [SerializeField] private Animator m_Animator;

    private void Awake()
    {
        // Inspector で未設定の場合、自動検索
        if (m_EnemySpon == null)
        {
            //非アクティブも含めてシーン内から検索
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var obj in allObjects)
            {
                //スポーン地点Callを探す
                if (obj.name == "スポーン地点Call" && obj.scene.isLoaded)
                {
                    m_EnemySpon = obj;
                    Debug.Log("自動で敵オブジェクトを設定しました: " + obj.name);
                    break;
                }
            }

            if (m_EnemySpon == null)
            {
                Debug.LogError("敵オブジェクト 'スポーン地点Call' がシーン内に見つかりませんでした。");
            }
        }
    }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();

        // 敵オブジェクトを再表示
        if (m_EnemySpon != null)
        {
            m_EnemySpon.SetActive(true);
            Debug.Log("敵を再表示しました: " + m_EnemySpon.name);
        }
    }

    private void Update()
    {
        // 攻撃アニメーション再生
        if (m_Animator != null)
        {
            m_Animator.SetBool("Attack", true);
        }
    }
}
