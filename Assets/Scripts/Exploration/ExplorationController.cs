using UnityEngine;
using TheArchitect.Core;
using TheArchitect.Core.Data;
using TheArchitect.Core.Data.Variables;
using TheArchitect.Core.Constants;
using TheArchitect.Cutscene;
using TheArchitect.Controllers.FirstPerson;

namespace TheArchitect.Exploration
{
    public class ExplorationController : MonoBehaviour
    {
        public const string OUTCOME_DESTROY_PARENT = "_destroyParent";

        [SerializeField] private WorldSelection m_WorldSelection;
        [SerializeField] private FirstPersonPlayerMovement m_Player;
        [SerializeField] private Transform m_WorldSelectablesParent;
        [SerializeField] private Console m_Console;
        [SerializeField] private Game m_Game;
        [SerializeField] private Transform m_DefaultSpawnPoint;
        
        // Start is called before the first frame update
        void Start()
        {
            this.m_WorldSelection.Selection = null;

            GameObject g = Instantiate(Resources.Load<GameObject>(ResourcePaths.PREFAB_FADE_FROM_BLACK));
            g.GetComponent<Animator>().SetFloat("speed", 1 / 3f);
            g.transform.SetParent(this.transform, false);
            Destroy(g, 3);
            

            string spawnPoint = this.m_Game.GetTextState(GameState.TEXT_SPAWN_POINT);
            this.m_Game.SetTextState(GameState.TEXT_SPAWN_POINT, null);
            Debug.Log($"SPAWN POINT: {spawnPoint}");
            if (spawnPoint != null)
            {
                Transform t = transform.Find(spawnPoint);
                if (t != null)
                {
                    Debug.Log($"SPAWN POINT COORDS: {t.position}");
                    this.m_Game.State.SetSpawnPosition(t.position);
                    this.m_Game.State.SetSpawnRotation(t.rotation);
                }
            }
            else if (this.m_DefaultSpawnPoint != null)
            {
                this.m_Game.State.SetSpawnPosition(this.m_DefaultSpawnPoint.position);
                this.m_Game.State.SetSpawnRotation(this.m_DefaultSpawnPoint.rotation);
            }

            this.m_Player.transform.position = this.m_Game.State.GetSpawnPosition();
            this.m_Player.transform.rotation = this.m_Game.State.GetSpawnRotation();
            Physics.SyncTransforms();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0)
                return;

            this.m_Player.gameObject.SetActive(this.m_WorldSelection.Selection == null && !this.m_Game.DisablePlayer);

            this.m_Game.State.SetSpawnPosition(this.m_Player.transform.position);
            this.m_Game.State.SetSpawnRotation(this.m_Player.transform.rotation);

            if (this.m_Player.gameObject.activeSelf && this.m_WorldSelection.Selection == null)
            {
                if ( (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")) && this.m_WorldSelection.Hover != null)
                {
                    WorldSelectionChangeRequest();
                }
            }
            
        }

        private void WorldSelectionChangeRequest()
        {
            // Deactivate current selection children
            if (this.m_WorldSelection.Selection != null) 
            {
                GameObject activation = this.m_WorldSelectablesParent.Find(this.m_WorldSelection.Selection.Name)
                    ?.GetComponent<WorldSelectableController>()
                    ?.GetActivationChild().gameObject;
                
                foreach (Transform t in activation.transform)
                    Destroy(t.gameObject);

                activation.SetActive(false);
            }
                    
            Transform hover = this.m_WorldSelectablesParent.Find(this.m_WorldSelection.Hover.Name);
            if (hover == null)
                return;

            // Is it too far ?
            if (!Physics.CheckSphere(
                    hover.position,
                    this.m_WorldSelection.Hover.InteractionDistance,
                    1 << this.m_Player.gameObject.layer))
            {
                m_Console.Log($"<color=cyan>{this.m_WorldSelection.Hover.DisplayName.ToUpper()}</color> is too far! You need to get closer");
                return;   
            }

            // Confirm selection
            this.m_WorldSelection.Selection = this.m_WorldSelection.Hover;
            Transform newSelection = this.m_WorldSelectablesParent.Find(this.m_WorldSelection.Selection.Name);

            // Activate activation
            newSelection
                ?.GetComponent<WorldSelectableController>()
                ?.GetActivationChild().gameObject.SetActive(true);
            
            CutsceneController cutsceneController = newSelection.GetComponentInChildren<CutsceneController>();
            if (cutsceneController!=null)
            {
                cutsceneController.Reload();
                cutsceneController.OnFinished.AddListener( outcome => {
                    this.m_WorldSelection.Selection = null;
                    newSelection
                        ?.GetComponent<WorldSelectableController>()
                        ?.GetActivationChild().gameObject.SetActive(false);
                        
                    if (outcome == OUTCOME_DESTROY_PARENT)
                    {
                        newSelection.gameObject.SetActive(false);
                        Destroy(newSelection.gameObject, 2);
                    }

                });
            }

        }

    }

}

