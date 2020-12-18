// Author Devon Wayman
// Date 12/16/2020

/// <summary>
/// Used to serialize game settings to JSON file and be read back
/// upon application launch
/// </summary>
public class GameSettings {
    public bool teleportMovement; // 0 for continuous, 1 for teleport
    public bool snapTurnMovement; // 0 for snap, 1 for continuous
    public bool postProcessEnabled; // 0 for disabled, 1 for enabled
}
