using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load texture files from bundles and set them to all required materials. Perhaps consider making one for each room?
/// </summary>
public class TextureLoader : MonoBehaviour
{
    [Header("DO NOT MOVE OR REMOVE OR YOU WILL DIE")]
    public BundleManager assetManager;

    [Header("Materials")]
    public Material plainmat;
    public Material Walls1;
    public Material Floor_1;
    public Material FloodLightBase;


    void Start()
    {
        //Load textures into respective materials

        //Room1
        {
            //PlainMat
            Texture albedoPM = assetManager.GetAsset<Texture>("plainmat", "Albedo");
            Texture normalsPM = assetManager.GetAsset<Texture>("plainmat", "Normal");
            Texture hmapPM = assetManager.GetAsset<Texture>("plainmat", "HeightMap");
            Texture ambientoccPM = assetManager.GetAsset<Texture>("plainmat", "AmbOc");
            Texture dmaskPM = assetManager.GetAsset<Texture>("plainmat", "DetMask");

            plainmat.SetTexture("_MainTex", albedoPM);
            plainmat.SetTexture("_BumpMap", normalsPM);
            plainmat.SetTexture("_ParallaxMap", hmapPM); /*wasted 30 minutes looking for this bugger*/
            plainmat.SetTexture("_OcclusionMap", ambientoccPM);
            plainmat.SetTexture("_DetailMask", dmaskPM);

            //Walls1
            Texture albedoW1 = assetManager.GetAsset<Texture>("walls1", "Albedo");
            Texture normalsW1 = assetManager.GetAsset<Texture>("walls1", "Normal");
            Texture hmapW1 = assetManager.GetAsset<Texture>("walls1", "HeightMap");
            Texture ambientoccW1 = assetManager.GetAsset<Texture>("walls1", "AmbOc");
            Texture emisW1 = assetManager.GetAsset<Texture>("walls1", "Glow");

            Walls1.SetTexture("_MainTex", albedoW1);
            Walls1.SetTexture("_BumpMap", normalsW1);
            Walls1.SetTexture("_ParallaxMap", hmapW1);
            Walls1.SetTexture("_OcclusionMap", ambientoccW1);
            Walls1.SetTexture("_EmissionMap", emisW1);

            //Floor_1
            /*Why load new bundle when old bundle have correct stuff?*/
            Texture emisF1 = assetManager.GetAsset<Texture>("floor_1", "Glow");

            Floor_1.SetTexture("_MainTex", albedoPM);
            Floor_1.SetTexture("_BumpMap", normalsPM);
            Floor_1.SetTexture("_ParallaxMap", hmapPM);
            Floor_1.SetTexture("_OcclusionMap", ambientoccPM);
            Floor_1.SetTexture("_DetailMask", dmaskPM);
            Floor_1.SetTexture("_EmissionMap", emisF1);

            //FloodLightbase
            Texture albedoFLB = assetManager.GetAsset<Texture>("floodlightbase", "Albedo");
            Texture normalsFLB = assetManager.GetAsset<Texture>("floodlightbase", "Normal");

            FloodLightBase.SetTexture("_MainTex", albedoFLB);
            FloodLightBase.SetTexture("_BumpMap", normalsFLB);
            FloodLightBase.SetTexture("_EmissionMap", emisF1);
        }
    }
}
