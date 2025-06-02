using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // UI操作に必要
using UnityEngine.SceneManagement;//Unityエンジンのシーン管理プログラムを利用する


public class StartScript : MonoBehaviour
{

    // public TextMeshProUGUI TMText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("s1");//secondを呼び出します
            // Debug.Log("aa");

        }
        // Debug.Log("aab");
    }
}
