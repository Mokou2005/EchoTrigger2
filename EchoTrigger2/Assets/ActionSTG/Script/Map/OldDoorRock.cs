using UnityEngine;

public class OldDoorRock : MonoBehaviour
{
    [Header("鍵の名前")]
    public string m_KeyName;
    [Header("キャラクターのアニメーター")]
    public Animator m_PushAnimator;
    [Header("ドアのアニメーター")]
    public Animator m_OpenAnimator;
    //ドアの開く音
    public AudioClip m_Doorsound;
    private AudioSource m_audioSource;
    //プレイヤーがドアをおしたか
    private bool pushPlayed = false;
    //エリア
    private bool isInArea = false;
    //アイテム管理参照
    private ItemManager m_itemManager;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {


        // 入力は毎フレームチェック
        if (isInArea && Input.GetKeyDown(KeyCode.E))
        {
            //keyは持っているか
            if (m_itemManager.HasItem(m_KeyName))
            {
                // 開閉切り替え
                pushPlayed = !pushPlayed;
                // 音を再生
                m_audioSource.PlayOneShot(m_Doorsound);
                // アニメーション状態切り替え
                OC_DoorAnimaier(pushPlayed);
                // キャラ押しアニメ
                m_PushAnimator.SetBool("Push", true);
                // キャラ押し終了
                Invoke(nameof(ResetPush), 0.5f);
            }
        }
    }
    //アニメーターを元のステートに戻す
    private void ResetPush()
    {
        m_PushAnimator.SetBool("Push", false);


    }
    //エリアに入ったら
    private void OnTriggerEnter(Collider other)
    {

        //パラメータを持っている人だけ
        if (other.GetComponent<Parameta>())
        {
            m_itemManager = other.GetComponent<ItemManager>();
            //keyは持っているか
            if (m_itemManager.HasItem(m_KeyName))
            {
                //エリア検知
                isInArea = true;
            }
            //Keyを持っていなければ
            else
            {
            }
        }
    }
    //エリアに出たら
    private void OnTriggerExit(Collider other)
    {
        //パラメータを持っている人だけ
        if (other.GetComponent<Parameta>())
        {
            //keyは持っているか
            if (m_itemManager.HasItem(m_KeyName))
            {
                //エリア非検知
                isInArea = false;

            }
            //Keyを持っていなければ
            else
            {

            }

        }
    }

    public void OC_DoorAnimaier(bool Flag)
    {
        //開くアニメーション
        m_OpenAnimator.SetBool("Open", Flag);
    }
}
