using System;
using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core.Data;
using TheArchitect.Core.Constants;

namespace TheArchitect.Core.Controllers
{
	public class PanelTrophies : MonoBehaviour 
	{
		[SerializeField] private RectTransform m_PanelContent;
		[SerializeField] private GameObject m_PrefabItem;
		[SerializeField] private Button m_ButtonClear;
		[SerializeField] private Button m_ButtonBack;

		public Button ButtonBack { get { return this.m_ButtonBack; } }

		void Start()
		{
			const string CONFIRM_TEXT = "CONFIRM?";
			
			this.m_ButtonClear.onClick.AddListener( () => {
				Text text = this.m_ButtonClear.GetComponentInChildren<Text>();
				if (text.text != CONFIRM_TEXT)
					text.text = CONFIRM_TEXT;
				else
				{
					text.text = "CLEARED";
					Trophy.WipeUnlockData();
					Build();
				}
			});

			Build();
		}

		void Build() 
		{
			foreach (Transform item in this.m_PanelContent) {
				Destroy(item.gameObject);
			}

			var trophies = Resources.LoadAll<Trophy>(ResourcePaths.SO_TROPHIES);

			foreach (var trophy in trophies)
			{
				PanelTrophyItem panelTrophyItem = GameObject.Instantiate(this.m_PrefabItem).GetComponent<PanelTrophyItem>();
				panelTrophyItem.transform.SetParent(m_PanelContent, false);
				panelTrophyItem.SetTrophy(trophy);
			} 
		}

		void Update()
		{
			CursorManager.RequestVisible();
			CursorManager.RequestUnlocked();
		}

	}
}

