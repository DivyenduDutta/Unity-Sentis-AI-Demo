using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Sentis;

public class DLModelInferencer : MonoBehaviour
{
    [SerializeField]
    private ModelAsset modelAsset;
    private Model runtimeModel;
        
    [SerializeField]
    private Texture2D catTexture;
    [SerializeField]
    private Texture2D dogTexture;

    private void Start(){

        // create a runtime model
        runtimeModel = ModelLoader.Load(modelAsset);
        
        Debug.Log($"Are compute shaders supported on this platform? {SystemInfo.supportsComputeShaders}");
        
        // create appropriate sized tensor for input to DL model for inference
        Tensor catTensor = TextureConverter.ToTensor(catTexture, width:224, height:224, channels:3);
        Tensor dogTensor = TextureConverter.ToTensor(dogTexture, width:224, height:224, channels:3);
        
        Debug.Log("Inferring on dog picture");
        InferOnImage(dogTensor);    
        
        Debug.Log("Inferring on cat picture");
        InferOnImage(catTensor);  
    }

    private void InferOnImage(Tensor inputTensor){
        
        // create an engine to run the model
        IWorker m_Worker = WorkerFactory.CreateWorker(BackendType.GPUCompute, runtimeModel, verbose:false);
        //Debug.Log(catTensor.shape); //shape is (1,3,224,224)
        
        // run the model
        m_Worker.Execute(inputTensor);
        
        // get inference results from model and display them
        TensorFloat output = m_Worker.PeekOutput() as TensorFloat; //no need to dispose this
        //download the tensor to the CPU to display it
        output.MakeReadable();
        
        Debug.Log($"Is cat?: {output[0] < output[1]} with prob: Dog - {output[0]:P3} and Cat - {output[1]:P3}");
        //output.PrintDataPart(10);

        m_Worker.Dispose(); //this disposes the tensor float output shallow copy as well
        inputTensor.Dispose();
    }
    private Texture2D Resize(Texture2D source, int newWidth, int newHeight)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Point;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D nTex = new Texture2D(newWidth, newHeight);
        nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0,0);
        nTex.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return nTex;
    }
}
