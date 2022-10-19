using UnityEngine;
using Photon.Pun;

namespace jerry
{
    /// <summary>
    ///隨機生成(固定點)
    /// </summary>
    public class SpawnChicken : MonoBehaviour
    {
        [SerializeField, Header("烤雞")]
        private GameObject prefabChicken;
        [SerializeField, Header("生成頻率"), Range(0, 5)]
        private float intervalSpawn = 2.5f;
        [SerializeField, Header("生成點")]
        private Transform[] spawnPoints;

        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating("Spawn", 0, intervalSpawn);
                //invokerepeating/重複調用 (名稱,等待時間,每秒生成)
            }

        }

        private void Spawn()
        {
            int random = Random.Range(0, spawnPoints.Length);
            //length 最後陣列
            PhotonNetwork.Instantiate(prefabChicken.name, spawnPoints[random].position, Quaternion.identity);
        }
    }

}

