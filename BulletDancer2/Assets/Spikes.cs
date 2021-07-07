using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] SpriteRenderer sr;
    private float time = 0;
    private int currentSprite = 0;
    private bool isAnimationForeward = true;
    private int spritesNumber;
    private bool isSpikesReadyToGo = false;
    private void Start()
    {
        spritesNumber = sprites.Count;
    }
    void Update()
    {
        time += Time.deltaTime;

        if (time > 1 && !isSpikesReadyToGo)
        {
            isSpikesReadyToGo = true;
        }
        else if (time > 0.1f && isSpikesReadyToGo)
        {
            time = 0;
            if (isAnimationForeward && currentSprite < spritesNumber - 1)
            {
                currentSprite++;
            }
            else if (isAnimationForeward && currentSprite >= spritesNumber - 1)
            {
                isAnimationForeward = false;
                currentSprite--;
            }
            else if (!isAnimationForeward && currentSprite > 0)
            {
                currentSprite--;
            }
            else
            {
                isAnimationForeward = true;
                isSpikesReadyToGo = false;
            }

            if (isSpikesReadyToGo)
                sr.sprite = sprites[currentSprite];
        }
    }
}
