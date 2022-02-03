using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
  public class EntityPool : MonoBehaviour {
    
    private static readonly Dictionary<string, List<GameEntity>> GameObjectPool = new();

    public void Awake() {
    }

    // Gets the object pool for a particular prefab. If one does not exist, then this method will create one.
    private static List<GameEntity> GetPool(string prefabName) {
      var pool = GameObjectPool.ContainsKey(prefabName) ? GameObjectPool[prefabName] : null;
      if (pool != null) return pool;
      pool = new List<GameEntity>();
      GameObjectPool[prefabName] = pool;
      return pool;
    }

    private static List<GameEntity> GetPool(GameEntity gameEntity) {
      return GetPool(gameEntity.poolKey);
    }
    
    public static GameEntity Instantiate(string prefabName) {
      var pool = GetPool(prefabName);
     
      // If the pool contains an entity, then pop it, activate it, and finally return it.
      if (pool.Count > 0) {
        var entity = pool[0];
        pool.RemoveAt(0);
        entity.gameObject.SetActive(true);
        entity.transform.parent = null;
        entity.OnActivate();
        return entity;
      }

      Debug.Log($"Trying to instantiate \"{prefabName}\"");
      var prefab = Resources.Load($"Prefabs/{prefabName}");
      var go = Instantiate(prefab) as GameObject;
      try {
        var entity = go!.GetComponent<GameEntity>();
        if (entity == null) {
          throw new Exception("Entity component not found on prefab.");
        }
        entity.poolKey = prefabName;
        entity.OnActivate();
        return entity;
      } catch {
        Debug.Log($"Couldn't get Entity \"{prefabName}\" from Prefab");
        throw;
      }
    }

    public static void Return(GameEntity gameEntity) {
      if (gameEntity.poolKey == null) {
        throw new Exception("Entity does not have GameObjectPoolKey");
      }

      if (!GameObjectPool.ContainsKey(gameEntity.poolKey)) {
        throw new Exception($"Game Object Pool for \"{gameEntity.poolKey}\" does not exist");
      }
      var pool = GetPool(gameEntity);
      gameEntity.OnDeactivate();
      gameEntity.transform.parent = null;
      gameEntity.gameObject.SetActive(false);
      pool.Add(gameEntity);
    }
  }
}