using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChangerSource : MonoBehaviour
{
    #region Fields
    private List<AIEnemy> aiEnemies;
    private List<BuildingEffects> buildingEffects;

    private int maxPositions = 128; // IMPORTANT: This number must be reflected in the TextureChanger.shader file
    private int maxBuildings = 8; // IMPORTANT: This number must be reflected in the TextureChanger.shader file
    private Vector4[] aiPositions;
    private float[] buildingsBlendStartRadius;
    private int activeEnemies = 0;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        aiEnemies = new List<AIEnemy>(32);
        buildingEffects = new List<BuildingEffects>(8);

        aiPositions = new Vector4[maxPositions];
        buildingsBlendStartRadius = new float[maxBuildings];
    }

    private void Update()
    {
        /* Shader data update */

        int skippedEnemies = 0;
        activeEnemies = 0;

        for (int i = 0; i < buildingEffects.Count + aiEnemies.Count && i < maxPositions; ++i)
        {
            if (i < buildingEffects.Count)
            {
                aiPositions[i].x = buildingEffects[i].transform.position.x;
                aiPositions[i].y = buildingEffects[i].transform.position.y;
                aiPositions[i].z = buildingEffects[i].transform.position.z;
                /* The w component is used to pass the effectOnMapRadius of the building */
                aiPositions[i].w = buildingEffects[i].effectOnMapRadius;

                buildingsBlendStartRadius[i] = buildingEffects[i].GetBlendRadius();
            }
            else
            {
                if (aiEnemies[i - buildingEffects.Count].gameObject.activeSelf)
                {
                    aiPositions[i - skippedEnemies].x = aiEnemies[i - buildingEffects.Count].transform.position.x;
                    aiPositions[i - skippedEnemies].y = aiEnemies[i - buildingEffects.Count].transform.position.y;
                    aiPositions[i - skippedEnemies].z = aiEnemies[i - buildingEffects.Count].transform.position.z;
                    aiPositions[i - skippedEnemies].w = aiEnemies[i - buildingEffects.Count].effectOnMapRadius;
                    ++activeEnemies;
                }
                else ++skippedEnemies;
            }
        }
    }
    #endregion

    #region Public Methods
    public void UpdateMaterial(Material material)
    {
        material.SetInt("_ActiveEnemies", activeEnemies);
        material.SetInt("_BuildingsCount", buildingEffects.Count);
        material.SetVectorArray("_AiPositions", aiPositions);
        material.SetFloatArray("_BuildingsBlendStartRadius", buildingsBlendStartRadius);
    }

    public void AddBuildingEffect(BuildingEffects buildingEffect)
    {
        buildingEffects.Add(buildingEffect);
        UnityEngine.Assertions.Assert.IsTrue(buildingEffects.Count < maxBuildings, "ERROR: The amount of buildings exceeds the maximum amount of building accepted by the TextureChanger shader (" + maxBuildings + "). The buildings exceeding the maximum will be ignored!");
    }

    public void RemoveBuildingEffect(BuildingEffects buildingEffect)
    {
        buildingEffects.Remove(buildingEffect);
    }

    public void AddEnemy(AIEnemy enemy)
    {
        aiEnemies.Add(enemy);
        UnityEngine.Assertions.Assert.IsTrue(aiEnemies.Count < maxPositions - maxBuildings, "ERROR: The amount of enemies exceeds the maximum amount of enemies accepted by the TextureChanger shader (" + (maxPositions - maxBuildings) + "). The enemies exceeding the maximum will be ignored!");
    }

    public void RemoveEnemy(AIEnemy enemy)
    {
        aiEnemies.Remove(enemy);
    }
    #endregion
}
