
using Engine.Core;
using Engine.Mechanics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Engine.Core.Simulation;

namespace Engine
{
    [System.Serializable]
    public class Game : MonoBehaviour
    {
        public Valve.VR.InteractionSystem.Player player;
        private string activeScene;
        public AudioSource audioSource;
        public AudioClip[] audioClipArray;

        void Awake()
        {
            Debug.Log("Engine.Game.Awake(" + SceneManager.GetActiveScene().name + ")");
            if (Model.application == null)
            {
                Model.application = new App();
            }
            Model.Game = this;
            activeScene = Model.application.state["activeScene"].getString();
            if (activeScene == Scene.Mansion.ToString())
            {
                Scenes.MansionScene.Load();
            }
            Model.application.render();
        }

        void Start()
        {
            bool isStarted = Model.application.state["started"].getBool();
            if ((SceneManager.GetActiveScene().name != Scene.Loading.ToString() || !isStarted) && audioClipArray != null && audioClipArray.Length > 0)
            {
                audioSource.PlayOneShot(audioClipArray[0]);
                Cursor.visible = false;
            }
            if (activeScene != Scene.Loading.ToString())
            {
                if (!isStarted)
                {
                    Model.application.setState(Model.startGameAction());
                }
            } else
            {
                Model.application.render();
            }
            Simulation.Schedule<Events.ZoomInCamera>();
        } 

        public bool isIdle()
        {
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-idle")
                {
                    return true;
                }
            }
            return false;
        }

        public void ChangeScene(string targetScene)
        {
            if (Model.application.DEBUG)
            {
                Debug.Log("Engine.Game.ChangeScene(" + targetScene + ")");
            }
            string currentSceneName = SceneManager.GetActiveScene().name;
            bool flag = true;
            if (targetScene != "Loading")
            {
                if (currentSceneName != "Loading")
                {
                    flag = false;
                    Schedule<Events.SceneChange>(1.5f).targetScene = targetScene;
                    Model.application.setState(
                        Model.mergeActions(
                            new List<Dictionary<string, Prop>>() {
                                Model.setSceneAction(Scene.Loading.ToString()),
                                Model.toggleLoadingAction(true)
                            }
                        )
                    );
                }
            }
            if (flag)
            {
                Model.application.setState(
                    Model.mergeActions(
                        new List<Dictionary<string, Prop>>() {
                            Model.setSceneAction(targetScene),
                            Model.toggleLoadingAction(false)
                        }
                    )
                );
            }
        }

        public IEnumerator zoomOutCamera(Camera camera, int from, int to, float delay, Func<Camera, int> callback = null)
        {
            yield return new WaitForSecondsRealtime(delay);
            camera.fieldOfView = from;
            if (from < to)
            {
                yield return zoomOutCamera(camera, from + 2, to, delay, callback);
            }
            else
            {
                StopCoroutine(Model.coroutines["zoomOutCamera"]);
                if (callback != null)
                {
                    callback(camera);
                }
            }
        }

        public IEnumerator zoomInCamera(Camera camera, int from, int to, float delay, Func<Camera, int> callback = null)
        {
            yield return new WaitForSecondsRealtime(delay);
            camera.fieldOfView = from;
            if (from > to)
            {
                yield return zoomInCamera(camera, from - 2, to, delay, callback);
            }
            else
            {
                StopCoroutine(Model.coroutines["zoomInCamera"]);
                if (callback != null)
                {
                    callback(camera);
                }
            }
        }

        void Update()
        {
            if (Model.application.DEBUG && Model.application.DEEP_DEBUG)
            {
                Debug.Log("Engine.Game.Update()");
            }
            /*
            if (Model.fpsScene != null && Model.fpsScene["FPS.Enemies.Robot"] != null)
            {
                (Model.fpsScene["FPS.Enemies.Robot"] as Components.Enemy).Update();
            }
            if (Model.fpsScene != null && Model.fpsScene["FPS.Enemies.Turret"] != null)
            {
                (Model.fpsScene["FPS.Enemies.Turret"] as Components.Enemy).Update();
            }
            bool isStarted = Model.application.state["started"].getBool() == true;
            bool isPaused = Model.application.state["paused"].getBool() == true;
            bool isDead = Model.application.state["dead"].getBool() == true;
            if (Input.GetButtonUp("Jump"))
            {
                if (!isStarted)
                {
                    Model.application.setState(Model.startGame());
                } else if (isDead)
                {
                    Model.application.setState(Model.revivePlayer());
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetButtonDown("Menu"))
            {
                if (isStarted && !isDead)
                {
                    if (!isPaused)
                    {
                        Model.application.setState(Model.pauseGame());
                    } else
                    {
                        Model.application.setState(Model.continueGame());
                    }
                }
            }
            */
        }
    }
}