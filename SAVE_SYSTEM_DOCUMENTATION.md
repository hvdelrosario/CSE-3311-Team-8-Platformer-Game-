# Save System Implementation

## Overview
The save system has been updated to persist checkpoints, hearts (player health), player positions, and current scene across game sessions. When you exit and return to the game, you'll load into the exact scene, checkpoint, and health state you left off at.

## Key Features

### 1. **Scene Persistence**
- The current scene name is now saved in `GameData`
- When loading a saved game, the system automatically loads you into the correct scene
- Scene-specific data (checkpoints, player position) is properly restored

### 2. **Checkpoint System**
- Checkpoints are saved by their ID number
- When loading a game, the most recent checkpoint is automatically activated
- The player spawns at the checkpoint position (with a 0.5 unit vertical offset to prevent clipping)
- If no checkpoint was reached, the player spawns at the level's start position

### 3. **Health/Hearts Persistence**
- Player health is saved and restored across sessions
- Heart UI updates correctly to show the saved health state
- Hearts lost before saving remain lost when you reload

### 4. **Level Progression**
- When completing a level (reaching the Goal), the game saves your progress
- The next level starts fresh with checkpoint ID reset to -1
- This ensures you spawn at the correct start position for each new level, not at a checkpoint from a previous level

## Modified Files

### GameData.cs
- Added `currentSceneName` field to track which scene the player is in
- Default scene is set to "TutorialLevel"

### DataPersistenceManager.cs
- Added `LoadGameAndScene()` method that:
  - Loads the saved game data
  - Automatically loads the correct scene if different from current
  - Properly initializes all data persistence objects

### GameManager.cs
- **Critical Fix**: Scene assets (including checkpoints) are now loaded BEFORE player positioning
  - This ensures the checkpoints list is populated before `LoadData()` is called
  - Prevents null reference errors when trying to spawn at a checkpoint
- `LoadData()` now:
  - Activates the saved checkpoint visually
  - Positions the player at the checkpoint
  - Handles the case where no checkpoint exists (spawns at start)
- `SaveData()` saves the current scene name
- Added `loadLevel2Assets()` method for future Level 2 implementation

### Goal.cs
- Now saves the game when the player reaches the goal
- Resets the checkpoint ID to -1 for the next level
- This ensures proper spawning at the start of new levels

### PlayerStats.cs
- Removed position handling from `LoadData()` 
- Position is now managed by GameManager based on checkpoint data
- Still handles health loading/saving

### SaveSlotsMenu.cs
- Updated to use `LoadGameAndScene()` when loading an existing save
- New games still start at TutorialLevel as expected

## How It Works

### Starting a New Game:
1. Player selects "New Game" from save slot menu
2. `NewGame()` creates fresh GameData with default values
3. Scene loads to "TutorialLevel"
4. Player spawns at start position (-12, 3.5, 0)
5. Checkpoint ID is -1 (no checkpoints reached)

### During Gameplay:
1. Player touches checkpoints, updating `mostRecentCheckpoint` in GameManager
2. Health changes are tracked in PlayerStats
3. Game auto-saves on application quit
4. Goal triggers a save before transitioning to next level

### Loading an Existing Game:
1. Player selects "Load Game" and picks a save slot
2. `LoadGameAndScene()` reads the saved data
3. If needed, loads the correct scene (e.g., Level1)
4. GameManager loads level assets (creating checkpoints list)
5. `OnSceneLoaded` triggers, calling `LoadData()` on all persistence objects
6. GameManager positions player at saved checkpoint
7. PlayerStats restores health
8. Checkpoint is visually activated (turns green)
9. Heart UI updates to show current health

### Cross-Scene Behavior:
- When you finish TutorialLevel and enter Level1, checkpoint is reset
- You spawn at Level1's start position
- Your health persists from the previous level
- If you quit in Level1 and reload, you spawn in Level1 at your last checkpoint

## Testing Checklist

✓ Start new game → Should spawn at TutorialLevel start
✓ Touch checkpoint → Should turn green and activate
✓ Take damage → Hearts should disappear
✓ Exit game → Reopen and load save
✓ Should load into same scene at last checkpoint with correct health
✓ Complete level → Should transition to next level
✓ New level should start at beginning (not previous checkpoint)
✓ Exit and reload → Should be in the new level at its start position

## Important Notes

- The checkpoint reset in `Goal.cs` is essential - without it, you'd spawn at invalid checkpoint positions in new levels
- Asset loading order is critical - checkpoints must exist before `LoadData()` attempts to use them
- Player position is managed by GameManager, not PlayerStats, to ensure checkpoint-based spawning works correctly
