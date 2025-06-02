using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI操作に必要
using TMPro;
using UnityEngine.SceneManagement;//Unityエンジンのシーン管理プログラムを利用する
using System;


public class PosCulc : MonoBehaviour
{
    public GameObject Player;
    public GameObject Manekin;
    private float maxHP = 100f;
    private float currentHP = 100f;
    private float alpha = 0.05f;
    private bool isGameOver = false;
    private float elapsedTime = 0f; // 経過時間を記録
    // スライダーの参照
    public Slider hpSlider;
    public TextMeshProUGUI TMText;
    public TextMeshProUGUI TMTextEND;
    public TextMeshProUGUI TMTextCountDown;
    private bool isGameStart = false;
    private float countDown = 3f;
    public TextMeshProUGUI TMTextRanking;





    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        hpSlider.maxValue = maxHP; // スライダーの最大値を設定
        hpSlider.value = currentHP; // 現在のHPを反映
        TMTextEND.gameObject.SetActive(false);
        TMTextRanking.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStart)
        {
            GameStartF();
            return;
        }
        ;

        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("s1");//secondを呼び出します

            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene("title");//titleを呼び出します

            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                // SampleKey という名前のキーとそのデータを削除
                // PlayerPrefs.DeleteKey("testRanking");
            }
            return;
        }// ゲームオーバーなら処理しない
        elapsedTime += Time.deltaTime; // 経過時間を加算
        // TMText.text = $"Score:{elapsedTime}";
        TMText.text = $"Score:{elapsedTime.ToString("F2")}";

        Vector3 ManekinPos = Manekin.transform.position;
        ManekinPos.y -= 50f;


        float distance = Vector3.Distance(Player.transform.position, ManekinPos);
        TakeDamage(distance);
        if (isGameOver) GameOverF();


        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("////////////");
            Debug.Log("Player: " + Player.transform.position);
            Debug.Log("Manekin: " + Manekin.transform.position);
            Debug.Log("ManekinPos: " + ManekinPos);
            Debug.Log("Distance: " + distance);
            Debug.Log("HP: " + currentHP);
            Debug.Log("Elapsed Time: " + elapsedTime);
        }


    }
    public void GameStartF()
    {
        countDown -= Time.deltaTime;
        if (countDown < 0)
        {
            isGameStart = true;
            TMTextCountDown.gameObject.SetActive(false);
            return;
        }
        TMTextCountDown.text = $"{Math.Ceiling(countDown)}";
    }
    public void GameOverF()
    {
        TMTextEND.gameObject.SetActive(true);
        TMTextRanking.text = "";
        TMTextRanking.gameObject.SetActive(true);
        TMTextEND.text = $"Score:{elapsedTime}\nRestat:R\nTitle:T";
        // ランキング更新
        RankingUpdate("testRanking", elapsedTime);

        // 更新後のランキングの出力
        RankingLoad("testRanking");


    }
    public void TakeDamage(float damage)
    {
        // HPを減らす処理
        currentHP -= alpha * damage;
        if (currentHP < 0) currentHP = 0;

        // スライダーに現在のHPを反映
        hpSlider.value = currentHP;

        // HPが0になったときの処理
        if (currentHP == 0 && !isGameOver)
        {
            isGameOver = true;
            Debug.Log("ゲームオーバー！");
            Debug.Log("スコア（生存時間）: " + elapsedTime + "秒");
            // ここにゲームオーバーの処理を追加（例：リトライボタンを表示するなど）
        }
    }
    // ランキング更新メソッド
    void RankingUpdate(string rankingKey, float newScore)
    {
        // rankingText にランキングデータを代入する
        // 初期値として "500,400,300,200,100" を設定
        string rankingText = PlayerPrefs.GetString(rankingKey, "0,0,0,0,0");

        // splitメソッドを使い、"," で区切られている値を rankingArr 配列に代入する
        string[] rankingTextArr = rankingText.Split(',');

        // ランキングの人数を topNScores に代入する
        int topNScores = rankingTextArr.Length;

        // ランキングより 1 大きいランキング配列を作成する
        float[] rankingFloatArr = new float[topNScores + 1];

        // rankingFloatArr 配列に float 変換したスコアを代入する
        for (int i = 0; i < topNScores; i++)
        {
            rankingFloatArr[i] = float.Parse(rankingTextArr[i]);
        }

        // rankingFloatArrの一番後ろに新しいスコアを代入する
        rankingFloatArr[topNScores] = newScore;

        // rankingFloatArrを昇順ソート
        Array.Sort(rankingFloatArr);
        // 降順にするために配列の順序を反転
        Array.Reverse(rankingFloatArr);

        // 保存用のランキングテキストの初期化
        string uploadRankingText = "";

        for (int i = 0; i < topNScores; i++)
        {
            // rankingFloatArr配列を保存用のテキストに代入していく（最後だけ , を外す）
            if (i < topNScores - 1)
            {
                uploadRankingText += rankingFloatArr[i].ToString("F2") + ","; // 小数点2桁まで
            }
            else
            {
                uploadRankingText += rankingFloatArr[i].ToString("F2");
            }
        }

        // ランキングの保存
        PlayerPrefs.SetString(rankingKey, uploadRankingText);
    }


    // ランキング取得メソッド
    void RankingLoad(string rankingKey)
    {
        // rankingText にランキングデータを代入する
        // 今回は初期値として "500,400,300,200,100" を代入する
        string rankingText = PlayerPrefs.GetString(rankingKey, "500,400,300,200,100");

        // splitメソッドを使い、"," で区切られている値を rankingArr 配列に代入する
        // rankingArrの中身：["500","400","300","200","100"]
        string[] rankingTextArr = rankingText.Split(',');

        // ランキングの人数を topNScores に代入する
        int topNScores = rankingTextArr.Length;

        // ランキングを出力する（今回は Debug.Log(); で仮出力）
        TMTextRanking.text = "";
        for (int i = 0; i < topNScores; i++)
        {
            Debug.Log($"{i + 1}位：{rankingTextArr[i]}");
            TMTextRanking.text += $"{i + 1}位：{rankingTextArr[i]}\n";

        }

        return;
    }
}
