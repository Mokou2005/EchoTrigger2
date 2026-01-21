using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
///　物を投げるUIの処理
/// </summary>
public class ItemCount : MonoBehaviour
{
    [Header("もっているアイテムの数")]
    public float m_ItemCount = 3f;

    [Header("アイテムの数のText")]
    [SerializeField] private Text m_ItemText;

    //アイテムを使ったかどうか
    public bool m_UseItem = false;

    //実行中かどうか
    public bool m_IsWarningRunning = false;

    /// <summary>
    /// アイテムの消費処理
    /// </summary>
    public void UseStoneItem()
    {
        //アイテムを一個以上持っているか
        if (m_ItemCount >= 0)
        {
            //アイテムを一つ減らす
            if (m_UseItem)
            {
                m_ItemCount -= 1;

                m_UseItem = false;

                Debug.Log("残りアイテム数:" + m_ItemCount);
                UpdateItemText();
            }
        }
        else
        {
            Debug.Log("アイテムが０個です！");
        }
    }

    /// <summary>
    /// Textに残り数を変更
    /// </summary>
    void UpdateItemText()
    {
        m_ItemText.text = "×" + ((int)m_ItemCount).ToString();
    }
}
