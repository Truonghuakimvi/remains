using System.Collections;
using TMPro;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public Camera mainCam;
    public GameObject ui;
    public GameObject rangeUI;

    public GameObject detailUI;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI attackText;

    public Vector3 baselineTurretPos = new Vector3(0f, 0f, -8.08f);
    public bool smoothCamera = true;
    public float smoothTime = 0.18f;

    private Vector3 baselineCameraPos;
    private Coroutine camMoveRoutine;
    private Node target;

    private Turret selectedTurret;
    private Healer selectedHealer;
    public RectTransform hpBarForeground;
    private float hpBarOriginalWidth;
    private CanvasGroup detailCanvasGroup;
    private Coroutine fadeRoutine;
    public float fadeDuration = 0.25f;

    void Awake()
    {
        mainCam = Camera.main;
        baselineCameraPos = mainCam.transform.position;
        hpBarOriginalWidth = hpBarForeground.sizeDelta.x;

        detailCanvasGroup = detailUI.GetComponent<CanvasGroup>();
        detailCanvasGroup.alpha = 0f;
        detailUI.SetActive(false);
    }

    void Update()
    {
        if (GameManager.gameEnded)
        {
            Hide();
            return;
        }

        if (ui.activeSelf && detailUI && detailUI.activeSelf)
        {
            UpdateDetailUIRuntime();
        }
    }

    public void SetTarget(Node node)
    {
        target = node;
        transform.position = target.transform.position;

        selectedTurret = null;
        selectedHealer = null;

        if (target.turret)
        {
            selectedTurret = target.turret.GetComponent<Turret>();
            selectedHealer = selectedTurret ? null : target.turret.GetComponent<Healer>();

            var rangeUISource = selectedTurret ? selectedTurret.rangeUI : selectedHealer?.rangeUI;
            if (rangeUISource)
                SetRangeUI(rangeUISource);
        }

        AlignCameraToTurret(target.transform);
        ui.SetActive(true);
        rangeUI.SetActive(true);
        Time.timeScale = 0.25f;

        SetupDetailUI();
    }

    private void SetupDetailUI()
    {
        if (!detailUI)
            return;

        if (selectedTurret != null || selectedHealer != null)
        {
            if (titleText)
                titleText.text = "Turret";
            UpdateDetailUIRuntime();
            FadeDetailUI(true);
        }
        else
        {
            FadeDetailUI(false);
        }
    }

    private void UpdateDetailUIRuntime()
    {
        if (!hpText || !attackText)
            return;

        if (selectedTurret == null && selectedHealer == null)
            return;

        int hp = Mathf.CeilToInt(selectedTurret ? selectedTurret.health : selectedHealer.health);
        int maxHp = Mathf.CeilToInt(
            selectedTurret ? selectedTurret.maxHealth : selectedHealer.maxHealth
        );

        hpText.text = $"{hp}/{maxHp}";
        attackText.text = $"ATK {(selectedTurret ? selectedTurret.damage : selectedHealer.damage)}";

        if (hpBarForeground != null && maxHp > 0)
        {
            float fill = Mathf.Clamp01((float)hp / maxHp);
            Vector2 size = hpBarForeground.sizeDelta;
            size.x = Mathf.Lerp(size.x, hpBarOriginalWidth * fill, Time.unscaledDeltaTime * 10f);
            hpBarForeground.sizeDelta = size;
        }
    }

    private void SetRangeUI(GameObject newRangeUIObject)
    {
        RectTransform newRangeUI = newRangeUIObject.GetComponent<RectTransform>();
        rangeUI.GetComponent<RectTransform>().sizeDelta = newRangeUI.sizeDelta;
        rangeUI.GetComponent<RectTransform>().anchoredPosition = newRangeUI.anchoredPosition;
    }

    public void Hide()
    {
        ui.SetActive(false);
        rangeUI.SetActive(false);

        FadeDetailUI(false);

        Time.timeScale = PlayerStats.gameSpeed;

        AlignCameraToPosition(baselineCameraPos);

        selectedTurret = null;
        selectedHealer = null;
        target = null;
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }

    private Vector3 GetCameraPositionForTurret(Vector3 turretPos)
    {
        float xCam = baselineCameraPos.x + (turretPos.x - baselineTurretPos.x);
        float zCam = baselineCameraPos.z + (turretPos.z - baselineTurretPos.z);
        float yCam = baselineCameraPos.y;

        return new Vector3(xCam, yCam, zCam);
    }

    private void AlignCameraToTurret(Transform turret)
    {
        if (!mainCam || !turret)
            return;
        AlignCameraToPosition(GetCameraPositionForTurret(turret.position));
    }

    private void AlignCameraToPosition(Vector3 dest)
    {
        if (camMoveRoutine != null)
            StopCoroutine(camMoveRoutine);
        camMoveRoutine = StartCoroutine(SmoothMoveCamera(dest));
    }

    private IEnumerator SmoothMoveCamera(Vector3 dest)
    {
        var camT = mainCam.transform;

        while ((camT.position - dest).sqrMagnitude > 0.0001f)
        {
            float k = 1f - Mathf.Exp(-Time.unscaledDeltaTime / Mathf.Max(0.0001f, smoothTime));
            camT.position = Vector3.Lerp(camT.position, dest, k);
            yield return null;
        }

        camT.position = dest;
        camMoveRoutine = null;
    }

    private void FadeDetailUI(bool show)
    {
        if (detailCanvasGroup == null)
            return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        if (show)
        {
            detailUI.SetActive(true);
            fadeRoutine = StartCoroutine(
                FadeCanvasGroup(detailCanvasGroup, detailCanvasGroup.alpha, 1f)
            );
        }
        else
        {
            fadeRoutine = StartCoroutine(
                FadeCanvasGroup(
                    detailCanvasGroup,
                    detailCanvasGroup.alpha,
                    0f,
                    () =>
                    {
                        detailUI.SetActive(false);
                    }
                )
            );
        }
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup cg,
        float from,
        float to,
        System.Action onComplete = null
    )
    {
        float elapsed = 0f;
        float duration = fadeDuration;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        cg.alpha = to;
        onComplete?.Invoke();
        fadeRoutine = null;
    }
}
