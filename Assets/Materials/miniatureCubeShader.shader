Shader "Custom/miniatureCubeShader"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0,255)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Pass
        {
            Blend Zero One
            ZWrite off
            Stencil
            {
                Ref 1          // Reference value
                Comp Always    // Always write to stencil buffer
                Pass Replace   // Replace stencil buffer value with Ref (1)
            }
        }
    }
}