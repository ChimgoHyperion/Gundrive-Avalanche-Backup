using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class zoomScript : MonoBehaviour
{
    
    [SerializeField] KeyCode toggle;
    public bool zoomIn;
    private CinemachineVirtualCamera virCam;
    public float minimum;
    public float maximum;
    static float t = 0f;
    public float degree;

    public GameObject background;

    public float largeScale, SmallScale;
    // Start is called before the first frame update
    void Start()
    {
      
        virCam = GetComponent<CinemachineVirtualCamera>();

    }
    public void zoom()
    {
        if (zoomIn)
        {
            zoomIn = false;
        }
        else
        {
            zoomIn = true;
        }

    }
    // Update is called once per frame
    void Update()
    {
        /*  if (zoomIn==true)
          {
            // StopAllCoroutines();
              StartCoroutine(Lerp(virCam.m_Lens.OrthographicSize, maximum));
          }
          if (zoomIn== false)
          {
             // StopAllCoroutines();
              StartCoroutine(Lerp(virCam.m_Lens.OrthographicSize, minimum));
          }*/
        LerpZoom();
    }
    public void LerpZoom()
    {
        if (zoomIn)
        {
            virCam.m_Lens.OrthographicSize = Mathf.Lerp(virCam.m_Lens.OrthographicSize, maximum, degree);


            Vector3 LargeScale = new Vector3(largeScale,largeScale,largeScale);
            if (background != null)

                background.transform.localScale = Vector2.Lerp(background.transform.localScale, LargeScale, degree);
        }
        if (zoomIn==false)
        {
            virCam.m_Lens.OrthographicSize = Mathf.Lerp(virCam.m_Lens.OrthographicSize, minimum, degree);



            Vector3 smallScale = new Vector3(SmallScale,SmallScale,SmallScale);
           if(background!=null)
           
            background.transform.localScale = Vector2.Lerp(background.transform.localScale, smallScale, degree);

        }
    }


  public  IEnumerator Lerp (float start,float end)
    {
        t = 0f;
        while (virCam.m_Lens.OrthographicSize != end)
        {
            virCam.m_Lens.OrthographicSize = Mathf.Lerp(start, end, t);
            t += Time.deltaTime*2;
            yield return null;
        }
        yield return null;
    }
}
