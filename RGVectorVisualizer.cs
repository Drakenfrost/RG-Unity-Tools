using UnityEngine;

namespace RGUnityTools
{
    public class RGVectorVisualizer : MonoBehaviour
    {
        #region Singleton
        public static RGVectorVisualizer instance;

        private void Awake()
        {
            instance = this;
            vectors = new GameObject[maxVectors];
        }
        #endregion

        public Material vectorLine;
        public GameObject vectorArrowHead;
        [Range(1, 1000)]
        public int maxVectors = 100;
        [Range(0.05f, 1f)]
        public float lineTrim = 0.5f;

        [Range(0.1f, 5f)]
        public float lineThickness = 1f;
        [Range(0.1f, 5f)]
        public float arrowHeadScale = 1f;
        public Color baseColor = Color.green;

        private GameObject[] vectors;

        private void Start()
        {
            CreateVectorPool();
        }

        public void DrawVector(Vector3 origin, Vector3 vector, int id)
        {
            if (vectors[id] == null)
            {
                Debug.LogError("No vector found with ID: " + id);
                return;
            }

            DestroyVector(id);

            vectors[id].transform.position = origin + vector;
            vectors[id].transform.LookAt(origin + vector * 1.1f);
            vectors[id].GetComponentInChildren<MeshRenderer>().material.color = baseColor;
            vectors[id].transform.localScale = new Vector3(arrowHeadScale, arrowHeadScale, arrowHeadScale);
            vectors[id].SetActive(true);

            LineRenderer line = vectors[id].GetComponent<LineRenderer>();
            line.material = vectorLine;
            line.startColor = baseColor;
            line.endColor = baseColor;
            line.widthMultiplier = lineThickness;
            Vector3[] linePoints = new Vector3[2];
            linePoints[0] = origin;
            linePoints[1] = origin + vector - (vector.normalized * lineTrim);
            line.SetPositions(linePoints);
        }

        public void DrawVector(Vector3 origin, Vector3 vector, Color color, int id)
        {
            if (vectors[id] == null)
            {
                Debug.LogError("No vector found with ID: " + id);
                return;
            }

            DestroyVector(id);

            vectors[id].transform.position = origin + vector;
            vectors[id].transform.LookAt(origin + vector * 1.1f);
            vectors[id].GetComponentInChildren<MeshRenderer>().material.color = color; //Color overload.
            vectors[id].transform.localScale = new Vector3(arrowHeadScale, arrowHeadScale, arrowHeadScale);
            vectors[id].SetActive(true);

            LineRenderer line = vectors[id].GetComponent<LineRenderer>();
            line.material = vectorLine;
            line.startColor = color;
            line.endColor = color;
            line.widthMultiplier = lineThickness;
            Vector3[] linePoints = new Vector3[2];
            linePoints[0] = origin;
            linePoints[1] = origin + vector - (vector.normalized * lineTrim);
            line.SetPositions(linePoints);
        }

        public void DestroyVector(int id)
        {
            if (vectors[id] == null)
                return;

            vectors[id].SetActive(false);
        }

        private void CreateVectorPool()
        {
            if (vectorLine == null || vectorArrowHead == null)
            {
                Debug.LogWarning("Please assign the vector line and arrow head in the inspector.");
                return;
            }

            for (int i = 0; i < maxVectors; i++)
            {
                GameObject newVector = Instantiate(vectorArrowHead, Vector3.zero, Quaternion.identity);
                newVector.AddComponent<LineRenderer>();
                newVector.GetComponent<LineRenderer>().sortingOrder = 1;
                newVector.SetActive(false);
                vectors[i] = newVector;
            }
        }
    }
}
