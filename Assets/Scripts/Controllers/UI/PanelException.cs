using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheArchitect.Core.Controllers
{

    public class PanelException : MonoBehaviour
    {
        [SerializeField] public Button ButtonWhatever;
        [SerializeField] public Button ButtonStopShowing;
        [SerializeField] public Button ButtonCopy;
        [SerializeField] public Text TextException;

        void Update()
        {
            CursorManager.RequestUnlocked();
            CursorManager.RequestVisible();
        }
    }

}