using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;

namespace jerry
{
    /// <summary>
    /// ���a���
    /// </summary>
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("���ʳt��"), Range(0, 10)]
        private float speed = 3.5f;
        [Header("�ˬd�a�O���")]
        [SerializeField] private Vector3 groundOffset;//�ϰ�y��
        [SerializeField] private Vector3 groundSize;//�ϰ�ؤo
        [SerializeField, Header("���D����"), Range(0, 1000)]
        private float jump = 30f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "�]�B�}��";
        private bool isGround;
        private Transform childCanvas;

        private void OnDrawGizmos()//�z������.���|��ܦb�C����
        {
            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);//�C��(R,G,B,�z����)
            Gizmos.DrawCube(transform.position + groundOffset, groundSize);//��l�ȳ��O0.�]�t�ؤo.�ҥH�ݤ���
        }

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
            childCanvas = transform.GetChild(0);

            // ! �N���ϸq��//�ϥΪ̥H�~���������
            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);
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
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains("�N��"))//���p(�I����.�W��.�]�t("�N��")
            {
                PhotonNetwork.Destroy(collision.gameObject);
                //�s�uphoton���A��.�b���A���ݾP������
            }

        }

        [PunRPC]
        private void RPCUpdateName()
        {
            transform.Find("�e�����a�W��/�W�٤���").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        private void Jump()
        {
            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                //addforce/���O(�G���y��(X�b0,���D�B�I��))
                rig.AddForce(new Vector2(0, jump));
            }
        }

        private void CheckGround()
        {
            //overlapbox/��V�ϰ� (�D��y��+�ϰ�y��,�ϰ�ؤo,Z�b0)
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundOffset, groundSize, 0);
            isGround = hit;
        }

        private void Move()
        {
            //�]��������B�I�� h
            float h = Input.GetAxis("Horizontal");
            //velocity/�t��. �]�m����t��=�s�ظm�G���y��(�t�ׯB�IX��������B�I,Y�b0)
            rig.velocity = new Vector2(speed * h, rig.velocity.y);
            ani.SetBool(parWalk, h != 0);

            if (Mathf.Abs(h) < 0) return;
            transform.eulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
            // ? �P�_ h>0 ��������.���ߦ^��0.�����ߦ^��180
            childCanvas.localEulerAngles = new Vector3(0, h > 0 ? 180 : 0, 0);
        }
    }
}