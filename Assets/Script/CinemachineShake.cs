using UnityEngine;
using Unity.Cinemachine;  // pastikan namespace ini

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    [Tooltip("Drag your Cinemachine Virtual Camera di sini")]
    [SerializeField] private CinemachineCamera cinemachineVirtualCamera;

    private float shakeTimer;

    private void Awake()
    {
        // singleton pattern
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // jika kamu tidak assign via Inspector, coba GetComponent
        if (cinemachineVirtualCamera == null)
            cinemachineVirtualCamera = GetComponent<CinemachineCamera>();

        if (cinemachineVirtualCamera == null)
            Debug.LogError("CinemachineVirtualCamera tidak ditemukan di CinemachineShake!");
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
                StopShake();
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        if (cinemachineVirtualCamera == null) return;

        // dapatkan komponen noise Perlin dari VC
        var perlin = cinemachineVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (perlin == null)
        {
            Debug.LogWarning("Perlin Noise belum ditambahkan pada Virtual Camera!");
            return;
        }

        perlin.AmplitudeGain = intensity;
        shakeTimer = time;
    }
    private void StopShake()
    {
        if (cinemachineVirtualCamera == null) return;

            var perlin = cinemachineVirtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (perlin != null)
            perlin.AmplitudeGain = 0f;
    }
}
