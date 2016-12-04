using UnityEngine;
using System.Collections;
using Vuforia;

public class MultiTrackingScript : MonoBehaviour, ITrackableEventHandler
{
    public GameObject[] targets;
    private TrackableBehaviour[] targetsTracking;

    // Use this for initialization
    void Start () {
        targetsTracking = new TrackableBehaviour[targets.Length];
        for (int i = 0; i < targets.Length; i++) {
            targetsTracking[i] = targets[i].GetComponent<TrackableBehaviour>();
            targetsTracking[i].RegisterTrackableEventHandler(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTrackableStateChanged(
                            TrackableBehaviour.Status previousStatus,
                            TrackableBehaviour.Status newStatus)
    {
        GameObject bestTracked = GetBestTracked();
        if(bestTracked != null)
        {
            this.transform.parent = bestTracked.transform;
        }
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private string StatusToString(TrackableBehaviour.Status status)
    {
        string ret;
        switch (status)
        {
            case TrackableBehaviour.Status.DETECTED:
                ret = "DETECTED";
                break;
            case TrackableBehaviour.Status.EXTENDED_TRACKED:
                ret = "EXTENDED_TRACKED";
                break;
            case TrackableBehaviour.Status.NOT_FOUND:
                ret = "NOT_FOUND";
                break;
            case TrackableBehaviour.Status.TRACKED:
                ret = "TRACKED";
                break;
            case TrackableBehaviour.Status.UNDEFINED:
                ret = "UNDEFINED";
                break;
            case TrackableBehaviour.Status.UNKNOWN:
                ret = "UNKNOWN";
                break;
            default:
                ret = "UNKNOWN_STATUS";
                break;
        }
        return ret;
    }

    private GameObject GetBestTracked()
    {
        TrackableBehaviour.Status bestStatus = TrackableBehaviour.Status.NOT_FOUND;
        int bestIndex = -1;
        for (int i = 0; i < targetsTracking.Length; i++)
        {
            if(Compare(targetsTracking[i].CurrentStatus, bestStatus) > 0)
            {
                bestStatus = targetsTracking[i].CurrentStatus;
                bestIndex = i;
            }
        }

        if (bestIndex != -1)
        {
            return targets[bestIndex];
        }
        else
        {
            return null;
        }
    }

    private int Compare(TrackableBehaviour.Status a, TrackableBehaviour.Status b)
    {
        if (a == b) return 0;
        if (a == TrackableBehaviour.Status.TRACKED) return 1;
        if (b == TrackableBehaviour.Status.TRACKED) return -1;
        if (a == TrackableBehaviour.Status.EXTENDED_TRACKED) return 1;
        if (b == TrackableBehaviour.Status.EXTENDED_TRACKED) return -1;
        if (a == TrackableBehaviour.Status.DETECTED) return 1;
        if (b == TrackableBehaviour.Status.DETECTED) return -1;
        return 0;
    }
}
