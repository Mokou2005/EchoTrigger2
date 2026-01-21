using System.Collections.Generic;
using UnityEngine;
using StateMachineAI;
using UnityEngine.Rendering.Universal;


public class Respon : MonoBehaviour
{
    public MasterEnemySystem m_MES;

    [Header("敵のタイプの変更")]
    public int m_UnitType = 0;
    public GameObject m_Body;
    public Transform m_Player;
    public bool m_SetUpFlag;

    [System.Serializable]

    //リストを二重にする
    public struct AINames
    {
        [Header("敵の名前")]
        public string m_Name;
        public List<string> AIName;
    }

    public List<AINames> m_Ainame;

    public void Start()
    {

    }
    public void Update()
    {
        if (!m_SetUpFlag && m_MES)
        {
            SetUp();
            m_SetUpFlag = true;
        }
    }
    public void SetUp()
    {
        GameObject D = Instantiate(m_Body, transform.position, transform.rotation);
        m_MES.EnemyyAdd(D);
        EnemyAI EA = D.GetComponent<EnemyAI>();
        if (EA == null)
        {
            Debug.LogError($"{m_Body.name} に EnemyAI がアタッチされていません！", D);
            return;
        }
        EA.m_RSP = this;
        EA.m_Player = m_Player;
        //生成された敵にEnemyPatrol_Waypointがついてるか調べ、ついていなければ追加
        EnemyPatrol_Waypoint patrol = D.GetComponent<EnemyPatrol_Waypoint>();
        if (patrol == null)
        {
            patrol = D.AddComponent<EnemyPatrol_Waypoint>();
        }
        patrol.SetRespon(this);
        //指定したタイプのリストがない場合通知する
        if (m_Ainame[m_UnitType].AIName.Count == 0)
        {
            Debug.Log("リストはありますが中身が空です");
        }
        else
        {
            //m_UnitTypeに指定したタイプのステートをdummyにして入れる
            foreach (string dummy in m_Ainame[m_UnitType].AIName)
            {
                EA.AddStateByName(dummy);
            }




            EA.AISetUp();
        }
    }
}
