using System;
using System.Collections;
using UnityEngine;

namespace Moba
{
    public class CameraAnimation : MonoBehaviour
    {
        public GameObject chest;

        public float speed = 2f;
        private Vector3 lastPos;
        private Quaternion lastRot;
        public float animSmooth = 5f;

        void OnGUI() {
            GUI.Label(new Rect(10, Screen.height - 30, 200, 40),"Camera Speed");
            speed = GUI.HorizontalSlider(new Rect(120, Screen.height - 25, 200, 10), speed, 0, 10);
            GetComponent<Animation>().GetComponent<Animation>()["camera"].speed = Mathf.Clamp(speed, 0f, 10f);
        }

        void ChestOpen()
        {
            if (chest.GetComponent<Animation>().GetComponent<Animation>()["chest"].time == 0)
            {
                chest.GetComponent<Animation>().Play();
            }
        }

        void LateUpdate() {
            //force smooth animation
            if (GetComponent<Camera>().GetComponent<Animation>().IsPlaying("camera"))
            {
                transform.position = Vector3.Lerp(lastPos, transform.position, Time.deltaTime * animSmooth);
                transform.rotation = Quaternion.Lerp(lastRot, transform.rotation, Time.deltaTime * animSmooth);
                lastPos = transform.position;
                lastRot = transform.rotation;
            }
            else {
                GetComponent<Camera>().GetComponent<Animation>().Play();
                lastPos = transform.position;
                lastRot = transform.rotation;
            }

        }
    }
}
