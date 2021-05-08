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
    private UIQRCode qrCode = null;

    [SerializeField]
    private UIPageManager pageManager = null;

    [SerializeField]
    private UIModalManager modalManager = null;

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
            pageManager?.SegueToPage(1,
            onStart: () => { DisableAllButtons(); qrCode.StartLoading(); },
            onComplete: () => {
                EnableAllButtons();
                CreateLinkRequest();
            });
            }
        );

        cancelConnectButton.onClick.AddListener(() => {
                pageManager?.SegueBackward(0, onStart: () => {
                    DisableAllButtons();
                    service.StopMonitoringLinkRequestStatus();
                    service.DeleteLinkRequest();
                },
                onComplete: () => { EnableAllButtons(); qrCode.Reset(); });
            }
        );

        cancelFromErrorButton.onClick.AddListener(() => {
            pageManager?.SegueToPage(0, true);
            qrCode.Reset();
        }
        );

        cancelFromDeniedButton.onClick.AddListener(() => {
            pageManager?.SegueToPage(0, true);
            qrCode.Reset();
        }
        );

        doneButton.onClick.AddListener(() => { modalManager.DismissModal(); });


        service.AccessToken = accessToken;
    }

    void CreateLinkRequest() {
        service.CreateLinkRequest(
            onError: DisplayError,
            onResponse: (message) => {
                DisplayQRCodeAndListenForConnection();
            }
        );
    }

    void DisplayQRCodeAndListenForConnection() {
        service.GetQRCodeForLinkRequest(
            onError: DisplayError,
            onResponse: (texture) => {
                qrCode.DoneLoading(texture);
            }
        );


        service.MonitorLinkRequestStatus(
            queryInterval: 3,
            onError: DisplayError,
            onResponse: (status) => {
                Debug.Log($"Link request query - {status.status}");
                var statusString = status.status;

                if (statusString != "pending") {
                    service.StopMonitoringLinkRequestStatus();
                    service.DeleteLinkRequest();

                    if (statusString == "approved") {
                        Debug.Log($"It was approved!! User ID is {status.user_id}");
                        service.UserID = status.user_id;
                        DisplayApproved();
                    }
                    else if (statusString == "denied") {
                        Debug.Log($"Link request was denied :(");
                        DisplayDenied();
                    }
                }
            }
        );
    }

    void DisplayError(string message) {
        service.StopMonitoringLinkRequestStatus();
        service.DeleteLinkRequest();

        errorLabel.text = message;
        pageManager?.SegueToPage(4,
            onStart: DisableAllButtons,
            onComplete: EnableAllButtons);
    }

    void DisplayApproved() {
        pageManager?.SegueToPage(2,
            onStart: DisableAllButtons,
            onComplete: EnableAllButtons
        );


        service.GetUserInfo(
            onError: DisplayError,
            onResponse: (a) => { Debug.Log($"{a.username} has balance of {a.balance}"); }
        );
    }

    void DisplayDenied() {
        pageManager?.SegueToPage(3,
            onStart: DisableAllButtons,
            onComplete: EnableAllButtons
        );
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
