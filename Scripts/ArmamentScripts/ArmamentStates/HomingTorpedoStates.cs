using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace HomingTorpedoStates
{

    



    abstract class HomingTorpedoState_Base : StateBase
    {


        public HomingTorpedoTubeController controller;

        public StateMachineBase GetStateMachine()
        {
            return controller.GetStateMachine();
        }

        public void Transition(int next_)
        {
            GetStateMachine().Transition(next_);
        }
    }

    class HomingTorpedoState_Ready : HomingTorpedoState_Base
    {
        int frame = 0;
        public override void Initialize(GameObject parent)
        {
            //controller.justFired = false;
            frame = 0;
        }
        public override void Execute(GameObject parent)
        {
            if (frame > 0)
                controller.justFired = false;
            ++frame;
        }

        public override void End(GameObject parent)
        {

        }

    }

    class HomingTorpedoState_Target : HomingTorpedoState_Base
    {

        Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();     // Used for linking waveform textures
        Dictionary<int, GameObject>targets = new Dictionary<int, GameObject>();     // Used for linking gameObjects

        GameObject targetPanel;                                                     // Target selection panel. Retrieved from UIController
        GameObject[] uiObjs;                                                        // UI selection panels. Retrieved from UIController

        int frame = 0;
        int current_index = 0;      // Used for scrolling
        int selected_index = 0;     // Used for scrolling
        const int uiPanelLimit = 3; // Used for scrolling


        public override void Initialize(GameObject parent)
        {
            frame = 0;
            controller.isTargeting = true;
            current_index = selected_index = 0;


            // Prepare the targeting module
            UIController.Get().ActivateTargetingModule();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // In case there are no enemies at all
            if(enemies.Length <= 0)
            {
                Transition((int)HomingTorpedoStateMachine.StateEnum.Ready);
                return;
            }
            
            Vector3 direction = controller.GetFireDirection();
            direction.Normalize();

            // Targeting panel
            uiObjs = GameObject.FindGameObjectsWithTag("Targeting");

            foreach (GameObject obj in uiObjs)
            {
                obj.GetComponent<RawImage>().texture = new Texture2D(1280, 500);
            }


            RawImage rawImage = uiObjs[0].GetComponent<RawImage>();
            int width = rawImage.texture.width;
            int height = rawImage.texture.height;
            int index = 0;
            foreach (GameObject obj in enemies)
            {
                // Check distance
                if (Vector3.Distance(controller.transform.position, obj.transform.position) < MainController.Get().maximumTorpedoRange)
                    continue;
                Vector3 dist = obj.transform.position - controller.transform.position;
                AudioSource audioSource = obj.GetComponentInChildren<AudioSource>();

                // Check if within firing angle
                float angle = Vector3.Angle(direction, dist.normalized);
                if (angle > controller.aimAngle)
                    continue;

                // Generate a wave file
                textures.Add(index, MainController.Get().GenerateTextureFromAudio(audioSource, 1280, 500));
                targets.Add(index, obj);



                ++index;
            }

            // Apply waveform texture and apply to targeting panels
            if (textures.Count > 0)
                for (int ind = 0; ind < 3 && ind < textures.Count; ++ind)
                    uiObjs[ind].GetComponent<RawImage>().texture = textures[ind];
            else
            {
                Transition((int)HomingTorpedoStateMachine.StateEnum.Ready);
                controller.ClearState();
            }

            targetPanel = UIController.Get().GetTargetPanel();
            targetPanel.SetActive(true);

        }
        public override void Execute(GameObject parent)
        {
            // Just in case nothing is in view
            if (targets.Count < 1)
            {
                Transition((int)HomingTorpedoStateMachine.StateEnum.Ready);
                controller.ClearState();
                return;
            }

            Vector2 mouseDelta = Input.mouseScrollDelta;
            float delta = mouseDelta.y;

            // When scrolled
            if (mouseDelta.magnitude != 0)
            {

                // Only using the Y param of delta because we are only scrolling vertically
                selected_index += -(int)delta;


                //current_index = Mathf.Clamp(current_index, 0, targetPair.Count - 3);
                selected_index = Mathf.Clamp(selected_index, 0, textures.Count - 1);

                // Changes the rawImage texture when scrolling beyond the uiPanelCount limit
                if (selected_index > current_index + uiPanelLimit - 1 || selected_index < current_index)
                {
                    if (selected_index > current_index + uiPanelLimit - 1)
                        ++current_index;
                    else if (selected_index < current_index)
                        --current_index;

                    for (int ind = 0; ind < 3/*Targeting module Raw image count */ ; ++ind)
                    {
                        RawImage rimg = uiObjs[ind].GetComponent<RawImage>();
                        rimg.texture = textures[ind + current_index];
                    }
                }
            }

            targetPanel.transform.position = Vector3.Lerp(targetPanel.transform.position, uiObjs[selected_index - current_index].transform.position, 0.02f);
            MainController.Get().MuteAllExcept(targets[selected_index].GetComponentInChildren<AudioSource>().gameObject);


            // Because the transition is done within the same frame as when the mouse clicks
            // This will result in the GetMouseButtonDown(0) returning a true
            // So we wait until the next frame to perform this check
            if (frame <= 0)
            {
                ++frame;
                return;
            }
            if(Input.GetMouseButtonDown(0))       // when leftClicked
            {
                controller.SetTarget(targets[selected_index].transform);
                controller.justFired = true;

                Transition((int)HomingTorpedoStateMachine.StateEnum.Fire);
                return;
            }
            if(Input.GetMouseButtonDown(1))       // when rightClicked
            {
                controller.ClearState();
                Transition((int)HomingTorpedoStateMachine.StateEnum.Ready);
            }


        }

        public override void End(GameObject parent)
        {
            UIController.Get().DeactivateTargetingModule();
            targets.Clear();
            textures.Clear();
            controller.isTargeting = false;
            if (targetPanel != null)
                targetPanel.SetActive(false);
            MainController.Get().UnmuteAll();
        }

    }

    class HomingTorpedoState_Fire : HomingTorpedoState_Base
    {

        public override void Initialize(GameObject parent)
        {
            

            // Instantiate objects prepare the parameters 
            Transform transform = parent.transform;
            for (int i = 0; i < controller.barrel_count; ++i)
            {
                GameObject ord = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/HomingTorpedo"));
                Vector3 fire_pos = controller.GetFirePosition();
                fire_pos.x += (float)(-1 * 2.0f + i);
                fire_pos = Quaternion.Euler(transform.eulerAngles) * fire_pos;
                //fire_pos = transform.position + fire_pos;
                ord.transform.position = transform.position + fire_pos;
                //ord.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                ord.GetComponent<HomingTorpedoScript>().target = controller.target;

            }


            Transition((int)HomingTorpedoStateMachine.StateEnum.Reload);

        }
        public override void Execute(GameObject parent)
        {

        }

        public override void End(GameObject parent)
        {
            controller.ClearTarget();

        }

    }

    class HomingTorpedoState_Reload : HomingTorpedoState_Base
    {

        public Timer timer { get; private set; }
        public override void Initialize(GameObject parent)
        {
            controller.isReloading = true;
            if (timer == null)
                timer = new Timer(controller.reload_time);
            else
                timer.Reset();
        }

        public override void Execute(GameObject parent)
        {
            timer.Execute();
            if (timer.Done())
                Transition((int)HomingTorpedoStateMachine.StateEnum.Ready);
            if (timer.GetRemainingTime() < timer.GetTime())
                controller.justFired = false;
        }

        public override void End(GameObject parent)
        {
            timer.Reset();
            controller.isReloading = false;
        }
    }





}