using UnityEngine;
using System.Collections;

public class RippleEffect : MonoBehaviour {
    [SerializeField] AnimationCurve waveform;

    [Range(0.01f, 1.0f)]
    public float refractionStrength = 0.5f;

    public Color reflectionColor = Color.gray;

    [Range(0.01f, 1.0f)]
    public float reflectionStrength = 0.7f;

    [Range(1.0f, 5.0f)]
    public float waveSpeed = 1.25f;

    [Range(0.0f, 2.0f)]
    public float dropInterval = 0.5f;

    //[SerializeField] Shader shader;
    [SerializeField] Material material;
    class Droplet
    {
        Vector2 position;
        float time;

        public Droplet()
        {
            time = 1000;
        }

        public void Reset(Vector2 pos)
        {
			position = pos;
            time = 0;
        }

        public void Update()
        {
            time += Time.deltaTime * 2;
        }

        public Vector4 MakeShaderParameter(float aspect)
        {
            return new Vector4(position.x * aspect, position.y, time, 0);
        }
    }

    Droplet[] droplets;
    Texture2D gradTexture;
    Camera cam;
    float timer;
    int dropCount;

    public void Init(Camera cam) {
        this.cam = cam;
        
        droplets = new Droplet[3];
        droplets[0] = new Droplet();
        droplets[1] = new Droplet();
        droplets[2] = new Droplet();

        gradTexture = new Texture2D(2048, 1, TextureFormat.Alpha8, false);
        gradTexture.wrapMode = TextureWrapMode.Clamp;
        gradTexture.filterMode = FilterMode.Bilinear;
        for (var i = 0; i < gradTexture.width; i++)
        {
            var x = 1.0f / gradTexture.width * i;
            var a = waveform.Evaluate(x);
            gradTexture.SetPixel(i, 0, new Color(a, a, a, a));
        }
        gradTexture.Apply();
        
        material.hideFlags = HideFlags.None;
        material.SetTexture("_GradTex", gradTexture);
        //InvokeRepeating("Emitt", 2f, 2f);
        
        UpdateShaderParameters();
    }
    
    public void Emit(Vector2 pos) {
        droplets[dropCount++ % droplets.Length].Reset(pos);
    }

    void UpdateShaderParameters()
    {
        material.SetVector("_Drop1", droplets[0].MakeShaderParameter(cam.aspect));
        material.SetVector("_Drop2", droplets[1].MakeShaderParameter(cam.aspect));
        material.SetVector("_Drop3", droplets[2].MakeShaderParameter(cam.aspect));

        material.SetColor("_Reflection", reflectionColor);
        material.SetVector("_Params1", new Vector4(cam.aspect, 1, 1 / waveSpeed, 0));
        material.SetVector("_Params2", new Vector4(1, 1 / cam.aspect, refractionStrength, reflectionStrength));
    }

    void Emitt() {
        Emit(new Vector2(0.5f, 0.5f));
    }

    void Update()
    {
        if (dropInterval > 0)
        {
            timer += Time.deltaTime;
            while (timer > dropInterval)
            {
                //Emit();
                timer -= dropInterval;
            }
        }

        foreach (var d in droplets)
            d.Update();

        UpdateShaderParameters();
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(.3f);

    }

}