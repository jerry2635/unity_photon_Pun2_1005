using UnityEngine;
using Photon.Pun;

namespace jerry
{
    /// <summary>
    ///�H���ͦ�(�T�w�I)
    /// </summary>
    public class SpawnChicken : MonoBehaviour
    {
        [SerializeField, Header("�N��")]
        private GameObject prefabChicken;
        [SerializeField, Header("�ͦ��W�v"), Range(0, 5)]
        private float intervalSpawn = 2.5f;
        [SerializeField, Header("�ͦ��I")]
        private Transform[] spawnPoints;

        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating("Spawn", 0, intervalSpawn);
                //invokerepeating/���ƽե� (�W��,���ݮɶ�,�C��ͦ�)
            }

        }

        private void Spawn()
        {
            int random = Random.Range(0, spawnPoints.Length);
            //length �̫�}�C
            PhotonNetwork.Instantiate(prefabChicken.name, spawnPoints[random].position, Quaternion.identity);
        }
    }

}

