using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene1Ctr : MonoBehaviour
{
    private Button btn_Start;
    private Button btn_Exit;
    private Button btn_Set;
    private GameObject setUI;

    void Awake()
    {
        btn_Start = transform.GetChild(2).GetComponent<Button>();
        btn_Set = transform.GetChild(3).GetComponent<Button>();
        btn_Exit = transform.GetChild(4).GetComponent<Button>();
        setUI = transform.GetChild(5).gameObject;

        //法1：倾听oncLick事件，调用函数OnStartButtonClick
        btn_Start.onClick.AddListener(OnStartButtonClick);

        //btn_Set.onClick.AddListener(() =>
        //{
        //    setUI.SetActive(true);
        //});

        //法2：lambda
        btn_Exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(1);
    }
}
