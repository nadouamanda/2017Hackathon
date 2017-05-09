using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swimlaneAnim : MonoBehaviour {

    public GameObject cube;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 currentPosition;
    private Vector3 sector;
	private bool shouldAnimate = false;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if (shouldAnimate && !V3Equal(currentPosition, endPosition) ) {
			currentPosition += sector;
			gameObject.transform.position = currentPosition;
			Debug.Log ("ANIMATING");
		} else if (shouldAnimate && V3Equal(currentPosition, endPosition) ) {
			shouldAnimate = false;
		}
	}

	public bool V3Equal(Vector3 a, Vector3 b){
		return Vector3.SqrMagnitude(a - b) < 0.01;
	}

    public void StartAnimation(Vector3 movement)
    {
        startPosition = cube.gameObject.transform.position;
        currentPosition = cube.gameObject.transform.position;
        endPosition = startPosition + movement;
        sector = movement / 100;
		shouldAnimate = true;
    }

    public void animateToPosition(Vector3 movement)
    {
        startPosition = gameObject.transform.position;
        currentPosition = gameObject.transform.position;
        endPosition = movement;
        sector = (endPosition - startPosition) / 100;
		shouldAnimate = true;
    }
}
