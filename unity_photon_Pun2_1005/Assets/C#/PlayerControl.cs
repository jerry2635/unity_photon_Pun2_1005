using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

namespace jerry
{
    /// <summary>
    /// 玩家控制器
    /// </summary>
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("移動速度"), Range(0, 10)]
        private float speed = 3.5f;
        [Header("檢查地板資料")]
        [SerializeField] private Vector3 groundOffset;//區域座標
        [SerializeField] private Vector3 groundSize;//區域尺寸
        [SerializeField, Header("跳躍高度"), Range(0, 1000)]
        private float jump = 30f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "跑步開關";
        private bool isGround;
        private Transform childCanvas;
        private TextMeshProUGUI textChicken;
        private int countChicken;
        private int countChickenMax = 5;
        private CanvasGroup groupGame;
        private TextMeshProUGUI textWinner;
        private Button btuBackToLobby;

        private void OnDrawGizmos()//透明物件.不會顯示在遊戲內
        {
            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);//顏色(R,G,B,透明度)
            Gizmos.DrawCube(transform.position + groundOffset, groundSize);//初始值都是0.包含尺寸.所以看不到
        }

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
            childCanvas = transform.GetChild(0);

            // ! 代表反義詞//使用者以外的控制器關閉
            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);

            textChicken = transform.Find("畫布玩家名稱/烤雞數量").GetComponent<TextMeshProUGUI>();
            groupGame = GameObject.Find("畫布遊戲介面").GetComponent<CanvasGroup>();
            textWinner = GameObject.Find("勝利者").GetComponent<TextMeshProUGUI>();
            
            btuBackToLobby = GameObject.Find("返回遊戲大廳").GetComponent<Button>();
            btuBackToLobby.onClick.AddListener(() =>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("遊戲大廳");
                }
            });
        }

        private void Start()
        {
            GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

        private void Update()
        {
            Move();
            CheckGround();
            Jump();
            BackToTop();
        }

        /// <summary>
        /// 墜落後.回歸上方
        /// </summary>
        private void BackToTop()
        {
            if (transform.position.y < -20)
            {
                rig.velocity = Vector3.zero;
                transform.position = new Vector3(1.5f, 15, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains("烤雞"))//假如(碰撞器.名稱.包含("烤雞")
            {
                Destroy(collision.gameObject);
                
                //PhotonNetwork.Destroy(collision.gameObject);
                //連線photon伺服器.在伺服器端銷毀物件

                textChicken.text = (++countChicken).ToString();

                if (countChicken > countChickenMax) Win();
            }

        }

        /// <summary>
        /// 勝利條件
        /// </summary>
        private void Win()
        {
            groupGame.alpha = 1;
            groupGame.interactable = true;
            groupGame.blocksRaycasts = true;

            textWinner.text = "獲勝玩家:" + photonView.Owner.NickName;

            DestroyObject();//呼叫刪除物件
        }

        /// <summary>
        /// 刪除物件
        /// </summary>
        private void DestroyObject()
        {
            GameObject[] chickens = GameObject.FindGameObjectsWithTag("烤雞");//用Tag搜尋抓取物件

            for (int i = 0; i < chickens.Length; i++) Destroy(chickens[i]);

            Destroy(FindObjectOfType<SpawnChicken>().gameObject);//銷毀烤雞生成程式
        }

        [PunRPC]
        private void RPCUpdateName()
        {
            transform.Find("畫布玩家名稱/名稱介面").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        private void Jump()
        {
            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                //addforce/應力(二維座標(X軸0,跳躍浮點數))
                rig.AddForce(new Vector2(0, jump));
            }
        }

        private void CheckGround()
        {
            //overlapbox/跨越區域 (主體座標+區域座標,區域尺寸,Z軸0)
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffset, groundSize, 0);
            isGround = hit;
        }

        private void Move()
        {
            //設水平控制器浮點數 h
            float h = Input.GetAxis("Horizontal");
            //velocity/速度. 設置剛體速度=新建置二維座標(速度浮點X水平控制浮點,Y軸0)
            rig.velocity = new Vector2(speed * h, rig.velocity.y);
            ani.SetBool(parWalk, h != 0);

            if (Mathf.Abs(h) < 0.1f) return;
            transform.eulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
            // ? 判斷 h>0 成不成立.成立回傳0.不成立回傳180
            childCanvas.localEulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
        }
    }
}
