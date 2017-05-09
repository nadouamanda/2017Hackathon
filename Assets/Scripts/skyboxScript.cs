using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxScript : MonoBehaviour {

    public Texture texture;
	// Use this for initialization
	void Start () {
        Handheld.PlayFullScreenMovie("StarWars.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
    }
	
	// Update is called once per frame
}
