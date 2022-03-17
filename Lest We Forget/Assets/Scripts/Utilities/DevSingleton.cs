using UnityEngine;

public class DevSingleton<T> : MonoBehaviour where T : Component {

    private static T instance;

    public static T Instance {
        get {
            if (instance == null) {
                var objs = FindObjectsOfType(typeof(T)) as T[];

                Debug.Log($"{objs.Length} instances of {typeof(T).Name} found");

                if (objs.Length > 0) instance = objs[0];

                if (instance == null) {
                    GameObject obj = new() {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}

public class DevSingletonPersistent<T> : MonoBehaviour where T : Component {
    public static T Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance == null) {
            Instance = this as T;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }
    }
}