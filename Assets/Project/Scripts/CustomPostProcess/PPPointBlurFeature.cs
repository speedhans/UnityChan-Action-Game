using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPPointBlurFeature : ScriptableRendererFeature
{
    CustomColorGradingPass m_Pass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_Pass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(m_Pass);
    }

    public override void Create()
    {
        m_Pass = new CustomColorGradingPass(RenderPassEvent.BeforeRenderingPostProcessing);
    }
}

public class CustomColorGradingPass : ScriptableRenderPass
{
    static readonly string m_RenderTag = "PP PointBlur";
    static readonly int m_MainTexID = Shader.PropertyToID("_MainTex");

    static readonly int m_ShaderPropertyBlurPointX = Shader.PropertyToID("_BlurPointX");
    static readonly int m_ShaderPropertyBlurPointY = Shader.PropertyToID("_BlurPointY");
    static readonly int m_ShaderPropertyBlurPower = Shader.PropertyToID("_BlurPower");
    static readonly int m_ShaderPropertySampleCount = Shader.PropertyToID("_SampleCount");

    static public Vector2 m_BlurPoint;
    static public float m_BlurPower;
    static public float m_BlurSampleCount;

    static public bool m_UsePass = false;

    Material m_Material;
    RenderTargetIdentifier m_Source;
    RenderTargetHandle m_TemporaryColorTexture;

    static public void SetPassData(Vector2 _Point, float _Power, float _SampleCount)
    {
        m_BlurPoint = _Point;
        m_BlurPower = _Power;
        m_BlurSampleCount = Mathf.Floor(_SampleCount);
    }

    public CustomColorGradingPass(RenderPassEvent _Event)
    {
        renderPassEvent = _Event; // ScriptableRenderPass 내장 변수
        var shader = Shader.Find("Custom/URP PointBlur Shader");
        if (shader == null)
        {
            Debug.LogError("Shader not found.");
            return;
        }
        m_Material = CoreUtils.CreateEngineMaterial(shader);
        m_TemporaryColorTexture.Init("_TemporaryColorTexture2");
    }

    public void Setup(in RenderTargetIdentifier _Sour)
    {
        m_Source = _Sour;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!m_Material) return;
        if (!renderingData.cameraData.postProcessEnabled) return;

        if (!m_UsePass) return;
        if (m_BlurSampleCount < 1) return;

        m_Material.SetFloat(m_ShaderPropertyBlurPointX, m_BlurPoint.x);
        m_Material.SetFloat(m_ShaderPropertyBlurPointY, m_BlurPoint.y);
        m_Material.SetFloat(m_ShaderPropertyBlurPower, -m_BlurPower);
        m_Material.SetFloat(m_ShaderPropertySampleCount, m_BlurSampleCount);

        CommandBuffer cmd = CommandBufferPool.Get(m_RenderTag);

        RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        opaqueDesc.depthBufferBits = 0;
        cmd.GetTemporaryRT(m_TemporaryColorTexture.id, opaqueDesc, FilterMode.Point);
        cmd.SetGlobalTexture(m_MainTexID, m_Source);

        cmd.Blit(m_Source, m_TemporaryColorTexture.Identifier(), m_Material, 0);
        cmd.Blit(m_TemporaryColorTexture.Identifier(), m_Source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
