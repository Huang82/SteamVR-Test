using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PickUpItem : MonoBehaviour
{
    public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;
    //public Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags;
    public SteamVR_Action_Boolean steamVRB_Right;
    public SteamVR_Action_Boolean steamVRB_Left;
    public GameObject ControlGO;
    public bool restoreOriginalParent = false;

    private bool IsPickUp = false;
    private Hand NowPickUpHand = null;
    private bool IsRightPickUp = false;
    private bool IsLeftPickUp = false;


    private Interactable inter;
    private Throwable throwable;

    private Hand rightHand;
    private Hand leftHand;

    void Start()
    {
        if (ControlGO == null)
        {
            Debug.LogError("請設定要拿起的物件.");
            ControlGO.SendMessage("OnDetachedFromHand", NowPickUpHand, SendMessageOptions.DontRequireReceiver);
            NowPickUpHand = null;
            return;
        }

        inter = ControlGO.GetComponent<Interactable>();
        throwable = ControlGO.GetComponent<Throwable>();

        //ControlGO.SetActive(false);

        rightHand = Player.instance.rightHand;
        leftHand = Player.instance.leftHand;
    }


    void Update()
    {
        if (NowPickUpHand != null)
            Debug.LogError(NowPickUpHand.ObjectIsAttached(ControlGO));

        if (ControlGO == null)
        {
            Debug.LogError("請設定要拿起的物件.");
            return;
        }

        bool right_ = steamVRB_Right.GetState(rightHand.handType);
        bool left_ = steamVRB_Left.GetState(leftHand.handType);


        //if (!right_ && !left_ && IsPickUp)
        //if (NowPickUpHand != null && !NowPickUpHand.ObjectIsAttached(ControlGO) && !right_  && !left_ && IsPickUp)
        if (IsPickUp)
        {

            if (IsRightPickUp && !right_)
                DetachObject();

            
            if (IsLeftPickUp && !left_)
                DetachObject();

            Debug.LogError("aaa");
            
            return;
        }


        if ((right_ || left_) && !IsPickUp)
        {
            if (right_)
            {
                NowPickUpHand = rightHand;
                IsRightPickUp = true;
            }
            else if (left_)
            {
                NowPickUpHand = leftHand;
                IsLeftPickUp = true;
            }

            //ControlGO.transform.position = NowPickUpHand.transform.position;
            //ControlGO.SetActive(true);

            //ControlGO.transform.rotation = NowPickUpHand.transform.rotation;

            //ControlGO.SendMessage("OnAttachedToHand", NowPickUpHand, SendMessageOptions.DontRequireReceiver);
            //ControlGO.SendMessage("HandHoverUpdate", NowPickUpHand, SendMessageOptions.DontRequireReceiver);

            //GrabTypes startingGrabType = NowPickUpHand.GetGrabStarting();

            NowPickUpHand.AttachObject(ControlGO, GrabTypes.Scripted, attachmentFlags);

            IsPickUp = true;
            Debug.LogError("bbb");
        }
        else if ((right_ || left_) && IsPickUp)
        {
            ControlGO.SendMessage("HandAttachedUpdate", NowPickUpHand, SendMessageOptions.DontRequireReceiver);
            Debug.LogError("ccc");
        }

    }

    private void DetachObject()
    {
        ControlGO.SendMessage("OnDetachedFromHand", NowPickUpHand, SendMessageOptions.DontRequireReceiver);
        NowPickUpHand.DetachObject(ControlGO, restoreOriginalParent);
        //ControlGO.SetActive(false);
        IsPickUp = false;
        NowPickUpHand = null;

        IsRightPickUp = false;
        IsLeftPickUp = false;
    }
}
