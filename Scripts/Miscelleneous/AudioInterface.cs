using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInterface : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField]
    Texture2D waveformTexture;

    public AK.Wwise.Event SFXEvent;
    public AK.Wwise.Event pauseEvent;

    bool isPosted = false;

    private void Awake()
    {
        //Graphics.Blit(waveformTexture, renderTexture);
    }
    void Start()
    {
        //Post();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Texture2D GetTexture()
    {
        return waveformTexture;
    }

    public void Post()
    {
        if (isPosted)
            return;
        SFXEvent.Post(GetComponentInParent<DeepEndEntityController>().gameObject);
        isPosted = true;

    }   

    public void Pause()
    {
        if (!isPosted)
            return;
        pauseEvent.Post(GetComponentInParent<DeepEndEntityController>().gameObject);
        isPosted = false;


    }

    //public RenderTexture GetRenderTexture()
    //{
    //    return renderTexture;
    //}
}
