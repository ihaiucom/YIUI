Shader "Zeng/Mdx/particles"
{

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TeamColor("TeamColor", Color) = (1,0,0,0)

        u_emitter("u_emitter", int) = 0
        u_filterMode("u_filterMode", int) = 1


        // Shared Array
        u_colors("u_colors", Color) = (1,1,1,1)
        u_intervals("u_intervals", Vector) = (0,0,0,0)

        // Shared
        u_lifeSpan("u_lifeSpan", Float) = 1
        u_columns("u_columns", Float) = 1
        u_rows("u_rows", Float) = 1

        // Instances
        //a_health("a_health", Float) = 1


        // Particle2
        u_timeMiddle("u_timeMiddle", Float) = 0.5
        u_teamColored("u_teamColored", int) = 0
        u_scaling("u_scaling", Float) = 1
            



        _BlendOp("__blendop", Float) = 0.0
        _SrcBlend("__src", Float) = 1.0
        _DstBlend("__dst", Float) = 0.0
        _ZWrite("__zw", Float) = 1.0
        _Cull("__cull", Float) = 2.0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 100
        BlendOp [_BlendOp],[_BlendOp]
        Blend [_SrcBlend][_DstBlend],[_SrcBlend][_DstBlend]
        ZWrite [_ZWrite]
        Cull [_Cull]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma   multi_compile_instancing
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };


            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, a_health)
                UNITY_DEFINE_INSTANCED_PROP(float, a_facing)
                //UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_INSTANCING_BUFFER_END(Props)

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform float4 _TeamColor;

            uniform int u_emitter;
            uniform int u_filterMode;


            // Shared Array
            uniform float4 u_colors[3];
            uniform float4 u_intervals[4];

            // Shared
            uniform float u_lifeSpan;
            uniform float u_columns;
            uniform float u_rows;

            // Particle2
            uniform float u_timeMiddle;
            uniform int u_teamColored;
            uniform float u_scaling[3];
            
            float3 lookAtCamera(float4 vertex)
            {
                float3 center = float3(0,0,0);
                //视角方向：摄像机的坐标减去物体的点
				float3 view = mul(unity_WorldToObject,float4(_WorldSpaceCameraPos,1));
                float3 normalDir = view - center;
                //归一化
				normalDir = normalize(normalDir);
                float3 upDir = abs(normalDir.y) > 0.999 ? float3(0, 0, 1) : float3(0, 1, 0);
                //叉乘  cross(A,B)返回两个三元向量的叉积(cross product)。注意，输入参数必须是三元向量
				float3 rightDir = normalize(cross(upDir,normalDir));
				upDir = normalize(cross(normalDir, rightDir));
                //计算中心点偏移
				float3 centerOffs = vertex.xyz - center;
                //位置的变换
				float3 localPos = center + rightDir * centerOffs.x+upDir*centerOffs.y+normalDir* centerOffs.z;
                return localPos;
            }
            
            float getCell(float3 interval, float factor){
                 float start = interval.x;
                  float end = interval.y;
                  float repeat = interval.z;
                  float spriteCount = end - start;

                  if (spriteCount > 0.0) {
                    // Repeating speeds up the sprite animation, which makes it effectively run N times in its interval.
                    // E.g. if repeat is 4, the sprite animation will be seen 4 times, and thus also run 4 times as fast.
                    // The sprite index is limited to the number of actual sprites.
                    return min(start + fmod(floor(spriteCount * repeat * factor), spriteCount), u_columns * u_rows - 1.0);
                  }

                  return start;
            }

            v2f vert (appdata v)
            {
                v2f o;
                

                UNITY_SETUP_INSTANCE_ID(v);
                float  health = UNITY_ACCESS_INSTANCED_PROP(Props, a_health);
                float  facing = UNITY_ACCESS_INSTANCED_PROP(Props, a_facing);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                float factor = (u_lifeSpan - health) / u_lifeSpan;

                int index = 0;
                if (factor < u_timeMiddle) {
                    factor = factor / u_timeMiddle;
                    index = 0;
                } else {
                    factor = (factor - u_timeMiddle) / (1.0 - u_timeMiddle);
                    index = 1;
                }

                factor = min(factor, 1.0);

                float scale = lerp(u_scaling[index], u_scaling[index + 1], factor);
                float4 color = lerp(u_colors[index], u_colors[index + 1], factor);

                float4 vertexValue = v.vertex;

                float cs = cos(facing);
                Float sn = sin(facing);
                float x = vertexValue.x * cs - vertexValue.z * sn;
                float y = vertexValue.x * sn + vertexValue.z * cs;
                vertexValue.x = x;
                vertexValue.z = y;

                vertexValue = mul(float4x4(
                    scale, 0, 0, 0,
                    0, scale, 0, 0,
                    0, 0, scale, 0,
                    0, 0, 0, 1),vertexValue);


                //vertexValue.xyz =  lookAtCamera(vertexValue);
                o.vertex = UnityObjectToClipPos(vertexValue);

              /*  float4 ori = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
                float4 vt = v.vertex;
                vt.y = vt.z;
                vt.z = 0;
                vt.xyz += ori.xyz;
                o.vertex = mul(UNITY_MATRIX_P, vt);*/


                // uv
                
                float3 interval;
                interval = u_intervals[index].xyz;
                float cell = getCell(interval, factor);

                float left = floor(fmod(cell, u_columns));
                float top = floor(cell / u_columns);
              //  float right = left + 1.0;
              //  float bottom = top + 1.0;
                
              left /= u_columns;
              //right /= u_columns;
              top /= u_rows;
              //bottom /= u_rows;

              float2 uv = v.uv * float2(1.0/u_columns, 1.0/u_rows) + float2(left, top);
              //uv.y = 1.0 - uv.y;
              
                o.uv = uv ;
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.color = color;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                UNITY_SETUP_INSTANCE_ID(i);


                // sample the texture
                half4 texColor = tex2D(_MainTex, i.uv);
                half3 color = texColor.a * texColor.rgb + (1.0 - texColor.a) * _TeamColor.rgb * _TeamColor.a;

                half4 albedo = half4(color, texColor.a);

                albedo.rgb *= i.color.rgb;
                //albedo.rgb = i.color.a;

                //albedo.rgba = (half4)i.color.a;

                // EMITTER_PARTICLE2
                if (u_emitter == 0 && (u_filterMode == 2.0 || u_filterMode == 3.0) && albedo.a < 0.02) {
                    discard;
                }

                // EMITTER_PARTICLE2
                if (u_emitter == 0 && (u_filterMode == 4.0) && albedo.a < 0.75) {
                    discard;
                }

                // EMITTER_RIBBON
                if (u_emitter == 1 && u_filterMode == 1.0 && albedo.a < 0.75) {
                    discard;
                }



                fixed4 finalColor = albedo;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                return finalColor;
            }
            ENDCG
        }
    }
}
