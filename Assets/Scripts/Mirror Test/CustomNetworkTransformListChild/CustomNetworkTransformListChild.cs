using System.Collections.Generic;
using UnityEngine;
using Mirror;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class CustomNetworkTransformListChild : NetworkBehaviour
{
    public bool IsPlayer = false;       // 是在Player Prefab裡傳輸嗎?
    // TrackTransform
    [SerializeField] private Transform[] TrackTransform;

    private SendStruct m_SendStruct;

    public class SendStruct
    {
        public Vector3[] ArrayPos;
        public Quaternion[] ArrayRot;
    }

    private void Start()
    {
        m_SendStruct = new SendStruct();

        m_SendStruct.ArrayPos = new Vector3[TrackTransform.Length];
        m_SendStruct.ArrayRot = new Quaternion[TrackTransform.Length];

        if (!base.isLocalPlayer && IsPlayer)
        {
            foreach (Transform tf in TrackTransform)
            { 
                var rd = tf.gameObject.GetComponent<Rigidbody>();
                if (rd != null)
                    rd.isKinematic = true;
            }

        }
    }

    private void Update()
    {
        if (base.hasAuthority)
            SendTransform();
    }

    public void SendTransform()
    {
        for (int i = 0; i < TrackTransform.Length; i++)
        {
            m_SendStruct.ArrayPos[i] = TrackTransform[i].position;
            m_SendStruct.ArrayRot[i] = TrackTransform[i].rotation;
        }

        SendTransformCmd(m_SendStruct);
        //SendTransformRPC(m_SendStruct);
    }

    // Transform
    [Command(requiresAuthority = false)]
    public void SendTransformCmd(SendStruct ts)
    {
        SendTransformRPC(ts);
    }


    [ClientRpc(includeOwner = false)]
    public void SendTransformRPC(SendStruct ts)
    {
        for (int i = 0; i < TrackTransform.Length; i++)
        {
            TrackTransform[i].position = ts.ArrayPos[i];
            TrackTransform[i].rotation = ts.ArrayRot[i];
        }
    }

    // ---------切換權限----------
    public void SwitchAuthorityFun()
    {
        SwitchAuthority(this.gameObject.GetComponent<NetworkIdentity>(), MirrorBehaviour_Self.instance.netIdentity);
    }

    [Command(requiresAuthority = false)]
    public void SwitchAuthority(NetworkIdentity go_Net, NetworkIdentity target_Net)
    {
        go_Net.RemoveClientAuthority();
        go_Net.AssignClientAuthority(target_Net.connectionToClient);
    }

    #region Start & Stop Callbacks

    /// <summary>
    /// This is invoked for NetworkBehaviour objects when they become active on the server.
    /// <para>This could be triggered by NetworkServer.Listen() for objects in the scene, or by NetworkServer.Spawn() for objects that are dynamically created.</para>
    /// <para>This will be called for objects on a "host" as well as for object on a dedicated server.</para>
    /// </summary>
    public override void OnStartServer() { }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() { }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient() { }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() { }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() { }

    /// <summary>
    /// Called when the local player object is being stopped.
    /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStopLocalPlayer() {}

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority">AssignClientAuthority</see> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnectionToClient parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() { Debug.LogError("此物件權限換你了"); }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

    #endregion
}
