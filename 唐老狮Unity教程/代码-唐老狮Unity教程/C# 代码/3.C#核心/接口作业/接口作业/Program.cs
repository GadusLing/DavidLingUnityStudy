namespace 接口作业
{
    internal class Program
    {
        interface IRegister
        {
            public void Register();
        }
        class Person : IRegister
        {
            public void Register()
            {
                Console.WriteLine("人到派出所登记");
            }
        }
        class Car : IRegister
        {
            public void Register()
            {
                Console.WriteLine("汽车到车管所登记");
            }
        }
        class House : IRegister
        {
            public void Register()
            {
                Console.WriteLine("房子到房管局登记");
            }
        }

        abstract class Animal
        {
            public abstract void Walk();
        }

        interface IFlyable
        {
            void Fly();
        }

        class Sparrow : Animal, IFlyable
        {
            public override void Walk()
            {
                Console.WriteLine("麻雀可以走");
            }

            public void Fly()
            {
                Console.WriteLine("麻雀在天空中飞翔");
            }
        }

        class Penguin : Animal, IFlyable
        {
            public override void Walk()
            {
                Console.WriteLine("企鹅在冰面上摇摆行走");
            }
            public void Fly()
            {
                Console.WriteLine("企鹅不能飞");
            }
            public void Swim()
            {
                Console.WriteLine("企鹅在水中游泳");
            }
        }

        class Parrot : Animal, IFlyable
        {
            public override void Walk()
            {
                Console.WriteLine("鹦鹉在树枝上跳跃");
            }
            public void Fly()
            {
                Console.WriteLine("鹦鹉在天空中飞翔");
            }
        }

        class Ostrich : Animal
        {
            public override void Walk()
            {
                Console.WriteLine("鸵鸟在地面上奔跑");
            }
        }

        class Helicopter : IFlyable
        {
            public void Fly()
            {
                Console.WriteLine("直升机在天空中飞翔");
            }
        }

        class Swan : Animal, IFlyable
        {
            public override void Walk()
            {
                Console.WriteLine("天鹅在走路");
            }
            public void Fly()
            {
                Console.WriteLine("天鹅在天空中飞翔");
            }
            public void Swim()
            {
                Console.WriteLine("天鹅在水中游泳");
            }
        }

        static void Main(string[] args)
        {
            // 题目1：
            // 人、汽车、房子都需要登记，人需要到派出所登记，汽车需要去车管所
            // 登记，房子需要去房管局登记
            // 使用接口实现登记方法
            IRegister[] registrables = new IRegister[]
            {
                new Person(),
                new Car(),
                new House()
            };
            foreach (var registrable in registrables)
            {
                registrable.Register();
            }

            // 题目2：
            // 麻雀、鸵鸟、企鹅、鹦鹉、直升机、天鹅
            // 直升机和部分鸟能飞
            // 驼鸟和企鹅不能飞
            // 企鹅和天鹅能游泳
            // 除直升机，其它都能走
            // 请用面向对象相关知识实现
            Animal[] animals = new Animal[]
            {
                new Sparrow(),
                new Penguin(),
                new Parrot(),
                new Ostrich(),
                new Swan()
            };
            foreach (var animal in animals)
            {
                animal.Walk();
                if (animal is IFlyable flyable)
                {
                    flyable.Fly();
                }
                if (animal is Penguin penguin)
                {
                    penguin.Swim();
                }
                else if (animal is Swan swan)
                {
                    swan.Swim();
                }
            }


            // 题目3：
            // 多态来模拟移动硬盘、U盘、MP3音到电脑上读取数据
            // 移动硬盘与U盘都属于存储设备
            // MP3属于播放设备
            // 但他们都能插在电脑上传输数据
            // 电脑提供了一个USB接口
            // 请实现电脑的传输数据的功能
            IUSB[] usbDevices = new IUSB[]
            {
                new HardDrive("移动硬盘"),
                new USBDrive("U盘"),
                new MP3Player("MP3播放器")
            };
            foreach (var usbDevice in usbDevices)
            {
                usbDevice.Connect();
                usbDevice.TransferData();
            }
        }

        interface IUSB
        {
            void Connect();
            void TransferData();
        }

        class StorageDevice
        {
            public string Name { get; set; }

            public StorageDevice(string name)
            {
                Name = name;
            }
        }

        class HardDrive : StorageDevice, IUSB
        {
            public HardDrive(string name) : base(name) { }

            public void Connect()
            {
                Console.WriteLine($"{Name} 已连接");
            }
            public void TransferData()
            {
                Console.WriteLine($"从 {Name} 传输数据到电脑");
            }
        }

        class USBDrive : StorageDevice, IUSB
        {
            public USBDrive(string name) : base(name) { }

            public void Connect()
            {
                Console.WriteLine($"{Name} 已连接");
            }
            public void TransferData()
            {
                Console.WriteLine($"从 {Name} 传输数据到电脑");
            }
        }

        class MP3Player : StorageDevice, IUSB
        {
            public MP3Player(string name) : base(name) { }

            public void Connect()
            {
                Console.WriteLine($"{Name} 已连接");
            }
            public void TransferData()
            {
                Console.WriteLine($"从 {Name} 传输音乐到电脑");
            }
        }


    }
}
