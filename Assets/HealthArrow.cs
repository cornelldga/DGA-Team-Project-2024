using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages the player health UI display.
/// </summary>
public class HealthArrow : MonoBehaviour
{

    private Player player;
    private float maxRotZ;
    private float minRotZ;
    private float totalRange;
    private float currPercentage = 100;
    private float maxHealth;

    [SerializeField] private float shakeAmount = 2.0f;
    [SerializeField] private float shakeSpeed = 10.0f;
    [SerializeField] private Animator lowHealthAnim;

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
        minRotZ = 80;
        totalRange = minRotZ;
        maxHealth = player.GetHealth();
        lowHealthAnim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        currPercentage = 1 - (player.GetHealth() / maxHealth);
        

        //rectTransform.localRotation = Quaternion.Euler(0, 0, maxRotZ);
        float currRange = totalRange * currPercentage;
        
        float currRotation = maxRotZ - currRange;
        //Debug.Log("currRotation " + currRotation);


        currentTime += Time.deltaTime * shakeSpeed;
        float randomRotation = (Mathf.PerlinNoise(currentTime, 0) * 2 - 1) * shakeAmount;

        rectTransform.localRotation = Quaternion.Euler(0, 0, currRotation + randomRotation);
        baseRotation = currRotation;

        // Check if should warn about low player health
        if ((1 - currPercentage) < 0.3)
        {
            lowHealthAnim.enabled = true;
        }
        else
        {
            lowHealthAnim.enabled = false;
        }

    }
}