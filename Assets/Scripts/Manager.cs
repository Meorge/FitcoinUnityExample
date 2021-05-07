using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using Fitcoin;

public class Manager : MonoBehaviour
{
    private readonly string accessToken = "3AijTyw7pGwEKRtXYiPhIK9yX3JqsQ3mTiXTnSBsRGc";
    
    [SerializeField]
    private FitcoinService service = null;

    // Start is called before the first frame update
    void Start()
    {
        service.AccessToken = accessToken;
        service?.CreateLinkRequest(
            onInternalError: InternalErrorOccurred,
            onResponse: LinkRequestDataReceived);
    }

    void InternalErrorOccurred(string message) {
        Debug.LogError($"Internal error! {message}");
    }

    void LinkRequestDataReceived(long code, string linkID) {
        Debug.Log($"Link request data received ({code})");
        Debug.Log($"Content: {linkID ?? "null"}");

        if (code != 200) {
            Debug.LogError($"Error of some kind");
            return;
        }
        
        Debug.Log($"Got the link ID! It's {linkID}");
    }
}
