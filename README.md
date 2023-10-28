#### Demo of using Unity Sentis (Open Beta) to perform inference for image classification task

A pretrained resnet model was converted to the ONNX format using Pytorch's internal ONNX converter. This model performs image classification. In the demo we test it by inferencing on a cat and a dog image.

##### General steps to run inference on an ML model
- import the ONNX file into Unity 
- [create a runtime model](https://docs.unity3d.com/Packages/com.unity.sentis@1.2/manual/import-a-model-file.html) (ie, representation of the model in Unity Sentis)
- [create an engine to run the model](https://docs.unity3d.com/Packages/com.unity.sentis@1.2/manual/create-an-engine.html)
- [run the model to get output](https://docs.unity3d.com/Packages/com.unity.sentis@1.2/manual/run-a-model.html)
- [get and print output from the model](https://docs.unity3d.com/Packages/com.unity.sentis@1.2/manual/get-the-output.html)

##### Inference Result
![[images/Inference Results.png]]