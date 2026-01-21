using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("このアイテムの名前")]
    public string itemName = "LabKey";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{itemName} を取得！");

            // クエストマネージャーに通知
            FindObjectOfType<QuestManager>().OnItemObtained(itemName);

            // アイテムを消す
            Destroy(gameObject);
        }
    }
}
