using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static bool VisibleRequested;
    private static bool UnlockRequested;

    public static void RequestVisible()
    {
        VisibleRequested = true;
    }

    public static void RequestUnlocked()
    {
        UnlockRequested = true;
    }

    void Update()
    {
        Cursor.visible = VisibleRequested;
        Cursor.lockState = UnlockRequested ? CursorLockMode.None : CursorLockMode.Locked;

        VisibleRequested = false;
        UnlockRequested = false;
    }

}
