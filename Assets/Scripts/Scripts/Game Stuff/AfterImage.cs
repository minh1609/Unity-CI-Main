using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float afterImageDelay;
    private float afterImageDelaySeconds;
    public GameObject afterImage;
    public bool makeImages = false;

    // Start is called before the first frame update
    void Start()
    {
        afterImageDelaySeconds = afterImageDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeImages)
        {
            if (afterImageDelaySeconds > 0)
            {
                afterImageDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentImage = Instantiate(afterImage, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentImage.GetComponent<SpriteRenderer>().sprite = currentSprite;
                afterImageDelaySeconds = afterImageDelay;
                Destroy(currentImage, 1f);
            }
        }
    }
}
