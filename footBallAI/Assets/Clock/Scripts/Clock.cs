using UnityEngine;
using System.Collections;

namespace FootBallAI
{
    public class Clock : MonoBehaviour
    {
        //private static Clock _instance;

        //-- set start time 00:00
        public int minutes = 0;
        public int hour = 0;
        public int seconds = 0;
        public int gameTime = 0;
        public bool realTime = true;

        public GameObject pointerSeconds;
        public GameObject pointerMinutes;
        public GameObject pointerHours;

        //-- time speed factor
        public float clockSpeed = 1.0f;     // 1.0f = realtime, < 1.0f = slower, > 1.0f = faster

        //-- internal vars
        float msecs = 0;


        void Start()
        {
            //-- set real time
            if (realTime)
            {
                //hour=System.DateTime.Now.Hour;
                //minutes=System.DateTime.Now.Minute;
                //seconds=System.DateTime.Now.Second;
            }
        }

        void Update()
        {
            //-- calculate time
            msecs += Time.deltaTime * clockSpeed;
            if (msecs >= 1.0f)
            {
                msecs -= 1.0f;
                seconds++;
                gameTime++;
                if (seconds >= 60)
                {
                    seconds = 0;
                    minutes++;
                    if (minutes > 60)
                    {
                        minutes = 0;
                        hour++;
                        if (hour >= 24)
                            hour = 0;
                    }
                }
            }

            if(gameTime >= 120)
            {
                Time.timeScale = 0;
                if(GameManage.GetGM.getLeftScore == GameManage.GetGM.getRightScore)
                {
                    UIManager.getUI.setWIN(2);
                }
                else if (GameManage.GetGM.getLeftScore > GameManage.GetGM.getRightScore)
                {
                    UIManager.getUI.setWIN(0);
                }
                else
                {
                    UIManager.getUI.setWIN(1);
                }
            }

            //-- calculate pointer angles
            float rotationSeconds = (360.0f / 60.0f) * seconds;
            float rotationMinutes = (360.0f / 60.0f) * minutes;
            float rotationHours = ((360.0f / 12.0f) * hour) + ((360.0f / (60.0f * 12.0f)) * minutes);

            //-- draw pointers
            pointerSeconds.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationSeconds);
            pointerMinutes.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationMinutes);
            pointerHours.transform.localEulerAngles = new Vector3(0.0f, 0.0f, rotationHours);

        }
    }
}