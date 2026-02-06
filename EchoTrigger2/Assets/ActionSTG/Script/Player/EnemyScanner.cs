using UnityEngine;
using System.Collections;

public class EnemyScanner : MonoBehaviour
{
    public Material m_scanMat;
    public float m_speed = 10f;
    private bool m_isScanning = false;

    void Update()
    {
        //オプション中またはメモを見ている間は操作禁止
        if (Options.m_IsOptionsOpen || Memo.m_IsMemoOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && !m_isScanning)
        {
            StartCoroutine(Scan());
        }
    }

    IEnumerator Scan()
    {
        m_isScanning = true;
        m_scanMat.SetFloat("_Scan", 1);
        m_scanMat.SetFloat("_ScanRadius", 0);

        float radius = 0f;
        while (radius < 100f)
        {
            radius += Time.deltaTime * m_speed;
            m_scanMat.SetFloat("_ScanRadius", radius);
            yield return null;
        }

        m_scanMat.SetFloat("_Scan", 0);
        m_isScanning = false;
    }
}
