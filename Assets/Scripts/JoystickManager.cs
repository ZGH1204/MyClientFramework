using UnityEngine;

public class JoystickManager : MonoBehaviour
{
    public Transform controller;
    public CharacterController characterController;
    public Camera fspCam;
    public AnimatorPlay animatorPlay;

    public ETCJoystick moveJoystick;
    public ETCJoystick rotationJoystick;
    public ETCButton shoot;
    public ETCButton jump;

    private Vector3 m_startEulerAngles;
    private Vector3 startPosition;

    private Vector3 offset;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 onMoveMotion;
    private float jumpRate;

    private void Start()
    {
        //移动
        moveJoystick.onMoveStart.AddListener(() =>
        {
        });
        moveJoystick.onMove.AddListener((deltaPosition) =>
        {
            controller.transform.localEulerAngles += new Vector3(0, fspCam.transform.localEulerAngles.y - controller.transform.localEulerAngles.y, 0);

            onMoveMotion = new Vector3(deltaPosition.x, 0.2f, deltaPosition.y) * 3f * Time.deltaTime;

            if (Mathf.Abs(deltaPosition.y) > Mathf.Abs(deltaPosition.x))
            {
                animatorPlay.Walk_Crouch_Rilfe();
            }
            else
            {
                if (deltaPosition.x > 0)
                {
                    animatorPlay.Run_Strafe_Right();
                }
                else
                {
                    animatorPlay.Run_Strafe_Left();
                }
            }
        });
        moveJoystick.onMoveEnd.AddListener(() =>
        {
            animatorPlay.Run_End();
            onMoveMotion = Vector3.zero;
        });

        //射击
        shoot.onUp.AddListener(() => { Debug.Log(""); });

        //调
        jump.onUp.AddListener(() =>
        {
            if (jumpRate < 0)
            {
                jumpRate = 1f;
                moveDirection.y = 7f;
                animatorPlay.Jump();
            }
        });
    }

    private void Update()
    {
        jumpRate -= Time.deltaTime;

        var v1 = controller.right * onMoveMotion.x + controller.forward * onMoveMotion.z + controller.up * onMoveMotion.y;
        var v2 = v1 + new Vector3(0, moveDirection.y * Time.deltaTime - v1.y, 0);
        v2.y = v2.y < -0.2f ? -0.2f : v2.y;

        characterController.Move(v2);
        moveDirection.y -= 20f * Time.deltaTime;
    }

    /// <summary>
    /// 根据手指滑动向量，计算相机需要跟随旋转的向量
    /// </summary>
    public static Vector3 CameraRotationFollowFinger(Camera camera, Vector3 startPosition, Vector3 position)
    {
        float hFOV = GetCameraHorizontalAngle(camera);
        //转换系数
        float k = 2f * camera.farClipPlane * Mathf.Tan(hFOV / 2f / 180f * Mathf.PI) / Screen.width;
        float angle1 = Mathf.Atan(Mathf.Abs(position.x - Screen.width / 2f) * k / camera.farClipPlane);
        float angle2 = Mathf.Atan(Mathf.Abs(startPosition.x - Screen.width / 2f) * k / camera.farClipPlane);
        float disAngleX = 0f;
        if (startPosition.x > Screen.width / 2f)
        {
            disAngleX = position.x > Screen.width / 2f ? (angle1 - angle2) : -1f * (angle1 + angle2);
        }
        else
        {
            disAngleX = position.x < Screen.width / 2f ? -1f * (angle1 - angle2) : (angle1 + angle2);
        }

        float vFOV = GetCameraVerticalAngle(camera);
        //转换系数
        k = 2f * camera.farClipPlane * Mathf.Tan(vFOV / 2f / 180f * Mathf.PI) / Screen.height;
        angle1 = Mathf.Atan(Mathf.Abs(position.y - Screen.height / 2f) * k / camera.farClipPlane);
        angle2 = Mathf.Atan(Mathf.Abs(startPosition.y - Screen.height / 2f) * k / camera.farClipPlane);
        float disAngleY = 0f;
        if (startPosition.y > Screen.height / 2f)
        {
            disAngleY = position.y > Screen.height / 2f ? (angle1 - angle2) : -1f * (angle1 + angle2);
        }
        else
        {
            disAngleY = position.y < Screen.height / 2f ? -1f * (angle1 - angle2) : (angle1 + angle2);
        }

        return new Vector3(disAngleY * 180f / Mathf.PI, disAngleX * -180f / Mathf.PI);
    }

    /// <summary>
    /// 相机水平夹角
    /// </summary>
    public static float GetCameraHorizontalAngle(Camera cam)
    {
        var radAngle = cam.fieldOfView * Mathf.Deg2Rad;
        var radHFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * cam.aspect);
        var hFOV = Mathf.Rad2Deg * radHFOV;
        return hFOV;
    }

    /// <summary>
    /// 相机垂直夹角
    /// </summary>
    public static float GetCameraVerticalAngle(Camera cam)
    {
        var vFOV = cam.fieldOfView;
        return vFOV;
    }
}