// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PaintCascade"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 31.7
		_Smoothnes("Smoothnes", Float) = 0
		_MainColor("MainColor", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			half filler;
		};

		uniform float4 _MainColor;
		uniform float _Smoothnes;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Emission = _MainColor.rgb;
			o.Smoothness = _Smoothnes;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
453;135;925;884;543.1535;199.1261;1.44649;True;False
Node;AmplifyShaderEditor.CommentaryNode;13;-2336.356,1143.167;Inherit;False;1435.14;937.6008;Comment;12;2;3;4;7;11;8;10;9;12;15;25;26;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TransformPositionNode;22;-56.1493,139.9833;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;1;-56.10346,-74.77785;Inherit;False;Property;_MainColor;MainColor;7;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;14;481.5428,119.0264;Inherit;False;15;OpacityMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;-300.6233,75.90703;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-498.7693,77.50885;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-586.4283,511.7597;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;16;-792.3555,58.04034;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;20;-759.6974,601.0742;Inherit;False;Property;_VO_Amplitud;VO_Amplitud;9;0;Create;True;0;0;0;False;0;False;0;12.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;17;-780.174,440.2362;Inherit;False;15;OpacityMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-1128.739,1682.666;Inherit;False;OpacityMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1136.217,1786.6;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;9;-1248.767,1411.038;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;10;-1569.203,1821.768;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1.56;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1495.917,1404.441;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;8.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;25;-1875.557,1842.917;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;2,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1718.218,1412.24;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;26;-2141.564,1944.145;Inherit;False;Property;_NoisePannerSpeed;NoisePannerSpeed;6;0;Create;True;0;0;0;False;0;False;0,0;-0.1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-2167.493,1822.778;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;4;-1979.041,1459.975;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;3;-2000.643,1193.167;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-2286.356,1196.6;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;488.2423,6.051025;Inherit;False;Property;_Smoothnes;Smoothnes;5;0;Create;True;0;0;0;False;0;False;0;43.86;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;681.6437,-107.928;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;PaintCascade;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.2;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;31.7;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;8;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;22;0;21;0
WireConnection;21;0;18;0
WireConnection;21;1;16;2
WireConnection;21;2;16;3
WireConnection;18;0;16;1
WireConnection;18;1;19;0
WireConnection;19;0;17;0
WireConnection;19;1;20;0
WireConnection;15;0;12;0
WireConnection;12;0;10;0
WireConnection;12;1;9;0
WireConnection;9;0;8;0
WireConnection;10;0;25;0
WireConnection;8;0;7;0
WireConnection;25;0;11;0
WireConnection;25;2;26;0
WireConnection;7;0;4;0
WireConnection;7;1;3;0
WireConnection;4;0;3;0
WireConnection;3;0;2;2
WireConnection;0;2;1;0
WireConnection;0;4;24;0
ASEEND*/
//CHKSM=C1C9906E8A4F4804FBF9FBB99ABBA22A335A4D0C