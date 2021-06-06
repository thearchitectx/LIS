using UnityEngine;
using System.Xml.Serialization;
using TheArchitect.Cutscene.Data;

namespace TheArchitect.Cutscene.Action
{

    public class AutosaveAction : CutsceneAction
    {
        [XmlAttribute("label")]
        public string Label;

        public override string Update(CutsceneInstance cutscene, CutsceneController controller)
        {
            string rootPath = Application.persistentDataPath;
            string label = string.IsNullOrEmpty(Label) ? "AUTOSAVE" : $"AUTOSAVE ({Label})";
            string slot = "autosave";
            
            controller.Game.State.Save(rootPath, slot, label, Application.version);
            controller.StartCoroutine(
                TheArchitect.Controllers.UI.PanelSaveIO.PanelSaveIO.TakeSaveScreenshot(rootPath, slot)
            );
            return OUTPUT_NEXT;
        }
    }
}