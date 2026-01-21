using UnityEngine;

public class BlockPlant : MonoBehaviour
{
    [Header("ロックされている物")]
    public GameObject m_RockObject;
    [Header("道具の名前")]
    public string m_ItemName;
    [Header("キャラクターのアニメーター")]
    public Animator m_PushAnimator;
    [Header("道具の画像")]
    public GameObject m_Image;


    //エリアに入ったかどうか
    private bool m_isArea;
    //アイテム管理を参照
    private ItemManager m_ItemManager;

    private void Start()
    {
        //非表示
        m_Image.SetActive(false);
    }
    private void Update()
    {
        if (m_isArea && Input.GetKeyDown(KeyCode.E))
        {
            // クエストマネージャーに通知
            FindObjectOfType<QuestManager>().OnItemObtained(m_ItemName);
            m_RockObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("マッチ未所持。エリアに入った");
            m_ItemManager = other.GetComponent<ItemManager>();
            //パールを持っているか？
            if (m_ItemManager.HasItem(m_ItemName))
            {
                Debug.Log("マッチを持っています！");
                //エリア検知
                m_isArea = true;
                m_Image.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("マッチ未所持。エリアから出た");
            //パールを持っているか？
            if (m_ItemManager.HasItem(m_ItemName))
            {
                //エリア非検知
                m_isArea = false;
                m_Image.SetActive(false);
            }

        }
    }
}

