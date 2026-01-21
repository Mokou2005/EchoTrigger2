using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("ロックされている物")]
    public GameObject m_RockObject;
    [Header("道具の名前")]
    public string m_ItemName;
    [Header("キャラクターのアニメーター")]
    public Animator m_PushAnimator;
    [Header("道具がない場合の画像")]
    public GameObject m_NoImage;
    [Header("道具がある場合の画像")]
    public GameObject m_OnImage;


    //エリアに入ったかどうか
    private bool m_isArea;
    //アイテム管理を参照
    private ItemManager m_ItemManager;

    private void Start()
    {
        m_NoImage.SetActive(false);
        m_OnImage.SetActive(false);
    }
    private void Update()
    {
        if (m_isArea && Input.GetKeyDown(KeyCode.E))
        {
            // クエストマネージャーに通知
            FindObjectOfType<QuestManager>().OnItemObtained(m_ItemName);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
     
            m_ItemManager = other.GetComponent<ItemManager>();
            //パールを持っているか？
            if (m_ItemManager.HasItem(m_ItemName))
            {
                Debug.Log("バールを持っています！");
                //エリア検知
                m_isArea=true;
                //画像を表示
                m_OnImage.SetActive(true);
            }
            else
            {
                Debug.Log("バールを持っていません！");
                //画像表示
                m_NoImage.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //パールを持っているか？
            if (m_ItemManager.HasItem(m_ItemName))
            {
       
                //エリア非検知
                m_isArea = false;
                //画像を非表示
                m_OnImage.SetActive(false);
            }
            else
            {
               
                //画像非表示
                m_NoImage.SetActive(false);
            }
        }
    }
}
