using CsEcs;
using CsEcs.SimpleEdits;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests
{
    public class Tests
    {
        Ecs _ecs;

        [SetUp]
        public void Setup()
        {
            _ecs = new Ecs("TEST_ECS");
        }
        
        [Test]
        public void CanCreateEntities()
        {
            EntityBuilder.New(_ecs, "TEST");
            Assert.AreEqual(1, _ecs.EntityCount);
            Assert.AreEqual(0, _ecs.ComponentCount);
        }

        [Test]
        public void CanRetrieveEntityIdOnCreation()
        {
            var id = EntityBuilder.New(_ecs)
                .Add(new TestName("This is a name"))
                .Add(new TestPosition(1, 1))
                .Done();

            Assert.AreNotEqual("", id);
        }

        [Test]
        public void CanAddComponentsToEntities()
        {
            EntityBuilder.New(_ecs, "TEST")
                .Add(new TestName("TESTADD"))
                .Add(new TestPosition(10, 20));

            Assert.AreEqual("TESTADD", _ecs.Get<TestName>("TEST").Name);
            Assert.AreEqual(20, _ecs.Get<TestPosition>("TEST").Y);
        }

        [Test]
        public void CannotAddSameComponentTwice()
        {
            Assert.Throws(typeof(ArgumentException), () =>
                EntityBuilder.New(_ecs, "TEST")
                    .Add(new TestName("THISNAME"))
                    .Add(new TestName("ANOTHERNAME")));
        }

        [Test]
        public void CanRemoveComponent()
        {
            EntityBuilder.New(_ecs, "E-1")
                .Add(new TestName("NAME-1"))
                .Add(new TestPosition(1, 1));

            EntityBuilder.New(_ecs, "E-2")
                .Add(new TestName("NAME-2"))
                .Add(new TestPosition(2, 2));

            _ecs.RemoveComponent("E-1", "TestPosition");

            Assert.IsNull(_ecs.Get<TestPosition>("E-1"));
            Assert.IsNotNull(_ecs.Get<TestName>("E-1"));

            Assert.IsNotNull(_ecs.Get<TestPosition>("E-2"));
        }

        [Test]
        public void CanFindEntitiesWith()
        {
            EntityBuilder.New(_ecs, "YES-1")
                .Add(new TestName("NAME-1"))
                .Add(new TestPosition(1, 1));

            EntityBuilder.New(_ecs, "NO-1")
                .Add(new TestName("NAME-2"));

            EntityBuilder.New(_ecs, "NO-2")
                .Add(new TestPosition(3,3));

            EntityBuilder.New(_ecs, "YES-2")
                .Add(new TestName("NAME-4"))
                .Add(new TestPosition(4, 4));

            var result = _ecs.EntitiesWith("TestName", "TestPosition");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("YES-1", result[0]);
            Assert.AreEqual("YES-2", result[1]);
        }

        [Test]
        public void CanGetComponents()
        {
            EntityBuilder.New(_ecs, "YES-1")
                .Add(new TestName("NAME-1"))
                .Add(new TestPosition(1, 1));

            EntityBuilder.New(_ecs, "NO-1")
                .Add(new TestName("NAME-2"));

            EntityBuilder.New(_ecs, "NO-2")
                .Add(new TestPosition(3, 3));

            EntityBuilder.New(_ecs, "YES-2")
                .Add(new TestName("NAME-4"))
                .Add(new TestPosition(4, 4));

            var result = _ecs.GetComponents<TestName, TestPosition>();
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual("NAME-1", result[0].Item1.Name);
            Assert.AreEqual(4, result[1].Item2.X);
        }

        [Test]
        public void CanGetComponentsFromReducedEntityList()
        {
            EntityBuilder.New(_ecs, "ONE")
                .Add(new TestName("one"))
                .Add(new TestPosition(1, 1));

            EntityBuilder.New(_ecs, "TWO")
                .Add(new TestName("two"))
                .Add(new TestPosition(2, 2));

            EntityBuilder.New(_ecs, "THREE")
                .Add(new TestName("three"))
                .Add(new TestPosition(3, 3));

            EntityBuilder.New(_ecs, "FOUR")
                .Add(new TestName("four"))
                .Add(new TestPosition(4, 4));

            EntityBuilder.New(_ecs, "FIVE")
                .Add(new TestName("five"))
                .Add(new TestPosition(5, 5));

            var result = _ecs.GetComponents<TestName, TestPosition>("ONE", "FOUR", "FIVE");
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("four", result[1].Item1.Name);
        }


        [Test]
        public void CanDestroyEntity()
        {
            EntityBuilder.New(_ecs, "YES-1")
                .Add(new TestName("NAME-1"))
                .Add(new TestPosition(1, 1));

            EntityBuilder.New(_ecs, "NO-1")
                .Add(new TestName("NAME-2"));

            EntityBuilder.New(_ecs, "YES-2")
                .Add(new TestName("NAME-4"))
                .Add(new TestPosition(4, 4));

            _ecs.DestroyEntity("NO-1");

            Assert.IsNull(_ecs.Entity("NO-1"));
            Assert.IsNotNull(_ecs.Entity("YES-1"));
            Assert.IsNotNull(_ecs.Entity("YES-2"));
        }

        [Test]
        public void CanEditEntityWithEditClass()
        {
            _ecs.New("TEST")
                .Add(new TestName("NAME"))
                .Add(new TestPosition(1, 1));

            var test = _ecs.GetComponents<TestName, TestPosition>().First();
            test.Item1.DoEdit(new StringEdit("NEW-NAME"));
            test.Item2.DoEdit(new PosEdit(5, 5));

            var nameResult = _ecs.Get<TestName>("TEST");
            Assert.AreEqual("NEW-NAME", nameResult.Name);

            var posResult = _ecs.Get<TestPosition>("TEST");
            Assert.AreEqual(5, posResult.X);
            Assert.AreEqual(5, posResult.Y);

        }

        [Test]
        public void CanCopyEntityToNewEcs()
        {
            var target = new Ecs("TARGET");

            _ecs.New("TEST")
                .Add(new TestPosition(1, 1))
                .Add(new TestName("TEST_NAME"));

            _ecs.CopyEntityTo("TEST", target);

            var result = target.Get<TestName>("TEST");
            Assert.AreEqual("TEST_NAME", result.Name);

            Assert.AreEqual("TARGET", result.MyEcs.Id);
        }

        [Test]
        public void CanMoveEntityToNewEcs()
        {
            var target = new Ecs("TARGET");

            _ecs.New("TEST")
                .Add(new TestPosition(1, 1))
                .Add(new TestName("TEST_NAME"));

            _ecs.MoveEntityTo("TEST", target);

            var result = target.Get<TestName>("TEST");
            Assert.AreEqual("TEST_NAME", result.Name);

            Assert.AreEqual("TARGET", result.MyEcs.Id);
        }

        [Test]
        public void CanOverwriteExistingEntityOnCopy()
        {
            var target = new Ecs("TARGET");
            target.New("TEST")
                .Add(new TestPosition(1, 1))
                .Add(new TestName("OLD_NAME"));

            _ecs.New("TEST")
                .Add(new TestPosition(10, 10))
                .Add(new TestName("NEW_NAME"));

            _ecs.CopyEntityTo("TEST", target);

            (var testName, var testPos) = target.Get<TestName, TestPosition>("TEST");
            Assert.AreEqual("NEW_NAME", testName.Name);
            Assert.AreEqual(10, testPos.X);
        }

        [Test]
        public void CanAddComponentIndex()
        {
            _ecs.AddIndex("TestPosition");
            Assert.IsTrue(_ecs.ComponentIndexes.ContainsKey("TestPosition"));
        }

        [Test]
        public void WhenIndexableComponentIsAddedIndexUpdates()
        {
            _ecs.AddIndex("TestPosition");
            
            var item = _ecs.New("ITEM")
                .Add(new TestPosition(8, 9));

            var index = _ecs.ComponentIndexes["TestPosition"];
            Assert.IsTrue(index.ContainsKey("8/9"));

            var result = _ecs.EntitiesInIndex("TestPosition", "8/9").Single();
            Assert.AreEqual("ITEM", result);
        }

        [Test]
        public void ComponentIndexHoldsMultipleEntityIds()
        {
            _ecs.AddIndex("TestPosition");

            _ecs.New("ITEM_A")
                .Add(new TestPosition(8, 9));

            _ecs.New("ITEM_B")
                .Add(new TestPosition(8, 9));

            var result = _ecs.EntitiesInIndex("TestPosition", "8/9");
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void IndexesUpdateWhenComponentsEdited()
        {
            _ecs.AddIndex("TestPosition");

            _ecs.New("ITEM_A")
                .Add(new TestPosition(8, 9));

            _ecs.New("ITEM_B")
                .Add(new TestPosition(8, 9));

            var comp2 = _ecs.Get<TestPosition>("ITEM_B");
            comp2.DoEdit(new PosEdit(18, 19));

            var result = _ecs.EntitiesInIndex("TestPosition", "8/9");
            Assert.AreEqual(1, result.Count);

            var result2 = _ecs.EntitiesInIndex("TestPosition", "18/19");
            Assert.AreEqual(1, result2.Count);
            Assert.AreEqual("ITEM_B", result2.Single());
        }

        [Test]
        public void IndexUpdatesWhenEntityRemoved()
        {
            _ecs.AddIndex("TestPosition");

            _ecs.New("ITEM_A")
                .Add(new TestPosition(8, 9));

            _ecs.New("ITEM_B")
                .Add(new TestPosition(8, 9));

            var result2 = _ecs.EntitiesInIndex("TestPosition", "8/9");
            Assert.AreEqual(2, result2.Count);

            _ecs.DestroyEntity("ITEM_A");

            var result1 = _ecs.EntitiesInIndex("TestPosition", "8/9");
            Assert.AreEqual(1, result1.Count);
            Assert.AreEqual("ITEM_B", result1.Single());

        }

        [Test]
        public void IndexUpdatesWhenComponentsRemoved()
        {
            _ecs.AddIndex("TestPosition");

            _ecs.New("ITEM_A")
                .Add(new TestPosition(8, 9));

            _ecs.New("ITEM_B")
                .Add(new TestPosition(8, 9));

            var result2 = _ecs.EntitiesInIndex("TestPosition", "8/9");
            Assert.AreEqual(2, result2.Count);

            _ecs.RemoveComponent("ITEM_A", "TestPosition");

            var result1 = _ecs.EntitiesInIndex("TestPosition", "8/9");
            Assert.AreEqual(1, result1.Count);
            Assert.AreEqual("ITEM_B", result1.Single());
        }
    }


    public class TestPosition : Component<PosEdit>, IIndexable
    {
        public override Type MyType => typeof(TestPosition);
        public int X { get; set; }
        public int Y { get; set; }

        public TestPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override void DoEdit(PosEdit values)
        {
            base.DoEdit(values);
            X = values.X ?? X;
            Y = values.Y ?? Y;
        }

        public override IComponent Copy()
        {
            return new TestPosition(X,Y);
        }

        public string IndexKey => $"{X}/{Y}";
    }

    public class TestName:Component<StringEdit>
    {
        public override Type MyType => typeof(TestName);
        public string Name { get; set; }

        public TestName(string name)
        {
            Name = name;
        }

        public override void DoEdit(StringEdit values)
        {
            Name = values.NewValue ?? Name;
        }

        public override IComponent Copy()
        {
            return new TestName(Name);
        }
    }

}