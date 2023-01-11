using UnityEngine;
using UnityEngine.Assertions;
using Photon.Pun;

namespace RingCrisis
{
    /// <summary>
    /// ターゲットの生成を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class TargetManager : MonoBehaviour
    {
        private static readonly float SpawnInterval = 5.0f;

        [SerializeField]
        private RpcManager _rpcManager = null;

        [SerializeField]
        private Target _targetPrefab = null;

        [SerializeField]
        private GameObject _fxSpawn = null;

        [SerializeField]
        private float _randomSpawnPosition = 0f;

        private bool _activated;

        private float _timer;

        public void ActivateSpawn()
        {
            _activated = true;

            SpawnTarget(new Vector3(Random.Range(-_randomSpawnPosition, _randomSpawnPosition),
                0, Random.Range(-_randomSpawnPosition, _randomSpawnPosition)));
        }

        public void DeactivateSpawn()
        {
            _activated = false;
        }

        private void Awake()
        {
            Assert.IsNotNull(_rpcManager);
            Assert.IsNotNull(_targetPrefab);
            Assert.IsNotNull(_fxSpawn);

            _rpcManager.OnTargetSpawn += SpawnTarget;
        }

        private void Update()
        {
            if (!_activated || !PhotonNetwork.IsMasterClient)
            {
                return;
            }

            // 一定時間ごとにターゲットを生成する
            _timer += Time.deltaTime;
            if (_timer > SpawnInterval)
            {
                _timer -= SpawnInterval;
                _rpcManager.SendTargetSpawn(new Vector3(Random.Range(-_randomSpawnPosition, _randomSpawnPosition),
                0, Random.Range(-_randomSpawnPosition, _randomSpawnPosition)));
            }
        }

        private void SpawnTarget(Vector3 position)
        {
            SpawnTargetLocal(position);
        }

        private void SpawnTargetLocal(Vector3 worldPosition)
        {
            Instantiate(_targetPrefab, worldPosition, Quaternion.identity);
            Instantiate(_fxSpawn, worldPosition, Quaternion.identity);
        }
    }
}
