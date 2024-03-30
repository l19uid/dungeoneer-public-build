Shader "Custom/AtlasSpriteOutline"
{
    Properties
    {
        _Color ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (0.002, 0.03)) = 0.005
        _MainTex ("Base (RGB)", 2D) = "white" { }
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        fixed _Outline;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Basic texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            // Outline
            fixed alpha = tex2D(_MainTex, IN.uv_MainTex + float2(_Outline, 0)).a;
            alpha = max(alpha, tex2D(_MainTex, IN.uv_MainTex - float2(_Outline, 0)).a);
            alpha = max(alpha, tex2D(_MainTex, IN.uv_MainTex + float2(0, _Outline)).a);
            alpha = max(alpha, tex2D(_MainTex, IN.uv_MainTex - float2(0, _Outline)).a);

            o.Albedo = c.rgb;
            o.Alpha = c.a = alpha;
        }
        ENDCG
    }
}