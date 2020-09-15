using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Object _area;
    public IArea Area
    {
        get { return _area as IArea; }
    }

    [SerializeField] private AnimationCurve xAxis, yAxis;
    [SerializeField] private string turnAxisX, turnAxisY;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Transform rotationTarget;

    public void Start()
    {
        CinemachineCore.GetInputAxis = GetDragAxis;
    }

    public float GetDragAxis(string axis)
    {
        if (axis == turnAxisX)
        {
            if(Input.GetButton(turnAxisX))
            {
                return Input.GetAxis("Mouse X");
            }
            else
            {
                return 0;
            }
        }
        return Input.GetAxis(axis);
    }

    void Update()
    {
        Vector3 dir = new Vector3(
            xAxis.Evaluate(Mathf.Clamp01(Input.mousePosition.x / Screen.width)),
            0,
            yAxis.Evaluate(Mathf.Clamp01(Input.mousePosition.y / Screen.height))) 
                      * Time.deltaTime * movementSpeed;

        if (rotationTarget)
        {
            transform.rotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(rotationTarget.forward, Vector3.up));
        }

        transform.position = Area.GetPositionInArea(transform.position +
            transform.right * dir.x +
            transform.forward * dir.z);
    }

    public void MoveTo(Vector3 destination)
    {
        transform.position = Area.GetPositionInArea(destination);
    }

    public void MoveTo(RaycastHit destination)
    {
        MoveTo(destination.point);
    }
}
