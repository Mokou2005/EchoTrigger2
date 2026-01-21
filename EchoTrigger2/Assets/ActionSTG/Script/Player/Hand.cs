using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header("右手のボーン")]
    public Transform m_Hand;
    [Header("銃のオブジェクト")]
    public GameObject m_Gun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //作成
        GameObject gun = Instantiate(m_Gun, m_Hand);
        //銃を手の中心に固定
        gun.transform.localPosition = Vector3.zero;
        //手の向きに一致
        gun.transform.localRotation = Quaternion.identity;
    }

 
}
