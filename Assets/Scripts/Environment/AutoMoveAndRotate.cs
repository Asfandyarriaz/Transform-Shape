using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class AutoMoveAndRotate : MonoBehaviour
    {
        public Vector3andSpace moveUnitsPerSecond;
        public Vector3andSpace rotateDegreesPerSecond;
        public bool ignoreTimescale;
        private float m_LastRealTime;

		public bool isReverse;
		public float reverseTime= 10;

        //public override void OnBecameInvisible()
        //{
        //    this.enabled = false;
        //}

        //public override void OnBecameVisible()
        //{
        //    this.enabled = true;

        //}
        private void Start()
        {
            m_LastRealTime = Time.realtimeSinceStartup;
			if(isReverse){
				InvokeRepeating ("ReverseRotation",reverseTime,reverseTime);
			}
        }

		void ReverseRotation(){
			rotateDegreesPerSecond.value = rotateDegreesPerSecond.value * -1;
			moveUnitsPerSecond.value = moveUnitsPerSecond.value * -1;

			//transform.localScale = new Vector3 (transform.localScale.x,transform.localScale.y,-transform.localScale.z);

		}
        // Update is called once per frame
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (ignoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - m_LastRealTime);
                m_LastRealTime = Time.realtimeSinceStartup;
            }
            transform.Translate(moveUnitsPerSecond.value*deltaTime, moveUnitsPerSecond.space);
            transform.Rotate(rotateDegreesPerSecond.value*deltaTime, moveUnitsPerSecond.space);
        }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }
}
