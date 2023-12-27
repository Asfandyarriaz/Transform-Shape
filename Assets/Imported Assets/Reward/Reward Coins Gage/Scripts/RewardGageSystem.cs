using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace RewardCoinGage
{
    public class RewardGageSystem : MonoBehaviour
    {
        [Header(" Elements ")]
       
        [SerializeField] private Transform multiplierTextsParent;
        [SerializeField] private Needle needle;
        private Mesh mesh;

        

        [Header(" Settings ")]
        [NaughtyAttributes.OnValueChanged("GenerateMesh")]
        [Range(3, 10)]
        [SerializeField] private int resolution;

        [NaughtyAttributes.OnValueChanged("GenerateMesh")]
        [Range(.1f, 3f)]
        [SerializeField] private float radius;

        [NaughtyAttributes.OnValueChanged("GenerateMesh")]
        [Range(0f, 0.005f)]
        [SerializeField] private float multiplierTextScaleFactor;

        [NaughtyAttributes.OnValueChanged("GenerateMesh")]
        [Range(0f, 2f)]
        [SerializeField] private float multiplierTextDistanceRatio;

        [NaughtyAttributes.OnValueChanged("GenerateMesh")]
        [Range(0, 360)]
        [SerializeField] private float totalAngle;

        [Header(" Data ")]
        [NaughtyAttributes.OnValueChanged("GenerateMesh")]
        [SerializeField] private TriangleData[] rewardTrianglesData;
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();


        [Header(" Coins ")]
        [SerializeField] private int earnedCoins;


        [Header(" Events ")]
        public Action<int> onContinueButtonClicked;
        public Action<int> onWatchVideoButtonClicked;

        public CoinHandler coinHandler;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void OnDestroy()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            //earnedCoins =GameOverMenuManager.Instance.rewardGold;
            // Check if there already is an event system in the scene
            // If not, create one
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
                CreateEventSystem();
 
            GenerateMesh();
        }

        private void CreateEventSystem()
        {
            GameObject eventSystem = new GameObject("Event System");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < multiplierTextsParent.childCount; i++)
            {
                multiplierTextsParent.GetChild(i).GetComponent</*TextMeshPro*/Text>().color = GetCurrentTriangleIndex() == i ? Color.white : Color.gray;
                multiplierTextsParent.GetChild(i).GetComponent</*TextMeshPro*/Text>().fontSize = GetCurrentTriangleIndex() == i ? 40 : 36;
            }
        }

        public void GenerateMesh()
        {
//            // Clear all the previous multiplier texts
//            while (multiplierTextsParent.childCount > 0)
//            {
//                Transform t = multiplierTextsParent.GetChild(0);
//                t.SetParent(null);

//#if UNITY_EDITOR

//                if (!UnityEditor.EditorApplication.isPlaying)
//                    DestroyImmediate(t.gameObject);
//                else
//                    Destroy(t.gameObject);

//#endif
//            }

            vertices.Clear();
            triangles.Clear();
            colors.Clear();

            int triangleStartIndex = 0;

            float angleSum = 0;

            for (int i = 0; i < rewardTrianglesData.Length; i++)
            {
                TriangleData data = rewardTrianglesData[i];

                float targetAngle = totalAngle * data.anglePercent / 100;
                float angleLeft = Mathf.Clamp(totalAngle - angleSum, 0, totalAngle);
                float angle = Mathf.Min(targetAngle, angleLeft);
                float startAngle = Mathf.Min(totalAngle, angleSum);

                angleSum += angle;

                // If it's the last one, fill what's remaining
                if (i == rewardTrianglesData.Length - 1)
                    angle = angleLeft;

                Triangle triangle = new Triangle(resolution, radius, angle, startAngle);

                vertices.AddRange(triangle.GetVertices());


                // Color the triangles
                for (int k = 0; k < resolution; k++)
                    colors.Add(data.color);



                // We need to rearrange the triangles now
                // We add the amount of vertices of the previous triangle as a startIndex
                int[] currentTriangleTriangles = triangle.GetTriangles();

                for (int j = 0; j < currentTriangleTriangles.Length; j++)
                    currentTriangleTriangles[j] += triangleStartIndex;

                triangles.AddRange(currentTriangleTriangles);

                triangleStartIndex += triangle.GetVertices().Length;




                // Add the Multiplier Text
                /*Vector3 spawnPosition = triangle.GetCenterDirection() * radius * multiplierTextDistanceRatio + Vector3.back * .001f;
                spawnPosition = filter.transform.TransformPoint(spawnPosition);

                TextMeshPro multiplierTextInstance = Instantiate(multiplierTextPrefab, spawnPosition, Quaternion.identity, multiplierTextsParent);
                multiplierTextInstance.text = "x" + data.multiplier;
                multiplierTextInstance.color = data.textColor;

                // Also set the size of the text depending on the angle of the triangle and the radius
                // Should be something like this
                float textScale = radius * angle * multiplierTextScaleFactor;
                multiplierTextInstance.transform.localScale = textScale * Vector3.one;*/
            }



            //mesh = new Mesh();

            //mesh.vertices = vertices.ToArray();
            //mesh.triangles = triangles.ToArray();
            //mesh.colors = colors.ToArray();

            //mesh.RecalculateBounds();
            //filter.mesh = mesh;
        }



        public int GetRewardCoins()
        {
            int rewardIndex = GetCurrentTriangleIndex();

            isRewardedAd = false;
            //GameOverMenuManager.Instance.GoldMultiplyFactor = rewardTrianglesData[rewardIndex].multiplier * earnedCoins;
            return  rewardGold =rewardTrianglesData[rewardIndex].multiplier * earnedCoins;
            
        }

        public int GetEarnedCoins()
        {
            return earnedCoins;
        }

        public void SetEarnedCoins(int earnedCoins)
        {
            this.earnedCoins = earnedCoins;
        }

        private int GetCurrentTriangleIndex()
        {
            int rewardIndex = -1;
            float currentAngle = 0;

            for (int i = 0; i < rewardTrianglesData.Length; i++)
            {
                TriangleData data = rewardTrianglesData[i];
                float triangleAngle = data.anglePercent * totalAngle / 100;

                if (triangleAngle + currentAngle > needle.GetAngle())
                {
                    rewardIndex = i;
                    break;
                }

                currentAngle += triangleAngle;
            }

            // In case we don't find an index, it means it's the latest triangle
            if (rewardIndex == -1)
                rewardIndex = rewardTrianglesData.Length - 1;

            return rewardIndex;
        }

        public void WatchVideoButtonClicked()
        {
            // Stop the needle
            
            //if(AdsManager.instance&& AdsManager.instance.isAdReady())
            //{
            //    needle.Stop();
            //    if (GameOverMenuManager.Instance)
            //    {
            //        if (GameOverMenuManager.Instance.WatchVideoBg)
            //            GameOverMenuManager.Instance.WatchVideoBg.SetActive(true);
            //        if (GameOverMenuManager.Instance.RewardMeterBg)
            //            GameOverMenuManager.Instance.RewardMeterBg.SetActive(true);
            //    }
            //        OnRewardClick();
            //}
            //else
            //{
            //    GameOverMenuManager.Instance.NoVideoPopUp.SetActive(true);
            //}
            
        }

        public void OnRewardClick()
        {

            //AdsManager.instance.ShowRewardedAddCallBack(OnRewardSucces);
            

        }


        bool isRewardedAd = false;
        int rewardGold = 0;
        public void OnRewardSucces(bool isSuccess)
        {
            if (isSuccess)
            {
                isRewardedAd = true;

                onWatchVideoButtonClicked?.Invoke(GetRewardCoins());
                
                OnClaim();
            }
        }

        public void OnClaim()
        {
           
           // GameOverMenuManager.Instance.OnRewardSuccess(true);

        }
        public void voidContinueButtonCallback()
        {
            //onContinueButtonClicked?.Invoke(earnedCoins);
            needle.Stop();
            //if (GameOverMenuManager.Instance )
            //{
            //    if ( GameOverMenuManager.Instance.WatchVideoBg)
            //            GameOverMenuManager.Instance.WatchVideoBg.SetActive(true);
            //    if (GameOverMenuManager.Instance.RewardMeterBg)
            //            GameOverMenuManager.Instance.RewardMeterBg.SetActive(true);

            //    GameOverMenuManager.Instance.OnRewardContinue();
            //}

        }

        public void Reset()
        {
            needle.EnableRotation();
        }
    }

    [Serializable]
    public class TriangleData
    {
        [Range(0, 100)]
        public float anglePercent;
        public Color color;
        public Color textColor;
        public int multiplier;
    }
}