﻿using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    EntityManager manager;
    public GameObject player;
    public GameObject sandPrefab;
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject rockPrefab;
    public GameObject snowPrefab;
    const int worldHalfSize = 75;

    [Range(0.1f, 10f)]
    public float strength1 = 1f;

    [Range(0.01f, 1f)]
    public float scale1 = 0.1f;

    [Range(0.1f, 10f)]
    public float strength2 = 1f;

    [Range(0.01f, 1f)]
    public float scale2 = 0.1f;

    [Range(0.1f, 10f)]
    public float strength3 = 1f;

    [Range(0.01f, 1f)]
    public float scale3 = 0.1f;

    [Range(0, 100f)]
    public float sandLevel = 2f;

    [Range(0, 100f)]
    public float dirtLevel = 4f;

    [Range(0, 100f)]
    public float grassLevel = 6f;

    [Range(0, 100f)]
    public float rockLevel = 8f;

    [Range(0, 100f)]
    public float snowLevel = 10f;



    // Start is called before the first frame update
    void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        GameDataManager.sand = GameObjectConversionUtility.ConvertGameObjectHierarchy(sandPrefab, settings);
        GameDataManager.dirt = GameObjectConversionUtility.ConvertGameObjectHierarchy(dirtPrefab, settings);
        GameDataManager.grass = GameObjectConversionUtility.ConvertGameObjectHierarchy(grassPrefab, settings);
        GameDataManager.rock = GameObjectConversionUtility.ConvertGameObjectHierarchy(rockPrefab, settings);
        GameDataManager.snow = GameObjectConversionUtility.ConvertGameObjectHierarchy(snowPrefab, settings);

        for (int z = -worldHalfSize; z <= worldHalfSize; z++)
        {
            for (int x = -worldHalfSize; x <= worldHalfSize; x++)
            {
                var height = Mathf.PerlinNoise(x * scale1, z * scale1) * strength1;
                var position = new Vector3(x, height, z);
                Entity instance;

                if (height <= GameDataManager.sandLevel)
                    instance = manager.Instantiate(GameDataManager.sand);
                else if (height <= GameDataManager.dirtLevel)
                    instance = manager.Instantiate(GameDataManager.dirt);
                else if (height <= GameDataManager.grassLevel)
                    instance = manager.Instantiate(GameDataManager.grass);
                else if (height <= GameDataManager.rockLevel)
                    instance = manager.Instantiate(GameDataManager.rock);
                else
                    instance = manager.Instantiate(GameDataManager.snow);


                manager.SetComponentData(instance, new Translation { Value = position });
                manager.SetComponentData(instance, new BlockData { initialPosition = position });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameDataManager.strength1 != strength1) GameDataManager.changedData = true;
        else if (GameDataManager.scale1 != scale1) GameDataManager.changedData = true;
        else if (GameDataManager.strength2 != strength2) GameDataManager.changedData = true;
        else if (GameDataManager.scale2 != scale2) GameDataManager.changedData = true;
        else if (GameDataManager.strength3 != strength3) GameDataManager.changedData = true;
        else if (GameDataManager.scale3 != scale3) GameDataManager.changedData = true;
        else if (GameDataManager.sandLevel != sandLevel) GameDataManager.changedData = true;
        else if (GameDataManager.dirtLevel!= dirtLevel) GameDataManager.changedData = true;
        else if (GameDataManager.grassLevel != grassLevel) GameDataManager.changedData = true;
        else if (GameDataManager.rockLevel != rockLevel) GameDataManager.changedData = true;
        else if (GameDataManager.snowLevel != snowLevel) GameDataManager.changedData = true;
        else if (GameDataManager.playerPosition != player.transform.position) GameDataManager.changedData = true;

        GameDataManager.playerPosition = player.transform.position;
        GameDataManager.strength1 = strength1;
        GameDataManager.scale1 = scale1;
        GameDataManager.strength2 = strength2;
        GameDataManager.scale2 = scale2;
        GameDataManager.strength3 = strength3;
        GameDataManager.scale3 = scale3;

        GameDataManager.sandLevel = sandLevel;
        GameDataManager.dirtLevel = dirtLevel;
        GameDataManager.grassLevel = grassLevel;
        GameDataManager.rockLevel = rockLevel;
        GameDataManager.snowLevel = snowLevel;
    }
}
