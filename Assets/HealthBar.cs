using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private Player player;
    private float maxRotZ;
    private float minRotZ;
    private float totalRange;
    private float currPercentage = 100;
    private float maxHealth;

    [SerializeField] private float shakeAmount = 2.0f;
    [SerializeField] private float shakeSpeed = 10.0f;
    private float baseRotation;
    private float currentTime;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.getPlayer();
        rectTransform = GetComponent<RectTransform>();
        baseRotation = transform.eulerAngles.z;
        maxRotZ = rectTransform.eulerAngles.z;
        minRotZ = 360-maxRotZ;
        totalRange = (360 - maxRotZ) + minRotZ;
        maxHealth = player.GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        currPercentage = player.GetHealth() / maxHealth;
        //Debug.Log("Current health: " + currPercentage);
        Debug.Log("MaxRot: " + maxRotZ);
        Debug.Log("MinRot: " + minRotZ);
        Debug.Log("TotalRange: " + totalRange);
        //rectTransform.localRotation = Quaternion.Euler(0, 0, maxRotZ);
        float currRange = totalRange * currPercentage;
        float currRotation = minRotZ - currRange;
        Debug.Log("Current range: " + currRange);
        Debug.Log("Current rotation: " + currRotation);
        Debug.Log("Curr Health: " + currPercentage);

        currentTime += Time.deltaTime * shakeSpeed;
        float randomRotation = (Mathf.PerlinNoise(currentTime, 0) * 2 - 1) * shakeAmount;

        rectTransform.localRotation = Quaternion.Euler(0, 0, currRotation + randomRotation);
        baseRotation = currRotation;

    }
}
