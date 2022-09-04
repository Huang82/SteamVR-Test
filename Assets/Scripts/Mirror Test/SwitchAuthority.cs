using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAuthority : MonoBehaviour
{
    private NetworkIdentity m_NetworkIdentity;

    void Start()
    {
        m_NetworkIdentity = this.gameObject.GetComponent<NetworkIdentity>();
    }

    public void SwitchAuthorityFun()
    {
        MirrorBehaviour_Self.instance.SwitchAuthorityFun(m_NetworkIdentity, MirrorBehaviour_Self.instance.netIdentity);
    }
}
