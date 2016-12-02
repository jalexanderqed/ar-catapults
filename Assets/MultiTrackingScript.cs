using UnityEngine;
using System.Collections;
using Vuforia;

public class MultiTrackingScript : MonoBehaviour, ITrackableEventHandler
{
    private GameObject stones;
    private GameObject chips;
    private TrackableBehaviour stoneTrack;
    private TrackableBehaviour chipsTrack;

    // Use this for initialization
    void Start () {
	    stones = GameObject.Find("stones_target");
        chips = GameObject.Find("chips_target");

        stoneTrack = stones.GetComponent<TrackableBehaviour>();
        chipsTrack = chips.GetComponent<TrackableBehaviour>();

        stoneTrack.RegisterTrackableEventHandler(this);
        chipsTrack.RegisterTrackableEventHandler(this);
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
        TrackableBehaviour.Status chipsStatus = chipsTrack.CurrentStatus;
        TrackableBehaviour.Status stoneStatus = stoneTrack.CurrentStatus;
        if (chipsStatus == TrackableBehaviour.Status.TRACKED)
        {
            return chips;
        }

        if (stoneStatus == TrackableBehaviour.Status.TRACKED)
        {
            return stones;
        }

        if (chipsStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (chipsStatus != stoneStatus)
            {
                return chips;
            }
        }

        if (stoneStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (chipsStatus != stoneStatus)
            {
                return stones;
            }
        }

        if (chipsStatus == TrackableBehaviour.Status.DETECTED)
        {
            if (chipsStatus != stoneStatus)
            {
                return chips;
            }
        }

        if (stoneStatus == TrackableBehaviour.Status.DETECTED)
        {
            if (chipsStatus != stoneStatus)
            {
                return stones;
            }
        }

        return null;
    }
}
