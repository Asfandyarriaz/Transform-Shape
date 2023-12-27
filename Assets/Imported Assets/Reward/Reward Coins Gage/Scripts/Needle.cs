using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RewardCoinGage
{
    public class Needle : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private Transform parent;

        [Header(" Settings ")]
        [SerializeField] private float maxAngle;
        [SerializeField] private float rotationSpeed;
        private bool canRotate;

        [Header(" Events ")]
        public static Action<float> onNeedleStopped;
    
        // Start is called before the first frame update
        void Start()
        {
            canRotate = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (canRotate)
                Rotate();
        }

        private void Rotate()
        {
            float time = Time.time * rotationSpeed;

            float linearUp = time % maxAngle;
            float linearDown = maxAngle - (time % maxAngle);

            float angleOverTime = (Mathf.Max(linearUp, linearDown) - .75f * maxAngle) * 4;

            parent.localRotation = Quaternion.Euler(0, 0, angleOverTime);
        }

        public void Stop()
        {
            canRotate = false;

            float angle = GetAngle();

            onNeedleStopped?.Invoke(angle);
        }

        public float GetAngle()
        {
            float angle = (maxAngle * 2) - Vector3.SignedAngle(Vector3.right, (parent.position - transform.position).normalized, Vector3.back);

            if (angle == 360)
                angle = 0;

            return angle;
        }

        [NaughtyAttributes.Button]
        public void EnableRotation()
        {
            canRotate = true;
        }
    }
}