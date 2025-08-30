using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetTest : MonoBehaviour
{
        /*
    ==================== Unity Camera 常用设置 ====================

    1. Clear Flags（清除标志）
       - 决定相机渲染前如何清除屏幕。
       - Skybox（天空盒）、Solid Color（纯色）、Depth Only（只清深度）、Don’t Clear（不清除）。
       - 常用：主相机用 Skybox，小相机/特效用 Depth Only。

    2. Culling Mask（剔除遮罩）
       - 决定相机渲染哪些 Layer。
       - 常用：只渲染 UI 层、只渲染角色层等。

    3. Projection（投影方式）
       - Perspective（透视投影，3D 常用，远小近大）。
       - Orthographic（正交投影，2D 或策略游戏用，无远近大小变化）。

    4. Clipping Planes（裁剪平面）
       - Near（最近可见距离）、Far（最远可见距离）。
       - Near 太大：近处物体看不到；Far 太大：深度精度下降。

    5. Depth（相机深度顺序）
       - 控制相机渲染先后。
       - 数值小的相机先渲染，数值大的后渲染（覆盖前面的画面）。
       - 常用：UI 相机 Depth > 主相机。

    6. Rendering Path（渲染路径）
       - Forward（前向渲染，性能轻量，常用）。
       - Deferred（延迟渲染，支持大量灯光，更耗性能）。
       - Legacy Vertex Lit（旧的，很少用）。
       - 根据项目性能与光照需求选择。

    7. Target Texture（目标纹理）
       - 默认：渲染到屏幕。
       - 如果绑定 Render Texture，则渲染到一张纹理贴图。
       - 常用：小地图、监控摄像头效果。

    8. Occlusion Culling（遮挡剔除）
       - 是否启用遮挡剔除。
       - 启用后，相机不会渲染被挡住的物体，提升性能。

    -------------------- 其他设置 --------------------

    9. Viewport Rect（视口矩形）
       - 设置相机画面显示在屏幕的哪一部分（x, y, width, height）。
       - 常用：分屏游戏。

    10. Allow HDR（允许 HDR）
        - 是否输出高动态范围画面。
        - 画面亮暗层次更真实。

    11. Allow MSAA（允许抗锯齿）
        - 是否开启多重采样抗锯齿（边缘更平滑）。

    12. Allow Dynamic Resolution（动态分辨率）
        - 根据性能动态调整渲染分辨率，保证帧率。

    13. Target Display（目标显示器）
        - 当有多个显示器时，选择相机画面输出到哪个显示器。

    ===========================================================
    老师标红的（常用设置）：Clear Flags, Culling Mask, Projection, Clipping Planes, Depth,
    Rendering Path, Target Texture, Occlusion Culling
    ===========================================================
    */

}
