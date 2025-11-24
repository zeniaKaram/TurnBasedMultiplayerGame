# Step-by-Step Scene Setup Guide

Follow these steps to set up your game scene in Unity 6000.2.

## Prerequisites
- Unity 6000.2 is open
- Mirror package is installed (check Package Manager)
- Project is loaded

## Step 1: Create New Scene

1. Right-click in `Assets/Scenes/` folder
2. Create → Scene
3. Name it `GameScene`
4. Double-click to open it
5. Save the scene (Ctrl+S)

## Step 2: Create Core GameObjects

### 2.1 NetworkManager
1. Right-click in Hierarchy → Create Empty
2. Name it `NetworkManager`
3. Select it, in Inspector click "Add Component"
4. Search for `CustomNetworkManager` and add it
5. The component will automatically add Mirror's NetworkManager component

**Configure NetworkManager:**
- In the NetworkManager component (Mirror):
  - Set "Max Connections" to `2`
  - Enable "Auto Create Player"
  - We'll set Player Prefab later (Step 3)

### 2.2 CardDatabase
1. Right-click in Hierarchy → Create Empty
2. Name it `CardDatabase`
3. Add Component → `CardDatabase` (CardGame.Data namespace)
4. Verify `Assets/StreamingAssets/cards.json` exists (should already be there)

### 2.3 GameManager
1. Right-click in Hierarchy → Create Empty
2. Name it `GameManager`
3. Add Component → `GameManager` (CardGame.Game namespace)

### 2.4 NetworkGameManager
1. Right-click in Hierarchy → Create Empty
2. Name it `NetworkGameManager`
3. Add Component → `NetworkGameManager` (CardGame.Networking namespace)
4. This GameObject must have a NetworkIdentity component (add it if missing)

### 2.5 LocalPlayerManager
1. Right-click in Hierarchy → Create Empty
2. Name it `LocalPlayerManager`
3. Add Component → `LocalPlayerManager` (CardGame.Networking namespace)

## Step 3: Create Player Prefab

### 3.1 Create Prefab GameObject
1. Right-click in Hierarchy → Create Empty
2. Name it `PlayerPrefab`
3. Add Component → `PlayerNetworkIdentity` (CardGame.Networking namespace)
4. Add Component → `NetworkIdentity` (Mirror will add this, or add manually)
5. In NetworkIdentity:
   - Enable "Server Only" = false
   - Enable "Local Player Authority" = true

