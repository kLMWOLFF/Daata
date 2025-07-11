using UnityEngine;

public class LowGravityErosion : MonoBehaviour
{
    public GameObject cloudObject;
    public float checkDistance = 35f;
    public float countdownTime = 15f;
    public AudioSource moveSound;

    private float timer;
    private bool countdownStarted = false;
    private bool hasDisappeared = false;

    private EnvironmentTracker envTracker;
    private BlinkEnvironment blink;

    void Start()
    {
        timer = countdownTime;
        envTracker = FindObjectOfType<EnvironmentTracker>();
        blink = GetComponent<BlinkEnvironment>();

        if (envTracker == null)
            Debug.LogError("EnvironmentTracker not found.");
    }

    void Update()
    {
        if (cloudObject == null || envTracker == null) return;

        GameObject[] activeEnvs = envTracker.GetAllActiveEnvironments();
        if (activeEnvs.Length == 0)
        {
            ResetTimer();
            return;
        }

        float distance = Vector3.Distance(cloudObject.transform.position, transform.position);
        if (distance > checkDistance && !hasDisappeared)
        {
            if (!countdownStarted)
            {
                countdownStarted = true;
                if (blink != null) blink.isBlinkingAllowed = true;
                if (moveSound != null && !moveSound.isPlaying) moveSound.Play();
            }

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                gameObject.SetActive(false);
                hasDisappeared = true;
                if (blink != null) blink.isBlinkingAllowed = false;
                if (moveSound != null && moveSound.isPlaying) moveSound.Stop();
            }
        }
        else
        {
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = countdownTime;
        countdownStarted = false;
        hasDisappeared = false;
        if (blink != null) blink.isBlinkingAllowed = false;
        if (moveSound != null && moveSound.isPlaying) moveSound.Stop();
    }
}
