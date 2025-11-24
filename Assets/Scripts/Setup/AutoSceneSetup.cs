using CardGame.Data;
using CardGame.Game;
using CardGame.Networking;
using CardGame.UI;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Setup
{
    /// <summary>
    /// Automated scene setup script. Attach to an empty GameObject and run SetupScene() from context menu.
    /// This will create all necessary GameObjects and basic UI structure.
    /// You'll still need to manually assign some UI references and adjust positions.
    /// </summary>
    public class AutoSceneSetup : MonoBehaviour
    {
        [ContextMenu("Setup Complete Scene")]
        public void SetupCompleteScene()
        {
            Debug.Log("Starting automated scene setup...");

            // Create core GameObjects
            CreateNetworkManager();
            CreateCardDatabase();
            CreateGameManager();
            CreateNetworkGameManager();
            CreateLocalPlayerManager();
            
            // Create UI
            CreateCanvas();
            CreateLobbyUI();
            CreateGameUI();
            
            // Create prefabs
            CreatePlayerPrefab();
            CreateCardPrefab();

            Debug.Log("Scene setup complete! Please verify all references and adjust UI positions as needed.");
        }

        private void CreateNetworkManager()
        {
            if (GameObject.Find("NetworkManager") != null)
            {
                Debug.Log("NetworkManager already exists, skipping...");
                return;
            }

            GameObject go = new GameObject("NetworkManager");
            CustomNetworkManager networkManager = go.AddComponent<CustomNetworkManager>();
            NetworkManager mirrorNM = go.GetComponent<NetworkManager>();
            
            if (mirrorNM == null)
            {
                mirrorNM = go.AddComponent<NetworkManager>();
            }

            mirrorNM.maxConnections = 2;
            mirrorNM.autoCreatePlayer = true;

            Debug.Log("Created NetworkManager");
        }

        private void CreateCardDatabase()
        {
            if (GameObject.Find("CardDatabase") != null)
            {
                Debug.Log("CardDatabase already exists, skipping...");
                return;
            }

            GameObject go = new GameObject("CardDatabase");
            go.AddComponent<CardDatabase>();
            Debug.Log("Created CardDatabase");
        }

        private void CreateGameManager()
        {
            if (GameObject.Find("GameManager") != null)
            {
                Debug.Log("GameManager already exists, skipping...");
                return;
            }

            GameObject go = new GameObject("GameManager");
            go.AddComponent<GameManager>();
            Debug.Log("Created GameManager");
        }

        private void CreateNetworkGameManager()
        {
            if (GameObject.Find("NetworkGameManager") != null)
            {
                Debug.Log("NetworkGameManager already exists, skipping...");
                return;
            }

            GameObject go = new GameObject("NetworkGameManager");
            go.AddComponent<NetworkGameManager>();
            
            NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
            if (ni == null)
            {
                ni = go.AddComponent<NetworkIdentity>();
            }
            ni.serverOnly = false;

            Debug.Log("Created NetworkGameManager");
        }

        private void CreateLocalPlayerManager()
        {
            if (GameObject.Find("LocalPlayerManager") != null)
            {
                Debug.Log("LocalPlayerManager already exists, skipping...");
                return;
            }

            GameObject go = new GameObject("LocalPlayerManager");
            go.AddComponent<LocalPlayerManager>();
            Debug.Log("Created LocalPlayerManager");
        }

        private void CreateCanvas()
        {
            if (GameObject.Find("GameCanvas") != null)
            {
                Debug.Log("GameCanvas already exists, skipping...");
                return;
            }

            GameObject canvasGO = new GameObject("GameCanvas");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            // Create EventSystem if it doesn't exist
            if (GameObject.Find("EventSystem") == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            Debug.Log("Created Canvas");
        }

        private void CreateLobbyUI()
        {
            GameObject canvas = GameObject.Find("GameCanvas");
            if (canvas == null) return;

            if (GameObject.Find("LobbyUI") != null)
            {
                Debug.Log("LobbyUI already exists, skipping...");
                return;
            }

            GameObject lobbyUI = new GameObject("LobbyUI");
            lobbyUI.transform.SetParent(canvas.transform, false);
            LobbyUI lobbyComponent = lobbyUI.AddComponent<LobbyUI>();

            // Create Host Button
            GameObject hostBtn = CreateButton("HostButton", lobbyUI.transform, new Vector2(100, -50), new Vector2(200, 50));
            hostBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Host";
            lobbyComponent.hostButton = hostBtn.GetComponent<Button>();

            // Create Join Button
            GameObject joinBtn = CreateButton("JoinButton", lobbyUI.transform, new Vector2(100, -120), new Vector2(200, 50));
            joinBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Join";
            lobbyComponent.joinButton = joinBtn.GetComponent<Button>();

            // Create IP Input Field
            GameObject ipInput = CreateInputField("IPInputField", lobbyUI.transform, new Vector2(320, -120), new Vector2(200, 50));
            ipInput.GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "Enter IP (localhost)";
            lobbyComponent.ipInputField = ipInput.GetComponent<TMP_InputField>();

            // Create Status Text
            GameObject statusText = CreateText("StatusText", lobbyUI.transform, new Vector2(0, -50), new Vector2(600, 50));
            statusText.GetComponent<TextMeshProUGUI>().text = "Ready to play";
            statusText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            lobbyComponent.statusText = statusText.GetComponent<TextMeshProUGUI>();

            // Create Player Count Text
            GameObject playerCountText = CreateText("PlayerCountText", lobbyUI.transform, new Vector2(0, -100), new Vector2(300, 50));
            playerCountText.GetComponent<TextMeshProUGUI>().text = "Players: 0/2";
            playerCountText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            lobbyComponent.playerCountText = playerCountText.GetComponent<TextMeshProUGUI>();

            // Create Start Button
            GameObject startBtn = CreateButton("StartButton", lobbyUI.transform, new Vector2(0, -180), new Vector2(200, 50));
            startBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Start Game";
            lobbyComponent.startButton = startBtn.GetComponent<Button>();
            startBtn.GetComponent<Button>().interactable = false;

            Debug.Log("Created LobbyUI");
        }

        private void CreateGameUI()
        {
            GameObject canvas = GameObject.Find("GameCanvas");
            if (canvas == null) return;

            if (GameObject.Find("GameUI") != null)
            {
                Debug.Log("GameUI already exists, skipping...");
                return;
            }

            GameObject gameUI = new GameObject("GameUI");
            gameUI.transform.SetParent(canvas.transform, false);
            gameUI.SetActive(false); // Initially hidden
            GameUI gameComponent = gameUI.AddComponent<GameUI>();

            // Create Hand Container
            GameObject handContainer = new GameObject("HandContainer");
            handContainer.transform.SetParent(gameUI.transform, false);
            RectTransform handRT = handContainer.AddComponent<RectTransform>();
            handRT.anchorMin = new Vector2(0, 0);
            handRT.anchorMax = new Vector2(1, 0);
            handRT.anchoredPosition = new Vector2(0, 50);
            handRT.sizeDelta = new Vector2(-100, 200);
            handContainer.AddComponent<Image>().color = new Color(0, 0, 0, 0.1f);
            HorizontalLayoutGroup hlg = handContainer.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 20;
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            gameComponent.handContainer = handContainer.transform;

            // Create Score Text
            GameObject scoreText = CreateText("ScoreText", gameUI.transform, new Vector2(50, -50), new Vector2(300, 50));
            scoreText.GetComponent<TextMeshProUGUI>().text = "Your Score: 0";
            gameComponent.scoreText = scoreText.GetComponent<TextMeshProUGUI>();

            // Create Opponent Score Text
            GameObject oppScoreText = CreateText("OpponentScoreText", gameUI.transform, new Vector2(-50, -50), new Vector2(300, 50));
            oppScoreText.GetComponent<TextMeshProUGUI>().text = "Opponent Score: 0";
            oppScoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
            gameComponent.opponentScoreText = oppScoreText.GetComponent<TextMeshProUGUI>();

            // Create Turn Text
            GameObject turnText = CreateText("TurnText", gameUI.transform, new Vector2(0, -50), new Vector2(300, 50));
            turnText.GetComponent<TextMeshProUGUI>().text = "Turn 1/6";
            turnText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            gameComponent.turnText = turnText.GetComponent<TextMeshProUGUI>();

            // Create Cost Text
            GameObject costText = CreateText("CostText", gameUI.transform, new Vector2(0, -100), new Vector2(300, 50));
            costText.GetComponent<TextMeshProUGUI>().text = "Cost: 0/1";
            costText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            gameComponent.costText = costText.GetComponent<TextMeshProUGUI>();

            // Create Timer Text
            GameObject timerText = CreateText("TimerText", gameUI.transform, new Vector2(0, -150), new Vector2(300, 50));
            timerText.GetComponent<TextMeshProUGUI>().text = "Time: 30s";
            timerText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            gameComponent.timerText = timerText.GetComponent<TextMeshProUGUI>();

            // Create End Turn Button
            GameObject endTurnBtn = CreateButton("EndTurnButton", gameUI.transform, new Vector2(0, -220), new Vector2(200, 60));
            endTurnBtn.GetComponentInChildren<TextMeshProUGUI>().text = "End Turn";
            gameComponent.endTurnButton = endTurnBtn.GetComponent<Button>();

            // Create Game Status Text
            GameObject gameStatusText = CreateText("GameStatusText", gameUI.transform, new Vector2(0, -300), new Vector2(600, 50));
            gameStatusText.GetComponent<TextMeshProUGUI>().text = "";
            gameStatusText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            gameComponent.gameStatusText = gameStatusText.GetComponent<TextMeshProUGUI>();

            Debug.Log("Created GameUI");
        }

        private GameObject CreateButton(string name, Transform parent, Vector2 position, Vector2 size)
        {
            GameObject btn = new GameObject(name);
            btn.transform.SetParent(parent, false);
            
            RectTransform rt = btn.AddComponent<RectTransform>();
            rt.anchoredPosition = position;
            rt.sizeDelta = size;
            
            Image img = btn.AddComponent<Image>();
            Button button = btn.AddComponent<Button>();
            
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(btn.transform, false);
            RectTransform textRT = textGO.AddComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = "Button";
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 24;
            
            return btn;
        }

        private GameObject CreateText(string name, Transform parent, Vector2 position, Vector2 size)
        {
            GameObject textGO = new GameObject(name);
            textGO.transform.SetParent(parent, false);
            
            RectTransform rt = textGO.AddComponent<RectTransform>();
            rt.anchoredPosition = position;
            rt.sizeDelta = size;
            
            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = name;
            text.fontSize = 24;
            
            return textGO;
        }

        private GameObject CreateInputField(string name, Transform parent, Vector2 position, Vector2 size)
        {
            GameObject inputGO = new GameObject(name);
            inputGO.transform.SetParent(parent, false);
            
            RectTransform rt = inputGO.AddComponent<RectTransform>();
            rt.anchoredPosition = position;
            rt.sizeDelta = size;
            
            Image img = inputGO.AddComponent<Image>();
            img.color = Color.white;
            
            TMP_InputField input = inputGO.AddComponent<TMP_InputField>();
            
            // Create text area
            GameObject textArea = new GameObject("Text Area");
            textArea.transform.SetParent(inputGO.transform, false);
            RectTransform textAreaRT = textArea.AddComponent<RectTransform>();
            textAreaRT.anchorMin = Vector2.zero;
            textAreaRT.anchorMax = Vector2.one;
            textAreaRT.sizeDelta = Vector2.zero;
            
            // Create text
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(textArea.transform, false);
            RectTransform textRT = textGO.AddComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.sizeDelta = Vector2.zero;
            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
            text.text = "";
            text.fontSize = 24;
            
            // Create placeholder
            GameObject placeholderGO = new GameObject("Placeholder");
            placeholderGO.transform.SetParent(textArea.transform, false);
            RectTransform placeholderRT = placeholderGO.AddComponent<RectTransform>();
            placeholderRT.anchorMin = Vector2.zero;
            placeholderRT.anchorMax = Vector2.one;
            placeholderRT.sizeDelta = Vector2.zero;
            TextMeshProUGUI placeholder = placeholderGO.AddComponent<TextMeshProUGUI>();
            placeholder.text = "Enter text...";
            placeholder.fontSize = 24;
            placeholder.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            
            input.textViewport = textAreaRT;
            input.textComponent = text;
            input.placeholder = placeholder;
            
            return inputGO;
        }

        private void CreatePlayerPrefab()
        {
            // Check if prefab already exists
            GameObject existingPrefab = Resources.Load<GameObject>("PlayerPrefab");
            if (existingPrefab != null)
            {
                Debug.Log("PlayerPrefab already exists, skipping...");
                return;
            }

            // Create in scene first
            GameObject playerPrefab = new GameObject("PlayerPrefab");
            playerPrefab.AddComponent<PlayerNetworkIdentity>();
            NetworkIdentity ni = playerPrefab.AddComponent<NetworkIdentity>();
            ni.serverOnly = false;

            // Save as prefab
            #if UNITY_EDITOR
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Prefabs"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Prefabs");
            }
            string path = "Assets/Prefabs/PlayerPrefab.prefab";
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(playerPrefab, path);
            DestroyImmediate(playerPrefab);
            
            // Assign to NetworkManager
            GameObject networkManager = GameObject.Find("NetworkManager");
            if (networkManager != null)
            {
                NetworkManager nm = networkManager.GetComponent<NetworkManager>();
                if (nm != null)
                {
                    GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    nm.playerPrefab = prefab;
                    UnityEditor.EditorUtility.SetDirty(nm);
                }
            }
            #endif

            Debug.Log("Created PlayerPrefab");
        }

        private void CreateCardPrefab()
        {
            // Check if prefab already exists
            GameObject existingPrefab = Resources.Load<GameObject>("CardPrefab");
            if (existingPrefab != null)
            {
                Debug.Log("CardPrefab already exists, skipping...");
                return;
            }

            // Create card prefab structure
            GameObject cardPrefab = new GameObject("CardPrefab");
            CardUI cardUI = cardPrefab.AddComponent<CardUI>();
            
            RectTransform cardRT = cardPrefab.AddComponent<RectTransform>();
            cardRT.sizeDelta = new Vector2(150, 200);

            // Card Image (background)
            GameObject cardImage = new GameObject("CardImage");
            cardImage.transform.SetParent(cardPrefab.transform, false);
            RectTransform imgRT = cardImage.AddComponent<RectTransform>();
            imgRT.anchorMin = Vector2.zero;
            imgRT.anchorMax = Vector2.one;
            imgRT.sizeDelta = Vector2.zero;
            Image img = cardImage.AddComponent<Image>();
            img.color = new Color(0.9f, 0.9f, 0.9f);
            cardUI.cardImage = img;

            // Name Text
            GameObject nameText = CreateText("NameText", cardImage.transform, new Vector2(0, -10), new Vector2(-20, 30));
            nameText.GetComponent<TextMeshProUGUI>().text = "Card Name";
            nameText.GetComponent<TextMeshProUGUI>().fontSize = 18;
            nameText.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            cardUI.nameText = nameText.GetComponent<TextMeshProUGUI>();

            // Cost Text
            GameObject costText = CreateText("CostText", cardImage.transform, new Vector2(-50, -40), new Vector2(50, 25));
            costText.GetComponent<TextMeshProUGUI>().text = "Cost: 1";
            costText.GetComponent<TextMeshProUGUI>().fontSize = 14;
            cardUI.costText = costText.GetComponent<TextMeshProUGUI>();

            // Power Text
            GameObject powerText = CreateText("PowerText", cardImage.transform, new Vector2(50, -40), new Vector2(50, 25));
            powerText.GetComponent<TextMeshProUGUI>().text = "Power: 1";
            powerText.GetComponent<TextMeshProUGUI>().fontSize = 14;
            powerText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
            cardUI.powerText = powerText.GetComponent<TextMeshProUGUI>();

            // Ability Text
            GameObject abilityText = CreateText("AbilityText", cardImage.transform, new Vector2(0, 10), new Vector2(-20, 40));
            abilityText.GetComponent<TextMeshProUGUI>().text = "Ability";
            abilityText.GetComponent<TextMeshProUGUI>().fontSize = 12;
            cardUI.abilityText = abilityText.GetComponent<TextMeshProUGUI>();

            // Selected Indicator
            GameObject selectedIndicator = new GameObject("SelectedIndicator");
            selectedIndicator.transform.SetParent(cardImage.transform, false);
            RectTransform selRT = selectedIndicator.AddComponent<RectTransform>();
            selRT.anchorMin = Vector2.zero;
            selRT.anchorMax = Vector2.one;
            selRT.sizeDelta = Vector2.zero;
            Image selImg = selectedIndicator.AddComponent<Image>();
            selImg.color = new Color(1, 1, 0, 0.5f);
            selectedIndicator.SetActive(false);
            cardUI.selectedIndicator = selImg;

            // Card Button
            GameObject cardButton = new GameObject("CardButton");
            cardButton.transform.SetParent(cardImage.transform, false);
            RectTransform btnRT = cardButton.AddComponent<RectTransform>();
            btnRT.anchorMin = Vector2.zero;
            btnRT.anchorMax = Vector2.one;
            btnRT.sizeDelta = Vector2.zero;
            Image btnImg = cardButton.AddComponent<Image>();
            btnImg.color = new Color(1, 1, 1, 0);
            Button btn = cardButton.AddComponent<Button>();
            cardUI.cardButton = btn;

            // Save as prefab
            #if UNITY_EDITOR
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Prefabs"))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Prefabs");
            }
            string path = "Assets/Prefabs/CardPrefab.prefab";
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(cardPrefab, path);
            DestroyImmediate(cardPrefab);
            
            // Assign to GameUI
            GameObject gameUI = GameObject.Find("GameUI");
            if (gameUI != null)
            {
                GameUI gameUIComponent = gameUI.GetComponent<GameUI>();
                if (gameUIComponent != null)
                {
                    GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    gameUIComponent.cardPrefab = prefab;
                    UnityEditor.EditorUtility.SetDirty(gameUIComponent);
                }
            }
            #endif

            Debug.Log("Created CardPrefab");
        }
    }
}

