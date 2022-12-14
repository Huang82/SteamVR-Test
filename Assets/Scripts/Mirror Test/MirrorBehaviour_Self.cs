using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

public class MirrorBehaviour_Self : NetworkBehaviour
{
    public static MirrorBehaviour_Self instance;

    public float speed = 1000f;

    [SerializeField] private Rigidbody rd;

    [SerializeField] private Text m_Text;
    [SerializeField] private InputField m_InputField;

    private void Start()
    {
        if (base.isLocalPlayer)
        { 
            instance = this;
        }

     
        if (rd == null)
            rd = this.GetComponent<Rigidbody>();

        m_Text = GameObject.Find("ChatText").GetComponent<Text>();
        m_InputField = GameObject.Find("InputField").GetComponent<InputField>();
        GameObject.Find("Button").GetComponent<Button>().onClick.AddListener(SendMes);

        //if (isLocalPlayer)
        //    NetworkClient.localPlayer.name = UnityEngine.Random.Range(0, 100000).ToString();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKey(KeyCode.W))
        {
            rd.AddForce((Vector3.forward * Time.deltaTime * speed), ForceMode.Force);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rd.AddForce((Vector3.back * Time.deltaTime * speed), ForceMode.Force);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rd.AddForce((Vector3.right * Time.deltaTime * speed), ForceMode.Force);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rd.AddForce((Vector3.left * Time.deltaTime * speed), ForceMode.Force);
        }
    }

    // Chat
    [Command(requiresAuthority = false)]
    public void TestCmd(string s)
    {
        if (base.connectionToClient.connectionId != 0) return;

        Debug.LogError("TestCmd: " + m_InputField.text + "  " + this.name);
        TestRPC(s + "  " + this.name);
    }

    public void SendMes()
    {
        TestCmd(m_InputField.text);
        m_InputField.text = "";
    }

    [ClientRpc]
    public void TestRPC(string s)
    {
        Debug.LogError("TestRPC: " + s);
        m_Text.text += s + "\n";
    }

    // ------------?????v??----------------
    public void SwitchAuthorityFun(NetworkIdentity go_Net, NetworkIdentity target_Net)
    {
        SwitchAuthority(go_Net, target_Net);
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
    public override void OnStartServer() 
    {
        Debug.LogError("OnStartServer");
    }

    /// <summary>
    /// Invoked on the server when the object is unspawned
    /// <para>Useful for saving object data in persistent storage</para>
    /// </summary>
    public override void OnStopServer() 
    {
        Debug.LogError("OnStopServer");
    }

    /// <summary>
    /// Called on every NetworkBehaviour when it is activated on a client.
    /// <para>Objects on the host have this function called, as there is a local client on the host. The values of SyncVars on object are guaranteed to be initialized correctly with the latest state from the server when this function is called on the client.</para>
    /// </summary>
    public override void OnStartClient() 
    {
        Debug.LogError("OnStartClient");
    }

    /// <summary>
    /// This is invoked on clients when the server has caused this object to be destroyed.
    /// <para>This can be used as a hook to invoke effects or do client specific cleanup.</para>
    /// </summary>
    public override void OnStopClient() 
    {
        Debug.LogError("OnStopClient");
    }

    /// <summary>
    /// Called when the local player object has been set up.
    /// <para>This happens after OnStartClient(), as it is triggered by an ownership message from the server. This is an appropriate place to activate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStartLocalPlayer() 
    {
        Debug.LogError("OnStartLocalPlayer");
    }

    /// <summary>
    /// Called when the local player object is being stopped.
    /// <para>This happens before OnStopClient(), as it may be triggered by an ownership message from the server, or because the player object is being destroyed. This is an appropriate place to deactivate components or functionality that should only be active for the local player, such as cameras and input.</para>
    /// </summary>
    public override void OnStopLocalPlayer() 
    {
        Debug.LogError("OnStopLocalPlayer");
    }

    /// <summary>
    /// This is invoked on behaviours that have authority, based on context and <see cref="NetworkIdentity.hasAuthority">NetworkIdentity.hasAuthority</see>.
    /// <para>This is called after <see cref="OnStartServer">OnStartServer</see> and before <see cref="OnStartClient">OnStartClient.</see></para>
    /// <para>When <see cref="NetworkIdentity.AssignClientAuthority">AssignClientAuthority</see> is called on the server, this will be called on the client that owns the object. When an object is spawned with <see cref="NetworkServer.Spawn">NetworkServer.Spawn</see> with a NetworkConnectionToClient parameter included, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStartAuthority() 
    {
        Debug.LogError("OnStartAuthority");
    }

    /// <summary>
    /// This is invoked on behaviours when authority is removed.
    /// <para>When NetworkIdentity.RemoveClientAuthority is called on the server, this will be called on the client that owns the object.</para>
    /// </summary>
    public override void OnStopAuthority() 
    {
        Debug.LogError("OnStopAuthority");
    }

    #endregion
}
