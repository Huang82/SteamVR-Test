using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Valve.VR.InteractionSystem;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class SteamVR_Player : NetworkBehaviour
{

    [Header("RemoteHand")]
    public GameObject RemoteHand_left;
    public GameObject RemoteHand_right;

    public Transform[] left;
    public Transform[] right;

    private int index_Left = 0;
    private int index_Right = 0;

    [Header("Track")]
    public Transform[] TrackLeft;
    public Transform[] TrackRight;

    private bool IsInitTrackLeft = false;
    private bool IsInitTrackRight = false;

    private int index_TrackLeft = 0;
    private int index_TrackRight = 0;


    void Start()
    {
        Destroy(GameObject.Find("TempCamera"));

        if (base.isLocalPlayer)
        {
            left = new Transform[35];
            right = new Transform[35];

            TrackLeft = new Transform[35];
            TrackRight = new Transform[35];

            RemoveHandInit();
        }

    }

    void Update()
    {
        if (base.isLocalPlayer)
        { 
            if (!IsInitTrackLeft && !IsInitTrackRight && Player.instance != null)
                TrackHandInit(Player.instance.gameObject.transform);
            else
            {
                HandUpdate();
            }
        } 
    }

    private void HandUpdate()
    {
        HandUpdateLeft();

        HandUpdateRight();
    }

    private void HandUpdateLeft()
    {
        for (int i = 0; i < TrackLeft.Length; i++)
        {
            left[i].position = TrackLeft[i].position;
            left[i].rotation = TrackLeft[i].rotation;
        }
    }

    private void HandUpdateRight()
    {
        for (int i = 0; i < TrackRight.Length; i++)
        {
            right[i].position = TrackRight[i].position;
            right[i].rotation = TrackRight[i].rotation;
        }
    }

    // -------------Remove Hand----------------
    private void RemoveHandInit()
    {
        GameObject t_left = RemoteHand_left;
        GameObject t_right = RemoteHand_right;

        //Transform tf_left = GameObject.Instantiate<GameObject>(t_left).transform;
        //Transform tf_right = GameObject.Instantiate<GameObject>(t_right).transform;

        LeftHandAddList(t_left.transform);

        RightHandAddList(t_right.transform);
    }

    private void LeftHandAddList(Transform go)
    {
        left[index_Left] = go;
        index_Left++;

        for (int i = 0; i < go.childCount; i++)
        {
            if (go.GetChild(i).childCount > 0)
            {
                LeftHandAddList(go.GetChild(i));
            }
            else
            {
                left[index_Left] = go.GetChild(i);
                index_Left++;
            }
        }
    }

    private void RightHandAddList(Transform go)
    {
        right[index_Right] = go;
        index_Right++;

        for (int i = 0; i < go.childCount; i++)
        {
            if (go.GetChild(i).childCount > 0)
            {
                RightHandAddList(go.GetChild(i));
            }
            else
            {
                right[index_Right] = go.GetChild(i);
                index_Right++;
            }
        }
    }

    // -------------Track Hand-----------------
    private void TrackHandInit(Transform go)
    {
        if (IsInitTrackLeft && IsInitTrackRight)
            return;

        for (int i = 0; i < go.childCount; i++)
        {

            if (go.GetChild(i).childCount > 0)
                TrackHandInit(go.GetChild(i));

            if (go.GetChild(i).name == "LeftRenderModel Slim(Clone)")
            {
                LeftTrackHandAddList(go.GetChild(i));
                IsInitTrackLeft = true;
            }

            if (go.GetChild(i).name == "RightRenderModel Slim(Clone)")
            {
                RightTrackHandAddList(go.GetChild(i));
                IsInitTrackRight = true;
            }
        }
    }

    private void LeftTrackHandAddList(Transform go)
    {

        if (index_TrackLeft > (TrackLeft.Length - 1))
            return;

        TrackLeft[index_TrackLeft] = go;
        index_TrackLeft++;

        for (int i = 0; i < go.childCount; i++)
        {
            if (index_TrackLeft > (TrackLeft.Length - 1))
                return;

            if (go.GetChild(i).childCount > 0)
            {
                LeftTrackHandAddList(go.GetChild(i));
            }
            else
            {
                TrackLeft[index_TrackLeft] = go.GetChild(i);
                index_TrackLeft++;
            }
        }
    }

    private void RightTrackHandAddList(Transform go)
    {

        if (index_TrackRight > (TrackRight.Length - 1))
            return;

        TrackRight[index_TrackRight] = go;
        index_TrackRight++;

        for (int i = 0; i < go.childCount; i++)
        {
            if (index_TrackRight > (TrackRight.Length - 1))
                return;

            if (go.GetChild(i).childCount > 0)
            {
                RightTrackHandAddList(go.GetChild(i));
            }
            else
            {
                TrackRight[index_TrackRight] = go.GetChild(i);
                index_TrackRight++;
            }
        }
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
    public override void OnStartAuthority() { }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() { }

    #endregion
}
