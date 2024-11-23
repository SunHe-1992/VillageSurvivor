using UnityEngine;
using SunHeTBS;
public class FollowMouse : MonoBehaviour
{
    public Camera mainCamera; // 需要指定主摄像机
    public float zOffset = 0f; // Z轴的偏移量
    private void Start()
    {
        mainCamera = CameraController.Inst.GetComponent<Camera>();
    }
    void Update()
    {

        // 从鼠标获取屏幕坐标
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // 定义地面的平面（地面在 y = 0 的位置）
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        // 检测射线和地面的交点
        if (groundPlane.Raycast(ray, out float distance))
        {
            // 计算射线与地面相交的点
            Vector3 pt = ray.GetPoint(distance);
            // 更新GameObject的位置
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
