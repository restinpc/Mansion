
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

        public Vector3[] endPosition;
        public Quaternion[] endRotation;
        public float[] endMoveSpeed;
        public float[] endRotationSpeed;
        public int step = 0;
        public bool cinematic;
        public GameObject obj;

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
            int size = 6;
            this.endRotationSpeed = new float[size];
            this.endMoveSpeed = new float[size];
            this.endPosition = new Vector3[size];
            this.endRotation = new Quaternion[size]; 
            this.obj = GameObject.Find("FallbackObjects");

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
            if (Input.GetButtonUp("Jump"))
            {
                this.obj = GameObject.Find("FallbackObjects");

                Debug.Log(this.obj.gameObject.transform.rotation);
                Debug.Log(this.obj.gameObject.transform.position);

                this.obj.gameObject.transform.position = new Vector3(-14.27f, 1.12f, 2.54f);
                this.obj.gameObject.transform.rotation = new Quaternion(0.00751f, -0.50831f, -0.00444f, -0.86113f);

                IEnumerator myWaitCoroutine() { 
                    yield return new WaitForSeconds(1f);
                    cinematic = true;
                    
                    this.endPosition[0] = new Vector3(-2.13f, 1.70f, 16.21f);
                    this.endRotation[0] = new Quaternion(0, 0.99692f, -0.03481f, -0.07017f);
                    this.endRotationSpeed[0] = 28;
                    this.endMoveSpeed[0] = 4;

                    this.endPosition[1] = new Vector3(-20.69f, 12.16f, -8.33f);
                    this.endRotation[1] = new Quaternion(-0.08610f, -0.43729f, 0.04211f, -0.89420f);
                    this.endRotationSpeed[1] = 33;
                    this.endMoveSpeed[1] = 8;

                    /*
                     * (-0.08610, -0.43729, 0.04211, -0.89420)
(-20.69, 12.16, -8.33)
UnityEngine.Debug:Log (object)
Engine.Game:Update () (at Assets/Engine/Scripts/Game.cs:170)



                     */

                    this.endPosition[2] = new Vector3(-10.96f, 16.44f, 32.52f);
                    this.endRotation[2] = new Quaternion(-0.02254f, -0.97570f, 0.18084f, -0.12162f);
                    this.endRotationSpeed[2] = 27;
                    this.endMoveSpeed[2] = 10;

                    this.endPosition[3] = new Vector3(35.86f, 17.66f, 30.54f);
                    this.endRotation[3] = new Quaternion(-0.05272f, 0.94121f, -0.17444f, -0.28445f);
                    this.endRotationSpeed[3] = 10.5f;
                    this.endMoveSpeed[3] = 10;

                    this.endPosition[4] = new Vector3(47.43f, 8.42f, -1.20f);
                    this.endRotation[4] = new Quaternion(-0.00618f, 0.70591f, -0.00616f, -0.70825f);
                    this.endRotationSpeed[4] = 17;
                    this.endMoveSpeed[4] = 10;

                    this.endPosition[5] = new Vector3(25.96f, 1.33f, -1.94f);
                    this.endRotation[5] = new Quaternion(-0.01846f, 0.70862f, -0.01856f, -0.70510f);
                    this.endRotationSpeed[5] = 1;
                    this.endMoveSpeed[5] = 8;

                    yield return new WaitForSeconds(4.9f);
                    step = 1;
                    yield return new WaitForSeconds(4.8f);
                    step = 2;
                    yield return new WaitForSeconds(4.8f);
                    step = 3;
                    yield return new WaitForSeconds(4.9f);
                    step = 4;
                    yield return new WaitForSeconds(4f);
                    step = 5;
                    yield return new WaitForSeconds(5f);
                    cinematic = false;
                }
                StartCoroutine(myWaitCoroutine());
            }
            if (cinematic)
            {
                this.obj.transform.position = Vector3.MoveTowards(this.obj.transform.position, endPosition[step], endMoveSpeed[step] * Time.deltaTime);
                this.obj.transform.rotation = Quaternion.RotateTowards(this.obj.transform.rotation, endRotation[step], endRotationSpeed[step] * Time.deltaTime);
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