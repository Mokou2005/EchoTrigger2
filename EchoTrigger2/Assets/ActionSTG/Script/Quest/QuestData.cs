using UnityEngine;

[CreateAssetMenu(menuName = "Quest/New Quest Data")]
public class QuestData : ScriptableObject
{
    [Header("クエスト名")]
    public string m_QuestName;

    [Header("タイトル画像（上に表示）")]
    public Sprite m_TitleImage;

    [Header("説明画像（下に表示）")]
    public Sprite m_DescriptionImage;

    [Header("達成条件（アイテム名など）")]
    public string m_Item;

    [Header("次のクエスト")]
    public QuestData m_NextQuest;
}
