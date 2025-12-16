using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; 

public class PauseBlurController : MonoBehaviour
{

    [Tooltip("Le Volume Global qui contient l'effet Depth of Field")]
    public Volume globalVolume;

    private DepthOfField dof;
    private float defaultFocusDistance;
    private float blurFocusDistance = 0.1f;

    [Header("Réglages Flou")]
    public float blurAperture = 20f;
    public float blurFocalLength = 80f;

    public float transitionDuration = 0.1f;
    private Coroutine blurCoroutine;

    void Awake()
    {
        if (globalVolume.profile.TryGet<DepthOfField>(out dof))
        {
            defaultFocusDistance = dof.focusDistance.value;

            //Debug.Log($"[BlurController] Démarrage: Focus Distance lue: {defaultFocusDistance} m.");

            if (defaultFocusDistance < 1f)
            {
                //Debug.LogError($"[BlurController] Erreur: La Focus Distance initiale ({defaultFocusDistance} m) est trop faible! Veuillez régler l'asset Volume Profile sur 10m.");
                defaultFocusDistance = 10f;
                dof.focusDistance.value = defaultFocusDistance;
            }
        }
        else
        {
            //Debug.LogError("Composant Depth Of Field manquant dans le Volume Profile!");
            enabled = false;
        }
    }

    public void SetPauseBlur(bool isPaused)
    {
        if (blurCoroutine != null)
        {
            StopCoroutine(blurCoroutine);
        }

        if (isPaused)
        {
            dof.aperture.value = blurAperture;
            dof.focalLength.value = blurFocalLength;
        }
        else
        {

            dof.aperture.value = 1f; 
            dof.focalLength.value = 50f; 
        }

        float targetDistance = isPaused ? blurFocusDistance : defaultFocusDistance;
        blurCoroutine = StartCoroutine(TransitionFocusDistance(targetDistance));
    }

    private System.Collections.IEnumerator TransitionFocusDistance(float targetDistance)
    {
        float startDistance = dof.focusDistance.value;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / transitionDuration;

            dof.focusDistance.value = Mathf.Lerp(startDistance, targetDistance, t);

            yield return null;
        }

        dof.focusDistance.value = targetDistance;
        blurCoroutine = null;
    }
}
