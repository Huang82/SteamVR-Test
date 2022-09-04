using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandTest : MonoBehaviour
{
    public SteamVR_Action_Boolean menuPose = null;
    public SteamVR_Skeleton_Poser pose = null;
    public GameObject cube;

    private Hand leftHand = null;
    private Hand rightHand = null;
    private bool b = false;
    private Transform parent = null;

    void Start()
    {
        leftHand = Player.instance.leftHand;
        rightHand = Player.instance.rightHand;

        parent = cube.transform.parent;
    }


    void Update()
    {
        var left = menuPose.GetState(leftHand.handType);
        var right = menuPose.GetState(rightHand.handType);

        //Debug.LogError("Left: " + left);
        //Debug.LogError("Right: " + right);
        if (right)
        {
            if (!b)
            { 
                Player.instance.rightHand.skeleton.BlendToPoser(pose);
                Debug.LogError("aaa");
                b = true;
 
                    
                cube.transform.parent = Player.instance.rightHand.gameObject.transform;
                cube.transform.localPosition = Vector3.zero;
                cube.transform.localRotation = Quaternion.identity;

                
            }
        }
        else
        {
            if (b)
            { 
                Player.instance.rightHand.skeleton.BlendToSkeleton(0.2f);
                Debug.LogError("bbb");
                b = false;

                if (parent != null)
                {
                    cube.transform.parent = parent;
                }
            }
        }
    }
}
