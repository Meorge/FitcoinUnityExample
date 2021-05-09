using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStoreItemsContainer : MonoBehaviour
{
    public List<StoreItem> storeItems = new List<StoreItem>();
    public UIStoreItem storeItemPrefab = null;

    void Start() {
        foreach (StoreItem item in storeItems) {
            UIStoreItem newStoreItem = Instantiate(storeItemPrefab);
            newStoreItem.transform.SetParent(transform);
            newStoreItem.SetData(item);
        }
    }
}
