using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RewardCoinGage
{
    public class ContinueButton : MonoBehaviour
    {
        [Header(" Elements ")]
        [SerializeField] private RewardGageSystem rewardGage;
        [SerializeField] private TextMeshProUGUI coinAmountText;

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
            coinAmountText.text = rewardGage.GetEarnedCoins().ToString();
        }
    }
}
