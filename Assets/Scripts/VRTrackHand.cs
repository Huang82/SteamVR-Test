using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class VRTrackHand : MonoBehaviour
{
    [Header("RemoveHand")]
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
        left = new Transform[35];
        right = new Transform[35];

        TrackLeft = new Transform[35];
        TrackRight = new Transform[35];

        RemoveHandInit();
    }

    void Update()
    {
        if (!IsInitTrackLeft && !IsInitTrackRight)
            TrackHandInit(Player.instance.gameObject.transform);
        else
        {
            HandUpdate();
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
        GameObject t_left = Resources.Load<GameObject>("RemoveHand/LeftHand");
        GameObject t_right = Resources.Load<GameObject>("RemoveHand/RightHand");

        Transform tf_left = GameObject.Instantiate<GameObject>(t_left).transform;
        Transform tf_right = GameObject.Instantiate<GameObject>(t_right).transform;

        LeftHandAddList(tf_left);

        RightHandAddList(tf_right);
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
}
