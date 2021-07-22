
namespace WWIIVR {
    [System.Serializable]
    public class SaveData {

        private static SaveData instance;
        public static SaveData Current {
            get {
                if (instance == null) {
                    instance = new SaveData();
                }
                return instance;
            }
        }

        public PlayerProfile profile;


    }
}

