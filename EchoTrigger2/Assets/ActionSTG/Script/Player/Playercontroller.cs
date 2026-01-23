using System.Threading;
using UnityEngine;


public class Playercontroller : MonoBehaviour
{
   
    [Header("カメラスピード")]
    public float CameraSpeed ;
    [Header("ジャンプ力")]
    public float jumpSpeed ;
    [Header("スピード")]
    public float moveSpeed ;
    public bool isArea;
    public bool m_IsBitten = false;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;
    private Animator m_Animator;
    private Parameta m_Parameta;
    //地面についてるかどうか
    private bool Grounded = true;
    private void Start()
    {
        
        //格納
        m_Parameta = GetComponent<Parameta>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();
        m_Animator = GetComponent<Animator>();
    　　//カーソルを中央に固定
        Cursor.lockState=CursorLockMode.Locked;
        //カーソル非表示
        Cursor.visible = false;
    }

    private void Update()
    {
        //オプション中は操作禁止
        if (Options.m_IsOptionsOpen)
            return;
        //Shiftキーを長押しすると
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //スピードが上がる
            moveSpeed = 4f;
            m_Animator.SetBool("Run",true);
           
        }
        //Shiftキーを離すと
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //スピードがもとに戻る
            moveSpeed = 2f;
            m_Animator.SetBool("Run", false);
        }

    }
    void FixedUpdate()
    {
        //オプション中は操作禁止
        if (Options.m_IsOptionsOpen)
            return;
        //しゃがみ中は移動禁止
        if (Crouching.m_Crouching)
            return;
        //噛まれているときは移動禁止
        if (Bite.m_BiteOut)
            return;
        if (m_Parameta.m_IsDie)
            return;
        // キャラクターとカメラの左右回転（Y軸）
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * CameraSpeed, 0));
        //移動
        float moveX = (Input.GetAxis("Horizontal") * moveSpeed);
        float moveZ = (Input.GetAxis("Vertical") * moveSpeed);

        //水平方向の速度を設定（Y速度は維持）
        Vector3 velocity = transform.right * moveX + transform.forward * moveZ;
        velocity.y = m_Rigidbody.linearVelocity.y;
        m_Rigidbody.linearVelocity = velocity;
        //キャラアニメーションで動く
        m_Animator.SetFloat("X", Input.GetAxis("Horizontal"));
        m_Animator.SetFloat("Y", Input.GetAxis("Vertical"));
    }






}
