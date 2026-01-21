using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    /// <summary>
    /// 開始
    /// </summary>
    private void Start()
    {
        //自分のオブジェクトを消す
        gameObject.SetActive(false);
    }

    /// <summary>
    /// エリアに入ったらゴールの処理を行う
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ゴール！！");
        //※処理はここから
        SceneManager.LoadScene("GoalScecn");
    }
}
