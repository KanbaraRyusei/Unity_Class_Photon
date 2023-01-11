using System;
using Photon.Pun;
using UnityEngine;

namespace RingCrisis
{
    /// <summary>
    /// PhotonのRPC送受信を担うコンポーネント
    /// RequireComponent属性を使うと他のコンポーネントが必須であることを示すことができ、アタッチ時にその存在がチェックされるようになる
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
    public class RpcManager : MonoBehaviour
    {
        public event Action OnReceiveStartGame;

        public event Action<TeamColor, Vector3> OnShootRing;

        public event Action<Vector3> OnTargetSpawn;

        private PhotonView _photonView;

        public void SendStartGame()
        {
            // "StartGame" と書く代わりに nameof(StartGame) とすると、メソッド名の変更に対して保守性が高まります
            // https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/nameof
            _photonView.RPC(nameof(StartGame), RpcTarget.AllViaServer);
        }

        public void SendShootRing(TeamColor color, Vector3 dir)
        {
            _photonView.RPC(nameof(ShootRing), RpcTarget.AllViaServer, color, dir);
        }

        public void SendTargetSpawn(Vector3 position)
        {
            _photonView.RPC(nameof(TargetSpawn), RpcTarget.AllViaServer, position);
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        [PunRPC]
        private void StartGame()
        {
            OnReceiveStartGame?.Invoke();
        }

        [PunRPC]
        private void ShootRing(TeamColor color, Vector3 dir)
        {
            OnShootRing?.Invoke(color, dir);
        }

        [PunRPC]
        private void TargetSpawn(Vector3 vector3)
        {
            OnTargetSpawn?.Invoke(vector3);
        }
    }
}
