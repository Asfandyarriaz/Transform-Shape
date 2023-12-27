using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace RewardCoinGage
{
    public class WatchVideoButton : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private RewardGageSystem rewardGage;
        [SerializeField] private Text coinAmountText;

        [Header(" Events ")]
        public Action<int> onWatchVideoButtonClicked;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateCoinsText();
        }

        private void UpdateCoinsText()
        {
            coinAmountText.text = rewardGage.GetRewardCoins().ToString();
        }

        public void ButtonClickedCallback()
        {
            rewardGage.WatchVideoButtonClicked();
            //onWatchVideoButtonClicked?.Invoke(rewardGage.GetRewardCoins());
        }
    }
}