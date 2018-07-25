using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarSequence : MonoBehaviour {
    
    private Sequence carSequence = DOTween.Sequence();
    // public Rigidbody carBody = this.GetComponent<Rigidbody>() as Rigidbody;

    public void Route() {
        
        Vector3 point1 = new Vector3(-2.538818f, 1, 44.19336f);
        Vector3 point2 = new Vector3(-0.765625f, 1, 33.39453f);
        Vector3 point3 = new Vector3(-1.082764f, 1, 16.89648f);
        Vector3 point4 = new Vector3(-1.398438f, 1, -45.29688f);
        Vector3 point5 = new Vector3(-17.15283f, 1, -42.628f);
        Vector3 point6 = new Vector3(-29.68506f, 1, -41.82809f);
        Vector3 point7 = new Vector3(-40.15283f, 1, -37.74757f);
        Vector3 point8 = new Vector3(-45.06641f, 1, -33.38654f);
        Vector3 point9 = new Vector3(-50.06104f, 1, -26.5847f);
        Vector3 point10 = new Vector3(-52.55176f, 1, -17.63993f);
        Vector3 point11 = new Vector3(-52.33008f, 1, 16.87482f);
        Vector3 point12 = new Vector3(-50.53223f, 1, 25.63549f);
        Vector3 point13 = new Vector3(-45.87891f, 1, 32.78313f);
        Vector3 point14 = new Vector3(-40.33398f, 1, 37.24716f);
        Vector3 point15 = new Vector3(-31.57422f, 1, 41.34958f);
        Vector3 point16 = new Vector3(-19.60254f, 1, 42.22901f);

        //Sequence 1:
        //rigidbody.DOMove(point1, 5, false);
        //rigidbody.DOMove(point2, 5, false);
        //rigidbody.DOMove(point3, 5, false);
        //rigidbody.DOMove(point4, 5, false);
    }

     // rigidbody.DOMove(Vector3 point2, float duration, bool snapping);
     

     

     //Sequence 2:
     //pedestrian.transform
     //create new vector based on the difference
     //slow down to near the pedestrian
     //rigidbody.DOMove(point stop, 10, false)

     

     //Sequence 3:
     //rigidbody.DOMove(point5, 5, false);
     //rigidbody.DOMove(point6, 5, false);
     //rigidbody.DOMove(point7, 5, false);
     //rigidbody.DOMove(point8, 5, false);
     //rigidbody.DOMove(point9, 5, false);
     //rigidbody.DOMove(point10, 5, false);
     //rigidbody.DOMove(point11, 5, false);
     //rigidbody.DOMove(point12, 5, false);
     //rigidbody.DOMove(point13, 5, false);
     //rigidbody.DOMove(point14, 5, false);
     //rigidbody.DOMove(point15, 5, false);
     //rigidbody.DOMove(point16, 5, false);

    // carSequence.Append(transform.DOMoveX(45, 1));


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
