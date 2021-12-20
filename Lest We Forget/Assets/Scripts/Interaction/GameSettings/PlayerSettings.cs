namespace LWF.Settings {

    [System.Serializable]
    public class PlayerSettings {
        public bool continuousTurn; // determine if we should use snap or continuous turn
        public bool graphicContentEnabled; // turn graphic content on/off
        public string requestedMovie; // Most recent movie selection player has requested to view in the theater
    }
}
