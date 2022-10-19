using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

namespace jerry
{
    /// <summary>
    /// 大廳管理器
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region 資料大廳
        private TMP_InputField inputFieldPlayerName;
        private TMP_InputField inputFieldCreateRoomName;
        private TMP_InputField inputFieldJoinRoomName;

        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Button btnJoinRandomRoom;

        private string namePlayer;
        private string nameCreateRoom;
        private string nameJoinRoom;

        /// <summary>
        /// 主要登入畫布
        /// </summary>
        private CanvasGroup groupMain;
        #endregion

        #region 資料房間
        private TextMeshProUGUI textRoomName;
        private TextMeshProUGUI textRoomPlayer;
        private CanvasGroup groupRoom;
        private Button btnStartGame;
        private Button btnLeaveRoom;
        #endregion

        private void Awake()
        {
            GetLobbyObjectAndEvent();

            textRoomName = GameObject.Find("文字房間名稱").GetComponent<TextMeshProUGUI>();
            textRoomPlayer = GameObject.Find("文字房間人數").GetComponent<TextMeshProUGUI>();
            groupRoom = GameObject.Find("房間畫布").GetComponent<CanvasGroup>();
            btnStartGame = GameObject.Find("按鈕開始遊戲").GetComponent<Button>();
            btnLeaveRoom = GameObject.Find("按鈕離開房間").GetComponent<Button>();

            btnLeaveRoom.onClick.AddListener(LeaveRoom);

            //photonview 遠端同步客戶("RPC方法".針對使用者)
            btnStartGame.onClick.AddListener(() => photonView.RPC("RPCStartGame", RpcTarget.All));

            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 遠端同步客戶方法
        /// </summary>
        [PunRPC]
        private void RPCStartGame()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("遊戲場景");
        }

        /// <summary>
        /// 取得大廳物件與事件
        /// </summary>
        private void GetLobbyObjectAndEvent()
        {
            inputFieldPlayerName = GameObject.Find("輸入欄位玩家名稱").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("輸入欄位創建房間名稱").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("輸入欄位指定房間名稱").GetComponent<TMP_InputField>();

            btnCreateRoom = GameObject.Find("按鈕創建房間").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("按鈕加入指定房間").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("按鈕加入隨機房間").GetComponent<Button>();

            groupMain = GameObject.Find("主要登入").GetComponent<CanvasGroup>();

            //按下Enter 或 在其他地方點擊左鍵 結束編輯
            //輸入欄位.結束編輯.添加監聽((輸入欄位的字串)=>儲存)
            inputFieldPlayerName.onEndEdit.AddListener((input) =>
            {
                namePlayer = input;
                PhotonNetwork.NickName = namePlayer;
            });

            inputFieldCreateRoomName.onEndEdit.AddListener((input) => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener((input) => nameJoinRoom = input);

            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);

            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 連線至主機方法
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            print("<color=yellow>連線至主機成功!</color>");

            groupMain.interactable = true;//連線成功後可以顯示
            groupMain.blocksRaycasts = true;//連線成功後可以輸入

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoomName.text = "房間名稱" + PhotonNetwork.CurrentRoom.Name;
            textRoomPlayer.text = $"房間人數{  PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        /// <summary>
        /// 創建房間
        /// </summary>
        private void CreateRoom()
        {
            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 20;
            ro.IsVisible = true;
            PhotonNetwork.CreateRoom(nameCreateRoom, ro);
        }

        /// <summary>
        /// 加入房間
        /// </summary>
        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }

        /// <summary>
        /// 加入隨機房間
        /// </summary>
        private void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();

            groupRoom.alpha = 0;
            groupRoom.interactable = false;
            groupRoom.blocksRaycasts = false;
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoomName.text = "房間名稱:" + PhotonNetwork.CurrentRoom.Name;
            textRoomPlayer.text = $"房間人數{ PhotonNetwork.CurrentRoom.PlayerCount} /{ PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            textRoomPlayer.text = $"房間人數{  PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            textRoomPlayer.text = $"房間人數{  PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

    }
}

