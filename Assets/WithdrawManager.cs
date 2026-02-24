using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WithdrawManager : MonoBehaviour
{

    public TextMeshProUGUI CoinBalanceText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CoinBalanceText.text = "Coins :" + CoinBalanceHolder.Instance.virtualCurrencyBalance.ToString();
    }
}
