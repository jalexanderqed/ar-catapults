using UnityEngine;
using System.Collections;

public class CatapultScript : MonoBehaviour {

    public Camera camera;
    public Material normalSphere;
    public Material transSphere;
    public GameObject projectileObj;
	public bool amLocal = true;
	public GameObject netProjShow;
    private GameObject projectile;
    private GameObject ghostProjectile;
    private GameObject westBand;
    private GameObject eastBand;
    private GameObject westArm;
    private GameObject eastArm;
    private CaptureTracker captureTracker = new CaptureTracker();
    private bool projectileFlying = false;
	private GameObject bandFocus;

	private bool didLaunch = false;

	// Use this for initialization
	void Start () {
		bandFocus = netProjShow;
        ghostProjectile = this.transform.Find("GhostProjectile").gameObject;
        westBand = this.transform.Find("WestBand").gameObject;
        eastBand = this.transform.Find("EastBand").gameObject;
        westArm = this.transform.Find("WestCatArm").gameObject;
        eastArm = this.transform.Find("EastCatArm").gameObject;
        ResetProjectile();
    }

	public void setLocalProperties(){
		if (!amLocal) {
			return;
		}
		//netProjShow.GetComponent<Renderer> ().enabled = false;
	}

    // Update is called once per frame
    void Update() {
		if (!amLocal) {
			//netProjShow.GetComponent<Renderer>().enabled = !(netProjShow.transform.position == ghostProjectile.transform.position);
			return;
		}
		if (projectile && projectile.GetComponent<Rigidbody>().isKinematic) {
			netProjShow.transform.position = projectile.transform.position;
		}
        CheckTouches();

        float dist = Vector3.Distance(camera.transform.position, projectile.transform.position);
        if (projectileFlying)
        {
            if(projectile.transform.position.y <= 0)
            {
                ResetProjectile();
            }
        }
        else {
            if (captureTracker.update(dist))
            {
                CaptureProjectile();
                camera.transform.localPosition = new Vector3(0, 0, 0.5f);
            }
        }
    }

    void LateUpdate()
    {
        UpdateBands();
    }

    private void CaptureProjectile()
    {
        projectile.transform.parent = camera.transform;
        projectile.transform.localPosition = new Vector3(0, -0.5f, 1.5f);
        projectile.transform.localRotation = Quaternion.Euler(0, 0, 0);
        projectile.GetComponent<Renderer>().sharedMaterial = transSphere;
    }

    private void ReleaseProjectile()
    {
        projectile.transform.parent = this.transform;
        projectile.GetComponent<Rigidbody>().useGravity = true;
        projectile.GetComponent<Rigidbody>().velocity = (ghostProjectile.transform.position - projectile.transform.position) * 9;
        projectile.GetComponent<Rigidbody>().isKinematic = false;
        projectileFlying = true;
        bandFocus = ghostProjectile;
        projectile.GetComponent<Renderer>().sharedMaterial = normalSphere;
		projectile.GetComponent<Renderer> ().enabled = false;
		didLaunch = true;
		netProjShow.transform.position = ghostProjectile.transform.position;
    }

    private void ResetProjectile()
    {
		if (!amLocal)
			return;
        if (projectile)
        {
            Destroy(projectile);
        }
        projectile = Instantiate(projectileObj);
        projectile.transform.parent = this.transform;
        projectile.transform.position = ghostProjectile.transform.position;
        projectileFlying = false;
        projectile.GetComponent<Rigidbody>().useGravity = false;
        projectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectile.GetComponent<Renderer>().sharedMaterial = normalSphere;
        bandFocus = projectile;
		projectile.GetComponent<Renderer> ().enabled = true;

    }

    private void UpdateBands()
    {
        Vector3 convergePoint = bandFocus.transform.position;
        Vector3 westTop = westArm.transform.position + westArm.transform.up * westArm.transform.localScale.y * 2;
        Vector3 westBetween = westTop - convergePoint;
        float distance = westBetween.magnitude;
        westBand.transform.localScale = new Vector3(westBand.transform.localScale.x, westBand.transform.localScale.y, distance);
        westBand.transform.position = convergePoint + westBetween / 2;
        westBand.transform.LookAt(westTop);

        Vector3 eastTop = eastArm.transform.position + eastArm.transform.up * eastArm.transform.localScale.y * 2;
        Vector3 eastBetween = eastTop - convergePoint;
        distance = eastBetween.magnitude;
        eastBand.transform.localScale = new Vector3(eastBand.transform.localScale.x, eastBand.transform.localScale.y, distance);
        eastBand.transform.position = convergePoint + eastBetween / 2;
        eastBand.transform.LookAt(eastTop);
    }

    private void CheckTouches()
    {
		if (!amLocal)
			return;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit hitObj;
                Ray ray = camera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hitObj))
                {
                    if (hitObj.transform == projectile.transform) 
                    {
                        if (captureTracker.State == CaptureState.S_FREE)
                        {
                            captureTracker.capture();
                            CaptureProjectile();
                        }
                        else
                        {
                            captureTracker.release();
                            ReleaseProjectile();
                        }
                    }
                }
            }
        }
    }

	public bool getLaunched(){
		if (didLaunch) {
			didLaunch = false;
			return true;
		}
		return false;
	}

	public Vector3 getPos(){
		return projectile.transform.position;
	}

	public Vector3 getVel(){
		return projectile.GetComponent<Rigidbody> ().velocity;
	}
}

enum CaptureState
{
    S_CAPTURED,
    S_FREE
}

class CaptureTracker
{
    private const float distThresh = 0.5f;
    private const int releaseDelayThresh = 20;
    private CaptureState state = CaptureState.S_FREE;
    public CaptureState State {
        get
        {
            return state;
        }
    }
    private int releaseDelayCount = 0;

    public bool update(float dist)
    {
        if (state == CaptureState.S_FREE)
        {
            if(dist < distThresh && releaseDelayCount >= releaseDelayThresh)
            {
                state = CaptureState.S_CAPTURED;
                return true;
            }
            releaseDelayCount++;
        }
        return false;
    }

    public void capture()
    {
        state = CaptureState.S_CAPTURED;
    }

    public void release()
    {
        state = CaptureState.S_FREE;
        releaseDelayCount = 0;
    }
}