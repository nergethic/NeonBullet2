using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlitRenderFeature : ScriptableRendererFeature {
    [System.Serializable]
    public class MyFeatureSettings {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material FluidMaterialToBlit;
        public Material PixelizeMaterialToBlit;
    }
    public MyFeatureSettings settings = new MyFeatureSettings();
    
    PixelizePass pixelizePass;
    FluidPass fluidPass;
    public override void Create() {
        pixelizePass = new PixelizePass(
            "Pixelize custom pass",
            settings.WhenToInsert,
            settings.PixelizeMaterialToBlit
        );
        
        fluidPass = new FluidPass(
            "Fluid custom pass",
            settings.WhenToInsert,
            settings.FluidMaterialToBlit
        );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        //var cameraColorTargetIdent = renderer.cameraColorTarget;
        //pixelizePass.Setup(renderer, cameraColorTargetIdent);
        renderer.EnqueuePass(fluidPass);
        renderer.EnqueuePass(pixelizePass);
    }
}
