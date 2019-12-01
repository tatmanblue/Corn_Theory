using UnityEngine;
using UnityEngine.SceneManagement;

using CornTheory.Missions;
using CornTheory.Player;

namespace CornTheory
{
    public class SceenSwitcher : MonoBehaviour
    {
        [SerializeField] Texture2D defaultCursor = null;
        [SerializeField] Vector2 defaultCursorHotspot = new Vector2(0, 0);
        [SerializeField] Texture2D busyCursor = null;
        [SerializeField] Vector2 busyCursorHotspot = new Vector2(0, 0);

        [Header("Scene Change Options")]
        [SerializeField] bool showMissionsSceneStart = false;
        [SerializeField] float showDelay = 0.75f;
        [SerializeField] Material skyBox = null;

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            print("scene loaded: " + scene.name);
            SetSkyBox(scene);
            InvokeMissionPopup(scene);
            SetCursor();
        }

        private void SetSkyBox(Scene scene)
        {
            if (null == skyBox)
                return;

            RenderSettings.skybox = skyBox;
        }

        private void InvokeMissionPopup(Scene scene)
        {
            if ((scene.name == Constants.MainWorldScene && true == showMissionsSceneStart)
                || scene.name == Constants.DevPlayArenaScene)
            {
                PlayerState.Instance.GameState = GameUIState.InWorld;
                var missionManager = FindObjectOfType<MissionManager>();
                missionManager.Invoke("SceneOpened", showDelay);
            }
        }

        private void SetCursor()
        {
            if (null == defaultCursor)
                return;

            Cursor.SetCursor(defaultCursor, defaultCursorHotspot, CursorMode.Auto);
        }

        private void Awake()
        {
            print("SceneSwitcher.awake() called");
            print("adding event handler to SceneManager");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void Start() { }

        // called when the game is terminated
        void OnDisable()
        {
            print("SceneSwitcher.OnDisable() called");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void GotoMainMenu()
        {
            Cursor.SetCursor(busyCursor, busyCursorHotspot, CursorMode.Auto);
            // Changing state here assuming that we are coming from the world scenes
            PlayerState.Instance.GameState = GameUIState.AtMain;
            SceneManager.LoadScene(Constants.OpenMenuScene);
        }

        public void GotoMainScene()
        {
            // TODO: eval if it is right to not change state here
            // DONT CHANGE STATE HERE
            SceneManager.LoadScene(Constants.MainWorldScene);
        }

        public void GotoPlayArena()
        {
            // TODO: this is only a dev thing and shouldn't be in production
            SceneManager.LoadScene(Constants.DevPlayArenaScene);
        }

        public void Quit()
        {
            Cursor.SetCursor(busyCursor, busyCursorHotspot, CursorMode.Auto);
            Application.Quit();
        }
    }
}
