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

    void Start()
    {
        if (globalVolume.profile.TryGet<DepthOfField>(out dof))
        {
            // 1. Sauvegarde la valeur par défaut LUE DU VOLUME
            defaultFocusDistance = dof.focusDistance.value;

            // 🔥 DEBUG CRUCIAL : Regardez ce qui est affiché dans la console au lancement
            Debug.Log($"[BlurController] Démarrage: Focus Distance lue: {defaultFocusDistance} m.");

            // 2. On s'assure que la valeur lue n'est pas déjà notre valeur de flou maximal (0.1)
            if (defaultFocusDistance < 1f)
            {
                Debug.LogError($"[BlurController] Erreur: La Focus Distance initiale ({defaultFocusDistance} m) est trop faible! Veuillez régler l'asset Volume Profile sur 10m.");
                // Si la valeur est trop basse, on la force à la valeur non-floue souhaitée
                defaultFocusDistance = 10f;
                dof.focusDistance.value = defaultFocusDistance;
            }
        }
        else
        {
            Debug.LogError("Composant Depth Of Field manquant dans le Volume Profile!");
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
            // Rétablir les valeurs par défaut
            dof.aperture.value = 1f; // (Valeur par défaut ou celle que vous utilisez)
            dof.focalLength.value = 50f; // (Valeur par défaut)
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
