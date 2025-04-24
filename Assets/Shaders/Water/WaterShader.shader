// Shader per simular aigua amb moviment i il·luminació dinàmica
Shader "Unlit/WaterShader"
{
    // Propietats que es poden ajustar des de l'Inspector de Unity
    Properties
    {
        _MainTex ("Texture", 2D) = "blue" {}                                  // Textura principal de l'aigua
        _InverseDetail ("1 / Resolution of Plane", Float) = 0.03333333333333 // Detall invers del pla
        _MaxUVDisplacement ("Wonkiness", Range(0, 1)) = 0.4                  // Control del moviment ondulatori
        _NoiseMap ("Noise Map", 3D) = "white" {}                             // Mapa de soroll 3D per la il·luminació
        _SizeOfNoiseMap ("Size Noise Map", Int) = 10                         // Mida del mapa de soroll
        _InverseSizeNoiseMap ("1 / Size Of Noise Map", Float) = 0.1         // Invers de la mida del mapa
        _TestingZ ("Test Var", Float) = 1                                    // Variable de prova
        _LightIntensity ("Light Intensity", Range(0.5, 2.0)) = 1.0          // Intensitat de la llum
        
        // Nous paràmetres pel control del moviment
        _WaveSpeed ("Velocitat de les Ones", Range(0.1, 5.0)) = 1.0         // Controla la velocitat general del moviment
        _WaveAmplitude ("Amplitud de les Ones", Range(0.1, 2.0)) = 1.0      // Controla l'altura de les ones
        _WaveFrequency ("Freqüència de les Ones", Range(0.1, 5.0)) = 1.0    // Controla la freqüència de les ones
        _WaveDirection ("Direcció de les Ones", Vector) = (1,1,0,0)         // Direcció del moviment de les ones
    }
    SubShader
    {
        Pass
        {
            // La idea principal és distorsionar les UVs per donar la impressió de moviment
            // Per la il·luminació, utilitzem un mostreig de soroll 3D per tenir transicions suaus

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define PI 3.141592653589793238462643382479 // Constant PI amb precisió suficient
            #define ROOTOF_3OVER4 0.86602540378         // Arrel quadrada de 3/4

            #include "UnityCG.cginc"

            // Declaració de variables i textures
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler3D _NoiseMap;
            int _SizeOfNoiseMap;
            float _InverseSizeNoiseMap;
            float _InverseDetail;
            float _MaxUVDisplacement;
            float _TestingZ;
            float _LightIntensity;
            
            // Noves variables pel moviment
            float _WaveSpeed;
            float _WaveAmplitude;
            float _WaveFrequency;
            float4 _WaveDirection;

            // Estructura d'entrada del vertex shader
            struct VertIn {
                float4 positionVert : POSITION;
                float2 uv1 : TEXCOORD0;
            };

            // Estructura de sortida del vertex shader / entrada del fragment shader
            struct VertOut {
                float4 positionVert : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float calculatedPerlinLighting : TEXCOORD1;
                float3 DEBUG_COLOR : TEXCOORD2;
            };

            // Funció per calcular el desplaçament en X utilitzant sumes trigonomètriques
            float TrigSumX (float val) {
                float withTime = (val + _Time.y * _WaveSpeed) * _WaveFrequency;
                float waveDir = val * _WaveDirection.x;
                return _WaveAmplitude * (0.5 * sin(withTime + waveDir) + 
                       0.25 * cos(2 * withTime + waveDir) - 
                       0.25 * sin(PI * withTime + waveDir));
            }

            // Funció per calcular el desplaçament en Y utilitzant sumes trigonomètriques
            float TrigSumY(float val) {
                float withTime = (val + _Time.y * _WaveSpeed) * _WaveFrequency;
                float waveDir = val * _WaveDirection.y;
                return _WaveAmplitude * (-0.625 * cos(0.5 * withTime + waveDir) + 
                       0.1875 * sin(PI * withTime + waveDir) + 
                       0.0625 * sin(0.75 * withTime + waveDir));
            }

            // Obté la distorsió UV basada en la posició
            float2 GetUVDistortion (float2 posXZ) {
                float xDisp = TrigSumX(posXZ.x);
                float yDisp = TrigSumY(posXZ.y);
                return float2(xDisp, yDisp) * _InverseDetail * _MaxUVDisplacement;
            }

            // Desxifra el gradient del soroll
            float3 DecryptGradient(float4 uvPosition) {
                float3 pointInSpace = tex3Dlod(_NoiseMap, uvPosition);
                float3 relativeVector = pointInSpace - float3(0.5, 0.5, 0.5);
                return relativeVector * 2;
            }

            // Obté els gradients per al càlcul del soroll de Perlin
            void GetGradients(float4 bottomRightUVPos, out float3 gradients[8]) {
                // Mostreja els gradients dels 8 vèrtexs del cub
                gradients[0] = DecryptGradient(bottomRightUVPos);                                                          // Inferior Esquerra
                gradients[1] = DecryptGradient(bottomRightUVPos + float4(_InverseSizeNoiseMap, 0, 0, 0));                 // Inferior Dreta
                gradients[2] = DecryptGradient(bottomRightUVPos + float4(0, _InverseSizeNoiseMap, 0, 0));                 // Inferior Davant
                gradients[3] = DecryptGradient(bottomRightUVPos + float4(_InverseSizeNoiseMap, _InverseSizeNoiseMap, 0, 0)); // Inferior Davant Dreta
                gradients[4] = DecryptGradient(bottomRightUVPos + float4(0, 0, _InverseSizeNoiseMap, 0));                 // Superior Esquerra
                gradients[5] = DecryptGradient(bottomRightUVPos + float4(_InverseSizeNoiseMap, 0, _InverseSizeNoiseMap, 0)); // Superior Dreta
                gradients[6] = DecryptGradient(bottomRightUVPos + float4(0, _InverseSizeNoiseMap, _InverseSizeNoiseMap, 0)); // Superior Davant
                gradients[7] = DecryptGradient(bottomRightUVPos + float4(_InverseSizeNoiseMap, _InverseSizeNoiseMap, _InverseSizeNoiseMap, 0)); // Superior Davant Dreta
            }

            // Funció de suavitzat per eix
            float FadePerAxis (float value) {
                return value * value * value * (value * ( value * 6 - 15) + 10);
            }

            // Funció de suavitzat conjunta per als tres eixos
            float JointFadeFunction (float3 displacementXYZ) {
                return FadePerAxis(1 - displacementXYZ.x) * FadePerAxis(1 - displacementXYZ.y) * FadePerAxis(1 - displacementXYZ.z);
            }

            // Calcula els valors de suavitzat i les diferències per al soroll de Perlin
            void GetJointFadesWithDifferences (float3 positionInUnitCube, out float fades[8], out float3 diffs[8]) {
                // Càlcul per a cada vèrtex del cub
                diffs[0] = positionInUnitCube;
                fades[0] = JointFadeFunction( abs(diffs[0]) );

                diffs[1] = positionInUnitCube - float3(1, 0, 0);
                fades[1] = JointFadeFunction( abs(diffs[1]) );
                
                diffs[2] = positionInUnitCube - float3(0, 1, 0);
                fades[2] = JointFadeFunction( abs(diffs[2]) );
                
                diffs[3] = positionInUnitCube - float3(1, 1, 0);
                fades[3] = JointFadeFunction( abs(diffs[3]) );
                
                diffs[4] = positionInUnitCube - float3(0, 0, 1);
                fades[4] = JointFadeFunction( abs(diffs[4]) );
                
                diffs[5] = positionInUnitCube - float3(1, 0, 1);
                fades[5] = JointFadeFunction( abs(diffs[5]) );
                
                diffs[6] = positionInUnitCube - float3(0, 1, 1);
                fades[6] = JointFadeFunction( abs(diffs[6]) );
                
                diffs[7] = positionInUnitCube - float3(1, 1, 1);
                fades[7] = JointFadeFunction( abs(diffs[7]) );
            }

            // Constants per ajustar la sortida del soroll de Perlin
            #define OUTPUT_SHIFT 0.866025403784
            #define OUTPUT_SCALE 0.57735026919

            // Calcula el valor final del soroll de Perlin
            float GetFinalPerlinValue (float fades[8], float3 diffs[8], float3 gradients[8]) {
                return -1 * (
                    dot (diffs[0], gradients[0]) * fades[0] +
                    dot (diffs[1], gradients[1]) * fades[1] +
                    dot (diffs[2], gradients[2]) * fades[2] +
                    dot (diffs[3], gradients[3]) * fades[3] +
                    dot (diffs[4], gradients[4]) * fades[4] +
                    dot (diffs[5], gradients[5]) * fades[5] +
                    dot (diffs[6], gradients[6]) * fades[6] +
                    dot (diffs[7], gradients[7]) * fades[7]
                );
            }

            // Obté la brillantor del mapa de soroll
            float GetBrightnessFromNoiseMap(float3 sampleAt) {
                float4 unitSamplingPosition = float4(sampleAt.xyz, 0);
                float4 bottomLeftSample = float4( floor(unitSamplingPosition * _SizeOfNoiseMap) * _InverseSizeNoiseMap);
                float3 positionRelativeBottomLeft = (unitSamplingPosition - bottomLeftSample) * _SizeOfNoiseMap;

                float3 gradients[8];
                float3 diffs[8];
                float fades[8];

                GetGradients(bottomLeftSample, gradients);
                GetJointFadesWithDifferences(positionRelativeBottomLeft, fades, diffs);
                
                return (GetFinalPerlinValue(fades, diffs, gradients) + OUTPUT_SHIFT) * OUTPUT_SCALE;
            }

            // Vertex shader: Calcula la posició i la il·luminació
            VertOut vert (VertIn i) {
                VertOut o;
                o.positionVert = UnityObjectToClipPos(i.positionVert);

                float4 worldPositionVert = mul (unity_ObjectToWorld, i.positionVert );
                o.uv1 = i.uv1 + GetUVDistortion(worldPositionVert.xz);

                o.calculatedPerlinLighting = GetBrightnessFromNoiseMap(float3(i.uv1, _Time.x));
                o.uv1 = o.uv1 * _MainTex_ST.xy + _MainTex_ST.zw;

                return o;
            }

            // Fragment shader: Calcula el color final amb la il·luminació
            float4 frag(VertOut i) : SV_TARGET {
                float lightValue = lerp(0.5, i.calculatedPerlinLighting, 0.5) * _LightIntensity;
                return lightValue * tex2D(_MainTex, i.uv1);
            }

            ENDCG
        }
    }
}
