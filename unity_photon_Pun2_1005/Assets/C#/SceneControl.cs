using UnityEngine;
using Photon.Pun;

namespace jerry
{
    /// <summary>
    /// 場景控制:玩家進入後生成物件
    /// </summary>
    public class SceneControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("玩家預置物")]
        private GameObject prefabPlayer;

        private void Awake()
        {
            InitiaLizePlayer();
        }

        /// <summary>
        /// 初始化玩家
        /// </summary>
        private void InitiaLizePlayer()
        {
            Vector3 pos = Vector3.zero;//生成座標名稱pos
            pos.x = Random.Range(-5f, 5f);//x軸隨機範圍
            pos.y = 6f;//y軸固定高度
            PhotonNetwork.Instantiate(prefabPlayer.name, pos, Quaternion.identity);//Instantiate/生成(名稱,座標,角度)
        }
    }
}

