# Quick Start Guide

## Option 1: Automated Setup (Recommended for First Time)

1. **Open Unity 6000.2** and open the project

2. **Create a new scene:**
   - Right-click in `Assets/Scenes/` → Create → Scene
   - Name it `GameScene`
   - Open it

3. **Run the automated setup:**
   - Right-click in Hierarchy → Create Empty
   - Name it `SetupHelper`
   - Add Component → `AutoSceneSetup` (CardGame.Setup namespace)
   - Right-click the component → "Setup Complete Scene"
   - Wait for it to finish (check Console for messages)

4. **Verify and adjust:**
   - Check that all GameObjects were created
   - Verify UI positions look good
   - Test the scene (Press Play, click Host)

5. **Delete the SetupHelper GameObject** (no longer needed)

## Option 2: Manual Setup

Follow the detailed guide in `SCENE_SETUP_STEP_BY_STEP.md` for complete manual setup instructions.

## What Gets Created Automatically

✅ NetworkManager with CustomNetworkManager  
✅ CardDatabase  
✅ GameManager  
✅ NetworkGameManager with NetworkIdentity  
✅ LocalPlayerManager  
✅ Canvas with proper scaling  
✅ LobbyUI with all buttons and text  
✅ GameUI with all UI elements  
✅ PlayerPrefab (saved to Assets/Prefabs/)  
✅ CardPrefab (saved to Assets/Prefabs/)  

## After Automated Setup

1. **Verify Player Prefab is assigned:**
   - Select NetworkManager
   - Check that Player Prefab field has the prefab assigned

2. **Verify Card Prefab is assigned:**
   - Select GameUI
   - Check that Card Prefab field has the prefab assigned

3. **Adjust UI positions if needed:**
   - Select UI elements and move them to desired positions
   - The automated setup creates basic layouts

4. **Test the scene:**
   - Press Play
   - Click "Host" button
   - Open second instance and click "Join"
   - Game should start when 2 players connect

## Troubleshooting Automated Setup

### "Some references are missing"
- The automated setup creates all GameObjects and assigns most references
- Check Console for any errors
- Manually verify all component references are assigned

### "Prefabs not created"
- Ensure you're in the Unity Editor (not runtime)
- Check that Assets/Prefabs/ folder exists
- Try creating prefabs manually (see SCENE_SETUP_STEP_BY_STEP.md)

### "UI looks wrong"
- UI positions are basic - adjust them manually
- Use the RectTransform tool to move and resize UI elements
- Refer to SCENE_SETUP_STEP_BY_STEP.md for recommended positions

## Next Steps

1. ✅ Scene is set up
2. ✅ Test locally with two instances
3. ✅ Build for Android
4. ✅ Test on Android devices
5. ✅ Record gameplay video

## Need More Help?

- See `SCENE_SETUP_STEP_BY_STEP.md` for detailed manual setup
- See `SETUP_GUIDE.md` for general setup information
- See `README.md` for project overview

