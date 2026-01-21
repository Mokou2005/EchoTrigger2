using UnityEngine;


public class DoorPushManual : MonoBehaviour
{
    [Header("キャラクターのアニメーター")]
    public Animator m_PushAnimator;//キャラクターのアニメーター
    [Header("ドアのアニメーター")]
    public Animator m_OpenAnimator;//ドアのアニメーター
    public GameObject m_Image;//画像
    public AudioClip m_Doorsound;//ドアの開く音
    private AudioSource m_audioSource;//効果音
    private bool pushPlayed = false;//プレイヤーがドアをおしたか
    private bool isInArea = false;//エリア

    private void Start()
    {
        //開始時は非表示
        m_Image.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {

            // 入力は毎フレームチェック
       
            if (isInArea && Input.GetKeyDown(KeyCode.E))
            {
                pushPlayed = !pushPlayed; // 開閉切り替え
                m_audioSource.PlayOneShot(m_Doorsound); // 音を再生

                OC_DoorAnimaier(pushPlayed); // アニメーション状態切り替え
                m_PushAnimator.SetBool("Push", true); // キャラ押しアニメ
                Invoke(nameof(ResetPush), 0.5f); // キャラ押し終了
                m_Image.SetActive(false); // UI非表示
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
           

        }
    }

    public void OC_DoorAnimaier(bool Flag)
    {
        //開くアニメーション
        m_OpenAnimator.SetBool("Open", Flag);
    }
}
