using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Test : MonoBehaviour
{
    public SteamVR_Action_Boolean steamVR;
    public GameObject go;

    private bool IsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        go.SetActive(false);
    }

    public void Update()
    {
        var right = steamVR.GetState(Player.instance.rightHand.handType);
        var left = steamVR.GetState(Player.instance.leftHand.handType);

        if (!left && !right)
        {
            go.SetActive(false);
            IsActive = false;
            return;
        }

        if (!IsActive)
        {
            if (right)
            {
                go.SetActive(true);
                IsActive = true;
            }
            else if (left)
            {
                go.SetActive(true);
                IsActive = true;
            }
        }

    }

}
