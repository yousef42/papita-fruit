using UnityEngine;
using System.Collections;
using TouchScript.Core;
using TouchScript.Devices.Display;
using TouchScript.Layers;
using TouchScript.Pointers;
using TouchScript.Utils.Attributes;
using TouchScript;
using System;

public class TouchController : MonoBehaviour
{
    public static TouchController Instance;

    public Action<Vector2> onPressed;
    public Action<Vector2> onUpdated;
    public Action<Vector2> onReleased;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed += pointersPressedHandler;
            TouchManager.Instance.PointersUpdated += pointersUpdatedHandler;
            TouchManager.Instance.PointersReleased += pointersReleasedHandler;
        }
    }

    void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed -= pointersPressedHandler;
            TouchManager.Instance.PointersUpdated -= pointersUpdatedHandler;
            TouchManager.Instance.PointersReleased -= pointersReleasedHandler;
        }
    }

    void pointersPressedHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            
        }

        for (int i = 0; i < e.Pointers.Count; i++)
        {
            if (i != 0) return;
            //print("Pressed Now 1.");
            onPressed?.Invoke(e.Pointers[0].Position);
        }
    }

    void pointersUpdatedHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            
        }

        for (int i = 0; i < e.Pointers.Count; i++)
        {
            if (i != 0) return;
            //print("Updated.");
            onUpdated?.Invoke(e.Pointers[0].Position);
        }
    }

    void pointersReleasedHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
        
        }

        for (int i = 0; i < e.Pointers.Count; i++)
        {
            if (i != 0) return;
            //print("Released.");
            onReleased?.Invoke(e.Pointers[0].Position);
        }
    }
}