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

    private void Start()
    {
        float xPos = worldGeneration._mazeWidth * worldGeneration._prefabSize;
        float zPos = worldGeneration._mazeDepth * worldGeneration._prefabSize;
        exitPos = new Vector2(xPos, zPos);
    }

    void Update()
    {
        float iconAngle = getAngle();
        Debug.Log(iconAngle);
        float playerAngle = player.localEulerAngles.y;
        
        if (iconAngle > 180)
        {
            iconAngle -= 180;
        }

        iconAngle = 90 - iconAngle;

        playerAngle -= iconAngle;
        if (playerAngle > 180)
        {
            playerAngle -= 360;
        }

        //Debug.Log(playerAngle);

        playerAngle = Mathf.Clamp(playerAngle, -21f, 21f);

        //float normalized = (playerAngle + 90f) / 180f;
        //float barWidth = 3060;
        //float newX = (normalized * barWidth) - (barWidth / 2f);

        //compassIcon.rectTransform.anchoredPosition = new Vector2(newX, compassIcon.rectTransform.anchoredPosition.y);



        float getAngle()
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
            Vector2 direction = exitPos - playerPos;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    }
}
