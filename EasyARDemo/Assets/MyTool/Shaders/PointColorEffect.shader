Shader "MyShader/PointColorEffect"
{

	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MyColor("MyColor",COLOR)=(1,1,1,1)
		_TargetColor("TargetColor", Color) = (1,0,0)
		_Near("Near", Range(0, 0.5)) = 0.1
		_AddValue("AddValue",Range(0,1))=0.2
		_Alpha("Alpha",range(0,1))=1 
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Lighting Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

        float3 RGBConvertToHSV(float3 rgb)  
        {  
            float R = rgb.x/255,G = rgb.y/255,B = rgb.z/255;  
            float3 hsv;  
            float max1=max(R,max(G,B));  
            float min1=min(R,min(G,B));  
            float del_max = max1 - min1;  
            hsv.z = max1;  
            if (del_max == 0)  
            {  
                hsv.x = 0;  
                hsv.y = 0;  
            }  
            else  
            {  
                hsv.y = del_max / max1;  
                float del_R = (((max1 - R) / 6) + (del_max / 2)) / del_max;  
                float del_G = (((max1 - G) / 6) + (del_max / 2)) / del_max;  
                float del_B = (((max1 - B) / 6) + (del_max / 2)) / del_max;  
                if (R == max1)hsv.x = del_B - del_G;  
                else if (G == max1)hsv.x = (1 / 3) + del_R - del_B;  
                else if (B == max1)hsv.x = (2 / 3) + del_G - del_R;  
                if (hsv.x < 0)hsv.x += 1;  
                if (hsv.x > 1)hsv.x -= 1;  
            }  
           
            return hsv;  
        }  
				float3 HSVConvertToRGB(float3 hsv)  
				{  
					float R,G,B;  
					//float3 rgb;  
					if( hsv.y == 0 )  
					{  
						/*R=G=B=hsv.z;*/  
						R = hsv.z * 255;  
						G = hsv.z * 255;  
						B = hsv.z * 255;  
					}  
					else  
					{  
						float var_r, var_g, var_b;  
						float var_h = hsv.x * 6;  
						if (var_h == 6)var_h = 0;  
						int var_i = (int)var_h;//把var_h转化为整数var_i；  
						float var_1 = hsv.z*(1 - hsv.y);  
						float var_2 = hsv.z*(1 - hsv.y*(var_h - var_i));  
						float var_3 = hsv.z*(1 - hsv.y*(1 - (var_h - var_i)));  
						if (var_i == 0) { var_r = hsv.z; var_g = var_3; var_b = var_1; }  
						else if (var_i == 1) { var_r = var_2; var_g = hsv.z; var_b = var_1; }  
						else if (var_i == 2) { var_r = var_1; var_g = hsv.z; var_b = var_3; }  
						else if (var_i == 3) { var_r = var_1; var_g = var_2; var_b = hsv.z; }  
						else if (var_i == 4) { var_r = var_3; var_g = var_1; var_b = hsv.z; }  
						else { var_r = hsv.z; var_g = var_1; var_b = var_2; }  
  
						R = var_r * 255;  
						G = var_g * 255;  
						B = var_b * 255;  
					}  
     
					return float3(R,G,B);  
				}         
			fixed GetHue(fixed3 rgb) {
				fixed hue = 0;
				fixed minValue = min(rgb.r, min(rgb.g, rgb.b));
				fixed maxValue = max(rgb.r, max(rgb.g, rgb.b));
				fixed delta = maxValue - minValue;
				if (delta != 0) {
					if (maxValue == rgb.r) {
						hue = (rgb.g - rgb.b) / delta;
					}
					else if (maxValue == rgb.g) {
						hue = 2.0 + (rgb.b - rgb.r) / delta;
					}
					else {
						hue = 4.0 + (rgb.r - rgb.g) / delta;
					}

					hue /= 6.0;

					if (hue < 0) {
						hue += 1.0;
					}
				}
				return hue;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed3 _TargetColor;
			fixed _Near;
			fixed3 _MyColor;
			float _AddValue;
			float _Alpha;

			fixed4 frag (v2f i) : SV_Target
			{
					fixed4 col = tex2D(_MainTex, i.uv);
					fixed4 tarCol= tex2D(_MainTex, i.uv);
				if(_Alpha==1)
				{
					
					fixed distance = GetHue(col.rgb) - GetHue(_TargetColor);
					if (distance > 0.5) {
						distance = 1.0 - distance;
					}
					else if (distance < -0.5) {
						distance = 1.0 + distance;
					}
					else {
						distance = abs(distance);
					}
				
					if (distance <= _Near)
					{
						//tarCol.r = _MyColor.r;
						//tarCol.g = _MyColor.g;
						//tarCol.b = _MyColor.b;
						fixed3 curHSV=RGBConvertToHSV(col.rgb);
						curHSV.x=RGBConvertToHSV(_MyColor).x+_AddValue;
						tarCol=fixed4(HSVConvertToRGB(curHSV),_Alpha);
				
					}
				}
				else if(_Alpha==0.5||_Alpha==-0.5)
				{
				fixed distance = RGBConvertToHSV(col.rgb).y - RGBConvertToHSV(_TargetColor).y;
				if (distance > 0.5) {
						distance = 1.0 - distance;
					}
					else if (distance < -0.5) {
						distance = 1.0 + distance;
					}
					else {
						distance = abs(distance);
					}
						if (distance <= _Near&&_Alpha==0.5)    //黑色
						{
							fixed3 curHSV=RGBConvertToHSV(col.rgb);
							curHSV.x=RGBConvertToHSV(col.rgb).x;
							curHSV.y=RGBConvertToHSV(col.rgb).y;
							curHSV.z=_AddValue;
							tarCol=fixed4(HSVConvertToRGB(curHSV),0);
				
						}
						if (distance <= _Near&&_Alpha==-0.5)    //白色
						{
							fixed3 curHSV=RGBConvertToHSV(col.rgb);
							curHSV.x=RGBConvertToHSV(col.rgb).x;
							curHSV.y=0;
							curHSV.z=_AddValue;
							tarCol=fixed4(HSVConvertToRGB(curHSV),0);
				
						}
				}
			
				else if(_Alpha==0)
				{
					tarCol= tex2D(_MainTex, i.uv);
				}
				return	tarCol;
			}
			ENDCG
		}
	}
}
