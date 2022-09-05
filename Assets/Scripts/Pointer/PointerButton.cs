using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointerButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnClick;

    private void CallEvent()
    {
        Debug.LogError("Ä²µo");
        OnClick.Invoke();
    }
}
