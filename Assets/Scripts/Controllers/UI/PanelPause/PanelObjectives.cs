using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core;

public class PanelObjectives : MonoBehaviour
{
    [SerializeField] private Transform m_Parent;
    [SerializeField] private GameObject m_TextObjectivePrefab;
    [SerializeField] private GameObject m_TextObjectiveDescriptionPrefab;
    [SerializeField] private Game m_Game;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var objective in this.m_Game.GetObjectives())
        {
            if (objective!=null)
            {
                var textTitle = GameObject.Instantiate(this.m_TextObjectivePrefab).GetComponent<Text>();
                textTitle.text = $"- {objective.Title}";
                textTitle.transform.SetParent(this.m_Parent, false);

                var textDescription = GameObject.Instantiate(this.m_TextObjectiveDescriptionPrefab).GetComponent<Text>();
                textDescription.text = objective.Description;
                textDescription.transform.SetParent(this.m_Parent, false);
            }
        }
    }

}
