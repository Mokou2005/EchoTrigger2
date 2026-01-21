
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //プレイヤーが持つアイテムリストをstringで保存
     public List<string> itemList = new List<string>();
    //アイテム追加
    public void AddItem(string itemName)
    {
        if (!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            Debug.Log("アイテムを追加しました: " + itemName);
        }
        else
        {
            Debug.Log("すでに持っています: " + itemName);
        }
    }
    // アイテムを所持しているか確認
    public bool HasItem(string itemName)
    {
        return itemList.Contains(itemName);
    }
}
