using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

namespace jerry
{
    /// <summary>
    /// �j�U�޲z��
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region ��Ƥj�U
        private TMP_InputField inputFieldPlayerName;
        private TMP_InputField inputFieldCreateRoomName;
        private TMP_InputField inputFieldJoinRoomName;

        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Button btnJoinRandomRoom;

        private string namePlayer;
        private string nameCreateRoom;
        private string nameJoinRoom;

        private CanvasGroup groupMain;
        #endregion

        #region ��Ʃж�
        private TextMeshProUGUI textRoomName;
        private TextMeshProUGUI textRoomPlayer;
        private CanvasGroup groupRoom;
        private Button btnStartGame;
        private Button btnLeaveGame;
        #endregion

        private void Awake()
        {
            GetLobbyObjectAndEvent();

            textRoomName = GameObject.Find("��r�ж��W��").GetComponent<TextMeshProUGUI>();
            textRoomPlayer = GameObject.Find("��r�ж��H��").GetComponent<TextMeshProUGUI>();
            groupRoom = GameObject.Find("�ж��e��").GetComponent<CanvasGroup>();
            btnStartGame = GameObject.Find("���s�}�l�C��").GetComponent<Button>();
            btnLeaveGame = GameObject.Find("���s���}�ж�").GetComponent<Button>();

            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// ���o�j�U����P�ƥ�
        /// </summary>
        private void GetLobbyObjectAndEvent()
        {
            inputFieldPlayerName = GameObject.Find("��J��쪱�a�W��").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("��J���Ыةж��W��").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("��J�����w�ж��W��").GetComponent<TMP_InputField>();

            btnCreateRoom = GameObject.Find("���s�Ыةж�").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("���s�[�J���w�ж�").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("���s�[�J�H���ж�").GetComponent<Button>();

            groupMain = GameObject.Find("�D�n�n�J").GetComponent<CanvasGroup>();

            //���UEnter �� �b��L�a���I������ �����s��
            //��J���.�����s��.�K�[��ť((��J��쪺�r��)=>�x�s)
            inputFieldPlayerName.onEndEdit.AddListener((input) => namePlayer = input);
            inputFieldCreateRoomName.onEndEdit.AddListener((input) => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener((input) => nameJoinRoom = input);

            btnCreateRoom.onClick.AddListener(CreateRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);
        }

        /// <summary>
        /// �s�u�ܥD����k
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            groupMain.interactable = true;//�s�u���\��i�H���
            groupMain.blocksRaycasts = true;//�s�u���\��i�H��J

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoomName.text = "�ж��W��:" + PhotonNetwork.CurrentRoom.Name;
            textRoomPlayer.text = $"�ж��H��{  PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        /// <summary>
        /// �Ыةж�
        /// </summary>
        private void CreateRoom()
        {
            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 20;
            ro.IsVisible = true;
            PhotonNetwork.CreateRoom(nameCreateRoom, ro);
        }

        /// <summary>
        /// �[�J�ж�
        /// </summary>
        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }

        /// <summary>
        /// �[�J�H���ж�
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
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            textRoomPlayer.text = $"�ж��H��{  PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            textRoomPlayer.text = $"�ж��H��{  PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

    }
}

