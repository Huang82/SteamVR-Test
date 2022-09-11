using ITRI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TrackVRPlayer : MonoBehaviour
{
    [SerializeField] private RemotePlayer m_RemotePlayer;
    [SerializeField] private Transform Head;
    [SerializeField] private Transform Hand_Left;
    [SerializeField] private Transform Hand_Right;

    [SerializeField] private float y = 0;

    private RemotePlayer.C2S_PlayerPose data;
    private Vector3 rot_left;
    private Vector3 rot_right;

    private bool IsGetHand = false;

    void Start()
    {
        data = new RemotePlayer.C2S_PlayerPose();

        rot_left = new Vector3();
        rot_right = new Vector3();

        data.left_hand_tracked = true;
        data.right_hand_tracked = true;
    }


    void Update()
    {
        //// 取得VR手模型
        //if (!IsGetHand)
        //{
        //    if (Player.instance.leftHand != null && Player.instance.rightHand != null)
        //    {
        //        Hand_Left = Player.instance.leftHand.mainRenderModel.gameObject.transform;
        //        Hand_Right = Player.instance.rightHand.mainRenderModel.gameObject.transform;
        //        IsGetHand = true;
        //    }
        //}

        //if (!IsGetHand)
        //    return;

        data.y = y;

        // left -60 20
        data.head_position = Head.localPosition;
        data.head_rotation = Head.localRotation;

        data.left_hand_position = Hand_Left.localPosition;

        rot_left.x = Hand_Left.localRotation.eulerAngles.x;
        rot_left.y = Hand_Left.localRotation.eulerAngles.y;
        rot_left.z = Hand_Left.localRotation.eulerAngles.z + 90f;

        data.left_hand_rotation = Quaternion.Euler(rot_left);

        //data.left_hand_rotation = Hand_Left.localRotation;

        // right 60 -20
        data.right_hand_position = Hand_Right.localPosition;

        rot_right.x = Hand_Right.localRotation.eulerAngles.x;
        rot_right.y = Hand_Right.localRotation.eulerAngles.y;
        rot_right.z = Hand_Right.localRotation.eulerAngles.z - 90f;

        data.right_hand_rotation = Quaternion.Euler(rot_right);
        //data.right_hand_rotation = Hand_Right.rotation;

        m_RemotePlayer.SetData(data);
    }
}
