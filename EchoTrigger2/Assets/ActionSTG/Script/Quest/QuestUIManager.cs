using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    [Header("タイトル画像")]
    public Image m_TitleImage;

    [Header("説明画像")]
    public Image m_DescriptionImage;

    // クエスト情報をUIに反映
    public void SetQuestUI(QuestData quest)
    {
        if (quest == null)
        {
            m_TitleImage.sprite = null;
            m_DescriptionImage.sprite = null;
            m_TitleImage.enabled = false;
            m_DescriptionImage.enabled = false;
            return;
        }

        m_TitleImage.enabled = true;
        m_DescriptionImage.enabled = true;

        m_TitleImage.sprite = quest.m_TitleImage;
        m_DescriptionImage.sprite = quest.m_DescriptionImage;
    }
}