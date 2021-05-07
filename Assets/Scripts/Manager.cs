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
    }

    float timer = 0f;

    void Update() {
        if (timer < 5f) {
            timer += Time.deltaTime;
        }

        else {
            Debug.Log("Checking link request status");
            
            service.GetLinkRequestStatus(
                onInternalError: (error) => {
                    Debug.LogError($"Internal error! {error}");
                },

                onResponse: (code, status) => {
                    Debug.Log($"Link request query ({code}) - {status.status}");
                    timer = 0f;
                }
            );
        }
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
