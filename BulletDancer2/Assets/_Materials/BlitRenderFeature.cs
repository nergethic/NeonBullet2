using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlitRenderFeature : ScriptableRendererFeature {
    [System.Serializable]
    public class MyFeatureSettings {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material FluidMaterialToBlit;
        public Material DistortionMaterialToBlit;
        public Material PixelizeMaterialToBlit;
    }
    public MyFeatureSettings settings = new MyFeatureSettings();
    
    FluidPass fluidPass;
    DistortionPass distortionPass;
    PixelizePass pixelizePass;

    public override void Create() {
        fluidPass = new FluidPass(
            "Fluid custom pass",
            settings.WhenToInsert,
            settings.FluidMaterialToBlit
        );
        
        distortionPass = new DistortionPass(
            "Distortion custom pass",
            settings.WhenToInsert,
            settings.DistortionMaterialToBlit
        );
        
        pixelizePass = new PixelizePass(
            "Pixelize custom pass",
            settings.WhenToInsert,
            settings.PixelizeMaterialToBlit
        );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        //var cameraColorTargetIdent = renderer.cameraColorTarget;
        //pixelizePass.Setup(renderer, cameraColorTargetIdent);
        renderer.EnqueuePass(fluidPass);
        renderer.EnqueuePass(distortionPass);
        renderer.EnqueuePass(pixelizePass);
    }
}
