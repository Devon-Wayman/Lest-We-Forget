// Author: Devon Wayman

/// <summary>
/// Primarily used for object pooling water explosions for DDay scene
/// </summary>
namespace WWIIVR.Interfaces {
    public interface IPooledObject {
        void OnObjectSpawn ();
    }
}