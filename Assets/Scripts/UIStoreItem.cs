using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Fitcoin;

public class UIStoreItem : MonoBehaviour
{
    [SerializeField]
    private Image icon = null;

    [SerializeField]
    private TextMeshProUGUI
        nameLabel = null,
        descriptionLabel = null,
        costLabel = null,
        ownedLabel = null;

    [SerializeField]
    private Button buyButton = null;

    public StoreItem item { get; private set; }

    private bool canBuy = false;


    public void Start() {
        buyButton.colors = Manager.instance.canPurchaseColors;
        Manager.instance.service.onUserInfoUpdated += UpdateData;

        buyButton.onClick.AddListener(BuyItem);
    }

    void BuyItem() {
        Manager.instance.PurchaseItem(item, onSuccess: (_) => UpdateVisual());
    }

    public void UpdateData(FitcoinUserInfo info = null) {
        bool userInfoExists = info != null;
        bool userHasEnough = (info?.balance ?? 0) >= item.cost;
        canBuy = userInfoExists && userHasEnough;

        buyButton.interactable = canBuy;

        Debug.Log("Data updated");
        UpdateVisual();
    }

    public void SetData(StoreItem newItem) {
        item = newItem;
        UpdateVisual();
    }

    public void UpdateVisual() {
        icon.sprite = item.icon;
        nameLabel.text = item.itemName;
        descriptionLabel.text = item.description;
        costLabel.text = $"{item.cost} <sprite name=fitcoin_flat>";

        int currentlyOwned = PlayerPrefs.GetInt(item.id, 0);
        ownedLabel.text = $"Owned: {currentlyOwned}";
    }
}
