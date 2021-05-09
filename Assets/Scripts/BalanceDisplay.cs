using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Fitcoin;

public class BalanceDisplay : MonoBehaviour
{
    [SerializeField]
    private FitcoinService service = null;

    [SerializeField]
    private TextMeshProUGUI balanceLabel = null, usernameLabel = null;

    // Start is called before the first frame update
    void Start()
    {
        service.onUserInfoUpdated += UpdateBalance;
        UpdateBalance(null);
    }

    void UpdateBalance(FitcoinUserInfo info = null) {
        balanceLabel.text = info?.balance.ToString() ?? "---";
        usernameLabel.text = info?.username ?? "Not logged in";
    }
}
