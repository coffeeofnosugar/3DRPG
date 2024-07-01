using Cinemachine;
using UnityEngine;

public class CMFreelookOnlyWhenRightMouseDown : MonoBehaviour
{
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse ScrollWheel")
        {
            return UnityEngine.Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }
}
