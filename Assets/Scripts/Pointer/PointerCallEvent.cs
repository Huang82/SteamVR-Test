using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PointerCallEvent : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Boolean VR_action;

    [HideInInspector] public bool IsHit = false;

    private void OnTriggerStay(Collider other)
    {
        Debug.LogError(other.name);
        if (IsHit && VR_action.GetStateDown(Player.instance.rightHand.handType))
        {
            Debug.LogError("aaa");
            other.SendMessage("CallEvent", SendMessageOptions.DontRequireReceiver);
        }
    }
}
