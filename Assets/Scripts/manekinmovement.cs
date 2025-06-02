using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manekinmovement : MonoBehaviour
{
    public GameObject t1; // 移動させる対象のオブジェクト
    [SerializeField]
    private float speed = 5f; // 移動速度
    [SerializeField]
    private float jumpForce = 1000f; // ジャンプ力
    private Rigidbody2D rb; // Rigidbody用の変数
    private bool isGrounded = true; // 地面についているかどうか
    private float wallPos = 5.9f;

    private float moveDirection = 0f;
    private float moveChangeInterval = 0.5f; // 方向変更の間隔
    private float nextMoveTime = 0f;
    public float jumpP=0.01f;

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
    }

    void Update()
    {
        if (t1 != null && rb != null)
        {
            if (Time.time > nextMoveTime)
            {
                moveDirection = Random.Range(-1f, 1f); // ランダムに左右の移動を決定
                if(moveDirection>=0)moveDirection=1f;
                else moveDirection=-1f;
                nextMoveTime = Time.time + moveChangeInterval; // 次の移動変更タイミングを更新
            }

            Vector3 move = new Vector3(moveDirection * speed * Time.deltaTime, 0, 0);
            t1.transform.position += move;

            // 壁を超えないように制限
            if (t1.transform.position.x < -wallPos || t1.transform.position.x > wallPos)
            {
                t1.transform.position -= move;
            }

            // ランダムにジャンプ
            if (isGrounded && Random.Range(0f, 1f) < jumpP) // 2%の確率でジャンプ
            {
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
        }
    }
}
