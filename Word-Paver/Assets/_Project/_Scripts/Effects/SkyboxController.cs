using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [Header("Skybox")]
    [SerializeField] private Material _skyboxMat;

    [Header("Colors")]
    [SerializeField] private Color _startColor = new Color(0.2235f, 0.7176f, 1f);
    [SerializeField] private Color _endColor = new Color(0.5255f, 0.2235f, 1f);

    [Header("Ranges")]
    [SerializeField] private float _maxNoiseSpeed = 0.1f;
    [SerializeField] private float _maxEffectIntensity = 0.7f;

    [Header("Cycle")]
    [SerializeField] private float _cycleDistance = 300f;

    private Material _runtimeMat;
    private float _noiseSpeed;

    private static readonly int SkyColorID = Shader.PropertyToID("_SkyColor");
    private static readonly int NoiseSpeedID = Shader.PropertyToID("_NoiseSpeed");
    private static readonly int EffectIntensityID = Shader.PropertyToID("_EffectIntensity");

    private void Awake()
    {
        _runtimeMat = new Material(_skyboxMat);

        RenderSettings.skybox = _runtimeMat;
        _noiseSpeed = _maxNoiseSpeed;

        ResetSky();
    }

    private void Start()
    {
        LevelController.Source.OnRunRestarted += ResetSky;
        GameManager.Source.OnGamePaused += HandlePause;
        GameManager.Source.OnGameUnpaused += HandleUnpause;
    }

    private void OnDestroy()
    {
        LevelController.Source.OnRunRestarted -= ResetSky;
        GameManager.Source.OnGamePaused -= HandlePause;
        GameManager.Source.OnGameUnpaused -= HandleUnpause;
    }

    private void Update()
    {
        if (GameManager.Source.CurrentGameState != GameState.OnPlay) return;

        float distance = GameManager.Source.DistanceTravelled;

        float t = Mathf.PingPong(distance, _cycleDistance) / _cycleDistance;

        UpdateSky(t);
    }

    private void UpdateSky(float t)
    {
        Color sky = Color.Lerp(_startColor, _endColor, t);
        float intensity = Mathf.Lerp(0f, _maxEffectIntensity, t);

        _runtimeMat.SetColor(SkyColorID, sky);
        _runtimeMat.SetFloat(EffectIntensityID, intensity);
    }

    private void ResetSky()
    {
        _runtimeMat.SetColor(SkyColorID, _startColor);
        _runtimeMat.SetFloat(NoiseSpeedID, _noiseSpeed);
        _runtimeMat.SetFloat(EffectIntensityID, 0f);
    }

    private void HandlePause()
    {
        _runtimeMat.SetFloat(NoiseSpeedID, 0f);
    }

    private void HandleUnpause()
    {
        _runtimeMat.SetFloat(NoiseSpeedID, _noiseSpeed);
    }
}
