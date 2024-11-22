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
        // ȷ��Canvas��World Spaceģʽ
        canvas.renderMode = RenderMode.WorldSpace;

        // ���ú��ʵ�����
        //canvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void LateUpdate()
    {
        // ��UIʼ���������
        if (mainCamera != null)
        {
            var camAngles = mainCamera.transform.eulerAngles;
            canvasRecttrans.eulerAngles = new Vector3(camAngles.x, camAngles.y, 0);
        }
    }
}