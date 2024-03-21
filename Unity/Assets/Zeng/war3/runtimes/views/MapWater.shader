Shader "Zeng/war3/MapWater"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}


        u_war3unitRate("u_war3unitRate", Float) = 128 // war3 和u3d的单位比例关系
        u_size_offset("u_size_offset", Vector) = (10, 10, 0, 0)
        u_offsetHeight("u_offsetHeight", Float) = 1.0

        u_heightMap("u_heightMap", 2D) = "white" {}
        u_waterHeightMap("u_waterHeightMap", 2D) = "white" {}




        u_tilesets("u_tilesets", 2DArray) = "" {}
        u_tilesetsIndex("u_tilesetsIndex", Int) = 0

        u_minDepth("u_minDepth", Float) = 0.078125 // 10 / 128
        u_deepLevel("u_deepLevel", Float) = 0.5    // 64 / 128
        u_maxDepth("u_maxDepth", Float) = 0.5625   // 72 / 128

        u_minDeepColor("u_minDeepColor", COLOR) = (0.0, 0.0, 1.0, 0.2)
        u_maxDeepColor("u_maxDeepColor", COLOR) = (0.0, 0.0, 1.0, 1.0)
        u_minShallowColor("u_minShallowColor", COLOR) = (0.0, 1.0, 1.0, 0.2)
        u_maxShallowColor("u_maxShallowColor", COLOR) = (0.0, 1.0, 1.0, 1.0)

    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" }
            LOD 100
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;

                    float4 v_color:COLOR1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                uniform float u_war3unitRate;
                uniform float4 u_size_offset;
                uniform float u_offsetHeight;

                sampler2D u_heightMap;
                sampler2D u_waterHeightMap;

                UNITY_DECLARE_TEX2DARRAY(u_tilesets);
                uniform int u_tilesetsIndex;



                uniform float u_minDepth;
                uniform float u_deepLevel;
                uniform float u_maxDepth;

                uniform float4 u_minDeepColor;
                uniform float4 u_maxDeepColor;
                uniform float4 u_minShallowColor;
                uniform float4 u_maxShallowColor;


                StructuredBuffer<float> waterFlagsBuffer;




                v2f vert(appdata v, uint instanceID : SV_InstanceID)
                {
                    v2f o;

                    float2 a_position = v.vertex.xz;
                    float vertIndex = v.vertex.y;


                    float2 u_size = u_size_offset.xy;
                    float2 u_offset = u_size_offset.zw;

                    float columns = u_size.x - 1;
                    float rows = u_size.y - 1;


                    float2 u_pixel = 1.0 / u_size;
                    float2 halfPixel = u_pixel * 0.5;




                    float2 corner = float2(fmod(instanceID, columns), floor(instanceID / columns)); // 当前实例对象 在第corner.x 列，corner.y 行
                    int i = int(corner.y * (columns + 1) + corner.x); // 我们需要重新计算一下 索引

                    float a_isWater = waterFlagsBuffer[instanceID];



                    float2 base = corner + a_position;


                    float2 height_uv = base / u_size + halfPixel; // 关键点 + halfPixel
                    float height = tex2Dlod(u_heightMap, float4(height_uv, 0.0, 0.0)).x;

                    float waterHeight = tex2Dlod(u_waterHeightMap, float4(height_uv, 0.0, 0.0)).x + u_offsetHeight;

                    float value = clamp(waterHeight - height, 0.0, 1.0);


                    if (value <= u_deepLevel) {
                        value = max(0.0, value - u_minDepth) / (u_deepLevel - u_minDepth);
                        o.v_color = lerp(u_minShallowColor, u_maxShallowColor, value);
                    }
                    else {
                        value = clamp(value - u_deepLevel, 0.0, u_maxDepth - u_deepLevel) / (u_maxDepth - u_deepLevel);
                        o.v_color = lerp(u_minDeepColor, u_maxDeepColor, value);
                    }

                    float3 position = float3(base + u_offset / u_war3unitRate, waterHeight);
                    position = float3(-position.y, position.z, position.x); // 坐标系转换


                    //float hh = (height - 1.0) / 8.0;
                    //float hh = a_isWater;
                    //o.v_color = float4(hh, hh, hh, 1.0);


                    o.vertex = UnityObjectToClipPos(float4(position, 1.0)) ;
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex) ;
                    UNITY_TRANSFER_FOG(o, o.vertex);
                    return o;

                }


                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = UNITY_SAMPLE_TEX2DARRAY(u_tilesets, float3(i.uv, u_tilesetsIndex));
                    col *= i.v_color;
                    //col.rgba = i.v_color.rgba;


                    // apply fog
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    return col;
                }
                ENDCG
            }
        }
}
