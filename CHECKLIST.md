# Project Checklist

## ✅ Code Implementation (Complete)

- [x] Add Mirror networking package to manifest.json
- [x] Create project folder structure
- [x] Create card data structures (CardData, CardAbility)
- [x] Create JSON card definitions (cards.json)
- [x] Implement event system (GameEvents)
- [x] Create networking layer with JSON messaging
- [x] Implement game logic (GameManager, Player)
- [x] Implement turn management and cost system
- [x] Implement card resolution and abilities
- [x] Create UI system (GameUI, CardUI, LobbyUI)
- [x] Create lobby/matchmaking system
- [x] Create README documentation
- [x] Create setup guide

## 🔧 Unity Editor Setup (To Do)

### Package Installation
- [ ] Open Unity 6000.2
- [ ] Wait for Mirror package to install automatically
- [ ] Verify Mirror is installed in Package Manager

### Scene Setup
- [ ] Create GameScene.unity
- [ ] Create NetworkManager GameObject with CustomNetworkManager
- [ ] Create CardDatabase GameObject
- [ ] Create GameManager GameObject
- [ ] Create NetworkGameManager GameObject
- [ ] Create LocalPlayerManager GameObject

### Prefabs
- [ ] Create PlayerPrefab with PlayerNetworkIdentity
- [ ] Assign PlayerPrefab to NetworkManager
- [ ] Create CardPrefab with CardUI component
- [ ] Set up CardPrefab UI elements (name, cost, power, ability text)

### UI Setup
- [ ] Create Canvas
- [ ] Set up LobbyUI with all references:
  - [ ] Host Button
  - [ ] Join Button
  - [ ] Start Button
  - [ ] Status Text
  - [ ] IP Input Field
  - [ ] Player Count Text
- [ ] Set up GameUI with all references:
  - [ ] Hand Container (Horizontal Layout Group)
  - [ ] Score Text
  - [ ] Opponent Score Text
  - [ ] Turn Text
  - [ ] Cost Text
  - [ ] Timer Text
  - [ ] End Turn Button
  - [ ] Game Status Text
  - [ ] Card Prefab reference

### Verification
- [ ] Verify cards.json exists in StreamingAssets
- [ ] Test scene in Play mode
- [ ] Test hosting a game
- [ ] Test joining a game
- [ ] Verify cards load correctly
- [ ] Verify UI updates properly
- [ ] Test full 6-turn match

## 📱 Build & Testing

### Android Build
- [ ] Configure Android build settings
- [ ] Set package name
- [ ] Set minimum API level
- [ ] Build APK
- [ ] Install on two Android devices
- [ ] Test multiplayer on devices

### Testing
- [ ] Test local multiplayer (two instances)
- [ ] Test on Android devices
- [ ] Verify all card abilities work
- [ ] Verify score calculation
- [ ] Verify turn timer
- [ ] Verify game end condition
- [ ] Test edge cases (empty hand, no cards selected, etc.)

## 📹 Deliverables

- [ ] Record gameplay video (full 6-turn match)
- [ ] Build final APK
- [ ] Create GitHub repository
- [ ] Push code to GitHub
- [ ] Update README with any additional notes
- [ ] Test all deliverables

## 🎯 Final Verification

- [ ] Two clients can play a 6-turn match
- [ ] Cards are JSON-driven and easy to modify
- [ ] Event system is working (decoupled architecture)
- [ ] Network messages use JSON (not raw RPC args)
- [ ] Code is clean and modular
- [ ] Documentation is complete

## 📝 Notes

- All code is implemented and ready
- Main work remaining is Unity Editor setup
- Follow SETUP_GUIDE.md for detailed instructions
- See IMPLEMENTATION_SUMMARY.md for technical details

