using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CompassIcon : MonoBehaviour
{

    [SerializeField] private WorldGenerator worldGeneration;
    Vector2 exitPos;
    public RawImage compassIcon;
    public Transform player;

    public float sensitivity;

    private void Start()
    {
        float xPos = (worldGeneration._mazeWidth - 1) * worldGeneration._prefabSize;
        float zPos = (worldGeneration._mazeDepth - 1) * worldGeneration._prefabSize;
        exitPos = new Vector2(xPos, zPos);
    }

    void Update()
    {
        float iconAngle = getAngle();
        //Debug.Log(iconAngle);
        float playerAngle = player.localEulerAngles.y;

        iconAngle = 90 - iconAngle;

        playerAngle -= iconAngle;
        if (playerAngle > 180)
        {
            playerAngle -= 360;
        }

        playerAngle *= -1;
        playerAngle /= sensitivity;
        
        playerAngle = Mathf.Clamp(playerAngle, -17f, 17f);

        float normalized = (playerAngle + 90f) / 180f;
        float barWidth = compassIcon.rectTransform.rect.width;
        float newX = (normalized * barWidth) - (barWidth / 2f);

        compassIcon.rectTransform.anchoredPosition = new Vector2(newX, compassIcon.rectTransform.anchoredPosition.y);



        float getAngle()
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
            Vector2 direction = exitPos - playerPos;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}
