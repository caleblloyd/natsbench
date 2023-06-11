using JetBrains.dotMemoryUnit;

namespace test1;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        void Isolator()
        {
            var myObject = new MyClass(this);
            Console.WriteLine($"{myObject.GetType()}");
        }
        
        Isolator();

        GC.Collect();
        
        dotMemory.Check(memory =>
        {
            var count = memory.GetObjects(where => where.Type.Is<MyClass>()).ObjectsCount;
            Console.WriteLine($"COUNT={count}");
            Assert.That(count, Is.EqualTo(0));
        });
        
        Assert.Fail("fail this one!");
    }

    private WeakReference<MyClass>? _weak;

    public MyClass? MyObj
    {
        get
        {
            if (_weak.TryGetTarget(out var target))
            {
                return target;
            }

            return null;
        }
        set
        {
            if (value == null)
            {
                _weak = null;
                return;
            }
            _weak = new WeakReference<MyClass>(value);
        }
    }
}

public class MyClass
{
    private readonly Tests _tests;

    public MyClass(Tests tests)
    {
        _tests = tests;
        _tests.MyObj = this;
    }
}