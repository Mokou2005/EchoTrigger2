using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.AI;

public class Bite : MonoBehaviour
{
    [SerializeField]private Animator m_Animator;
    [SerializeField] private DogSound m_DogSound;
    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] private Transform m_BitePoint;
    [SerializeField] private GameObject m_BitePrefab;
    [SerializeField] private Parameta m_Parameta;
    // プレイヤーのTransformを保持
    public Transform m_Target;
    public float m_Count=0;
    //遠吠えをしたか
    public bool m_Howl = false;
    //吠え終わったか
    public bool m_HowlFinished = false;
   　//プレイヤエリアに入ったか
    public bool m_Area = false;
    //Sound
    public bool m_BiteSound = false;
    //噛みついたか
    public bool m_Bite=false;
    //死んだか
    public bool m_Die=false;
    //噛まれたら動き停止
    public static bool m_BiteOut = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Parameta=GetComponent<Parameta>();
        m_Animator = GetComponent<Animator>();
        m_DogSound = GetComponent<DogSound>();
        m_agent = GetComponent<NavMeshAgent>();
       Transform found = null;
        foreach (Transform D in transform)
        {
            if (D.name == "BitePoint")
            {
                m_BitePoint = D;
            }
        }

        if (m_BitePrefab == null)
        {
            m_BitePrefab = Resources.Load<GameObject>("BitePrefab");

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Die)
        {
            //まだ吠えて無ければ吠えるアニメーション
            if (!m_Howl)
            {
                m_Animator.SetTrigger("Howling");
                m_DogSound.m_Source.PlayOneShot(m_DogSound.m_Howling);
                m_Howl = true;
            }
            //吠えて終わったら走るアニメーション
            if (m_Howl)
            {
                m_Animator.SetBool("Search", true);
            }
            //吠え終わったらPlayに向かって走り出す
            if (m_HowlFinished&&!m_Bite)
            {
                // ターゲットが未設定なら、プレイヤーを探す
                if (m_Target == null)
                {
                    GameObject player = GameObject.FindWithTag("Player");
                    if (player != null)
                        m_Target = player.transform;
                }
                //ターゲットがあるならそこに向かう
                if (m_Target != null)
                {
                    m_agent.enabled = true;
                    m_agent.SetDestination(m_Target.position);
                }
            }
            // プレイヤーに接触したら噛みつき開始
            if (m_Area && !m_Bite)
            {
                Debug.Log("噛みついた");
                m_Bite = true;
                m_agent.enabled = false;
                m_Animator.SetBool("Bite", true);
            }
        }
       


    }

    public void Cool()
    {
        m_HowlFinished = true;
    }
    //アニメーションイベントの関数
    public void BiteNow()
    {
        if (m_BitePoint && m_BitePrefab)
        {
            //噛みつく位置にオブジェクトを生成
            GameObject biteObj = Instantiate(m_BitePrefab, m_BitePoint.position, m_BitePoint.rotation);

            // DamegeSystemにこの犬のParametaを渡す
            DamegeSystem dmg = biteObj.GetComponent<DamegeSystem>();
            Parameta myParam = GetComponent<Parameta>();
            //この二つが格納されていたら
            if (dmg && myParam)
                dmg.m_Parameta = myParam;
            //0.2秒後に消す
            Destroy(biteObj, 0.2f);
        }
        m_Bite = true;
        m_agent.enabled = false;
       
    }
    //アニメーションイベントの関数
    public void BiteEnd()
    {
        m_Bite = false;
        m_Area = false;
        m_agent.enabled = true;
        m_Animator.SetBool("Bite", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        //playerが入ったら
        if (other.CompareTag("Player"))
        {
            //エリアに入った
            m_Area = true;
            
        }
    }

}
