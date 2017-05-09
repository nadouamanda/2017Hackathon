using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class posterSelectHandler : MonoBehaviour {

    [SerializeField]
    public Texture normalTexture;    
    [SerializeField]
    public Texture clickedTexture;
    [SerializeField]
    public VRInteractiveItem interactiveItem;
    [SerializeField]
    public Renderer m_renderer;
    public VRInput vrInput;

	public laneObjectLinker linker;

    private void Awake()
    {
       
    }


    /*private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        m_InteractiveItem.OnClick += HandleClick;
        m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        m_InteractiveItem.OnClick -= HandleClick;
        m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
    }*/
    

    //Handle the Over event
    public void HandleOver()
    {   
        Debug.Log("Show over state");
        
    }


    //Handle the Out event
    public void HandleOut()
    {
        Debug.Log("Show out state");
        m_renderer.material.mainTexture = normalTexture;
    }


    //Handle the Click event
    public void HandleClick()
    {
        Debug.Log("Show click state");
		//moveSwimLane (moveBehind: true);
        m_renderer.material.mainTexture = clickedTexture;
    }


    //Handle the DoubleClick event
    public void HandleDoubleClick()
    {
        Debug.Log("Show double click");
        
    }

    //Handle the swipe events by applying AddTorque to the Ridigbody
    public void HandleSwipe(VRInput.SwipeDirection swipeDirection)
    {
		Debug.Log ("SWIPE DETECTED");
        switch (swipeDirection)
        {
            case VRInput.SwipeDirection.NONE:
                break;
            case VRInput.SwipeDirection.UP:
                break;
            case VRInput.SwipeDirection.DOWN:
                break;
			case VRInput.SwipeDirection.LEFT:
				moveSwimLane (moveBehind: false);
                break;
			case VRInput.SwipeDirection.RIGHT:
				moveSwimLane (moveBehind: true);
                break;
        }
    }

	private void moveSwimLane(bool moveBehind) {
		if (linker == null) {
			Debug.LogError ("linker is NULL cannot animate!!");
			return;
		}

		for (int i = 0; i < linker.swimlane.Length; i++) {
			swimlaneAnim anim = linker.swimlane [i].GetComponent<swimlaneAnim> ();
			swimlaneAnim animBehind = linker.swimlaneBehind [i].GetComponent<swimlaneAnim> ();
			swimlaneAnim animFront = linker.swimlaneInFront [i].GetComponent<swimlaneAnim> ();
			if (moveBehind) {
				animBehind.animateToPosition (anim.cube.transform.position);
				anim.animateToPosition (animFront.cube.transform.position);
				animFront.animateToPosition (animBehind.cube.transform.position);
			} else {
				animFront.animateToPosition (anim.cube.transform.position);
				anim.animateToPosition (animBehind.cube.transform.position);
				animBehind.animateToPosition (animFront.cube.transform.position);
			}
		}
	}

}
