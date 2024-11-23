using UnityEngine;
using SunHeTBS;
public class FollowMouse : MonoBehaviour
{
    public Camera mainCamera; // ��Ҫָ���������
    public float zOffset = 0f; // Z���ƫ����
    private void Start()
    {
        mainCamera = CameraController.Inst.GetComponent<Camera>();
    }
    void Update()
    {

        // ������ȡ��Ļ����
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // ��������ƽ�棨������ y = 0 ��λ�ã�
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        // ������ߺ͵���Ľ���
        if (groundPlane.Raycast(ray, out float distance))
        {
            // ��������������ཻ�ĵ�
            Vector3 pt = ray.GetPoint(distance);
            // ����GameObject��λ��
            transform.position = pt;
        }

        if (Input.GetMouseButtonUp(0)) // confirm
        {
            BattleDriver.Inst.EndDecidingBuidlingLocation(true);
        }
        if (Input.GetMouseButtonUp(1)) //  cancel 
        {
            BattleDriver.Inst.EndDecidingBuidlingLocation(false);
        }
    }

}
