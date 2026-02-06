using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Memo : MonoBehaviour
{
    [Header("Playerアニマーター")]
    public Animator m_Animator;
    [Header("画像")]
    public GameObject m_PickUpImage;
    [Header("Memoのキャンバス")]
    public Canvas m_Canvas;
    [Header("音楽")]
    public AudioClip m_MemoAudio;
    [Header("Hpバー")]
    public Image m_HPUI;
    [Header("HpバーRed")]
    public Image m_HPUIRed;
    [Header("Buttunのscript")]
    public Buttun m_buttun;
    private AudioSource m_AudioSource;
    //エリアに入ったかどうか
    private bool m_Aria = false;
    //Eキーを押したかどうか
    private bool m_E_KeyPush = false;

    /// <summary>
    /// メモが開いているかどうか（他のスクリプトから参照用）
    /// </summary>
    public static bool m_IsMemoOpen { get; private set; } = false;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        //非表示
        m_PickUpImage.SetActive(false);
        m_Canvas.enabled = false;
        //初期状態はメモを閉じている
        m_IsMemoOpen = false;
    }
    private void Update()
    {
        if (m_Aria && !m_E_KeyPush && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("開く"); 
            m_E_KeyPush = true;
            //メモが開いている状態
            m_IsMemoOpen = true;
            //画像表示
            m_Canvas.enabled = true;
            m_buttun.m_TABImage.enabled = true;
            //画像非表示
            m_HPUI.enabled = false;
            m_HPUIRed.enabled = false;
            //音楽
            m_AudioSource.PlayOneShot(m_MemoAudio);
        }
        if (m_E_KeyPush && Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Memo閉じる");
            m_E_KeyPush = false;
            //メモが閉じている状態
            m_IsMemoOpen = false;
            //画像表示
            m_HPUI.enabled = true;
            m_HPUIRed.enabled = true;
            //画像非表示
            m_Canvas.enabled = false;
            m_buttun.m_TABImage.enabled = false;

            //音楽
            m_AudioSource.PlayOneShot(m_MemoAudio);

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //playerなら
        if (other.CompareTag("Player"))
        {
            m_Aria = true;
            //表示
            m_PickUpImage.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //playerでなら
        if (other.CompareTag("Player"))
        {
            m_Aria = false;
            //非表示
            m_PickUpImage.SetActive(false);
        }
    }
}
