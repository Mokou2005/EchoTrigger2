using UnityEngine;

public class KeyPadRockDoor : MonoBehaviour
{
    [Header("ドアのアニメーター")]
    public Animator m_OpenAnimator;
    [Header("空かない時の画像")]
    public GameObject m_NoImage;
    [Header("KeyPadRockscript")]
    public KeyPadRock m_KeyPadRock;
    //ドアの開く音
    public AudioClip m_Doorsound;
    private AudioSource m_audioSource;
    private bool m_Activation=false;

    private void Start()
    {
        //開始時は非表示
        m_NoImage.SetActive(false);
        m_audioSource = GetComponent<AudioSource>();
       

    }

    private void Update()
    {
        //暗号があってたら
        if (m_KeyPadRock.m_OpenDoor&& !m_Activation)
            {
                //開く音
                m_audioSource.PlayOneShot(m_Doorsound);
                //ドアアニメーション（オン）
                OC_DoorAnimaier(true);
            m_Activation = true;

            }

    }

    

    public void OC_DoorAnimaier(bool Flag)
    {
        //開くアニメーション
        m_OpenAnimator.SetBool("Open", Flag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& !m_KeyPadRock.m_OpenDoor)
        {
            Debug.Log("パスワードをうってください");
            m_NoImage.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !m_KeyPadRock.m_OpenDoor)
        {
            m_NoImage.SetActive(false);
        }
    }
}


