using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInterface : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField]
    Texture2D waveformTexture;

    //RenderTexture renderTexture;

    private void Awake()
    {
        //Graphics.Blit(waveformTexture, renderTexture);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Texture2D GetTexture()
    {
        return waveformTexture;
    }

    //public RenderTexture GetRenderTexture()
    //{
    //    return renderTexture;
    //}
}
