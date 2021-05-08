using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Fitcoin;

public class Manager : MonoBehaviour
{
    private readonly string accessToken = "3AijTyw7pGwEKRtXYiPhIK9yX3JqsQ3mTiXTnSBsRGc";
    
    [SerializeField]
    private FitcoinService service = null;

    [SerializeField]
    private RawImage qrCodeImage = null;

    [SerializeField]
    private UIPageManager pageManager = null;

    [SerializeField]
    private Button connectAccountButton = null;

    [SerializeField]
    private Button cancelConnectButton = null;

    [SerializeField]
    private Button cancelFromDeniedButton = null;

    [SerializeField]
    private Button cancelFromErrorButton = null;

    [SerializeField]
    private Button doneButton = null;

    [SerializeField]
    private TextMeshProUGUI errorLabel = null;

    // Start is called before the first frame update
    void Start()
    {
        // Set up buttons
        connectAccountButton.onClick.AddListener(() => {
            pageManager?.SegueToPage(1, onStart: DisableAllButtons, onComplete: () => {
                EnableAllButtons();
                DisplayQRCodeAndListenForConnection();
            });
            }
        );

        cancelConnectButton.onClick.AddListener(() => {
                pageManager?.SegueBackward(0, onStart: () => {
                    DisableAllButtons();
                    service.StopMonitoringLinkRequestStatus();
                    qrCodeImage.texture = null;
                },
                onComplete: EnableAllButtons);
            }
        );

        cancelFromErrorButton.onClick.AddListener(() => {
            pageManager?.SegueToPage(0, true);
        }
        );


        service.AccessToken = accessToken;
        service.LinkRequestID = "6095793157c689ad83c0106d";
    }

    void DisplayQRCodeAndListenForConnection() {
        service.GetQRCodeForLinkRequest(
            onInternalError: DisplayError,

            onResponse: (code, texture) => {
                Debug.Log($"QR code data received ({code}) - {texture}");
                qrCodeImage.texture = texture;
            }
        );


        service.MonitorLinkRequestStatus(
            queryInterval: 3,
            onInternalError: DisplayError,

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

    void DisplayError(string message) {
        service.StopMonitoringLinkRequestStatus();
        errorLabel.text = message;
        pageManager?.SegueToPage(4,
            onStart: DisableAllButtons,
            onComplete: EnableAllButtons);
    }

    void DisplayDenied() {
        service.
    }


    void DisableAllButtons() => SetAllButtonStatus(false);
    void EnableAllButtons() => SetAllButtonStatus(true);

    void SetAllButtonStatus(bool status) {
        connectAccountButton.enabled = status;
        cancelConnectButton.enabled = status;
        cancelFromDeniedButton.enabled = status;
        cancelFromErrorButton.enabled = status;
        doneButton.enabled = status;
    }
}
