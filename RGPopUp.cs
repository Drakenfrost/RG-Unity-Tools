using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RGPopUp : MonoBehaviour
{
    #region Singleton
    public static RGPopUp instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] Transform viewportTransform;
    [SerializeField] Canvas overlayCanvas;
    [SerializeField] Canvas worldspaceCanvas;
    private List<ActiveTextPopUp> activeTextPopUps;
    public float textInterpolation;
    public TextPopUp[] textPopups;

    private List<ActiveItemIconPopUp> activeItemIconPopUps;
    public GameObject itemIconPrefab;
    public float itemIconInterpolation;
    public float riseSpeed;
    public float fadeSpeed;

    private void Start()
    {
        activeTextPopUps = new List<ActiveTextPopUp>();
        activeItemIconPopUps = new List<ActiveItemIconPopUp>();
    }

    private void Update()
    {
        ManageActiveTextPopUps();
        ManageActiveItemIconPopUps();
    }

    /* ==== */
    /* Text:*/
    /* ==== */
    private void ManageActiveTextPopUps()
    {
        for (int i = 0; i < activeTextPopUps.Count; i++)
        {
            if (activeTextPopUps[i] != null)
            {
                ActiveTextPopUp popUp = activeTextPopUps[i];

                //Translation:
                Vector3 newPosition = new Vector3(popUp.changeX * Time.deltaTime, popUp.changeY * Time.deltaTime, 0) + popUp.transform.position;
                popUp.transform.position = Vector3.Lerp(popUp.transform.position, newPosition, textInterpolation);
                popUp.transform.forward = viewportTransform.forward;

                //Size:
                if (popUp.growing && popUp.text.fontSize <= popUp.maxSize)
                {
                    popUp.text.fontSize += popUp.growRate * Time.deltaTime;
                }
                else
                {
                    popUp.growing = false;
                    popUp.text.fontSize -= popUp.shrinkRate * Time.deltaTime;

                    if (popUp.fadeOnShrink)
                    {
                        popUp.color.a -= popUp.fadeRate * Time.deltaTime;
                        popUp.text.color = popUp.color;
                    }

                    if (popUp.text.fontSize <= 0)
                        DestroyTextPopUp(popUp);
                }
            }
        }
    }

    void SpawnTextPopUp(int index, bool worldSpace, Vector3 position, string text)
    {
        if (textPopups[index].prefab != null)
        {
            TextPopUp textPopup = textPopups[index];
            ActiveTextPopUp newActiveTextPopup = new ActiveTextPopUp
            {
                color = textPopup.color,
                fadeOnShrink = textPopup.fadeOnShrink,
                growing = true,
                growRate = textPopup.growRate,
                shrinkRate = textPopup.shinkRate,
                fadeRate = textPopup.fadeRate,
                maxSize = textPopup.growToSize,
                changeX = Random.Range(-textPopup.maxSidewardDeviation, textPopup.maxSidewardDeviation),
                changeY = Random.Range(textPopup.minRiseSpeed, textPopup.maxRiseSpeed)
            };

            Canvas canvas;

            if (worldSpace)
                canvas = worldspaceCanvas;
            else
                canvas = overlayCanvas;

            GameObject newPopup = Instantiate(textPopup.prefab, position, Quaternion.Euler(viewportTransform.forward), canvas.transform);
            newActiveTextPopup.transform = newPopup.transform;

            newActiveTextPopup.text = newPopup.GetComponent<TextMeshProUGUI>();
            newActiveTextPopup.text.text = text;
            newActiveTextPopup.text.fontSize = textPopup.startSize;
            newActiveTextPopup.text.color = textPopup.color;

            activeTextPopUps.Add(newActiveTextPopup);
        }
    }

    void DestroyTextPopUp(ActiveTextPopUp textPopUp)
    {
        Destroy(textPopUp.transform.gameObject);

        if (activeTextPopUps.Contains(textPopUp))
            activeTextPopUps.Remove(textPopUp);
    }

    /* ===== */
    /* Image:*/
    /* ===== */
    void ManageActiveItemIconPopUps()
    {
        for (int i = 0; i < activeItemIconPopUps.Count; i++)
        {
            if (activeItemIconPopUps[i] != null)
            {
                ActiveItemIconPopUp popUp = activeItemIconPopUps[i];

                //Rising:
                Vector3 newPosition = new Vector3(0, popUp.riseSpeed * Time.deltaTime, 0) + popUp.transform.position;
                popUp.transform.position = Vector3.Lerp(popUp.transform.position, newPosition, textInterpolation);
                popUp.transform.forward = viewportTransform.forward;

                //Fading:
                Color newColor = popUp.icon.color;
                newColor.a -= popUp.fadeSpeed * Time.deltaTime;
                popUp.icon.color = newColor;

                if (newColor.a <= 0)
                {
                    DestroyItemIconPopUp(popUp);
                }
            }
        }
    }

    public void SpawnItemIconPopup(Sprite _icon ,bool worldSpace, Vector3 position)
    {
        if (_icon != null)
        {
            ActiveItemIconPopUp newItemIconPopup = new ActiveItemIconPopUp();
            if (newItemIconPopup != null)
            {
                newItemIconPopup.riseSpeed = riseSpeed;
                newItemIconPopup.fadeSpeed = fadeSpeed;
            }

            Canvas canvas;
            if (worldSpace)
                canvas = worldspaceCanvas;
            else
                canvas = overlayCanvas;

            GameObject newPopup = Instantiate(itemIconPrefab, position, Quaternion.Euler(viewportTransform.forward), canvas.transform);

            newItemIconPopup.transform = newPopup.transform;
            newItemIconPopup.icon = newPopup.GetComponent<Image>();
            newItemIconPopup.icon.sprite = _icon;

            activeItemIconPopUps.Add(newItemIconPopup);
        }
    }

    void DestroyItemIconPopUp(ActiveItemIconPopUp itemIconPopUp)
    {
        Destroy(itemIconPopUp.transform.gameObject);

        if (activeItemIconPopUps.Contains(itemIconPopUp))
            activeItemIconPopUps.Remove(itemIconPopUp);
    }
}

public class ActiveTextPopUp
{
    public Transform transform;
    public TextMeshProUGUI text;
    public Color color;
    public bool fadeOnShrink;
    public bool growing;
    public float growRate;
    public float shrinkRate;
    public float fadeRate;
    public float maxSize;
    public float changeX;
    public float changeY;
}

[System.Serializable]
public class TextPopUp
{
    public GameObject prefab;
    public Color color;
    public bool fadeOnShrink;
    public float startSize;
    public float growToSize;
    public float growRate;
    public float shinkRate;
    public float fadeRate;
    public float minRiseSpeed;
    public float maxRiseSpeed;
    public float maxSidewardDeviation;
}

[System.Serializable]
public class ActiveItemIconPopUp
{
    public Transform transform;
    public Image icon;
    public float fadeSpeed;
    public float riseSpeed;
}
