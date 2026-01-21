using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleLoadScene : MonoBehaviour
{
    [Header("一階のマップの素材")]
    [SerializeField] private GameObject[] m_FirstFloorMap;

    [Header("二階のマップの素材")]
    [SerializeField] private GameObject[] m_SecondFloorMap;

    public List<GameObject> m_Datas;

    private IEnumerator Start()
    {
        //一階の読み込み
        yield return StartCoroutine(LoadMapGroup(
            "Map/FirstFloor",
            true,
            result => m_FirstFloorMap = result
        ));

        //二階の読み込み
        yield return StartCoroutine(LoadMapGroup(
            "Map/SecondFloor",
            false,
            result => m_SecondFloorMap = result
        ));
    }

    /// <summary>
    /// 指定したフォルダに入っているマップPrefabをすべてロードし、
    /// ゲーム上に生成する処理
    /// </summary>
    /// <param name="path">Resources内のフォルダパス</param>
    /// <param name="active">最初から表示するかどうか</param>
    /// <param name="onLoaded">生成完了後に配列を受け取る処理</param>
    IEnumerator LoadMapGroup(
        string path,
        bool active,
        System.Action<GameObject[]> onLoaded
    )
    {
        // Resourcesフォルダ内の指定パスから
        // GameObject（Prefab）をすべて読み込む
        Object[] assets = Resources.LoadAll<GameObject>(path);

        // 読み込んだPrefabの数だけ
        // 生成したオブジェクトを入れる配列を用意
        GameObject[] result = new GameObject[assets.Length];

        // 読み込んだPrefabを1つずつ処理する
        for (int i = 0; i < assets.Length; i++)
        {
            // Prefabを実体（GameObject）として生成する
            GameObject obj = Instantiate(assets[i] as GameObject);
            m_Datas.Add(obj);
            // 生成したオブジェクトを
            // 表示するか・非表示にするかを設定
            obj.SetActive(active);

            // 生成したオブジェクトを配列に保存
            result[i] = obj;

            // 1フレーム待って処理を分割し、
            // フレーム落ちを防ぐ（疑似的な非同期処理）
            yield return null;
        }

        onLoaded?.Invoke(result);
    }

    // 1階表示
    public void ShowFirstFloor()
    {
        SetActiveGroup(m_FirstFloorMap, true);
        SetActiveGroup(m_SecondFloorMap, false);
    }

    // 2階表示
    public void ShowSecondFloor()
    {
        SetActiveGroup(m_FirstFloorMap, false);
        SetActiveGroup(m_SecondFloorMap, true);
    }

    void SetActiveGroup(GameObject[] group, bool active)
    {
        if (group == null) return;

        foreach (var obj in group)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}