### 3.2 Save as Prefab
1. Create folder: `Assets/Prefabs/` (if it doesn't exist)
2. Drag `PlayerPrefab` from Hierarchy to `Assets/Prefabs/` folder
3. Delete the GameObject from Hierarchy (we only need the prefab)

### 3.3 Assign to NetworkManager
1. Select `NetworkManager` in Hierarchy
2. In CustomNetworkManager component, find "Player Prefab" field
3. Drag `Assets/Prefabs/PlayerPrefab.prefab` to this field

## Step 4: Create UI Canvas

### 4.1 Create Canvas
1. Right-click in Hierarchy → UI → Canvas
2. Name it `GameCanvas`
3. Select Canvas, in Inspector:
   - Canvas Scaler → UI Scale Mode: "Scale With Screen Size"
   - Reference Resolution: X=1920, Y=1080
   - Match: 0.5 (width/height)

### 4.2 Create EventSystem (if not auto-created)
1. If EventSystem wasn't created automatically:
   - Right-click in Hierarchy → UI → Event System

## Step 5: Create Lobby UI

### 5.1 Create LobbyUI GameObject
1. Right-click on `GameCanvas` → Create Empty
2. Name it `LobbyUI`
3. Add Component → `LobbyUI` (CardGame.UI namespace)

### 5.2 Create Host Button
1. Right-click on `LobbyUI` → UI → Button - TextMeshPro
2. Name it `HostButton`
3. Set RectTransform:
   - Anchor: Top-Left
   - Pos X: 100, Pos Y: -50
   - Width: 200, Height: 50
4. Change button text to "Host"
5. In LobbyUI component, drag `HostButton` to "Host Button" field

### 5.3 Create Join Button
1. Right-click on `LobbyUI` → UI → Button - TextMeshPro
2. Name it `JoinButton`
3. Set RectTransform:
   - Anchor: Top-Left
   - Pos X: 100, Pos Y: -120
   - Width: 200, Height: 50
4. Change button text to "Join"
5. In LobbyUI component, drag `JoinButton` to "Join Button" field

### 5.4 Create IP Input Field
1. Right-click on `LobbyUI` → UI → Input Field - TextMeshPro
2. Name it `IPInputField`
3. Set RectTransform:
   - Anchor: Top-Left
   - Pos X: 320, Pos Y: -120
   - Width: 200, Height: 50
4. Set placeholder text to "Enter IP (localhost)"
5. In LobbyUI component, drag `IPInputField` to "IP Input Field" field

### 5.5 Create Status Text
1. Right-click on `LobbyUI` → UI → Text - TextMeshPro
2. Name it `StatusText`
3. Set RectTransform:
   - Anchor: Top-Center
   - Pos Y: -50
   - Width: 600, Height: 50
4. Set text to "Ready to play"
5. Center align the text
6. In LobbyUI component, drag `StatusText` to "Status Text" field

### 5.6 Create Player Count Text
1. Right-click on `LobbyUI` → UI → Text - TextMeshPro
2. Name it `PlayerCountText`
3. Set RectTransform:
   - Anchor: Top-Center
   - Pos Y: -100
   - Width: 300, Height: 50
4. Set text to "Players: 0/2"
5. Center align the text
6. In LobbyUI component, drag `PlayerCountText` to "Player Count Text" field

### 5.7 Create Start Button (Optional)
1. Right-click on `LobbyUI` → UI → Button - TextMeshPro
2. Name it `StartButton`
3. Set RectTransform:
   - Anchor: Top-Center
   - Pos Y: -180
   - Width: 200, Height: 50
4. Change button text to "Start Game"
5. In LobbyUI component, drag `StartButton` to "Start Button" field
6. Initially disable this button (game auto-starts when 2 players connect)

## Step 6: Create Game UI

### 6.1 Create GameUI GameObject
1. Right-click on `GameCanvas` → Create Empty
2. Name it `GameUI`
3. Add Component → `GameUI` (CardGame.UI namespace)
4. Initially disable this GameObject (enable when game starts)

### 6.2 Create Hand Container
1. Right-click on `GameUI` → UI → Panel
2. Name it `HandContainer`
3. Add Component → Horizontal Layout Group
4. Set RectTransform:
   - Anchor: Bottom-Stretch
   - Left: 50, Right: 50, Bottom: 50
   - Height: 200
5. In Horizontal Layout Group:
   - Spacing: 20
   - Child Alignment: Middle Center
   - Child Control Width: true
   - Child Control Height: true
6. In GameUI component, drag `HandContainer` to "Hand Container" field

### 6.3 Create Score Text
1. Right-click on `GameUI` → UI → Text - TextMeshPro
2. Name it `ScoreText`
3. Set RectTransform:
   - Anchor: Top-Left
   - Pos X: 50, Pos Y: -50
   - Width: 300, Height: 50
4. Set text to "Your Score: 0"
5. In GameUI component, drag `ScoreText` to "Score Text" field

### 6.4 Create Opponent Score Text
1. Right-click on `GameUI` → UI → Text - TextMeshPro
2. Name it `OpponentScoreText`
3. Set RectTransform:
   - Anchor: Top-Right
   - Pos X: -50, Pos Y: -50
   - Width: 300, Height: 50
4. Set text to "Opponent Score: 0"
5. Right-align the text
6. In GameUI component, drag `OpponentScoreText` to "Opponent Score Text" field

### 6.5 Create Turn Text
1. Right-click on `GameUI` → UI → Text - TextMeshPro
2. Name it `TurnText`
3. Set RectTransform:
   - Anchor: Top-Center
   - Pos Y: -50
   - Width: 300, Height: 50
4. Set text to "Turn 1/6"
5. Center align the text
6. In GameUI component, drag `TurnText` to "Turn Text" field

### 6.6 Create Cost Text
1. Right-click on `GameUI` → UI → Text - TextMeshPro
2. Name it `CostText`
3. Set RectTransform:
   - Anchor: Top-Center
   - Pos Y: -100
   - Width: 300, Height: 50
4. Set text to "Cost: 0/1"
5. Center align the text
6. In GameUI component, drag `CostText` to "Cost Text" field

### 6.7 Create Timer Text
1. Right-click on `GameUI` → UI → Text - TextMeshPro
2. Name it `TimerText`
3. Set RectTransform:
   - Anchor: Top-Center
   - Pos Y: -150
   - Width: 300, Height: 50
4. Set text to "Time: 30s"
5. Center align the text
6. In GameUI component, drag `TimerText` to "Timer Text" field

### 6.8 Create End Turn Button
1. Right-click on `GameUI` → UI → Button - TextMeshPro
2. Name it `EndTurnButton`
3. Set RectTransform:
   - Anchor: Top-Center
   - Pos Y: -220
   - Width: 200, Height: 60
4. Change button text to "End Turn"
5. In GameUI component, drag `EndTurnButton` to "End Turn Button" field

### 6.9 Create Game Status Text
1. Right-click on `GameUI` → UI → Text - TextMeshPro
2. Name it `GameStatusText`
3. Set RectTransform:
   - Anchor: Top-Center
   - Pos Y: -300
   - Width: 600, Height: 50
4. Set text to ""
5. Center align the text
6. In GameUI component, drag `GameStatusText` to "Game Status Text" field

## Step 7: Create Card Prefab

### 7.1 Create Card GameObject
1. Right-click in Hierarchy → Create Empty
2. Name it `CardPrefab`
3. Add Component → `CardUI` (CardGame.UI namespace)

### 7.2 Create Card Background
1. Right-click on `CardPrefab` → UI → Image
2. Name it `CardImage`
3. Set RectTransform:
   - Width: 150, Height: 200
4. Set color to light gray or add a card background image
5. In CardUI component, drag `CardImage` to "Card Image" field

### 7.3 Create Name Text
1. Right-click on `CardImage` → UI → Text - TextMeshPro
2. Name it `NameText`
3. Set RectTransform:
   - Anchor: Top-Stretch
   - Top: -10, Left: 10, Right: 10
   - Height: 30
4. Set text to "Card Name"
5. Center align, font size: 18, bold
6. In CardUI component, drag `NameText` to "Name Text" field

### 7.4 Create Cost Text
1. Right-click on `CardImage` → UI → Text - TextMeshPro
2. Name it `CostText`
3. Set RectTransform:
   - Anchor: Top-Left
   - Pos X: 10, Pos Y: -40
   - Width: 50, Height: 25
4. Set text to "Cost: 1"
5. Font size: 14
6. In CardUI component, drag `CostText` to "Cost Text" field

### 7.5 Create Power Text
1. Right-click on `CardImage` → UI → Text - TextMeshPro
2. Name it `PowerText`
3. Set RectTransform:
   - Anchor: Top-Right
   - Pos X: -10, Pos Y: -40
   - Width: 50, Height: 25
4. Set text to "Power: 1"
5. Font size: 14, right-align
6. In CardUI component, drag `PowerText` to "Power Text" field

### 7.6 Create Ability Text
1. Right-click on `CardImage` → UI → Text - TextMeshPro
2. Name it `AbilityText`
3. Set RectTransform:
   - Anchor: Bottom-Stretch
   - Bottom: 10, Left: 10, Right: 10
   - Height: 40
4. Set text to "Ability"
5. Center align, font size: 12
6. In CardUI component, drag `AbilityText` to "Ability Text" field

### 7.7 Create Selected Indicator
1. Right-click on `CardImage` → UI → Image
2. Name it `SelectedIndicator`
3. Set RectTransform to match CardImage (stretch to fill)
4. Set color to yellow with 50% opacity (alpha: 128)
5. Initially disable this GameObject
6. In CardUI component, drag `SelectedIndicator` to "Selected Indicator" field

### 7.8 Create Card Button
1. Right-click on `CardImage` → UI → Button
2. Name it `CardButton`
3. Set RectTransform to match CardImage (stretch to fill)
4. Remove the Text child (we don't need button text)
5. Set button colors:
   - Normal: White (transparent)
   - Highlighted: Light blue
6. In CardUI component, drag `CardButton` to "Card Button" field

### 7.9 Save Card Prefab
1. Create folder: `Assets/Prefabs/` (if it doesn't exist)
2. Drag `CardPrefab` from Hierarchy to `Assets/Prefabs/` folder
3. Delete the GameObject from Hierarchy
4. Select `GameUI` in Hierarchy
5. In GameUI component, drag `Assets/Prefabs/CardPrefab.prefab` to "Card Prefab" field

## Step 8: Final Configuration

### 8.1 Verify NetworkGameManager
1. Select `NetworkGameManager` in Hierarchy
2. Ensure it has a NetworkIdentity component
3. If missing, add Component → NetworkIdentity
4. In NetworkIdentity:
   - Enable "Server Only" = false
   - Enable "Local Player Authority" = false

### 8.2 Set Up UI Visibility
1. Select `LobbyUI` - should be enabled (visible)
2. Select `GameUI` - should be disabled initially (hidden)
3. GameUI will be enabled when game starts

### 8.3 Verify All References
Go through each component and verify all fields are assigned:
- **LobbyUI**: All 6 references assigned
- **GameUI**: All 8 references assigned
- **CardUI**: All 6 references assigned (in prefab)
- **NetworkManager**: Player Prefab assigned

## Step 9: Test the Scene

1. Save the scene (Ctrl+S)
2. Press Play
3. Click "Host" button
4. Open a second Unity instance (or build and run)
5. In second instance, click "Join" with "localhost" as IP
6. Game should start automatically when 2 players connect

## Troubleshooting

### "Cards JSON file not found"
- Ensure `Assets/StreamingAssets/cards.json` exists
- Check that CardDatabase GameObject exists

### "NetworkManager not found"
- Ensure NetworkManager GameObject exists
- Check that CustomNetworkManager component is attached

### UI not updating
- Verify all UI component references are assigned
- Check that GameUI and LobbyUI are enabled/disabled correctly
- Ensure GameManager instance exists

### Cards not displaying
- Verify CardPrefab is assigned to GameUI
- Check that HandContainer has Horizontal Layout Group
- Ensure CardDatabase loaded cards successfully

### Network connection issues
- Ensure both instances are on the same network
- Check firewall settings
- Verify NetworkManager is properly configured

## Quick Checklist

- [ ] NetworkManager created and configured
- [ ] PlayerPrefab created and assigned
- [ ] CardDatabase GameObject exists
- [ ] GameManager GameObject exists
- [ ] NetworkGameManager GameObject exists with NetworkIdentity
- [ ] LocalPlayerManager GameObject exists
- [ ] Canvas created with proper scaling
- [ ] LobbyUI created with all references
- [ ] GameUI created with all references
- [ ] CardPrefab created and assigned to GameUI
- [ ] All component references assigned
- [ ] Scene saved

