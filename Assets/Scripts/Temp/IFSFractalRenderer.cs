using System;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class IFSFractalRenderer : MonoBehaviour
{
    [SerializeField] private SettingsManager settingsManager;
    
    [Header("Compute Shader and Output")]
    public ComputeShader ifsShader;
    public RenderTexture targetTexture;

    [Header("Image Settings")]
    public int resolution = 1024;
    public int iterations = 10;
    public int seedCount = 1000000;

    [Header("IFS Settings")]
    public Vector2 domainMin;
    public Vector2 domainMax;

    private float[] _ifsRules;

    [ReadOnly] public RenderTexture resultTexture;
    ComputeBuffer hitBuffer;
    ComputeBuffer ruleBuffer;
    ComputeBuffer seedBuffer;

    int maxThreads = 1024;
    private int maxThreadsPerGroup = 64;

    private float startTime = 0f;

    private void OnEnable()
    {
        if (settingsManager == null)
        {
            Debug.LogError("SettingsManager is not assigned!");
            return;
        }

        settingsManager.OnStartRunning += Generate;
    }
    
    private void OnDisable()
    {
        settingsManager.OnStartRunning -= Generate;
    }

    void OnDestroy()
    {
        resultTexture?.Release();
        targetTexture?.Release();
        hitBuffer?.Release();
        ruleBuffer?.Release();
        seedBuffer?.Release();
    }

    public void Generate()
    {
        GetSettings();
        
        startTime = Time.realtimeSinceStartup;
        if (ifsShader == null || targetTexture == null)
        {
            Debug.LogError("Assign ComputeShader and Target texture!");
            return;
        }

        if (_ifsRules == null || _ifsRules.Length % 6 != 0)
        {
            Debug.LogError("IFS Rules must be multiple of 6 floats!");
            return;
        }

        int ruleCount = _ifsRules.Length / 6;

        Debug.Log($"Generating with {ruleCount} rules, {seedCount} seeds, {iterations} iterations");

        // Allocate resources
        int totalPixels = resolution * resolution;
        hitBuffer?.Release();
        hitBuffer = new ComputeBuffer(totalPixels, sizeof(uint));
        hitBuffer.SetData(new uint[totalPixels]);

        ruleBuffer?.Release();
        ruleBuffer = new ComputeBuffer(_ifsRules.Length, sizeof(float));
        ruleBuffer.SetData(_ifsRules);

        // Generate seeds
        seedBuffer?.Release();
        seedBuffer = new ComputeBuffer(seedCount, sizeof(float) * 2);
        seedBuffer.SetData(GenerateSeeds(seedCount));

        // Setup render texture
        if (resultTexture != null)
            resultTexture.Release();

        resultTexture = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGB32);
        resultTexture.enableRandomWrite = true;
        resultTexture.Create();

        // AccumulateHits
        int kernel = ifsShader.FindKernel("AccumulateHits");

        ifsShader.SetBuffer(kernel, "HitBuffer", hitBuffer);
        ifsShader.SetBuffer(kernel, "IFSRules", ruleBuffer);
        ifsShader.SetBuffer(kernel, "SeedPoints", seedBuffer);
        ifsShader.SetInt("RuleCount", ruleCount);
        ifsShader.SetInt("Iterations", iterations);
        ifsShader.SetInt("Resolution", resolution);
        ifsShader.SetVector("DomainMin", domainMin);
        ifsShader.SetVector("DomainMax", domainMax);
        ifsShader.SetInt("SeedCount", seedCount);

        int groups = Mathf.CeilToInt(seedCount / maxThreads);
        ifsShader.Dispatch(kernel, groups, 1, 1);

        // Read back to find MaxHitCount
        uint[] hits = new uint[totalPixels];
        hitBuffer.GetData(hits);

        uint maxHit = 0;
        foreach (var h in hits)
            if (h > maxHit) maxHit = h;

        Debug.Log($"Max hit count: {maxHit}");

        // NormalizeHits
        kernel = ifsShader.FindKernel("NormalizeHits");

        ifsShader.SetBuffer(kernel, "HitBuffer", hitBuffer);
        ifsShader.SetTexture(kernel, "Result", resultTexture);
        ifsShader.SetInt("Resolution", resolution);

        int threadGroups = Mathf.CeilToInt(resolution / maxThreadsPerGroup);
        ifsShader.Dispatch(kernel, threadGroups, threadGroups, 1);

        Graphics.Blit(resultTexture, targetTexture);

        Debug.Log($"IFS fractal generation complete in {Time.realtimeSinceStartup - startTime}.");
    }

    private void GetSettings()
    {
        //_ifsRules = settingsManager.ifsCode.SelectMany(arr => arr).ToArray();
        _ifsRules = settingsManager.ifsCode.SelectMany(arr => arr.Take(6).ToArray()).ToArray();
    }

    private Vector2[] GenerateSeeds(int count)
    {
        Vector2[] seeds = new Vector2[count];
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(domainMin.x, domainMax.x);
            float y = Random.Range(domainMin.y, domainMax.y);
            seeds[i] = new Vector2(x, y);
        }
        return seeds;
    }
}
