using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块小游戏.Input
{
    internal class InputThread
    {
        // 线程成员变量
        Thread inputThread;

        public event Action inputEvent; // 输入检测事件

        private static InputThread instance = new InputThread();

        public static InputThread Instance
        {
            get
            {
                return instance;
            }
        }

        private InputThread()
        {
            // 私有构造函数，防止外部实例化
            inputThread = new Thread(InputCheck);
            inputThread.IsBackground = true; // 设置为后台线程
            inputThread.Start(); // 启动输入检查线程
        }

        private void InputCheck()
        {
            while(true)
            {
                inputEvent?.Invoke(); 
            }
        }
    }
}
