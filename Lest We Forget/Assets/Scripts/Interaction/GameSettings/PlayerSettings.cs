namespace LWF.Settings {

    [System.Serializable]
    public class PlayerSettings {
        public bool graphicContentEnabled; // turn graphic content on/off
        public string requestedMovie; // Most recent movie selection player has requested to view in the theater
        public bool firstLaunch; // used to determine if the app has already been launmched. set to false  when application quits
    }
}
