// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/AlphaMask" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _AlphaTex ("Alpha mask (R)", 2D) = "white" {}
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100
    
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha 
    
    Pass {  
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
				float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            
            float4 _MainTex_ST;
            
            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                fixed4 col2 = tex2D(_AlphaTex, i.texcoord);
                col.a = col.a * col2.r;
                return col;// * col2;
            }
        ENDCG
    }
}

}