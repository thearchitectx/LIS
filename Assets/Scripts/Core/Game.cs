using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TheArchitect.Core.Data;
using TheArchitect.Core.Constants;
using System.Xml.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TheArchitect.Core
{
    [CreateAssetMenu(fileName = "Game", menuName = "Data/Game State")]
    public class Game : ScriptableObject
    {
        public const string FLAG_DISABLE_PLAYER = "#DISABLE_PLAYER";
        public const string FLAG_ALLOW_SAVING = "#ALLOW_SAVING";
        [SerializeField] public bool EditorAutoNewGame;
        [SerializeField] private string m_StartStage;

        [NonSerialized] private Stage[] m_Stages;
        [NonSerialized] private Item[] m_Items;
        [NonSerialized] private Perk[] m_Perks;
        [NonSerialized] private GameState m_State;
        [NonSerialized] public Stage LoadingStage;
        [NonSerialized] public bool AllowSaving;

        [NonSerialized] public UnityEvent<string> OnObjectivesUpdate = new UnityEvent<string>();
        
        [NonSerialized] private bool m_DisablePlayer;
        [NonSerialized] private int m_StateChangeCount;

        public bool DisablePlayer {
             get { return m_DisablePlayer; }
             set {
                 this.SetFlagState(FLAG_DISABLE_PLAYER, value ? 1 : 0);
             }
         }

        
        public GameState State {
            get { 
                if (m_State==null) 
                {
                    this.m_State = new GameState();
                    CacheData();
                }
                
                return m_State; 
            }
            
            set {
                this.m_State = value;
                this.AllowSaving = true;
                CacheData();
                this.LoadStage(GetStage(State.Stage));
            }
        }

        public int StateChangeCount {
            get { return m_StateChangeCount; }
        }
        
        private void CacheData()
        {
            if (this.m_Items==null)
            {
                this.m_Items = Resources.LoadAll<Item>(ResourcePaths.SO_ITEMS);
                this.m_Stages = Resources.LoadAll<Stage>(ResourcePaths.SO_STAGES);
                this.m_Perks = Resources.LoadAll<Perk>(ResourcePaths.SO_PERKS);
            }
        }

        public void NewGame()
        {
            Debug.Log($"Starting new game");
            this.m_State = new GameState();

            if (this.m_StartStage == null || this.m_StartStage == "")
                this.m_StartStage = "classroom-intro";

            CacheData();

            this.AllowSaving = true;

            LoadStage(GetStage(this.m_StartStage));
        }

        public Stage GetStage(string name)
        {
            return this.m_Stages.Where( s => s.name == name ).FirstOrDefault();
        }

        public void LoadStage(Stage stage)
        {
            if (stage==null)
            {
                Debug.LogError($"Received null stage to load");
                return;
            }

            Debug.Log($"Activating state {stage.Scene}::{stage.MechanicPrefab}");

            State.Stage = stage.name;
            this.LoadingStage = stage;
            SceneManager.LoadScene("Loading", LoadSceneMode.Single);
        }
        
        #region PERKS
        public Perk[] GetActivePerks()
        {
            return this.m_Perks.Where( p => p.IsActive(this) ).ToArray();
        }

        public bool HasActivePerk(string perkName)
        {
            return this.m_Perks.Count( p => p.name == perkName && p.IsActive(this) ) > 0;
        }
        #endregion

        #region STATS
        public void SetCharacterStat(Character character, string stat, int value)
        {
            SetFlagState($"STAT_{character.name}_{stat}", value);
        }

        public bool HasCharacterStat(Character character, string stat)
        {
            return this.State.FlagIndex.ContainsKey($"STAT_{character.name}_{stat}");
        }
        
        public int GetCharacterStat(Character character, string stat)
        {
            return GetFlagState($"STAT_{character.name}_{stat}");
        }
        #endregion

        #region ITEMS
        public int GetItemCount(String itemName)
        {
            return GetFlagState(itemName);
        }

        public int GetItemCount(Item item)
        {
            return GetFlagState(item.name);
        }

        public int AddItem(Item item, int count)
        {
            int c = GetItemCount(item);
            Debug.Log($"{item.name}: {c} {count}");
            SetFlagState(item.name, c = c + count);
            return c;
        }

        public void SetItem(Item item, int count)
        {
            SetFlagState(item.name, count);
        }

        public Dictionary<Item, int> GetInventory()
        {
            Dictionary<Item, int> d = new Dictionary<Item, int>();
            foreach (Item i in this.m_Items)
            {
                int count = GetItemCount(i);
                if (count>0)
                    d.Add(i, count);
            }

            return d;
        }
        #endregion

        #region FLAGS
        public int GetFlagState(string name)
        {
            Flag f;
            return this.State.FlagIndex.TryGetValue(name, out f)
                ? f.State
                : 0;
        }

        public void SetFlagState(string name, int state)
        {
            if (name==FLAG_DISABLE_PLAYER)
                m_DisablePlayer = state != 0;
            if (name==FLAG_ALLOW_SAVING)
                AllowSaving = state != 0;

            this.State.FlagIndex[name] = new Flag() { Name = name, State = state };
            m_StateChangeCount++;

            #if UNITY_EDITOR
            Debug.Log($"SetFlagState {name}={state}  #{m_StateChangeCount}");
            #endif
        }
        #endregion

        #region TEXT
        public string GetTextState(string name)
        {
            TextData t;
            return this.State.TextIndex.TryGetValue(name, out t)
                ? t.State
                : null;
        }

        public void SetTextState(string name, string state)
        {
            #if UNITY_EDITOR
            Debug.Log($"SetTextState {name} {state}");
            #endif

            if (state==null)
                this.State.TextIndex.Remove(name);
            else
                this.State.TextIndex[name] = new TextData() {Name = name, State = state};

            m_StateChangeCount++;
        }
        #endregion

        #region FLOATS
        public float GetFloatState(string name)
        {
            Float f;
            return this.m_State.FloatIndex.TryGetValue(name, out f)
                ? f.State
                : float.NaN;
        }

        public void SetFloatState(string name, float state)
        {
            this.m_State.FloatIndex[name] = new Float() {Name = name, State = state};
            m_StateChangeCount++;
        }
        #endregion

        #region OBJECTIVE
        public void AddObjective(string name)
        {
            #if UNITY_EDITOR
            Debug.Log($"AddObjective {name}");
            #endif

            this.m_State.ObjectiveIndex.Add(name);
            OnObjectivesUpdate.Invoke(name);
            m_StateChangeCount++;
        }

        public void RemoveObjective(string name)
        {
            #if UNITY_EDITOR
            Debug.Log($"RemoveObjective {name}");
            #endif

            this.m_State.ObjectiveIndex.Remove(name);
            OnObjectivesUpdate.Invoke(name);
            m_StateChangeCount++;
        }

        public bool HasObjective(string name)
        {
            return this.m_State.ObjectiveIndex.Contains(name);
        }
        #endregion

    }

}