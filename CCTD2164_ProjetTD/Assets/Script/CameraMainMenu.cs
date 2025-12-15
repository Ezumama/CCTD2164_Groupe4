using UnityEngine;
using System.Collections;

public class TitleCameraController : MonoBehaviour
{
    [Header("Camera Positions")]
    public Transform[] cameraPoints;

    [Header("Timing")]
    public float changeInterval = 20f;
    public float fadeDuration = 1f;

    [Header("Smooth Motion")]
    public Vector2 positionAmplitude = new Vector2(0.15f, 0.1f);
    public Vector2 rotationAmplitude = new Vector2(1.5f, 2f);
    public float motionSpeed = 0.2f;

    [Header("Fade")]
    public CanvasGroup fadeCanvas;

    private Transform currentTarget;
    private Vector3 basePosition;
    private Quaternion baseRotation;

    private float motionTime;
    private int lastIndex = -1;

    void Start()
    {
        ChooseRandomPoint(true);
        StartCoroutine(CameraRoutine());
    }

    void Update()
    {
        if (currentTarget == null) return;

        motionTime += Time.deltaTime * motionSpeed;

        //Mouvement de position (un arc très léger)
        Vector3 posOffset = new Vector3(
            Mathf.Sin(motionTime) * positionAmplitude.x,
            Mathf.Cos(motionTime * 0.8f) * positionAmplitude.y,
            0f
        );

        // Mouvement ROTATION (clé du rendu cinématique)
        Quaternion rotOffset = Quaternion.Euler(
            Mathf.Sin(motionTime * 0.7f) * rotationAmplitude.x,
            Mathf.Cos(motionTime) * rotationAmplitude.y,
            0f
        );

        transform.position = basePosition + posOffset;
        transform.rotation = baseRotation * rotOffset;
    }

    void ChooseRandomPoint(bool instant = false)
    {
        if (cameraPoints.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, cameraPoints.Length);
        }
        while (newIndex == lastIndex && cameraPoints.Length > 1);

        lastIndex = newIndex;
        currentTarget = cameraPoints[newIndex];

        basePosition = currentTarget.position;
        baseRotation = currentTarget.rotation;

        motionTime = Random.Range(0f, 100f);

        if (instant)
        {
            transform.position = basePosition;
            transform.rotation = baseRotation;
        }
    }

    IEnumerator CameraRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);

            yield return Fade(1f);
            ChooseRandomPoint(true);
            yield return Fade(0f);
        }
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvas.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = targetAlpha;
    }
}



