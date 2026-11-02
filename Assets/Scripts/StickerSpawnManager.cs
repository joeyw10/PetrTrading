
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StickerSpawnManager : MonoBehaviour
{
    [Header("Sticker Images")]
    public Sprite beachSticker;
    public Sprite nasaSticker;
    public Sprite easterBunnySticker;

    [Header("Buttons and Popup")]
    public GameObject catchButton;
    public GameObject ignoreButton;
    public GameObject stickerDetectedPanel;
    public TMPro.TMP_Text stickerDetectedText;
    public UnityEngine.UI.Image detectedStickerImage;

    [Header("Player Marker")]
    public Transform playerMarker;

    [Header("Map Settings")]
    public RectTransform mapTransform;

    [Header("Spawn Settings")]
    public float stickerSize = 180f;
    public float minimumSpawnInterval = 10f;
    public float maximumSpawnInterval = 30f;

    [Header("Sticker Information")]
    public StickerInfo beachStickerInfo;
    public StickerInfo nasaStickerInfo;
    public StickerInfo easterBunnyStickerInfo;

    private void Start()
    {
        Debug.Log("Sticker Spawn Manager is ready!");

        SpawnSticker();
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

        int randomNumber = Random.Range(0, 3);

        Sprite chosenSticker;
        StickerInfo chosenInfo;
        PetrStickerClick.StickerType chosenType;

        if (randomNumber == 0)
        {
            chosenSticker = beachSticker;
            chosenInfo = beachStickerInfo;
            chosenType = PetrStickerClick.StickerType.Common;
        }
        else if (randomNumber == 1)
        {
            chosenSticker = nasaSticker;
            chosenInfo = nasaStickerInfo;
            chosenType = PetrStickerClick.StickerType.Rare;
        }
        else
        {
            chosenSticker = easterBunnySticker;
            chosenInfo = easterBunnyStickerInfo;
            chosenType = PetrStickerClick.StickerType.Legendary;
        }

        if (chosenSticker == null)
        {
            Debug.LogError("A sticker image is missing!");
            return;
        }

        GameObject stickerObject =
            new GameObject("SpawnedPetrSticker");

        stickerObject.transform.SetParent(mapTransform, false);

        Image stickerImage =
            stickerObject.AddComponent<Image>();

        stickerImage.sprite = chosenSticker;
        stickerImage.preserveAspect = true;
        stickerImage.raycastTarget = false;

        PetrStickerClick clickScript =
            stickerObject.AddComponent<PetrStickerClick>();
        clickScript.buttonSound = FindObjectOfType<ButtonSound>();

        clickScript.stickerDetectedPanel =
            stickerDetectedPanel;

        clickScript.stickerDetectedText =
            stickerDetectedText;
        clickScript.detectedStickerImage =
    detectedStickerImage;

        clickScript.catchButton = catchButton;
        clickScript.ignoreButton = ignoreButton;
        clickScript.playerMarker = playerMarker;
        clickScript.stickerType = chosenType;
        clickScript.stickerInfo = chosenInfo;

        RectTransform stickerRect =
            stickerObject.GetComponent<RectTransform>();

        stickerRect.sizeDelta =
            new Vector2(stickerSize, stickerSize);

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

        float disappearTime = Random.Range(60f, 120f);

        Destroy(stickerObject, disappearTime);
        ButtonSound soundManager = FindObjectOfType<ButtonSound>();

if (soundManager != null)
{
    soundManager.PlaySpawnSound();
}

        Debug.Log(
            "Petr sticker spawned. It will disappear in " +
            Mathf.RoundToInt(disappearTime) + " seconds."
        );
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
