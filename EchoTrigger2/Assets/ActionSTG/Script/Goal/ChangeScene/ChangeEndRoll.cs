using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// ゴールムービからエンドロールへ移動の処理
/// </summary>
public class ChangeEndRoll : MonoBehaviour
{
    /// <summary>
    /// アニメーションイベントでSecenを移動
    /// </summary>
    public void ChangeEndRollSecen()
    {
        SceneManager.LoadScene("EndRoll");
    }
}
