using UnityEngine;

public class RockDoorPush : MonoBehaviour
{
    [Header("鍵の名前")]
    public string m_KeyName;
    [Header("キャラクターのアニメーター")]
    public Animator m_PushAnimator;
    [Header("ドアのアニメーター")]
    public Animator m_OpenAnimator;
    [Header("鍵がない場合の画像")]
    public GameObject m_NoImage;
    [Header("鍵がある場合の画像")]
    public GameObject m_OnImage;
    //ドアの開く音
    public AudioClip m_Doorsound;
    private AudioSource m_audioSource;
    //プレイヤーがドアをおしたか
    private bool pushPlayed = false;
    //エリア（トリガー）
    private bool isInArea = false;
    //クエストをクリアしたか
    private bool m_Clear=false;



    //アイテム管理を参照
    private ItemManager m_itemManager;

    private void Start()
    {
        //開始時は非表示
        m_NoImage.SetActive(false);
        m_OnImage.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();

    }

    [System.Obsolete]
    private void Update()
    {
        // 入力は毎フレームチェック
        if (isInArea && Input.GetKeyDown(KeyCode.E) && !pushPlayed)
        {
            //Keyは持っているか
            if (m_itemManager.HasItem(m_KeyName))
            {
                m_Clear = true;
                //開く音
                m_audioSource.PlayOneShot(m_Doorsound);
                pushPlayed = true;
                //ドアアニメーション（オン）
                OC_DoorAnimaier(true);
                //キャラクタアニメーション
                m_PushAnimator.SetBool("Push", true);
                // 0.5秒後にPushを戻す
                Invoke(nameof(ResetPush), 0.5f);
                m_OnImage.SetActive(false);
                if (m_Clear)
                {
                    // クエストマネージャーに通知
                    FindObjectOfType<QuestManager>().OnItemObtained(m_KeyName);
                }
            }
        }
    }
    //アニメーターを元のステートに戻す
    private void ResetPush()
    {
        m_PushAnimator.SetBool("Push", false);
        pushPlayed = false;
    }
    //エリアに入ったら
    private void OnTriggerEnter(Collider other)
    {
        //パラメータを持っている人だけ
        if (other.GetComponent<Parameta>())
        {
            if (other.GetComponent<Parameta>().m_Team == "Player")
            {
                m_itemManager = other.GetComponent<ItemManager>();
                //Keyをもっていたら
                if (m_itemManager.HasItem(m_KeyName))
                {
                    //エリア検知
                    isInArea = true;
                    //画像表示
                    m_OnImage.SetActive(true);
                }
                else
                {
                    //画像表示
                    m_NoImage.SetActive(true);
                }
            }
        }
    }
    //エリアに出たら
    private void OnTriggerExit(Collider other)
    {
        //パラメータを持っている人だけ
        if (other.GetComponent<Parameta>())
        {
            if (other.GetComponent<Parameta>().m_Team == "Player")
            {
                //Keyをもっているか
                if (m_itemManager.HasItem(m_KeyName))
                {
                    //エリア非検知
                    isInArea = false;
                    //画像非表示
                    m_OnImage.SetActive(false);
                    //ドアアニメーション（オフ）
                    OC_DoorAnimaier(false);
                }
                else
                {
                    //画像非表示
                    m_NoImage.SetActive(false);
                }
            }
           
        }
    }

    public void OC_DoorAnimaier(bool Flag)
    {
        //開くアニメーション
        m_OpenAnimator.SetBool("Open", Flag);
    }
}
