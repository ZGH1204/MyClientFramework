using UnityEngine;
using System.Collections;

namespace Moba
{
    public class Crystals : MonoBehaviour
    {
        public Transform[] crystals;
        private Vector3[] originalPosition;

        public float rotationSpeed = 1f;
        public float floatingSpeed = 1f;
        public float floatingAmplitude = 1f;

        // Use this for initialization
        void Start()
        {
            originalPosition = new Vector3[crystals.Length];
            for (int i = 0; i < crystals.Length; i++)
            {
                originalPosition[i] = crystals[i].transform.localPosition;
            }

        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < crystals.Length; i++)
            {
                crystals[i].transform.localPosition = originalPosition[i] + new Vector3(0, 0, floatingAmplitude * Mathf.Sin(floatingSpeed * Time.time + i * (100 / crystals.Length))) / 2f;
                crystals[i].transform.Rotate(rotationSpeed * 10f * Time.deltaTime, 0, 0);
            }
        }
    }
}
