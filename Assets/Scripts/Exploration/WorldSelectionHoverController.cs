using UnityEngine;
using UnityEngine.UI;
using TheArchitect.Core.Data;

namespace TheArchitect.Exploration
{

    public class WorldSelectionHoverController : MonoBehaviour
    {
        [SerializeField] private Image m_ImageHover;
        [SerializeField] private Text m_TextHoverLabel;
        [SerializeField] private WorldSelection m_WorldSelection;
        [SerializeField] private LayerMask m_SelectableLayer;
        [SerializeField] private LayerMask m_DistanceCheckMask;
        private RectTransform m_CanvasTransform;

        private Transform m_HighlightAll;

        public WorldSelection WorldSelection { get { return m_WorldSelection; } }

        void Start()
        {
            var canvas = this.m_ImageHover.GetComponentInParent<Canvas>();
            this.m_CanvasTransform = canvas.transform.GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0)
                return;
                
            if (this.m_WorldSelection.Selection != null)
            {
                m_TextHoverLabel.gameObject.SetActive(false);
                m_ImageHover.gameObject.SetActive(false);
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            var selectableController = Physics.Raycast(ray, out hit, 10, this.m_SelectableLayer)
                ? hit.transform.GetComponent<WorldSelectableController>()
                : null;

            this.m_WorldSelection.Hover = selectableController != null
                ? selectableController.AsSelectionItem()
                : null;

            this.m_TextHoverLabel.gameObject.SetActive(this.m_WorldSelection.Hover != null);
            this.m_ImageHover.gameObject.SetActive(this.m_WorldSelection.Hover != null);
            
            if (this.m_WorldSelection.Hover != null)
            {
                this.m_TextHoverLabel.text = hit.collider.gameObject.name.ToUpper();

                Vector3[] boundPoints = new Vector3[8];
                Bounds bounds = hit.collider.bounds;
                boundPoints[0] = new Vector3(
                    bounds.center.x + bounds.extents.x,
                    bounds.center.y + bounds.extents.y,
                    bounds.center.z + bounds.extents.z
                );
                boundPoints[1] = new Vector3(
                    bounds.center.x + bounds.extents.x,
                    bounds.center.y + bounds.extents.y,
                    bounds.center.z - bounds.extents.z
                );
                boundPoints[2] = new Vector3(
                    bounds.center.x + bounds.extents.x,
                    bounds.center.y - bounds.extents.y,
                    bounds.center.z + bounds.extents.z
                );
                boundPoints[3] = new Vector3(
                    bounds.center.x + bounds.extents.x,
                    bounds.center.y - bounds.extents.y,
                    bounds.center.z - bounds.extents.z
                );
                boundPoints[4] = new Vector3(
                    bounds.center.x - bounds.extents.x,
                    bounds.center.y + bounds.extents.y,
                    bounds.center.z + bounds.extents.z
                );
                boundPoints[5] = new Vector3(
                    bounds.center.x - bounds.extents.x,
                    bounds.center.y + bounds.extents.y,
                    bounds.center.z - bounds.extents.z
                );
                boundPoints[6] = new Vector3(
                    bounds.center.x - bounds.extents.x,
                    bounds.center.y - bounds.extents.y,
                    bounds.center.z + bounds.extents.z
                );
                boundPoints[7] = new Vector3(
                    bounds.center.x - bounds.extents.x,
                    bounds.center.y - bounds.extents.y,
                    bounds.center.z - bounds.extents.z
                );

                for (int i = 0; i < boundPoints.Length; i++)
                {
                    boundPoints[i] = Camera.main.WorldToViewportPoint(boundPoints[i]);
                }
            
                float minX = boundPoints[0].x;
                float minY = boundPoints[0].y;
                float maxX = boundPoints[0].x;
                float maxY = boundPoints[0].y;
                
                for (int i = 1; i < boundPoints.Length; i++)
                {
                    minX = Mathf.Min(minX, boundPoints[i].x);
                    minY = Mathf.Min(minY, boundPoints[i].y);
                    maxX = Mathf.Max(maxX, boundPoints[i].x);
                    maxY = Mathf.Max(maxY, boundPoints[i].y);
                }
                
                minX *= this.m_CanvasTransform.rect.width;
                maxX *= this.m_CanvasTransform.rect.width;
                minY *= this.m_CanvasTransform.rect.height;
                maxY *= this.m_CanvasTransform.rect.height;
                
                int PAD = 20;
                this.m_ImageHover.rectTransform.anchoredPosition = new Vector2(minX - PAD, minY - PAD);
                this.m_ImageHover.rectTransform.sizeDelta = new Vector2( maxX - minX + PAD + PAD, maxY - minY + PAD + PAD);

                this.m_TextHoverLabel.rectTransform.anchoredPosition = new Vector2(minX + ((maxX-minX)/2), Mathf.Min(this.m_CanvasTransform.rect.height - this.m_TextHoverLabel.fontSize - PAD, maxY) );
                
                this.m_TextHoverLabel.text = this.m_WorldSelection.Hover.DisplayName.ToUpper();

                if (!Physics.CheckSphere(
                        selectableController.transform.position,
                        this.m_WorldSelection.Hover.InteractionDistance,
                        this.m_DistanceCheckMask))
                {
                    m_ImageHover.color = new Color(1, 0, 0, 0.25f);
                    m_TextHoverLabel.color = new Color(1, 0, 0);
                } else {
                    m_ImageHover.color = new Color(0, 1, 0, 0.25f);
                    m_TextHoverLabel.color = new Color(0, 1, 0);
                }
            }
        }
    }

}
