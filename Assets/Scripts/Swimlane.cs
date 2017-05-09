using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class Swimlane : MonoBehaviour { 
	public Texture[] textures;
    public Texture selectionTexture;

	GameObject[,] swimlanes = new GameObject[3, 8];
    // Use this for initialization
    void Start () {
        float x = 0;
        float y = 5f;
        float z = 0;
        int ry = 0;
		int[] radius = new int[3] { 15, 20, 25 };

        for (int sDepth = 0; sDepth < 3; sDepth++)  {
            for (int i = 0; i < 8; i++) {
                switch (i)
                {
                    case 0:
                        x = 0;
                        z = radius[sDepth];
                        ry = 90;
                        break;
                    case 1:
                        x = radius[sDepth] / (Mathf.Sqrt(2)) ;
                        z = radius[sDepth] / (Mathf.Sqrt(2));
                        ry = 135;
                        break;
                    case 2:
                        x = radius[sDepth];
                        z = 0;
                        ry = 180;
                        break;
                    case 3:
                        x = radius[sDepth] / (Mathf.Sqrt(2)); ;
                        z = -radius[sDepth] / (Mathf.Sqrt(2));
                        ry = 225;
                        break;
                    case 4:
                        x = 0;
                        z = -radius[sDepth];
                        ry = 270;
                        break;
                    case 5:
                        x = -radius[sDepth] / (Mathf.Sqrt(2)); 
                        z = -radius[sDepth] / (Mathf.Sqrt(2));
                        ry = 315;
                        break;
                    case 6:
                        x = -radius[sDepth];
                        z = 0;
                        ry = 360;
                        break;
                    case 7:
                        x = -radius[sDepth] / (Mathf.Sqrt(2));
                        z = radius[sDepth] / (Mathf.Sqrt(2));
                        ry = 45;
                        break;
                }
    
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<BoxCollider>();
                cube.AddComponent<VRInteractiveItem>();
                cube.transform.position = new Vector3(x, y, z);
                cube.transform.Rotate(new Vector3(0, ry, 0));
                cube.transform.localScale = new Vector3(1, 10, 10);
				cube.GetComponent<Renderer>().material.mainTexture = textures[sDepth];
                cube.name = sDepth + "-" + i;
                swimlanes[sDepth, i] = cube;

                cube.AddComponent<posterSelectHandler>();
                cube.AddComponent<VRInput>();
				cube.AddComponent<laneObjectLinker> ();
				cube.AddComponent<swimlaneAnim> ();

                VRInteractiveItem interactiveItem = cube.GetComponent<VRInteractiveItem>();
                posterSelectHandler eventHandlerForCube = cube.GetComponent<posterSelectHandler>();
                VRInput inputObject = cube.GetComponent<VRInput>();
				laneObjectLinker linker = cube.GetComponent<laneObjectLinker> ();
                
                eventHandlerForCube.interactiveItem = interactiveItem;
                eventHandlerForCube.vrInput = inputObject;
                eventHandlerForCube.m_renderer = cube.GetComponent<Renderer>();
				eventHandlerForCube.normalTexture = textures[sDepth];
                eventHandlerForCube.clickedTexture = selectionTexture;
				eventHandlerForCube.linker = linker;

                interactiveItem.OnOver += eventHandlerForCube.HandleOver;
                interactiveItem.OnOut += eventHandlerForCube.HandleOut;
                interactiveItem.OnClick += eventHandlerForCube.HandleClick;
                interactiveItem.OnDoubleClick += eventHandlerForCube.HandleDoubleClick;

				inputObject.OnSwipe += eventHandlerForCube.HandleSwipe;

				cube.GetComponent<swimlaneAnim> ().cube = cube;

            }

			y += 5f;
		}

		linkCubes ();
	}

	void linkCubes () {
		int numSwimLanes = swimlanes.GetLength (0);
		GameObject[] swimlane = new GameObject[swimlanes.GetLength (1)];
		GameObject[] swimlaneBehind = new GameObject[swimlanes.GetLength (1)];
		GameObject[] swimlaneInFront = new GameObject[swimlanes.GetLength (1)];
		for (int i = 0; i < numSwimLanes; i++) {
			int idxFront = 0;
			int idxBehind = 0;
			if (i == 0) {
				idxBehind = 1;
				idxFront = numSwimLanes - 1;
			} else if (i == numSwimLanes - 1) {
				idxBehind = 0;
				idxFront = numSwimLanes - 2;
			} else {
				idxBehind = i + 1;
				idxFront = i - 1;
			}
			swimlane = extractSwimLane (i);
			swimlaneBehind = extractSwimLane (idxBehind);
			swimlaneInFront = extractSwimLane (idxFront);

			for (int k = 0; k < swimlanes.GetLength(1) - 1; k++) {
				GameObject cube = swimlanes [i, k];
				laneObjectLinker linker = cube.GetComponent<laneObjectLinker>();
				if (linker == null) {
					Debug.LogError ("Linker is null! ABORTING!");
					return;
				}
				linker.swimlane = swimlane;
				linker.swimlaneBehind = swimlaneBehind;
				linker.swimlaneInFront = swimlaneInFront;
			}
		}
	}

	GameObject[] extractSwimLane(int lane) {
		GameObject[] swimlane = new GameObject[swimlanes.GetLength (1)];
		for (int idx = 0; idx < swimlane.Length; idx++) {
			swimlane[idx] = swimlanes[lane, idx];
		}
		return swimlane;
	}
	/*void linkCubes() {
		int frontLane = 0;
		int behindLane = 0;
		int numSwimLanes = swimlanes.GetLength (0);
		for (int i = 0; i < numSwimLanes; i++) {
			switch (i) {
			case 0:
				frontLane = numSwimLanes - 1;
				behindLane = 1;
				break;
			case (numSwimLanes-1):
				frontLane = numSwimLanes - 2;
				behindLane = 0;
				break;
			default:
				frontLane = i - 1;
				behindLane = i + 1;
				break;
			}
			for (int k = 1; k < swimlanes.GetLength(1) - 1; k++) {
				GameObject cube = swimlanes [i, k];
				laneObjectLinker linker = cube.GetComponent<laneObjectLinker>();
				if (linker == null) {
					Debug.LogError ("Linker is null! ABORTING!");
					return;
				}
				linker.cube = cube;
				linker.nextCubeInSwimlane = swimlanes [i, k + 1];
				linker.prevCubeInSwimlane = swimlanes [i, k - 1];
				linker.cubeInFront = swimlanes [frontLane, k];
				linker.cubeBehind = swimlanes [behindLane, k];
			}
			//first object in swimlane
			GameObject cubeA = swimlanes [i, 0];
			laneObjectLinker linkerA = cubeA.GetComponent<laneObjectLinker>();
			if (linkerA== null) {
				Debug.LogError ("Linker is null! ABORTING!");
				return;
			}
			linkerA.cube = cubeA;
			linkerA.nextCubeInSwimlane = swimlanes [i, 1];
			linkerA.prevCubeInSwimlane = null;
			linkerA.cubeInFront = swimlanes [frontLane, 0];
			linkerA.cubeBehind = swimlanes [behindLane, 0];

			//last object in lane
			cubeA = swimlanes[i, swimlanes.GetLength(1) - 1];
			linkerA = cubeA.GetComponent<laneObjectLinker>();
			if (linkerA== null) {
				Debug.LogError ("Linker is null! ABORTING!");
				return;
			}
			linkerA.cube = cubeA;
			linkerA.nextCubeInSwimlane = null;
			linkerA.prevCubeInSwimlane = swimlanes[i, swimlanes.GetLength(1) - 2];
			linkerA.cubeInFront = swimlanes [frontLane, swimlanes.GetLength(1) - 1];
			linkerA.cubeBehind = swimlanes [behindLane, swimlanes.GetLength(1) - 1];
		}

	}
	*/
	// Update is called once per frame
	void Update () {
		
	}

}
