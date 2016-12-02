using UnityEngine;
using System.Collections;
using Vuforia;

public class DebugColor : MonoBehaviour, ITrackableEventHandler
{
    public Material red;
    public Material white;
    public Material purple;
    public Material black;

    private GameObject myParent;
    private TrackableBehaviour parentTrack;

    // Use this for initialization
    void Start()
    {
        myParent = transform.parent.gameObject;
        parentTrack = myParent.GetComponent<TrackableBehaviour>();

        parentTrack.RegisterTrackableEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTrackableStateChanged(
                            TrackableBehaviour.Status previousStatus,
                            TrackableBehaviour.Status newStatus)
    {
        this.GetComponent<Renderer>().sharedMaterial = StatusToMaterial(newStatus);
    }

    private Material StatusToMaterial(TrackableBehaviour.Status status)
    {
        Material ret;
        switch (status)
        {
            case TrackableBehaviour.Status.DETECTED:
                ret = purple;
                break;
            case TrackableBehaviour.Status.EXTENDED_TRACKED:
                ret = red;
                break;
            case TrackableBehaviour.Status.TRACKED:
                ret = white;
                break;
            case TrackableBehaviour.Status.NOT_FOUND:
            case TrackableBehaviour.Status.UNDEFINED:
            case TrackableBehaviour.Status.UNKNOWN:
            default:
                ret = black;
                break;
        }
        return ret;
    }
}
