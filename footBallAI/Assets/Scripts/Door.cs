using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootBallAI
{
    public class Door : MonoBehaviour
    {
        public bool isL;

        private void OnCollisionEnter(Collision collision)
        {
            if (isL && collision.gameObject.tag == "Ball")
            {
                GameManage.GetGM.addRightScore();
                collision.gameObject.GetComponent<AudioSource>().Play();
            }
            else if (!isL && collision.gameObject.tag == "Ball")
            {
                GameManage.GetGM.addLeftScore();
                collision.gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }
}
