using System;
using System.Collections.Generic;

namespace 协变逆变作业
{
    // 基类和派生类
    public class Animal
    {
        public string Name { get; set; }
        public virtual void MakeSound() => Console.WriteLine("动物叫声");
    }

    public class Dog : Animal
    {
        public override void MakeSound() => Console.WriteLine("汪汪汪");
    }

    public class Cat : Animal
    {
        public override void MakeSound() => Console.WriteLine("喵喵喵");
    }

    // 协变接口 - 使用 out 关键字
    public interface IProducer<out T>
    {
        T Produce(); // 只能返回 T 类型，不能接收 T 类型参数
    }

    // 逆变接口 - 使用 in 关键字
    public interface IConsumer<in T>
    {
        void Consume(T item); // 只能接收 T 类型参数，不能返回 T 类型
    }

    // 实现协变接口
    public class AnimalProducer : IProducer<Animal>
    {
        public Animal Produce() => new Animal { Name = "通用动物" };
    }

    public class DogProducer : IProducer<Dog>
    {
        public Dog Produce() => new Dog { Name = "小狗" };
    }

    // 实现逆变接口
    public class AnimalConsumer : IConsumer<Animal>
    {
        public void Consume(Animal animal)
        {
            Console.WriteLine($"处理动物: {animal.Name}");
            animal.MakeSound();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 协变示例 ===");
            DemonstrateCovariance();

            Console.WriteLine("\n=== 逆变示例 ===");
            DemonstrateContravariance();

            Console.WriteLine("\n=== 内置泛型集合的协变逆变 ===");
            DemonstrateBuiltInVariance();
        }

        // 协变示例
        static void DemonstrateCovariance()
        {
            // 协变: 可以将 IProducer<Dog> 赋值给 IProducer<Animal>
            // 因为 Dog 是 Animal 的派生类
            IProducer<Animal> animalProducer = new DogProducer();
            Animal animal = animalProducer.Produce();
            Console.WriteLine($"协变产生的动物: {animal.Name}");
            animal.MakeSound();

            // IEnumerable<T> 也是协变的
            IEnumerable<Dog> dogs = new List<Dog> 
            { 
                new Dog { Name = "旺财" }, 
                new Dog { Name = "大黄" } 
            };
            
            // 可以将 IEnumerable<Dog> 赋值给 IEnumerable<Animal>
            IEnumerable<Animal> animals = dogs;
            Console.WriteLine("通过协变遍历狗的集合:");
            foreach (Animal a in animals)
            {
                Console.WriteLine($"- {a.Name}");
                a.MakeSound();
            }
        }

        // 逆变示例
        static void DemonstrateContravariance()
        {
            // 逆变: 可以将 IConsumer<Animal> 赋值给 IConsumer<Dog>
            // 因为能处理 Animal 的就能处理 Dog
            IConsumer<Dog> dogConsumer = new AnimalConsumer();
            Dog dog = new Dog { Name = "小白" };
            dogConsumer.Consume(dog);

            // Action<T> 也是逆变的
            Action<Animal> animalAction = (animal) => 
            {
                Console.WriteLine($"Action处理: {animal.Name}");
                animal.MakeSound();
            };

            // 可以将 Action<Animal> 赋值给 Action<Dog>
            Action<Dog> dogAction = animalAction;
            dogAction(new Dog { Name = "小黑" });
        }

        // 内置泛型的协变逆变示例
        static void DemonstrateBuiltInVariance()
        {
            // IEnumerable<T> 是协变的 (out T)
            List<Dog> dogList = new List<Dog> 
            { 
                new Dog { Name = "贝贝" }, 
                new Dog { Name = "豆豆" } 
            };
            IEnumerable<Animal> animalEnum = dogList; // 协变

            // Func<out TResult> 在返回值上协变
            Func<Dog> getDog = () => new Dog { Name = "函数狗" };
            Func<Animal> getAnimal = getDog; // 协变
            Console.WriteLine($"Func协变: {getAnimal().Name}");

            // Action<in T> 在参数上逆变
            Action<Animal> processAnimal = (animal) => Console.WriteLine($"处理: {animal.Name}");
            Action<Dog> processDog = processAnimal; // 逆变
            processDog(new Dog { Name = "Action狗" });

            // IComparer<T> 是逆变的 (in T)
            IComparer<Animal> animalComparer = Comparer<Animal>.Create((a1, a2) => 
                string.Compare(a1.Name, a2.Name));
            IComparer<Dog> dogComparer = animalComparer; // 逆变

            var sortedDogs = new List<Dog> 
            { 
                new Dog { Name = "Z狗" }, 
                new Dog { Name = "A狗" } 
            };
            sortedDogs.Sort(dogComparer);
            Console.WriteLine("排序后的狗:");
            sortedDogs.ForEach(d => Console.WriteLine($"- {d.Name}"));
        }
    }
}
