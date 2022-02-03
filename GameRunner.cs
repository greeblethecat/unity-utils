using System;
using UnityEngine;

namespace Utils {
  public class GameRunner : MonoBehaviour {
  
    public string initialGameStateName;

    // This is a singleton component that needs to be in the scene.
    private static GameRunner _instance = null;
    private static IGameState _currentGameState = null;

    void Awake() {
      _instance = this;
      ChangeGameState(initialGameStateName);
    }

    public static IGameState GetCurrentGameState() {
      return _currentGameState;
    }

    public static void ChangeGameState(IGameState nextGameState) {
      if (_currentGameState != null) {
        Debug.Log($"Stopping {_currentGameState.GetType().Name}");
        _currentGameState.Stop();
      }
      Debug.Log($"Starting {nextGameState!.GetType().Name}");
      _currentGameState = nextGameState;
      _currentGameState.Start();
    }

    public static void ChangeGameState(String nextGameStateName) {
      IGameState nextGameState = null;
      try {
        nextGameState = Helpers.InstantiateClass(nextGameStateName) as IGameState;
      } catch {
        Debug.Log($"Failed to instantiate initial game state \"{nextGameStateName}\"");
        throw;
      }

      ChangeGameState(nextGameState);
    }
  
  }
}