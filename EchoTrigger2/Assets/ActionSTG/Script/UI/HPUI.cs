using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [Header("HPƒo[‚ÌImage")]
    public Image m_HPImage;
    public void UpdateHp(int current, int max)
    {
       // MaxHP‚ª‚O‚æ‚è‘å‚«‚©‚Á‚½‚ç
        if (m_HPImage != null && max > 0)
        {
            //Œ»İ‚ÌHp‚ÆÅ‘åHp‚ğŠ„‚éŠ„‡
            float ratio = (float)current / max;
            //fillAmount‚Í“h‚è‚Â‚Ô‚µ‚Ì‚Ég‚¦‚é
            m_HPImage.fillAmount = ratio;

        }
    }
}
