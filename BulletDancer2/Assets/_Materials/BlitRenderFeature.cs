using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlitRenderFeature : ScriptableRendererFeature {
    [System.Serializable]
    public class MyFeatureSettings
    {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material MaterialToBlit;
    }
    public MyFeatureSettings settings = new MyFeatureSettings();
    
    PixelizePass pixelizePass;
    public override void Create() {
        pixelizePass = new PixelizePass(
            "My custom pass",
            settings.WhenToInsert,
            settings.MaterialToBlit
        );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        //var cameraColorTargetIdent = renderer.cameraColorTarget;
        //pixelizePass.Setup(renderer, cameraColorTargetIdent);
        renderer.EnqueuePass(pixelizePass);
    }
}
