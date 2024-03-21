using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class TestRenderEnumValue : MonoBehaviour
{
    public BlendOp blendOp;
    public float blendOpValue;

    public BlendMode blendMode;
    public float blendModeValue;

    public RenderQueue renderQueue;
    public int renderQueueValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        blendOpValue = (float)blendOp;
        blendModeValue = (float)blendMode;
        renderQueueValue = (int)renderQueue;
    }
}
