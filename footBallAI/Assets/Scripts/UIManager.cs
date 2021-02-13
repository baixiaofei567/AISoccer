using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace FootBallAI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instrance;
        public Text redText, blueText;

        [SerializeField] GameObject winPanel;

        bool flag = true;
        // Start is called before the first frame update
        private void Awake()
        {
            _instrance = this;
        }

        public static UIManager getUI
        {
            get
            {
                return _instrance;
            }
        }

        // Update is called once per frame
        void Update()
        {
            redText.text = GameManage.GetGM.getLeftScore.ToString();
            blueText.text = GameManage.GetGM.getRightScore.ToString();
        }

        public void setWIN(int flag)//1为红赢，2为蓝，3为平局
        {
            winPanel.SetActive(true);
            winPanel.transform.GetChild(flag).gameObject.SetActive(true);
        }

        public void ReStart()
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
        }

        public void Quit()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }

        public void Set()
        {
            if (flag)
            {
                winPanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                winPanel.SetActive(false);
                Time.timeScale = 1;
            }
            flag = !flag;
        }
    }
}
