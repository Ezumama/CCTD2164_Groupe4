using UnityEngine;

public class WaveSong : MonoBehaviour
{
    [Header("Wave Settings")]
    public float minScaleY = 0.85f;
    public float maxScaleY = 1.10f;
    public float waveSpeed = 1f;

    private RectTransform rect;
    private float baseScaleX;
    private float baseScaleZ;
    private float timeOffset;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        baseScaleX = rect.localScale.x;
        baseScaleZ = rect.localScale.z;

        timeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * waveSpeed + timeOffset) + 1f) * 0.5f;
        float scaleY = Mathf.Lerp(minScaleY, maxScaleY, t);

        rect.localScale = new Vector3(baseScaleX, scaleY, baseScaleZ);
    }
}


