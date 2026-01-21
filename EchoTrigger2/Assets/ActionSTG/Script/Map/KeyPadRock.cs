using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
public class KeyPadRock : MonoBehaviour
{
    [Header("正解のコード")]
    public string m_KeypadCode = "1234";
    [Header("ディスプレイに表示するテキスト")]
    public TextMeshProUGUI m_DisplayText;
    [Header("正解の効果音")]
    public AudioClip m_Success;
    [Header("失敗の効果音")]
    public AudioClip m_Failure;
    [Header("選択効果音")]
    public AudioClip m_Push;
    [Header("カメラ")]
    public Camera m_Camera;
    [Header("不正解用の電気")]
    public GameObject m_Electricity;
    [Header("ダメージ")]
    public int m_DamageOnFail = 20;
    [Header("Buttanのscript")]
    public Buttun m_Buttan;
    [Header("吹き出し")]
    public GameObject m_Hukidasi;
    // プレイヤーの参照を保存しておく
    private Parameta m_PlayerParameta;
    //エリアに入ったかどうか？
    private bool m_Aria = false;
    // Keypadを押したかどうか
    private bool m_Expanded = false;
    //入力の文字列
    private string m_Input = "";
    // 効果音を鳴らすための AudioSource
    private AudioSource m_AudioSource;
    //正解したかどうか;
    public bool m_OpenDoor = false;


    void Start()
    {
        // 最初にリセット表示
        UpdateDisplay();
        // AudioSource を取得
        m_AudioSource = GetComponent<AudioSource>();
        //非表示
        m_Camera.enabled = false;
        m_Electricity.SetActive(false);
        m_Hukidasi.SetActive(false);
    }
    private void Update()
    {
        // Eキーを押したら大きくする
        if (m_Aria && Input.GetKeyDown(KeyCode.E) && !m_Expanded&& !m_OpenDoor)
        {
            Debug.Log("KEYPADを選択！");
            m_Expanded = true;
            //画像表示
            m_Buttan.m_TABImage.enabled = true;
            m_Buttan.m_ENTERImage.enabled = true;
            m_Buttan.m_BACKSPACEImage.enabled = true;
            //カメラ表示
            m_Camera.enabled = true;
            //入力をリセット
            m_Input = "";
            UpdateDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && m_Expanded&& !m_OpenDoor)
        {
            Debug.Log("KEYPADから退出！");
            m_Expanded = false;
            //画像非表示
            m_Buttan.m_TABImage.enabled = false;
            m_Buttan.m_ENTERImage.enabled = false;
            m_Buttan.m_BACKSPACEImage.enabled = false;
            //カメラを非表示
            m_Camera.enabled = false;
            UpdateDisplay();
        }
        //拡大中の時だけ入力を受け付ける
        if (m_Expanded)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (m_Input.Length < 4)
                {
                    if (Input.GetKeyDown(i.ToString()))
                    {
                        m_AudioSource.PlayOneShot(m_Push);
                        m_Input += i.ToString();
                        UpdateDisplay();
                    }
                }
            }
            //Backspaceキーで削除
            if (Input.GetKeyDown(KeyCode.Backspace) && m_Input.Length > 0)
            {
                m_AudioSource.PlayOneShot(m_Push);
                m_Input = m_Input.Substring(0, m_Input.Length - 1);
                UpdateDisplay();
            }
            //エンターキーで判定
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (m_Input == m_KeypadCode)
                {
                    Debug.Log("正解！");
                    m_AudioSource.PlayOneShot(m_Success);
                    m_OpenDoor = true;
                    m_Camera.enabled = false;
                    //画像非表示
                    m_Buttan.m_TABImage.enabled = false;
                    m_Buttan.m_ENTERImage.enabled = false;
                    m_Buttan.m_BACKSPACEImage.enabled = false;
                }
                else
                {
                    Debug.Log("不正解");
                    m_AudioSource.PlayOneShot(m_Failure);
                    StartCoroutine(EffectTime());
                    m_Camera.enabled = false;
                    m_Expanded = false;
                    //画像非表示
                    m_Buttan.m_TABImage.enabled = false;
                    m_Buttan.m_ENTERImage.enabled = false;
                    m_Buttan.m_BACKSPACEImage.enabled = false;
                    //プレイヤーにダメージを与える
                    if (m_PlayerParameta != null)
                    {
                        m_PlayerParameta.TakeDamege(m_DamageOnFail);
                    }
                    else
                    {
                        Debug.Log("m_PlayerParametaが入ってません。");
                    }



                }
                // 判定後にリセットするならここ
                m_Input = "";
                UpdateDisplay();
            }

        }

    }
    private void UpdateDisplay()
    {
        if (m_DisplayText != null)
        {
            // 入力中は入力値を表示(最初は____に設定)
            m_DisplayText.text = m_Input.PadRight(4, '_');
        }
    }
    //effectの時間
    private IEnumerator EffectTime()
    {
        // 表示
        m_Electricity.SetActive(true);
        // 1秒待つ
        yield return new WaitForSeconds(1f);
        // 非表示
        m_Electricity.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_OpenDoor)
        {
            Debug.Log("Keypadの範囲に入りました！");
            //エリアに入った
            m_Aria = true;
            // プレイヤーのParametaを取得して保持
            m_PlayerParameta = other.GetComponent<Parameta>();
            m_Hukidasi.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !m_OpenDoor)
        {
            Debug.Log("Keypadの範囲から出ました！");
            //エリアに入った
            m_Aria = false;
            m_PlayerParameta = null;
            m_Hukidasi.SetActive(false);
        }
    }
}
