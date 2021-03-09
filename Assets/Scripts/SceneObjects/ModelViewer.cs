using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace TheArchitect.SceneObjects
{

    public class ModelViewer : SceneObject
    {
        public const string HOTSPOT_FOUND = "HOTSPOT_FOUND";
        public const string QUIT = "QUIT";
        [SerializeField] private Transform m_TransformModel;
        [SerializeField] private CinemachineVirtualCamera m_Camera;
        [SerializeField] private Image m_ImageHotspot;
        [SerializeField] private string m_AxisX;
        [SerializeField] private string m_AxisY;
        [SerializeField] private string m_AxisZ;
        [SerializeField] private float m_AxisXMultiplier = 1;
        [SerializeField] private float m_AxisYMultiplier = 1;
        [SerializeField] private float m_AxisZMultiplier = 1;
        [SerializeField] private float m_HotspotDistanceThreshold = 0.8f;

        [SerializeField] private Collider m_HotspotCollider;
        [SerializeField] private RectTransform m_CanvasTransform;
        [SerializeField] private RectTransform m_ImageContainer;
        
        private bool m_MotionActive = true;

        public void DisableMotion()
        {
            this.m_MotionActive = false;
        }
        
        public void EnableMotion()
        {
            this.m_MotionActive = true;
        }

        public void SetHotspotDistanceThreshold(string valueAsString)
        {
            float.TryParse(valueAsString, out this.m_HotspotDistanceThreshold);
        }

        public void LoadModel(string resourcePath)
        {
            GameObject prefab = Resources.Load<GameObject>(resourcePath);
            if (prefab==null)
            {
                Debug.LogWarning($"Can't find {resourcePath}");
                return;
            }

            GameObject model = Instantiate(prefab);
            model.name = prefab.name;
            this.m_TransformModel = model.transform;
            this.m_TransformModel.SetParent(this.transform, false);

            prefab = Resources.Load<GameObject>(resourcePath+"Hotspot");
            if (prefab==null)
            {
                Debug.LogWarning($"Can't find {resourcePath}");
                return;
            }

            GameObject hotspot = Instantiate(prefab);
            hotspot.name = prefab.name;
            hotspot.transform.SetParent(this.m_TransformModel, false);
            this.m_HotspotCollider = hotspot.GetComponent<Collider>();

            this.m_Camera.LookAt = this.m_TransformModel;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.deltaTime == 0)
                return;

            if (this.m_MotionActive)
            {
                this.m_TransformModel.Rotate(new Vector3(
                        Input.GetAxis(m_AxisX) * m_AxisXMultiplier, Input.GetAxis(m_AxisY) * m_AxisYMultiplier, Input.GetAxis(m_AxisZ) * m_AxisZMultiplier
                    ),
                    Space.World
                );

                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
                {
                    Outcome = QUIT;
                }
            }

            m_Camera.m_Lens.FieldOfView = Mathf.Clamp(m_Camera.m_Lens.FieldOfView + Input.GetAxis("Mouse ScrollWheel") + Input.GetAxis("Vertical"), 20,50);

            float dist = Mathf.Clamp01( 1 - ( m_Camera.transform.position - m_HotspotCollider.ClosestPoint(m_Camera.transform.position) ).magnitude );
            dist = dist > m_HotspotDistanceThreshold ? 1 : dist * 0.5f;

            m_ImageHotspot.color = new Color(dist < 1 ? 1 : 0, 1, dist < 1 ? 1 : 0, dist);
            m_ImageHotspot.rectTransform.localScale = new Vector2(2 - dist, 2 - dist);

            Vector2 pos = Camera.main.WorldToViewportPoint(m_HotspotCollider.transform.position);
            pos = new Vector2(
                        pos.x * m_CanvasTransform.rect.width,
                        pos.y * m_CanvasTransform.rect.height
                    );
            m_ImageContainer.anchoredPosition = pos;

            if (dist == 1)
            {
                this.m_MotionActive = false;
                Outcome = HOTSPOT_FOUND;
                m_ImageContainer.Rotate(new Vector3(0, 0, Time.deltaTime * 100));
                this.m_TransformModel.rotation = Quaternion.RotateTowards(
                    this.m_TransformModel.rotation,
                    this.m_HotspotCollider.transform.localRotation,
                    Time.deltaTime * 100
                );
            }
        }
    }

}