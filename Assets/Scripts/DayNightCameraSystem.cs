using UnityEngine;

public class DayNightCameraSystem : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Transform sunGroup;
    public Transform moonGroup;
    public Light sunLight;
    public Light moonLight;

    [Header("Configurações do Céu")]
    public Material skyboxMaterial;
    [Range(0, 1)] public float minSkyExposure = 0.05f;
    [Range(0, 2)] public float maxSkyExposure = 1.0f;

    [Header("Cores e Atmosfera")]
    public Gradient sunColor;
    public float nightIntensity = 0.05f; // Diminuí para ficar mais escuro

    [Header("Configurações de Ciclo")]
    [Range(0, 1)] public float timeOfDay;
    public float dayDuration = 60f;

    private void Update()
    {
        timeOfDay += Time.deltaTime / dayDuration;
        if (timeOfDay > 1) timeOfDay = 0;

        UpdateSystem();
    }

    void UpdateSystem()
    {
        // 1. Cálculos de Fase
        float sunPhase = timeOfDay;
        float moonPhase = (timeOfDay + 0.5f) % 1f;

        // 2. Mover corpos celestes
        MoveAndLight(sunGroup, sunLight, sunPhase, true);
        MoveAndLight(moonGroup, moonLight, moonPhase, false);

        // 3. Pegar a altura do sol para controlar o céu (baseado no Z de -140 a 140)
        float t = (sunGroup.localPosition.z - (-140f)) / 280f;
        float heightFactor = Mathf.Max(0, Mathf.Sin(t * Mathf.PI));

        // --- O SEGREDO PARA O CÉU ESCURECER ---
        if (skyboxMaterial != null)
        {
            // Forçamos a exposição do Skybox para baixo
            float exposure = Mathf.Lerp(minSkyExposure, maxSkyExposure, heightFactor);
            skyboxMaterial.SetFloat("_Exposure", exposure);

            // Forçamos o Tint do céu para preto na noite
            Color skyColor = Color.Lerp(Color.black, Color.gray, heightFactor);
            skyboxMaterial.SetColor("_SkyTint", skyColor);

            // AVISAR O UNITY QUE O AMBIENTE MUDOU
            RenderSettings.ambientIntensity = Mathf.Lerp(nightIntensity, 1f, heightFactor);
            DynamicGI.UpdateEnvironment();
        }
    }

    void MoveAndLight(Transform body, Light light, float phase, bool isSun)
    {
        float posX = 80f;
        float posZ = Mathf.Lerp(-140f, 140f, phase);

        // Cálculo da parábola
        float tParabola = (posZ - (-140f)) / 280f;
        float heightFactor = Mathf.Sin(tParabola * Mathf.PI);
        float posY = -10f + (heightFactor * 40f);

        body.localPosition = new Vector3(posX, posY, posZ);
        if (player != null) body.LookAt(player.position);

        if (light != null)
        {
            if (isSun)
            {
                light.color = sunColor.Evaluate(Mathf.Max(0, heightFactor));
                // Se o sol está abaixo de Y=0, a intensidade é ZERO
                light.intensity = body.localPosition.y > 0 ? heightFactor * 1.5f : 0;
            }
            else
            {
                // Lua só brilha se o sol estiver "escondido" (Y < 0)
                float sunY = sunGroup.localPosition.y;
                light.intensity = sunY < 0 ? 0.3f : 0;
            }
        }
    }
}