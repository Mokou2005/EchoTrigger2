using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("最初のクエストデータ")]
    public QuestData m_StartQuest;
    private QuestData m_questData;
    [Header("UIマネージャー参照")]
    public QuestUIManager m_UIManager;
    void Start()
    {
        StartQuest(m_StartQuest);
    }

    void StartQuest(QuestData quest)
    {
        m_questData = quest;
        Debug.Log($"クエスト開始:{m_questData.m_QuestName}");

        var ui = FindObjectOfType<QuestUIManager>();
        if (ui != null)
        {
            ui.SetQuestUI(m_questData);
            Debug.Log("UI更新完了！");
        }
        else
        {
            Debug.LogWarning("QuestUIManager がシーンに見つかりません！");
        }
    }

    public void OnItemObtained(string itemName)
    {
        //現在のクエストがないならなにもしない
        if (m_questData == null) return;
        //現在のクエストのアイテムと一致したら
        if (itemName == m_questData.m_Item)
        {
            Debug.Log($"クエスト達成: {m_questData.m_NextQuest}");
            //次のクエストが設定されてたら次のクエストへ
            if (m_questData.m_NextQuest != null)
            {
                StartQuest(m_questData.m_NextQuest);
            }
            else
            {
                Debug.Log("全クエスト完了！");
            }
        }
    }
}
