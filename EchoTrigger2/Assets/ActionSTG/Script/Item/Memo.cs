
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Memo : MonoBehaviour
{
    [Header("Playerアニメーター")]
    public Animator m_Animator;
    [Header("拾う画像")]
    public GameObject m_PickUpImage;
    [Header("Memoのキャンバス")]
    public Canvas m_Canvas;
    [Header("紙の音")]
    public AudioClip m_MemoAudio;
    [Header("Hpバー")]
    public Image m_HPUI;
    [Header("Hpバー赤")]
    public Image m_HPUIRed;
    [Header("Buttunのscript")]
    public Buttun m_buttun;
    private AudioSource m_AudioSource;
    //エリアに入ったかどうか
    private bool m_Aria = false;
    //Eキーを押したかどうか
    private bool m_E_KeyPush = false;
 
    

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        //非表示
        m_PickUpImage.SetActive(false);
        m_Canvas.enabled = false;
    }
    private void Update()
    {
        if (m_Aria && !m_E_KeyPush && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("メモを開いた"); 
            m_E_KeyPush = true;           
            //画像を表示
            m_Canvas.enabled = true;
            m_buttun.m_TABImage.enabled = true;
            //画像を非表示
            m_HPUI.enabled = false;
            m_HPUIRed.enabled = false;
            //メモの音
            m_AudioSource.PlayOneShot(m_MemoAudio);
        }
        if (m_E_KeyPush && Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Memoを閉じた");
            m_E_KeyPush = false;
            //画像を表示
            m_HPUI.enabled = true;
            m_HPUIRed.enabled = true;
            //画像を非表示
            m_Canvas.enabled = false;
            m_buttun.m_TABImage.enabled = false;

            //メモの音
            m_AudioSource.PlayOneShot(m_MemoAudio);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //playerが入ったら
        if (other.CompareTag("Player"))
        {
            m_Aria = true;
            //表示
            m_PickUpImage.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //playerがでたら
        if (other.CompareTag("Player"))
        {
            m_Aria = false;
            //非表示
            m_PickUpImage.SetActive(false);
        }
    }
}
