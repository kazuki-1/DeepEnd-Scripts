using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmamentController : MonoBehaviour
{
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    /*---------------------------------Variables----------------------------*/
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    public enum Armaments
    {
        Cannon,
        AimedTorpedo,
        HomingTorpedo
    }

    public class ArmamentList
    {
        public List<ArmamentBase> list;
        public Armaments type;
        public ArmamentList() { list = new List<ArmamentBase>(); }
        public bool fired = false;
    }


    // Start is called before the first frame update
    public List<ArmamentList> armaments { get; private set; } = new List<ArmamentList>();

    public int cannonReloadTime = 10;
    public int aimedTorpedoReloadTime = 30;
    public int homingTorpedoReloadTime = 45;



    KeyCode prev_key;
    KeyCode cur_key;
    int key_count;
    bool prev = false;

    Armaments current_armament;
    bool DoubleTapped(KeyCode key)
    {
        bool keydown = Input.GetKeyDown(key);
        prev = keydown;
        return prev == keydown;

    }


    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    /*---------------------------------Functions----------------------------*/
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/


    public virtual void Start()
    {
        ArmamentList cannons =      new ArmamentList();
        cannons.type = Armaments.Cannon;

        ArmamentList aimedTubes =   new ArmamentList();
        aimedTubes.type = Armaments.AimedTorpedo;

        ArmamentList homingTubes =  new ArmamentList();
        homingTubes.type = Armaments.HomingTorpedo;

        foreach (CannonController cannon in GetComponentsInChildren<CannonController>())
            cannons.list.Add(cannon);

        foreach (AimedTorpedoTubeController tubes in GetComponentsInChildren<AimedTorpedoTubeController>())
            aimedTubes.list.Add(tubes);

        foreach (HomingTorpedoTubeController tubes in GetComponentsInChildren<HomingTorpedoTubeController>())
            homingTubes.list.Add(tubes);


        if (cannons.list.Count > 0)
            armaments.Add(cannons);
        if (aimedTubes.list.Count > 0)
            armaments.Add(aimedTubes);
        if (homingTubes.list.Count > 0)
            armaments.Add(homingTubes);



        current_armament = Armaments.Cannon;


        


    }

    // Update is called once per frame
    public virtual void Update()
    {
        SelectArmament();
        FireArmaments();
        RotateArmament();
        RenderArmamentTargetingZone();
    }

    public virtual void FireArmaments()
    {
        if(Input.GetMouseButtonDown(0))             // Left click
        {
            ArmamentList cur = armaments[(int)current_armament];
            foreach (ArmamentBase armament in cur.list)
            {
                //HomingTorpedoTubeController entity = (armament as HomingTorpedoTubeController);
                if (current_armament == Armaments.HomingTorpedo)
                {

                    foreach (ArmamentBase arm in cur.list)
                    {
                        HomingTorpedoTubeController entity = (arm as HomingTorpedoTubeController);
                        if (entity.justFired || entity.isTargeting)
                        {
                            cur.fired = true;
                            break;
                        }
                        cur.fired = false;
                    }
                }
                if (armament.GetState() && !cur.fired)
                {
                    armament.Fire();
                    break;
                }

            }

        }
    }

    protected void RenderArmamentTargetingZone()
    {
        TargetZoneController target = GetComponent<TargetZoneController>();
        ArmamentList cur = armaments[(int)current_armament];
        foreach (ArmamentBase armament in cur.list)
        {
            if (armament.GetState())
            {
                target.Enable();    
                target.SetPosition(armament);
                break;
            }
            else/* if (!armament.isReloading && armament.outOfBounds)*/
                target.Disable();
            //else
            //    target.Disable();
        }
    }


    protected void SelectArmament()
    {


        
        if (Input.GetKeyDown(KeyCode.Alpha1))
            cur_key = KeyCode.Alpha1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            cur_key = KeyCode.Alpha2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            cur_key = KeyCode.Alpha3;


        // Checks if double tapped
        if (Input.anyKeyDown)
        {
            if (cur_key != KeyCode.None && prev_key != KeyCode.None)
            {

                if (prev_key == cur_key)        // If cur key matches last
                    ++key_count;
                else                            // If it doesn't
                    key_count = 0;
            }
        }

        if (key_count > 0)
        {
            switch (cur_key)
            {
                case KeyCode.Alpha1:
                    current_armament = Armaments.Cannon;
                    break;
                case KeyCode.Alpha2:
                    current_armament = Armaments.AimedTorpedo;
                    break;
                case KeyCode.Alpha3:
                    current_armament = Armaments.HomingTorpedo;
                    break;

            }
            key_count = 0;

        }
        prev_key = cur_key;


    }

    public virtual void RotateArmament()
    {
        foreach(ArmamentBase armament in armaments[(int)current_armament].list)
        {
            armament.RotateToPoint(GetDirection());
        }
    }
    public Armaments GetCurrentArmament()
    {
        return current_armament;
    }

    public void ReloadAll()
    {
        foreach (ArmamentList cur in armaments)
        {
            foreach (ArmamentBase armament in cur.list)
                armament.Reload();
        }
    }

    public Vector3 GetDirection()
    {

        Transform player_transform = GameObject.Find("DeepEndPlayer").transform;

        Vector3 pos = Camera.main.transform.position;
        Vector3 dist = player_transform.position - pos;
        dist.y = 0.0f;
        pos = player_transform.position + dist.normalized * 200.0f;
        pos.y = 0.0f;

        return pos;

    }

}
