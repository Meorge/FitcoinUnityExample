using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Fitcoin;

public class Manager : MonoBehaviour
{
    private readonly string accessToken = "3AijTyw7pGwEKRtXYiPhIK9yX3JqsQ3mTiXTnSBsRGc";
    
    [SerializeField]
    private FitcoinService service = null;

    [SerializeField]
    private RawImage qrCodeImage = null;

    // Start is called before the first frame update
    void Start()
    {
        service.AccessToken = accessToken;
        service.LinkRequestID = "6095793157c689ad83c0106d";
        // service?.CreateLinkRequest(
        //     onInternalError: InternalErrorOccurred,
        //     onResponse: LinkRequestDataReceived);
        
        service.GetQRCodeForLinkRequest(
            onInternalError: (error) => {
                Debug.LogError($"Internal error! {error}");
            },

            onResponse: (code, texture) => {
                Debug.Log($"QR code data received ({code}) - {texture}");
                qrCodeImage.texture = texture;
            }
        );


        service.MonitorLinkRequestStatus(
            queryInterval: 3,
            onInternalError: (error) => {
                Debug.LogError($"Internal error! {error}");
            },

            onResponse: (code, status) => {
                Debug.Log($"Link request query ({code}) - {status.status}");
                var statusString = status.status;

                if (statusString != "pending") {
                    service.StopMonitoringLinkRequestStatus();

                    if (statusString == "approved") {
                        Debug.Log($"It was approved!! User ID is {status.user_id}");
                    }
                    else if (statusString == "denied") {
                        Debug.Log($"Link request was denied :(");
                    }
                }
            }
        );
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
