using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera fspCma;

    /// <summary>
    /// 跟随目标
    /// </summary>
    public Transform followTarget;

    /// <summary>
    /// 俯视距离的偏移量
    /// </summary>
    public float offset = 0.5f;

    /// <summary>
    /// 设置中间的相机位置个数
    /// </summary>
    public int gears = 5;

    /// <summary>
    /// 方向向量
    /// </summary>
    private Vector3 dir;

    /// <summary>
    /// 待选的相机位置(观察点)
    /// </summary>
    private Vector3[] readyPosition;

    /// <summary>
    /// 射线碰撞检测器
    /// </summary>
    private RaycastHit hit;

    /// <summary>
    /// 相机跟随的移动速度
    /// </summary>
    public int moveSpeed = 1;

    /// <summary>
    /// 相机旋转的速度
    /// </summary>
    public float turnSpeed = 10f;

    private void Start()
    {
        //获取方向向量
        dir = transform.position - followTarget.position;
        //实例化观察点数组
        readyPosition = new Vector3[gears];
    }

    private void Update()
    {
        return;

        //相机的起点--->相机最佳视角
        Vector3 begin = dir + followTarget.position;
        //相机的终点--->相机最差视角保证能看到玩家
        Vector3 end = followTarget.position + Vector3.up * (dir.magnitude - offset);
        //把起点和终点放到观察点的数组中
        readyPosition[0] = begin;
        readyPosition[readyPosition.Length - 1] = end;
        //获取相机中间点的位置
        for (int i = 1; i < readyPosition.Length - 1; i++)
        {
            //求中间点各个点的坐标,比例为i / (gears - 1)
            readyPosition[i] = Vector3.Lerp(begin, end, i / (readyPosition.Length - 1));
        }
        //备选方案--->前面所有的观察点都看不到玩家
        //都看不到就把相机放到最佳视角
        Vector3 watchPoint = begin;
        //遍历观察点数组,挑选我们最佳观察点
        for (int i = 0; i < readyPosition.Length; i++)
        {
            if (CheckWatchPoint(readyPosition[i]))
            {
                //设置观察点
                watchPoint = readyPosition[i];
                //得到观察点退出循环
                break;
            }
        }
        //相机平滑移动到最佳观察点
        transform.position = Vector3.Lerp(transform.position, watchPoint, Time.deltaTime * moveSpeed);
        /*
          //旋转的不是很平滑
          //相机旋转到最佳角度看向玩家
          transform.LookAt(followTarget);
         */
        //获取方向向量
        Vector3 lookDir = followTarget.position - watchPoint;
        //获取四元数
        Quaternion lookQua = Quaternion.LookRotation(lookDir);
        //平滑转向玩家
        transform.rotation = Quaternion.Lerp(transform.rotation, lookQua, Time.deltaTime * turnSpeed);
        //固定相机y轴z轴的旋转
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
    }

    /// <summary>
    /// 检测观察点是否可以看到目标
    /// </summary>
    /// <param name="point">待选的观察点</param>
    /// <returns><c>true</c>看得到<c>false</c>看不到</returns>
    private bool CheckWatchPoint(Vector3 point)
    {
        if (Physics.Raycast(point, followTarget.position - point, out hit))
        {
            //如果射线检测到的是玩家
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }
}