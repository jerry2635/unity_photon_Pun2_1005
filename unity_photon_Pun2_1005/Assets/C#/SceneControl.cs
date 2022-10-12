using UnityEngine;
using Photon.Pun;

namespace jerry
{
    /// <summary>
    /// ��������:���a�i�J��ͦ�����
    /// </summary>
    public class SceneControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("���a�w�m��")]
        private GameObject prefabPlayer;

        private void Awake()
        {
            InitiaLizePlayer();
        }

        /// <summary>
        /// ��l�ƪ��a
        /// </summary>
        private void InitiaLizePlayer()
        {
            Vector3 pos = Vector3.zero;//�ͦ��y�ЦW��pos
            pos.x = Random.Range(-5f, 5f);//x�b�H���d��
            pos.y = 6f;//y�b�T�w����
            PhotonNetwork.Instantiate(prefabPlayer.name, pos, Quaternion.identity);//Instantiate/�ͦ�(�W��,�y��,����)
        }
    }
}

