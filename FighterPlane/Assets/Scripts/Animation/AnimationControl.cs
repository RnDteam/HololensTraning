using UnityEngine;
using System.Collections;

public class AnimationControl : MonoBehaviour {

    private Animator anim;

    // Use this for initialization
    public virtual void Start () {
        anim = gameObject.GetComponent<Animator>();
    
        anim.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public IEnumerator PlayAnimation(string animationName)
    {
        anim.enabled = true;
        anim.Play(animationName, -1, 0);
        yield return new WaitForSeconds(16);
        anim.StopPlayback();
        anim.enabled = false;
    }
}
