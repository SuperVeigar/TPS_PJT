using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform p_trTargetTr = null;
    public float p_fDist = 10.0f;
    public float p_fHeight = 3.0f;
    public float p_fDampTrace = 20.0f;
    public float p_fCamWidthHalf = 2.0f;

    private RaycastHit m_hitCamBack;
    private RaycastHit m_hitCamSide;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        MoveCamera();
    }


    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, p_trTargetTr.position - (p_trTargetTr.forward * p_fDist) + Vector3.up * p_fHeight, Time.deltaTime * p_fDampTrace);
        transform.LookAt(p_trTargetTr.position);
    }

    // Detecting obstacles
    private void MoveCamera_2()
    {
        Vector3 v3NewCamPos = Vector3.Lerp(transform.position, p_trTargetTr.position - (p_trTargetTr.forward * p_fDist) + Vector3.up * p_fHeight, Time.deltaTime * p_fDampTrace);
        Vector3 v3LookAtPos = p_trTargetTr.position;
        Vector3 v3RayCastPos = p_trTargetTr.position + new Vector3(0, p_fHeight * 0.5f, 0);

        // Check any obstacle behind the character interupting this camera.
        if (Physics.Raycast(v3RayCastPos, p_trTargetTr.forward * -1, out m_hitCamBack, p_fDist) == true)
        {
            v3NewCamPos = p_trTargetTr.position - (p_trTargetTr.forward * m_hitCamBack.distance) + Vector3.up * p_fHeight;
        }

        transform.position = v3NewCamPos;
        transform.LookAt(v3LookAtPos);
    }

    // Detecting obstacles on back and adjust 'cam lookat pos'
    private void MoveCamera_3()
    {
        Vector3 v3NewCamPos = Vector3.Lerp(transform.position, p_trTargetTr.position - (p_trTargetTr.forward * p_fDist) + Vector3.up * p_fHeight, Time.deltaTime * p_fDampTrace);
        Vector3 v3LookAtPos = p_trTargetTr.position;
        Vector3 v3RayCastPos = p_trTargetTr.position + new Vector3(0, p_fHeight * 0.5f, 0);

        // Check any obstacle behind the character interupting this camera.
        if (Physics.Raycast(v3RayCastPos, p_trTargetTr.forward * -1, out m_hitCamBack, p_fDist) == true)
        {
            v3NewCamPos = p_trTargetTr.position - (p_trTargetTr.forward * m_hitCamBack.distance) + Vector3.up * p_fHeight;
            v3LookAtPos = p_trTargetTr.position + p_trTargetTr.forward * (p_fDist - m_hitCamBack.distance);            
        }

        transform.position = v3NewCamPos;
        transform.LookAt(v3LookAtPos);
    }

    // Detecting obstacles on back and both sids, then adjust 'cam lookat pos'
    private void MoveCamera_4()
    {
        Vector3 v3NewCamPos = Vector3.Lerp(transform.position, p_trTargetTr.position - (p_trTargetTr.forward * p_fDist) + Vector3.up * p_fHeight, Time.deltaTime * p_fDampTrace);
        Vector3 v3LookAtPos = p_trTargetTr.position;
        Vector3 v3RayCastPos = p_trTargetTr.position + new Vector3(0, p_fHeight * 0.5f, 0);        

        // Check any obstacle behind the character interupting this camera.
        if (Physics.Raycast(v3RayCastPos, p_trTargetTr.forward * -1, out m_hitCamBack, p_fDist) == true)
        {
            v3NewCamPos = p_trTargetTr.position - (p_trTargetTr.forward * m_hitCamBack.distance) + Vector3.up * p_fHeight;
            v3LookAtPos = p_trTargetTr.position + p_trTargetTr.forward * (p_fDist - m_hitCamBack.distance);
        }


        // Check any obstacle on cam's both sides to avoid cam sight penetration.
        if (Physics.Raycast(v3NewCamPos, transform.right, out m_hitCamSide, p_fCamWidthHalf) == true )
        {
            float fAngle = CalcAngle(transform.right, m_hitCamSide.normal);
            Debug.Log("fAngle " + fAngle);

            float fDist = Mathf.Tan(fAngle) * p_fCamWidthHalf;
            fDist = Mathf.Ceil(fDist * 10) * 0.1f;
            fDist = Mathf.Min(fDist, p_fDist);
            Debug.Log("fDist " + fDist);

            v3NewCamPos = v3NewCamPos + p_trTargetTr.forward * fDist;
        }
        //else if (Physics.Raycast(v3NewCamPos, transform.right * -1, out m_hitCamSide, p_fCamWidth) == true)
        //{
        //    float fAngle = CalcAngle(transform.right, m_hitCamSide.normal);
        //    float fDist = Mathf.Tan(fAngle) * p_fCamWidth;
        //
        //    v3NewCamPos = v3NewCamPos + p_trTargetTr.forward * fDist;
        //}
        

        transform.position = v3NewCamPos;
        transform.LookAt(v3LookAtPos);
    }

    // Return radian
    private float CalcAngle(Vector3 v3Vec1, Vector3 v3Vec2)
    {
        float fAngle = 0.0f;
        float fDot = Vector3.Dot(v3Vec1, v3Vec2);

        fAngle = Mathf.Acos(fDot);

        if(fAngle > Mathf.PI * 0.5)
        {
            fAngle= Mathf.PI - fAngle;
        }

        fAngle = Mathf.Ceil(fAngle * 10) * 0.1f;

        return fAngle;
    }
}
