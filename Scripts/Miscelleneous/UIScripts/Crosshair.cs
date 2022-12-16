using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crosshair : MonoBehaviour
{
    
    public ArmamentBase armament;

    public Vector2 idlePosition;
    Vector2 size;
    Vector2 center;
    //bool prevState_ = false;

    // Start is called before the first frame update
    void Start()
    {
        size = GetComponent<RectTransform>().rect.size;
        RawImage rawImage=  GetComponent<RawImage>();
        
        size = new Vector2((float)rawImage.texture.width, (float)rawImage.texture.height);


        center = new Vector2(Screen.width / 2, Screen.height / 2);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateColour();
        UpdatePosition();
    }

    void UpdateColour()
    {
        RawImage rawImage= GetComponent<RawImage>();

        bool curState_ = armament.GetState();
        switch(curState_)
        {
            // Armament not ready
            case false:
                rawImage.color = Color.Lerp(rawImage.color, Color.yellow, 0.05f);
                break;

            // Armament ready
            case true:
                rawImage.color = Color.Lerp(rawImage.color, Color.green, 0.05f);
                break;


        }
        {
            //if(prevState_ != curState_)
            //{
            //    switch (curState_)
            //    {
            //        // armament not ready
            //        case 0:
            //            rawImage.color = Color.Lerp()
            //
            //    }
            //
            //}
            //
            //
            //Color32[] pixels = new Color32[(int)size.x * (int)size.y];
            //Texture2D tex = (Texture2D)GetComponent<RawImage>().texture;
            //pixels = tex.GetPixels32();
            //if (prevState_ != armament.GetState())
            //{
            //    for (int index = 0; index < (int)(size.x * size.y); ++index)
            //    {
            //
            //        // Changes colour of crosshair in response to out of bounds or not
            //        if (!pixels[index].Equals(MainController.Colours.blank))
            //        {
            //            if (armament.GetState())
            //                pixels[index] = MainController.Colours.green;
            //            else
            //                pixels[index] = MainController.Colours.yellow;
            //        }
            //
            //    }
            //    tex.SetPixels32(pixels);
            //    tex.Apply();
            //}
        }
    }
    void UpdatePosition()
    {

        RectTransform rect = GetComponent<RectTransform>();

        Vector3 screenForward = Camera.main.transform.forward;
        screenForward.y = 0;
        screenForward.Normalize();

        Vector3 armamentDirection = armament.transform.forward;
        armamentDirection.y = 0;
        armamentDirection.Normalize();

        float angle = Vector3.SignedAngle(screenForward, armamentDirection, Vector3.up);
        if (angle > 90.0f)
            rect.position = idlePosition;
        else
        {
            float influence = angle / 90.0f;
            Vector2 offset = center * influence;
            offset.y = 250;
            rect.position = center + offset;
        }


    }
}
