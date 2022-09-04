using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CustomThrowable : Throwable
{
    protected override void HandAttachedUpdate(Hand hand)
    {
        if (onHeldUpdate != null)
            onHeldUpdate.Invoke(hand);
    }
}
