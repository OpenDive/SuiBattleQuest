using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;
using TcgEngine;
using UnityEngine.Networking;

namespace TcgEngine.Web3
{
    public class Web3Controller : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void ConnectToWallet(string objectName, string callback);

        [DllImport("__Internal")]
        private static extern void ApproveEntryFee(string objectName, string callback, string amount);

        [DllImport("__Internal")]
        private static extern void StakeEntryFee(string objectName, string callback, string amount);

        public TextMeshProUGUI debugLog;

        public double amountToStake;
        public string amountToStakeStr;

        public GameObject beginUI;
        public GameObject stakeUI;
        public InputField input;

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        //void Start()
        //{
        //    connectToWalletBtn.onClick.AddListener(OnClickConnectWallet);
        //    registerForRacetBtn.onClick.AddListener(OnClickStakeEntryFee);
        //}

        void Update()
        {
        }

        /**
         * Parses user input and stores in the amount to stake variable
         */
        public void GetInput()
        {
            amountToStakeStr = input.text;
            amountToStake = int.Parse(input.text);
            Debug.Log(amountToStake);
        }

        /**
         * Called when ZK Login returns
         */
        public void OnZkLogin()
        {
            debugLog.text = "On ZK Login!";
            // Switch scenes
        }

        // TODO: Figure out why this isn't being triggered correctly
        public void OnSuiAddress(string suiaddress)
        {
            if (!PlayerPrefs.HasKey("SUI_ADDRESS"))
                PlayerPrefs.SetString("SUI_ADDRESS", suiaddress);

            Debug.Log($"Sui Address: {suiaddress}");
            string tmpAddr = "0x114a63e533262fee088dcfe8c015d6786ec681611abe3c72c77abdf278a8f0f5";

            //StartCoroutine(FetchKioskData(suiaddress));
            StartCoroutine(FetchKioskData(tmpAddr));
        }

        // TODO: Implement Kiosk fetching for the REST API
        public IEnumerator FetchKioskData(string address)
        {
            string url = $"http://localhost:5173/api/getSuiFrens?address={address}";
            UnityWebRequest webRequest = WebRequest.Create(url);
            yield return WebTool.SendRequest(webRequest);

            // Check for errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                // Process the response
                Debug.Log($"Response: {webRequest.downloadHandler.text}");
            }

            SceneNav.GoTo("Menu");
        }

        /**
         * OnClick function called when user clicks on "Connect" button
         * Passes callback function "ConnectToWalleCallback"
         */
        public void OnClickConnectWallet()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                ConnectToWallet(gameObject.name, "ConnectToWalletCallback");
            }
            else
            {
                ConnectToWalletCallback("Connect To MetaMask Callback from Unity");
            }
            Debug.Log("Clicked on Connect to Wallet Button!");
            // TODO: Do something with the UI, like a loading bar or a spinner
            //StartCoroutine(PlayUIEffect(CLICK));
        }

        /**
         * Callback function called by Dapp when the user Connects to their wallet
         * Should call any next steps
         */
        void ConnectToWalletCallback(string data)
        {
            // TODO: Do things with the UI
            //       For example, Add sound effects that signal succesfull wallet connection
            //       For example, go to next scene
            //       For example, turn on and off a digalog scen

            //StartCoroutine(PlayUIEffect(TRANSACTION_SUCCESS));
            //OpenStakePopup();
            debugLog.text = "Wallet Connected!";
        }

        /**
         * OnClick function when user clicks on "Connect" button
         * Checks whether we are in a WebGL version of the game, or PC
         */
        public void OnClickApproveEntryFee()
        {
            // string value = amountToStake.ToString(); // IGNORE
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // Call external function "ApproveEntryFee"
                // Pass the name of the callback function "ApproveEntryFeeCallback"
                ApproveEntryFee(gameObject.name, "ApproveEntryFeeCallback", amountToStakeStr);
            }
            else
            {
                ApproveEntryFeeCallback("Approve Entry Fee Callback from Unity");
            }
            Debug.Log("Clicked on Approve Entry Fee Button!");

            // TODO: Do stuff with the UI while we wait for the callback
        }

        /**
         * Callback function called by the Dapp when the user approves BUSD
         */
        void ApproveEntryFeeCallback(string data)
        {
            //beginUI.SetActive(false);
            //stakeUI.SetActive(true);

            debugLog.text = "Approved Entry Fee!";
        }

        /**
         * OnClick function triggered when the user click on the "Stake" button
         * Passes down the callback "StakeBUSDCallback" to the Dapp
         */
        public void OnClickStakeEntryFee()
        {
            // Grab entry fee text if needed
            //TMP_InputField stakeInputField = StakeInputField.GetComponent<TMP_InputField>();
            //string text = stakeInputField.GetComponent<TMP_InputField>().text;

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                StakeEntryFee(gameObject.name, "StakeEntryFeeCallback", amountToStakeStr);
            }
            else
            {
                Debug.Log("Entry Fee has been staked");
                StakeEntryFeeCallback("Switching to main scene");
            }

            // TODO: Do something with the UI while we wait for the Dapp to return with a sucessful transaction
            //StartCoroutine(PlayUIEffect(CLICK));
        }

        /**
         * Callback function called by Dapp once the wallet transaction for staking is succesful
         */
        public void StakeEntryFeeCallback(string data)
        {
            // TODO: Do something with the UI
            //       For example, go to the next scene
            debugLog.text = "Staked Entry Fee!";
        }

        void DisplayTime(float timeToDisplay)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            //timeText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);
        }
    }
}