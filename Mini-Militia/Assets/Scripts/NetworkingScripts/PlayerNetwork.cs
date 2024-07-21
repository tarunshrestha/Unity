using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField]private NetworkVariable<Vector3> _netPlayerPos = new(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Quaternion> _netPlayerRotation = new(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> _netPlayerScale = new(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Quaternion> _netHandRotation = new(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Quaternion> _netHeadRotation = new(writePerm: NetworkVariableWritePermission.Owner);
    public Transform handTransform;
    public Transform headTransform;
    public GunController gunController;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        GameInfo.instance.AddPlayerServerRPC(NetworkManager.Singleton.LocalClient.ClientId);
        Debug.Log(NetworkManager.Singleton.LocalClient.ClientId);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOwner)
        {
            _netPlayerPos.Value = transform.position;
            _netPlayerRotation.Value = transform.rotation;
            _netPlayerScale.Value = transform.localScale;
            _netHandRotation.Value = handTransform.localRotation;
            _netHeadRotation.Value = headTransform.localRotation;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _netPlayerPos.Value, 16 * Time.fixedDeltaTime);
            //transform.position = _netPlayerPos.Value;
            transform.rotation = Quaternion.Slerp(transform.rotation, _netPlayerRotation.Value, 16 * Time.deltaTime);
            handTransform.localRotation = Quaternion.Slerp(handTransform.localRotation, _netHandRotation.Value, 16 * Time.deltaTime);
            headTransform.localRotation = Quaternion.Slerp(headTransform.localRotation, _netHeadRotation.Value, 16 * Time.deltaTime);
            transform.localScale = _netPlayerScale.Value;
        }
    }
    [ServerRpc]
    public void RequestShootServerRpc()
    {
        ShootClientRpc();
    }

    [ClientRpc]
    void ShootClientRpc()
    {
        if (IsLocalPlayer)
            return;
        //gunController.Shoot();
        GetComponent<PlayerController>().gunManager.ShootforClient();
    }

    [ServerRpc]
    public void MakeWalkTrueorFalseServerRpc(bool isWalking)
    {
        MakeWalkTrueClientRpc(isWalking);
    }

    [ClientRpc]
    public void MakeWalkTrueClientRpc(bool isWalking)
    {
        GetComponent<Animator>().SetBool("walk", isWalking);
    }
    [ServerRpc]
    public void MakeBackWalkTrueorFalseServerRpc(bool isWalking)
    {
        MakeBackWalkTrueClientRpc(isWalking);
    }

    [ClientRpc]
    public void MakeBackWalkTrueClientRpc(bool isWalking)
    {
        GetComponent<Animator>().SetBool("backwalk", isWalking);
    }

    [ServerRpc]
    public void ThrowBombServerRpc()
    {
        ThrowBombClientRpc();
    }

    [ClientRpc]
    void ThrowBombClientRpc()
    {
        if (IsLocalPlayer)
            return;
        playerController.ThrowBomb();
    }

    [ServerRpc]
    public void UpdateMiniHealthBarServerRpc(float _life)
    {
        UpdateMiniHealthBarClientRpc(_life);
    }

    [ClientRpc]
    public void UpdateMiniHealthBarClientRpc(float _life)
    {
        GetComponent<PlayerManager>().UpdateMiniHealthBar(_life);
    }

    struct PlayerNetworkData :INetworkSerializable
    {
        private float _x, _y;
        private float _zRot;
        internal Vector3 Position
        {
            get => new Vector3(_x, _y, 0);
            set
            {

                _x = value.x;
                _y = value.y;
            }
        }
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _zRot);

            
        }
    }

}
