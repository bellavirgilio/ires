using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathManager : MonoBehaviour {
    
    public Vector3[] waypoints1 = { new Vector3(-6.921875f,1f,42.59715f), new Vector3(-4.191406f,1f,42.44843f), new Vector3(-2.089844f,1f,41.15955f), new Vector3(-1.609375f,1f,38.01234f), new Vector3(-2.068359f,1f,-42.65283f) };
    public Vector3[] waypoints2 = { new Vector3(-1.714844f,1f,-18.45227f), new Vector3(-0.5429688f,1f,-38.03741f), new Vector3(-0.90625f,1f,-40.88074f), new Vector3(-3.402344f,1f,-42.45447f), new Vector3(-6.457031f,1f,-42.87268f), new Vector3(-23.32813f,1f,-42.41516f), new Vector3(-32.60156f,1f,-40.63574f), new Vector3(-36.08203f,1f,-39.28369f), new Vector3(-40.13672f,1f,-37.08124f), new Vector3(-43.65625f,1f,-34.58752f), new Vector3(-47.67969f,1f,-30.06952f), new Vector3(-50.73828f,1f,-25.1709f), new Vector3(-51.85156f,1f,-19.78284f), new Vector3(-52.64453f,1f,-13.20251f), new Vector3(-52.57813f,1f,13.17505f), new Vector3(-51.83594f,1f,20.64825f), new Vector3(-49.5f,1f,27.2572f), new Vector3(-44.98828f,1f,32.98553f), new Vector3(-39.89453f,1f,37.48236f), new Vector3(-33.46484f,1f,40.55725f), new Vector3(-26.43164f,1f,42.14038f), new Vector3(-17.91406f,1f,42.48737f) };
    public PathType pathType = PathType.Linear;

	void Start () {
        Sequence sequence = DOTween.Sequence();
        Tween path1 = transform.DOPath(waypoints1, 30, pathType).SetLookAt(0.01f).SetSpeedBased(true);
        // .SetLookAt orients the transform to the path
        Tween path2 = transform.DOPath(waypoints2, 30, pathType).SetLookAt(0.01f).SetSpeedBased(true);

        sequence.Append(path1);



        sequence.Append(path2);
    }


    // KNOWN ISSUES:
    // first point of second sequence is too early
    // last point of first path and first point of second path need
    // to be dependent on where the hydrant/pedestrian is
    //
    // needs to be speed-based (or time needs to be calculated based on speed) (.SetSpeedBased() not working?)
}
