using UnityEngine;
using UnityEngine.UI;

public class Buttun : MonoBehaviour
{
    [Header("TAB�̉摜")]
    public RawImage m_TABImage;
    [Header("ENTER�̉摜")]
    public RawImage m_ENTERImage;
    [Header("BACKSPACE�̉摜")]
    public RawImage m_BACKSPACEImage;

    private void Start()
    {
        m_TABImage.enabled = false;
        m_ENTERImage.enabled = false;
        m_BACKSPACEImage.enabled = false;
    }

}
