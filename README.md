# Turn-Based Multiplayer Card Game

A turn-based multiplayer card game prototype built with Unity 6000.2, featuring online multiplayer, JSON-driven card system, and event-driven architecture.

## Networking Solution

This project uses **Mirror Networking** for multiplayer functionality. Mirror is a high-level networking library for Unity that provides:
- Easy-to-use networking primitives
- Built-in client-server architecture
- RPC (Remote Procedure Call) support
- SyncVar for automatic state synchronization

### Why Mirror?
- Unity-friendly and well-documented
- Active community and support
- Easy integration with Unity's component system
- Supports both authoritative server and peer-to-peer architectures

## Project Structure

```
Assets/
├── Scripts/
│   ├── Data/              # Card data structures and database
│   ├── Events/            # Event system for decoupled communication
│   ├── Game/              # Core game logic (GameManager, Player)
│   ├── Networking/        # Network layer with JSON messaging
│   └── UI/                # User interface components
├── StreamingAssets/
│   └── cards.json         # JSON card definitions
└── Scenes/
    └── GameScene          # Main game scene
```

## JSON Card System

Cards are defined in `Assets/StreamingAssets/cards.json`. Each card has the following structure:

```json
{
  "id": 1,
  "name": "Shield Bearer",
  "cost": 2,
  "power": 3,
  "ability": {
    "type": "GainPoints",
    "value": 2
  }
}
```

### Card Fields
- **id**: Unique identifier for the card
- **name**: Display name
- **cost**: Cost required to play the card
- **power**: Base strength (contributes to score)
- **ability**: Optional ability object with:
  - **type**: Ability keyword (GainPoints, StealPoints, etc.)
  - **value**: Integer parameter for the ability

### Supported Abilities
- **GainPoints**: Add value points to player score
- **StealPoints**: Take value points from opponent and add to your score
- **DoublePower**: Multiply this card's power by value (usually 2)
- **DrawExtraCard**: Draw value extra cards into hand
- **DiscardOpponentRandomCard**: Randomly remove value cards from opponent's hand
- **DestroyOpponentCardInPlay**: Remove value cards from opponent's revealed set before resolution

## Event-Driven Architecture

The game uses a centralized event system (`GameEvents`) for decoupled communication:

### Core Events
- **GameStart**: Triggered when a match begins
- **TurnStart**: Triggered at the start of each turn
- **PlayerEndedTurn**: Triggered when a player ends their turn
- **RevealCards**: Triggered when cards are revealed simultaneously
- **GameEnd**: Triggered when the game ends

### Benefits
- Decoupled components (UI, game logic, networking)
- Easy to extend with new features
- Testable and maintainable code
- Clear separation of concerns

## Networking Messages (JSON)

All network communication uses JSON messages, not raw RPC arguments:

### Game Start Message
```json
{
  "action": "gameStart",
  "playerIds": ["P1", "P2"],
  "totalTurns": 6
}
```

### End Turn Message
```json
{
  "action": "endTurn",
  "playerId": "P1",
  "selectedCardIds": [2, 5]
}
```

### Reveal Cards Message
```json
{
  "action": "revealCards",
  "playerId": "P1",
  "cardIds": [2, 5]
}
```

## Game Rules

### Match Flow
- **Match Length**: 6 turns (fixed)
- **Starting Deck**: 12 cards per player
- **Initial Hand**: 3 cards
- **Turn Start**: Draw +1 card
- **Turn Duration**: 30 seconds (auto-ends if timer expires)

### Cost System
- Turn 1: 1 cost available
- Turn 2: 2 cost available
- ...
- Turn 6: 6 cost available

Players can play multiple cards per turn as long as total cost ≤ available cost.

### Victory Condition
Highest score after 6 turns wins.

## How to Run & Test

### Prerequisites
- Unity 6000.2
- Mirror Networking (automatically installed via package manager)

### Setup Steps

1. **Open the project in Unity**
   - Open Unity Hub
   - Add project from disk
   - Select the project folder

2. **Wait for package installation**
   - Unity will automatically install Mirror from the git URL in `Packages/manifest.json`

3. **Set up the scene**
   - Open `Assets/Scenes/GameScene.unity`
   - Ensure the scene has:
     - NetworkManager (with CustomNetworkManager component)
     - NetworkGameManager GameObject
     - CardDatabase GameObject
     - GameManager GameObject
     - LocalPlayerManager GameObject
     - UI Canvas with GameUI and LobbyUI components

4. **Build for Android**
   - File → Build Settings
   - Select Android platform
   - Click "Build" or "Build and Run"

### Testing Locally

1. **Host a game**
   - Click "Host" button in the lobby
   - Note the IP address (or use "localhost")

2. **Join as second player**
   - Open a second instance (or use a second device on the same network)
   - Enter the host IP address
   - Click "Join"

3. **Play the game**
   - Game starts automatically when 2 players connect
   - Select cards from your hand
   - Click "End Turn" when ready
   - Cards are revealed simultaneously when both players end their turn

### Testing on Android

1. **Build APK**
   - File → Build Settings → Android
   - Configure Android settings (minimum API level, etc.)
   - Build APK

2. **Install on devices**
   - Install APK on two Android devices
   - Ensure both devices are on the same network
   - One device hosts, the other joins using the host device's IP address

## Key Features

✅ **Working multiplayer flow** - Two clients can play a full 6-turn match  
✅ **Clean, modular code** - Networking, game logic, and UI are separated  
✅ **JSON-driven card system** - Easy to extend with new cards and abilities  
✅ **Event-based architecture** - Decoupled, reusable components  
✅ **JSON messaging** - All network communication uses structured JSON messages  

## Troubleshooting

### Cards not loading
- Ensure `Assets/StreamingAssets/cards.json` exists
- Check that CardDatabase component is in the scene
- Verify JSON format is valid

### Network connection issues
- Ensure both clients are on the same network
- Check firewall settings
- Verify NetworkManager is properly configured

### UI not updating
- Check that GameUI component is attached to a GameObject in the scene
- Verify event subscriptions in GameUI.Start()
- Ensure GameManager instance exists

## Future Enhancements

- Reconnection handling for dropped connections
- Card animations and visual effects
- Sound effects and music
- Card tooltips and descriptions
- Match history and statistics
- Custom deck building

## License

This project is created for evaluation purposes.

