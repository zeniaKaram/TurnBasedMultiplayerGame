# Setup Guide

This guide will help you set up the Unity scene for the Turn-Based Multiplayer Card Game.

## Step 1: Install Mirror Networking

1. Open Unity 6000.2
2. The project should automatically install Mirror from the git URL in `Packages/manifest.json`
3. If it doesn't install automatically:
   - Window → Package Manager
   - Click the "+" button → "Add package from git URL"
   - Enter: `https://github.com/MirrorNetworking/Mirror.git?path=/Assets`

## Step 2: Create the Game Scene

1. Create a new scene: `Assets/Scenes/GameScene.unity`
2. Save the scene

## Step 3: Set Up Core GameObjects

### NetworkManager Setup

1. Create an empty GameObject named "NetworkManager"
2. Add Component → `CustomNetworkManager` (from CardGame.Networking namespace)
3. In the NetworkManager component:
   - Set "Player Prefab" (create this in Step 4)
   - Enable "Auto Create Player"
   - Set "Max Connections" to 2

### Player Prefab

1. Create an empty GameObject named "PlayerPrefab"
2. Add Component → `PlayerNetworkIdentity` (from CardGame.Networking namespace)
3. Add a NetworkIdentity component (Mirror will add this automatically)
4. Set the GameObject as a prefab:
   - Drag it to `Assets/Prefabs/` folder
   - Assign this prefab to NetworkManager's "Player Prefab" field

### CardDatabase

1. Create an empty GameObject named "CardDatabase"
2. Add Component → `CardDatabase` (from CardGame.Data namespace)
3. Ensure `Assets/StreamingAssets/cards.json` exists (already created)

### GameManager

1. Create an empty GameObject named "GameManager"
2. Add Component → `GameManager` (from CardGame.Game namespace)

### NetworkGameManager

1. Create an empty GameObject named "NetworkGameManager"
2. Add Component → `NetworkGameManager` (from CardGame.Networking namespace)

### LocalPlayerManager

1. Create an empty GameObject named "LocalPlayerManager"
2. Add Component → `LocalPlayerManager` (from CardGame.Networking namespace)

## Step 4: Set Up UI

### Canvas Setup

1. Right-click in Hierarchy → UI → Canvas
2. Name it "GameCanvas"
3. Set Canvas Scaler to "Scale With Screen Size"
4. Reference Resolution: 1920x1080

### Lobby UI

1. Create an empty GameObject as child of Canvas, name it "LobbyUI"
2. Add Component → `LobbyUI` (from CardGame.UI namespace)
3. Create UI elements:
   - Button: "HostButton" → Assign to LobbyUI's "Host Button"
   - Button: "JoinButton" → Assign to LobbyUI's "Join Button"
   - Button: "StartButton" → Assign to LobbyUI's "Start Button" (optional, game auto-starts)
   - TextMeshProUGUI: "StatusText" → Assign to LobbyUI's "Status Text"
   - TMP_InputField: "IPInputField" → Assign to LobbyUI's "IP Input Field"
   - TextMeshProUGUI: "PlayerCountText" → Assign to LobbyUI's "Player Count Text"

### Game UI

1. Create an empty GameObject as child of Canvas, name it "GameUI"
2. Add Component → `GameUI` (from CardGame.UI namespace)
3. Create UI elements:
   - Horizontal Layout Group: "HandContainer" → Assign to GameUI's "Hand Container"
   - TextMeshProUGUI: "ScoreText" → Assign to GameUI's "Score Text"
   - TextMeshProUGUI: "OpponentScoreText" → Assign to GameUI's "Opponent Score Text"
   - TextMeshProUGUI: "TurnText" → Assign to GameUI's "Turn Text"
   - TextMeshProUGUI: "CostText" → Assign to GameUI's "Cost Text"
   - TextMeshProUGUI: "TimerText" → Assign to GameUI's "Timer Text"
   - Button: "EndTurnButton" → Assign to GameUI's "End Turn Button"
   - TextMeshProUGUI: "GameStatusText" → Assign to GameUI's "Game Status Text"

### Card Prefab

1. Create an empty GameObject, name it "CardPrefab"
2. Add Component → `CardUI` (from CardGame.UI namespace)
3. Add UI elements as children:
   - Image: "CardImage" (background)
   - TextMeshProUGUI: "NameText" (card name)
   - TextMeshProUGUI: "CostText" (cost)
   - TextMeshProUGUI: "PowerText" (power)
   - TextMeshProUGUI: "AbilityText" (ability description)
   - Image: "SelectedIndicator" (highlight when selected)
   - Button: "CardButton" (for clicking)
4. Set CardButton's target to CardUI component
5. Save as prefab: `Assets/Prefabs/CardPrefab.prefab`
6. Assign to GameUI's "Card Prefab" field

## Step 5: Verify Setup

1. Ensure all GameObjects are in the scene
2. Check that all component references are assigned
3. Ensure `Assets/StreamingAssets/cards.json` exists
4. Test the scene:
   - Press Play
   - Click "Host" button
   - Open a second instance and click "Join"
   - Game should start automatically when 2 players connect

## Quick Setup Script

Alternatively, you can use the SceneSetupHelper:

1. Create an empty GameObject named "SceneSetupHelper"
2. Add Component → `SceneSetupHelper` (from CardGame.Setup namespace)
3. Right-click the component → "Setup Scene"
4. This will create all core GameObjects automatically
5. You still need to set up the UI manually

## Troubleshooting

### "Cards JSON file not found"
- Ensure `Assets/StreamingAssets/cards.json` exists
- Check that the file is not empty
- Verify JSON format is valid

### "NetworkManager not found"
- Ensure NetworkManager GameObject exists in scene
- Check that CustomNetworkManager component is attached

### UI not updating
- Verify all UI component references are assigned
- Check that GameUI and LobbyUI components are enabled
- Ensure GameManager instance exists

### Cards not displaying
- Verify CardPrefab is assigned to GameUI
- Check that HandContainer is a Horizontal Layout Group
- Ensure CardDatabase loaded cards successfully

