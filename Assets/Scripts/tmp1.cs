using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Unityエンジンのシーン管理プログラムを利用する


public class tmp1 : MonoBehaviour
{
    public GameObject t1; // 移動させる対象のオブジェクト
    [SerializeField]
    private float speed = 5f; // 移動速度
    [SerializeField]
    private float jumpForce = 1000f; // ジャンプ力
    private Rigidbody2D rb; // Rigidbody用の変数
    private bool isGrounded = true; // 地面についているかどうか
    private float wallPos=5.9f;


    void Start()
    {
        if (t1 != null)
        {
            rb = t1.GetComponent<Rigidbody2D>(); // t1のRigidbodyを取得
            if (rb == null)
            {
                Debug.LogError("Rigidbodyがアタッチされていません！");
            }
                        // Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Manekin1")); // プレイヤー同士の当たり判定を無効化

        }
        // df(speed);

    }

    void Update()
    {
        if (t1 != null && rb != null)
        {
            Vector3 move = Vector3.zero;

            // 左右移動
            if (Input.GetKey(KeyCode.A))
            {
                move.x += -speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                move.x += speed * Time.deltaTime;
            }

            t1.transform.position += move; // t1を移動
            if(t1.transform.position.x<-wallPos||t1.transform.position.x>wallPos)t1.transform.position-=move;


            // ジャンプをするためのコード（もしスペースキーが押されて、上方向に速度がない時に）
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                // リジッドボディに力を加える（上方向にジャンプ力をかける）
                rb.AddForce(transform.up * jumpForce);
                isGrounded = false;

            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor") // Floorタグのオブジェクトと接触したら
        {
            isGrounded = true;
            // df(0);
        }
    }

    public static void df<T>(T value)
    {
        Debug.Log(value);
    }


}
