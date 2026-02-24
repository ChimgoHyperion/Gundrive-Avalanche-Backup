using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LineRenderSync : MonoBehaviour,IPunObservable
{
    public PhotonView view;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                linePositioons[i] = lineRenderer.GetPosition(i);
                stream.SendNext(linePositioons[i]);
            }
        }
        else
        {
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                linePositioons[i] = (Vector3)stream.ReceiveNext();
                lineRenderer.SetPosition(i, linePositioons[i]);
            }
        }
        
    }
    [SerializeField] private LineRenderer lineRenderer;
    private Vector3[] linePositioons;

    // Start is called before the first frame update
    void Start()
    {
        linePositioons = new Vector3[lineRenderer.positionCount];
    }

    // Update is called once per frame
    void Update()
    {
      //  view.ObservedComponents.Add(lineRenderer);
    }
}
