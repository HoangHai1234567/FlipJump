using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIPopupBuilder
{
    [MenuItem("Tools/Build UI Popups")]
    public static void BuildAll()
    {
        ConfigureSprites();
        BuildPopupWin();
        BuildPopupLose();
        BuildPopupPause();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[UIPopupBuilder] Done! PopupWin, PopupLose, and PopupPause prefabs created in Assets/Prefabs/UI/");
    }

    [MenuItem("Tools/Setup Game Scene")]
    public static void SetupScene()
    {
        // 1. EventSystem
        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
            Undo.RegisterCreatedObjectUndo(es, "Create EventSystem");
        }

        // 2. GameManager
        GameManager gm = Object.FindObjectOfType<GameManager>();
        if (gm == null)
        {
            GameObject gmGo = new GameObject("GameManager");
            gm = gmGo.AddComponent<GameManager>();
            Undo.RegisterCreatedObjectUndo(gmGo, "Create GameManager");
        }

        // Assign popup prefabs
        GameObject winPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/PopupWin.prefab");
        GameObject losePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/PopupLose.prefab");
        GameObject pausePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/PopupPause.prefab");
        if (winPrefab != null) gm.popupWinPrefab = winPrefab;
        if (losePrefab != null) gm.popupLosePrefab = losePrefab;
        if (pausePrefab != null) gm.popupPausePrefab = pausePrefab;

        // Assign LevelCollection
        LevelCollection lc = AssetDatabase.LoadAssetAtPath<LevelCollection>("Assets/Data/LevelCollection.asset");
        if (lc != null) gm.levelCollection = lc;

        // LevelLoader
        LevelLoader ll = Object.FindObjectOfType<LevelLoader>();
        if (ll == null)
        {
            GameObject llGo = new GameObject("LevelLoader");
            ll = llGo.AddComponent<LevelLoader>();

            // Level container — use existing root "Level" or create one
            GameObject levelContainer = GameObject.Find("Level");
            if (levelContainer == null)
            {
                levelContainer = new GameObject("Level");
                Undo.RegisterCreatedObjectUndo(levelContainer, "Create Level Container");
            }
            ll.levelContainer = levelContainer.transform;
            ll.groundContainer = levelContainer.transform;

            // Ground layer
            ll.groundLayer = LayerMask.GetMask("Default");

            // Player transform
            ForcePoint fp = Object.FindObjectOfType<ForcePoint>();
            if (fp != null)
                ll.playerTransform = fp.transform;

            // Prefab registry
            ll.prefabRegistry = new PrefabEntry[]
            {
                LoadPrefabEntry("Square", "Assets/Prefabs/Square.prefab"),
                LoadPrefabEntry("PushForce", "Assets/Prefabs/PushForce.prefab"),
                LoadPrefabEntry("WinZone", "Assets/Prefabs/Demo Prefab/Obstacles/WinZone.prefab"),
                LoadPrefabEntry("wall", "Assets/Prefabs/Demo Prefab/BackGround/wall.prefab"),
                LoadPrefabEntry("BarStool", "Assets/Prefabs/Demo Prefab/Obstacles/BarStool.prefab"),
                LoadPrefabEntry("Block", "Assets/Prefabs/Demo Prefab/Obstacles/Block.prefab"),
                LoadPrefabEntry("Fridge", "Assets/Prefabs/Demo Prefab/Obstacles/Fridge.prefab"),
                LoadPrefabEntry("Microwave", "Assets/Prefabs/Demo Prefab/Obstacles/Microwave.prefab"),
                LoadPrefabEntry("Table", "Assets/Prefabs/Demo Prefab/Obstacles/Table.prefab"),
                LoadPrefabEntry("Platform", "Assets/Prefabs/Demo Prefab/Platforms/Platform.prefab"),
            };

            Undo.RegisterCreatedObjectUndo(llGo, "Create LevelLoader");
        }
        gm.levelLoader = ll;

        EditorUtility.SetDirty(gm);

        // 3. HUD Canvas
        UIIngame existingHud = Object.FindObjectOfType<UIIngame>();
        if (existingHud == null)
        {
            GameObject hudRoot = new GameObject("HUD");
            Canvas canvas = hudRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
            CanvasScaler scaler = hudRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            hudRoot.AddComponent<GraphicRaycaster>();

            // HUD Container
            GameObject container = new GameObject("HudContainer");
            container.transform.SetParent(hudRoot.transform, false);
            RectTransform containerRt = container.AddComponent<RectTransform>();
            containerRt.anchorMin = Vector2.zero;
            containerRt.anchorMax = Vector2.one;
            containerRt.sizeDelta = Vector2.zero;

            // Level Text - top center
            GameObject textGo = new GameObject("LevelText");
            textGo.transform.SetParent(container.transform, false);
            RectTransform textRt = textGo.AddComponent<RectTransform>();
            textRt.anchorMin = new Vector2(0.5f, 1f);
            textRt.anchorMax = new Vector2(0.5f, 1f);
            textRt.pivot = new Vector2(0.5f, 1f);
            textRt.sizeDelta = new Vector2(400, 60);
            textRt.anchoredPosition = new Vector2(0, -20);
            TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
            tmp.text = "Level 1";
            tmp.fontSize = 40;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;

            // Restart button - top right
            GameObject btnGo = new GameObject("RestartButton");
            btnGo.transform.SetParent(container.transform, false);
            RectTransform btnRt = btnGo.AddComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(1f, 1f);
            btnRt.anchorMax = new Vector2(1f, 1f);
            btnRt.pivot = new Vector2(1f, 1f);
            btnRt.sizeDelta = new Vector2(80, 80);
            btnRt.anchoredPosition = new Vector2(-20, -20);
            Image btnImg = btnGo.AddComponent<Image>();
            Sprite restartSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/button_restart.png");
            if (restartSprite != null) btnImg.sprite = restartSprite;
            btnImg.preserveAspect = true;
            Button restartBtn = btnGo.AddComponent<Button>();
            restartBtn.targetGraphic = btnImg;
            btnGo.AddComponent<ButtonPressEffect>();

            // Pause button - top left
            GameObject pauseBtnGo = new GameObject("PauseButton");
            pauseBtnGo.transform.SetParent(container.transform, false);
            RectTransform pauseBtnRt = pauseBtnGo.AddComponent<RectTransform>();
            pauseBtnRt.anchorMin = new Vector2(0f, 1f);
            pauseBtnRt.anchorMax = new Vector2(0f, 1f);
            pauseBtnRt.pivot = new Vector2(0f, 1f);
            pauseBtnRt.sizeDelta = new Vector2(80, 80);
            pauseBtnRt.anchoredPosition = new Vector2(20, -20);
            Image pauseBtnImg = pauseBtnGo.AddComponent<Image>();
            Sprite pauseSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/button_pause.png");
            if (pauseSprite != null) pauseBtnImg.sprite = pauseSprite;
            pauseBtnImg.preserveAspect = true;
            Button pauseBtn = pauseBtnGo.AddComponent<Button>();
            pauseBtn.targetGraphic = pauseBtnImg;
            pauseBtnGo.AddComponent<ButtonPressEffect>();

            // UIIngame script
            UIIngame hud = hudRoot.AddComponent<UIIngame>();
            hud.levelText = tmp;
            hud.restartButton = restartBtn;
            hud.pauseButton = pauseBtn;
            hud.hudContainer = container;

            Undo.RegisterCreatedObjectUndo(hudRoot, "Create HUD");
        }

        Debug.Log("[UIPopupBuilder] Scene setup complete! GameManager, EventSystem, and HUD created.");
    }

    private static void ConfigureSprites()
    {
        string[] paths = {
            "Assets/Art/UI/ribbon_victory.png",
            "Assets/Art/UI/ribbon_defeat.png",
            "Assets/Art/UI/button_rectangle_blue.png",
            "Assets/Art/UI/button_rectangle_green.png",
            "Assets/Art/UI/button_restart.png",
            "Assets/Art/UI/button_pause.png",
            "Assets/Art/UI/iconHome.png",
            "Assets/Art/UI/general_frame_2.png",
            "Assets/Art/UI/title_frame.png"
        };

        foreach (string path in paths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
            }
        }
    }

    private static void BuildPopupWin()
    {
        // Root: Canvas
        GameObject root = CreatePopupCanvas("PopupWin");

        // Background overlay
        CanvasGroup bgGroup = CreateOverlay(root);

        // Panel
        RectTransform panel = CreatePanel(root);

        // Ribbon
        CreateRibbon(panel.gameObject, "Assets/Art/UI/ribbon_victory.png");

        // Buttons container
        RectTransform btnContainer = CreateButtonContainer(panel.gameObject);

        // Buttons
        Button btnNext = CreateButton(btnContainer.gameObject, "ButtonNext", "NEXT LEVEL", "Assets/Art/UI/button_rectangle_green.png");
        Button btnReplay = CreateButton(btnContainer.gameObject, "ButtonReplay", "REPLAY", "Assets/Art/UI/button_rectangle_blue.png");
        Button btnHome = CreateButton(btnContainer.gameObject, "ButtonHome", "HOME", "Assets/Art/UI/button_rectangle_blue.png");

        // Add PopupWin script
        PopupWin popup = root.AddComponent<PopupWin>();
        popup.backgroundOverlay = bgGroup;
        popup.panel = panel;
        popup.buttonsContainer = btnContainer;
        popup.buttonNext = btnNext;
        popup.buttonReplay = btnReplay;
        popup.buttonHome = btnHome;

        // Save as prefab
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/UI"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "UI");

        // Disable before saving so OnEnable doesn't fire on load
        root.SetActive(false);
        PrefabUtility.SaveAsPrefabAsset(root, "Assets/Prefabs/UI/PopupWin.prefab");
        Object.DestroyImmediate(root);
    }

    private static void BuildPopupLose()
    {
        GameObject root = CreatePopupCanvas("PopupLose");

        CanvasGroup bgGroup = CreateOverlay(root);
        RectTransform panel = CreatePanel(root);
        CreateRibbon(panel.gameObject, "Assets/Art/UI/ribbon_defeat.png");
        RectTransform btnContainer = CreateButtonContainer(panel.gameObject);

        Button btnReplay = CreateButton(btnContainer.gameObject, "ButtonReplay", "REPLAY", "Assets/Art/UI/button_rectangle_green.png");
        Button btnHome = CreateButton(btnContainer.gameObject, "ButtonHome", "HOME", "Assets/Art/UI/button_rectangle_blue.png");

        PopupLose popup = root.AddComponent<PopupLose>();
        popup.backgroundOverlay = bgGroup;
        popup.panel = panel;
        popup.buttonsContainer = btnContainer;
        popup.buttonReplay = btnReplay;
        popup.buttonHome = btnHome;

        root.SetActive(false);
        PrefabUtility.SaveAsPrefabAsset(root, "Assets/Prefabs/UI/PopupLose.prefab");
        Object.DestroyImmediate(root);
    }

    private static void BuildPopupPause()
    {
        GameObject root = CreatePopupCanvas("PopupPause");

        CanvasGroup bgGroup = CreateOverlay(root);
        RectTransform panel = CreatePanel(root);

        // Title text instead of ribbon
        GameObject titleGo = new GameObject("TitleText");
        titleGo.transform.SetParent(panel.gameObject.transform, false);
        RectTransform titleRt = titleGo.AddComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0.5f, 1f);
        titleRt.anchorMax = new Vector2(0.5f, 1f);
        titleRt.pivot = new Vector2(0.5f, 1f);
        titleRt.sizeDelta = new Vector2(600, 120);
        titleRt.anchoredPosition = new Vector2(0, -30);
        TextMeshProUGUI titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
        titleTmp.text = "PAUSED";
        titleTmp.fontSize = 64;
        titleTmp.alignment = TextAlignmentOptions.Center;
        titleTmp.color = Color.white;

        RectTransform btnContainer = CreateButtonContainer(panel.gameObject);

        Button btnResume = CreateButton(btnContainer.gameObject, "ButtonResume", "RESUME", "Assets/Art/UI/button_rectangle_green.png");
        Button btnHome = CreateButton(btnContainer.gameObject, "ButtonHome", "HOME", "Assets/Art/UI/button_rectangle_blue.png");

        PopupPause popup = root.AddComponent<PopupPause>();
        popup.backgroundOverlay = bgGroup;
        popup.panel = panel;
        popup.buttonsContainer = btnContainer;
        popup.buttonResume = btnResume;
        popup.buttonHome = btnHome;

        root.SetActive(false);
        PrefabUtility.SaveAsPrefabAsset(root, "Assets/Prefabs/UI/PopupPause.prefab");
        Object.DestroyImmediate(root);
    }

    private static GameObject CreatePopupCanvas(string name)
    {
        GameObject go = new GameObject(name);
        Canvas canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        go.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        go.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080, 1920);
        go.AddComponent<GraphicRaycaster>();
        return go;
    }

    private static CanvasGroup CreateOverlay(GameObject parent)
    {
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(parent.transform, false);
        RectTransform rt = bg.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;

        Image img = bg.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.6f);

        CanvasGroup cg = bg.AddComponent<CanvasGroup>();
        return cg;
    }

    private static RectTransform CreatePanel(GameObject parent)
    {
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(parent.transform, false);
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(800, 900);
        rt.anchoredPosition = Vector2.zero;

        Image img = panel.AddComponent<Image>();
        img.color = new Color(1f, 1f, 1f, 0f); // transparent background
        return rt;
    }

    private static void CreateRibbon(GameObject parent, string spritePath)
    {
        GameObject ribbon = new GameObject("Ribbon");
        ribbon.transform.SetParent(parent.transform, false);
        RectTransform rt = ribbon.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.sizeDelta = new Vector2(700, 200);
        rt.anchoredPosition = new Vector2(0, 50);

        Image img = ribbon.AddComponent<Image>();
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        if (sprite != null)
        {
            img.sprite = sprite;
            img.preserveAspect = true;
        }
    }

    private static RectTransform CreateButtonContainer(GameObject parent)
    {
        GameObject container = new GameObject("Buttons");
        container.transform.SetParent(parent.transform, false);
        RectTransform rt = container.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.sizeDelta = new Vector2(600, 400);
        rt.anchoredPosition = new Vector2(0, 50);

        VerticalLayoutGroup vlg = container.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 20;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        return rt;
    }

    private static PrefabEntry LoadPrefabEntry(string name, string path)
    {
        return new PrefabEntry
        {
            name = name,
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path)
        };
    }

    private static Button CreateButton(GameObject parent, string name, string label, string spritePath)
    {
        GameObject btnGo = new GameObject(name);
        btnGo.transform.SetParent(parent.transform, false);
        RectTransform rt = btnGo.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(400, 80);

        Image img = btnGo.AddComponent<Image>();
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        if (sprite != null)
            img.sprite = sprite;
        img.type = Image.Type.Sliced;

        Button btn = btnGo.AddComponent<Button>();
        btn.targetGraphic = img;

        btnGo.AddComponent<ButtonPressEffect>();

        // Label text
        GameObject textGo = new GameObject("Label");
        textGo.transform.SetParent(btnGo.transform, false);
        RectTransform textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;

        TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 36;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return btn;
    }
}
