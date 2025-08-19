using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 贪吃蛇.Main.UI;


namespace 贪吃蛇.Main
{
    internal class S_EndScene : S_AbstractBaseScene
    {
        protected override void InitializeUI()
        {
            // 添加标题
            AddUIElement(new TitleElement("游戏结束"));
            AddSpacer(2); // 1行间距

            // 添加按钮
            AddUIElement(new ButtonElement("返回开始界面", () => Game.ChangeScene(E_SceneType.Start)));
            AddSpacer(); // 1行间距
            AddUIElement(new ButtonElement("退出游戏", () => Environment.Exit(0)));
        }
    }
}
