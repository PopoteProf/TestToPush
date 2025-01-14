using System;

public static class GameStaticManager {
    public static event EventHandler<bool> OnPauseChange;
    public static event EventHandler<float> OnPlayerHpChange;
    public static event EventHandler<float> OnPlayerTakeDamage;

    public static void SetPause(bool value) {
        OnPauseChange?.Invoke(null , value);
    }
    
    public static void SetPlayerHpChange(float value) {
        OnPlayerHpChange?.Invoke(null , value);
    }
    
    public static void SetPlayerTakeDamageHpChange(float value) {
        OnPlayerTakeDamage?.Invoke(null , value);
    }
}