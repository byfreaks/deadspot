using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OffsetCalculator {

	private static float Lengthdir_x(float len, float dir){
		//return Mathf.Rad2Deg*(Mathf.Cos(dir)) * len;
		return Mathf.Rad2Deg * (Mathf.Cos( dir*Mathf.Deg2Rad ) ) * len;
	}

	private static float Lengthdir_y(float len, float dir){
		return Mathf.Rad2Deg * (Mathf.Sin( dir*Mathf.Deg2Rad ) ) * len;
	}

	public static Vector3 GetOffsetPosition(bool isMouseDown, Vector3 origin, float angle,float x_offset, float y_offset){
		if (isMouseDown){
			return  new Vector2(origin.x + Lengthdir_x( x_offset,angle), origin.y + Lengthdir_y(y_offset,angle));
		} else {
			return  new Vector2(origin.x + Lengthdir_x( x_offset,angle), origin.y - Lengthdir_y(y_offset,angle));
		}
	}

}

