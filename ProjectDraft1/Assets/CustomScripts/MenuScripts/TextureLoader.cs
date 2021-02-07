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
    public Material Floor1;
    public Material FloodLightBase;
    public Material OilDrum;
    public Material Ceiling2;
    public Material Floor2;
    public Material Walls2;
    public Material Ceiling3;
    public Material DessertRoomWalls;
    public Material Floor3;
    public Material Walls3;
    public Material RainBarrel;
    public Material RainRoomWalls;
    public Material ShieldOver;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //Load textures into respective materials

        //Room1
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

        Floor1.SetTexture("_MainTex", albedoPM);
        Floor1.SetTexture("_BumpMap", normalsPM);
        Floor1.SetTexture("_ParallaxMap", hmapPM);
        Floor1.SetTexture("_OcclusionMap", ambientoccPM);
        Floor1.SetTexture("_DetailMask", dmaskPM);
        Floor1.SetTexture("_EmissionMap", emisF1);

        //FloodLightbase
        Texture albedoFLB = assetManager.GetAsset<Texture>("floodlightbase", "Albedo");
        Texture normalsFLB = assetManager.GetAsset<Texture>("floodlightbase", "Normal");

        FloodLightBase.SetTexture("_MainTex", albedoFLB);
        FloodLightBase.SetTexture("_BumpMap", normalsFLB);
        FloodLightBase.SetTexture("_EmissionMap", emisF1);
        

        //Room2
        //OilDrum
        Texture albedo2OD = assetManager.GetAsset<Texture>("oildrum", "Albedo2");
        Texture normalsOD = assetManager.GetAsset<Texture>("oildrum", "Normal");
        Texture dmaskOD = assetManager.GetAsset<Texture>("oildrum", "DetMask");

        OilDrum.SetTexture("_DetailAlbedoMap", albedo2OD);
        OilDrum.SetTexture("_BumpMap", normalsOD);
        OilDrum.SetTexture("_DetailMask", dmaskOD);

        //Ceiling2
        Texture ambientoccC2 = assetManager.GetAsset<Texture>("ceiling2", "AmbOc");

        Ceiling2.SetTexture("_OcclusionMap", ambientoccC2);
        Ceiling2.SetTexture("_BumpMap", normalsPM);

        //Floor2
        Texture normalsF2 = assetManager.GetAsset<Texture>("floor_2", "Normal");
        Texture hmapF2 = assetManager.GetAsset<Texture>("floor_2", "HeightMap");
        Texture ambientoccF2 = assetManager.GetAsset<Texture>("floor_2", "AmbOc");

        Floor2.SetTexture("_BumpMap", normalsF2);
        Floor2.SetTexture("_ParallaxMap", hmapF2);
        Floor2.SetTexture("_OcclusionMap", ambientoccF2);

        //Walls2
        Texture emisW2 = assetManager.GetAsset<Texture>("walls_2", "Glow");

        Walls2.SetTexture("_BumpMap", normalsPM);
        Walls2.SetTexture("_EmissionMap", emisW2);


        //Room3
        //Ceiling3
        Ceiling3.SetTexture("_BumpMap", normalsPM);
        Ceiling3.SetTexture("_OcclusionMap", ambientoccC2);

        //DessertRoomWalls
        Texture ambientoccDRW = assetManager.GetAsset<Texture>("dessertroomwalls", "AmbOc");

        DessertRoomWalls.SetTexture("_MainTex", emisW2);
        DessertRoomWalls.SetTexture("_BumpMap", normalsPM);
        DessertRoomWalls.SetTexture("_OcclusionMap", ambientoccDRW);
        DessertRoomWalls.SetTexture("_EmissionMap", emisW2);

        //Floor_3
        Floor3.SetTexture("_BumpMap", normalsF2);
        Floor3.SetTexture("_ParallaxMap", hmapF2);
        Floor3.SetTexture("_OcclusionMap", ambientoccF2);

        //Walls3
        Walls3.SetTexture("_MainTex", emisW2);
        Walls3.SetTexture("_BumpMap", normalsPM);
        Walls3.SetTexture("_OcclusionMap", ambientoccDRW);
        Walls3.SetTexture("_EmissionMap", emisW2);

        //RainBarrel
        RainBarrel.SetTexture("_BumpMap", normalsOD);
        RainBarrel.SetTexture("_DetailMask", dmaskOD);
        RainBarrel.SetTexture("_DetailAlbedoMap", albedo2OD);

        //RainRoomWalls
        RainRoomWalls.SetTexture("_MainTex", emisW2);
        RainRoomWalls.SetTexture("_BumpMap", normalsPM);
        RainRoomWalls.SetTexture("_OcclusionMap", ambientoccDRW);
        RainRoomWalls.SetTexture("_EmissionMap", emisW2);

        //ShieldOver
        Texture albedoSO = assetManager.GetAsset<Texture>("shieldover", "Albedo");

        ShieldOver.SetTexture("_MainTex", albedoSO);
    }
}
