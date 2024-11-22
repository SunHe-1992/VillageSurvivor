using UnityEngine;
using TMPro;

public class UIFollowTarget : MonoBehaviour
{
    private CameraController mainCamera;
    private Canvas canvas;
    private RectTransform canvasRecttrans;
    void Start()
    {
        mainCamera = CameraController.Inst;
        canvas = transform.Find("Canvas").GetComponent<Canvas>();
        canvasRecttrans = canvas.GetComponent<RectTransform>();
        // 确保Canvas是World Space模式
        canvas.renderMode = RenderMode.WorldSpace;

        // 设置合适的缩放
        //canvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void LateUpdate()
    {
        // 让UI始终面向相机
        if (mainCamera != null)
        {
            var camAngles = mainCamera.transform.eulerAngles;
            canvasRecttrans.eulerAngles = new Vector3(camAngles.x, camAngles.y, 0);
        }
    }
}