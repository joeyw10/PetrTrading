
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StickerSpawnManager : MonoBehaviour
{
    [Header("Sticker Images")]
    public Sprite beachSticker;
    public Sprite nasaSticker;
    public Sprite easterBunnySticker;
    public GameObject catchButton;
public GameObject ignoreButton;

    [Header("Map Settings")]
    public RectTransform mapTransform;

    [Header("Spawn Settings")]
    public float stickerSize = 500f;
    public float minimumSpawnInterval = 10f;
    public float maximumSpawnInterval = 30f;

    private void Start()
    {
        Debug.Log("Sticker Spawn Manager is ready!");

        // Spawn one sticker immediately.
        SpawnSticker();

        // Keep spawning stickers at random intervals.
        StartCoroutine(SpawnStickerRoutine());
    }

    private IEnumerator SpawnStickerRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(
                minimumSpawnInterval,
                maximumSpawnInterval
            );

            Debug.Log("Next sticker in " + waitTime + " seconds.");

            yield return new WaitForSeconds(waitTime);

            SpawnSticker();
        }
    }

    private void SpawnSticker()
    {
        if (mapTransform == null)
        {
            Debug.LogError("Please assign the Map Transform!");
            return;
        }

        // Randomly choose one of the three stickers.
        int randomNumber = Random.Range(0, 3);

        Sprite chosenSticker;

        if (randomNumber == 0)
        {
            chosenSticker = beachSticker;
        }
        else if (randomNumber == 1)
        {
            chosenSticker = nasaSticker;
        }
        else
        {
            chosenSticker = easterBunnySticker;
        }

        if (chosenSticker == null)
        {
            Debug.LogError("A sticker image is missing!");
            return;
        }

        // Create the sticker on the map.
        GameObject stickerObject =
            new GameObject("SpawnedPetrSticker");

        stickerObject.transform.SetParent(mapTransform, false);

        Image stickerImage = stickerObject.AddComponent<Image>();
        stickerImage.sprite = chosenSticker;
        stickerImage.preserveAspect = true;
        stickerImage.raycastTarget = true;

        PetrStickerClick clickScript =
    stickerObject.AddComponent<PetrStickerClick>();

clickScript.catchButton = catchButton;
clickScript.ignoreButton = ignoreButton;

        RectTransform stickerRect =
            stickerObject.GetComponent<RectTransform>();

        stickerRect.sizeDelta =
            new Vector2(stickerSize, stickerSize);

        // Choose a random position within the visible viewport.
        RectTransform viewport =
            mapTransform.parent.GetComponent<RectTransform>();

        if (viewport != null)
        {
            float halfWidth = viewport.rect.width / 2f;
            float halfHeight = viewport.rect.height / 2f;

            float randomX = Random.Range(-halfWidth, halfWidth);
            float randomY = Random.Range(-halfHeight, halfHeight);

            stickerRect.anchoredPosition =
                new Vector2(randomX, randomY);
        }
        else
        {
            stickerRect.anchoredPosition = Vector2.zero;
        }

        Debug.Log("A Petr sticker appeared!");

        // Each sticker disappears independently after 1–2 minutes.
        float disappearTime = Random.Range(60f, 120f);

        Destroy(stickerObject, disappearTime);

        Debug.Log(
            "Sticker will disappear in " +
            Mathf.RoundToInt(disappearTime) + " seconds."
        );
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
