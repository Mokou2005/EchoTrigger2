using UnityEngine;


public class DoorPush : MonoBehaviour
{
    [Header("キャラクターのアニメーター")]
    public Animator m_PushAnimator;
    [Header("ドアのアニメーター")]
    public Animator m_OpenAnimator;
    //画像
    public GameObject m_Image;
    //ドアの開く音
    public AudioClip m_Doorsound;
　　private AudioSource m_audioSource;
    //プレイヤーがドアをおしたか
    private bool pushPlayed = false;
    //エリア（トリガー）
    private bool isInArea = false;

    private void Start()
    {
        //開始時は非表示
        m_Image.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();
       
    }

    private void Update()
    {
        // 入力は毎フレームチェック
        if (isInArea && Input.GetKeyDown(KeyCode.E) && !pushPlayed)
        {
            //開く音
            m_audioSource.PlayOneShot(m_Doorsound);
            pushPlayed = true;
            //ドアアニメーション（オン）
            OC_DoorAnimaier(true);
            //キャラクタアニメーション
            m_PushAnimator.SetBool("Push", true);
            // 0.5秒後にPushを戻す
            Invoke(nameof(ResetPush), 0.5f);
            m_Image.SetActive(false);

        }
    }
    //アニメーターを元のステートに戻す
    private void ResetPush()
    {
        m_PushAnimator.SetBool("Push", false);
        pushPlayed = false;  
    }
    //エレベータ用ドア（自動ドア）
    public void ForceDoor(bool open)
    {
        //開閉アニメーション
        OC_DoorAnimaier(open);
        //サウンド再生
        if (open && m_Doorsound != null)
        {
            m_audioSource.PlayOneShot(m_Doorsound);
        }
    }
    //エリアに入ったら
    private void OnTriggerEnter(Collider other)
    {
        //パラメータを持っている人だけ
        if (other.GetComponent<Parameta>())
        {
           //エリア検知
            isInArea = true;
            //画像表示
            m_Image.SetActive(true);
        }
    }
    //エリアに出たら
    private void OnTriggerExit(Collider other)
    {
        //パラメータを持っている人だけ
        if (other.GetComponent<Parameta>())
        {
    
            //エリア非検知
            isInArea = false;
            //画像非表示
            m_Image.SetActive(false);
            //ドアアニメーション（オフ）
            OC_DoorAnimaier(false);
           
        }
    }

    public void OC_DoorAnimaier(bool Flag)
    {
        //開くアニメーション
        m_OpenAnimator.SetBool("Open", Flag);
    }
}