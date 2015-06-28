using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;
using CAM.Data;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class SimplePermissionTest
    {
        [TestMethod]
        public void TestToString()
        {
            var permission = new SimplePermission
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = ResourceType.Project.Id
            };
            var s = permission.ToString();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestToString_ResourceTypeUnknown()
        {
            var permission = new SimplePermission
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = Permission.EditProject.Id,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = 0
            };
            var s = permission.ToString();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestToString_PermissionIdUnknown()
        {
            var permission = new SimplePermission
            {
                ForeignResourceId = 1,
                IsAllowed = true,
                PermissionId = 0,
                PrincipalId = 2,
                ResourceId = 3,
                ResourceTypeId = ResourceType.Project.Id
            };
            var s = permission.ToString();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void TestGetHashCode_AreEqual()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5

            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action tester = () =>
            {
                Assert.AreEqual(firstPermission.GetHashCode(), secondPermission.GetHashCode());
            };
            tester();
        }

        [TestMethod]
        public void TestGetHashCode_AreNotEqual()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5
            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action equalTester = () =>
            {
                Assert.AreEqual(firstPermission.GetHashCode(), secondPermission.GetHashCode());
            };
            Action notEqualTester = () =>
            {
                Assert.AreNotEqual(firstPermission.GetHashCode(), secondPermission.GetHashCode());
            };
            equalTester();

            secondPermission.PrincipalId = firstPermission.PrincipalId - 1;
            notEqualTester();

            secondPermission.PrincipalId = firstPermission.PrincipalId;
            equalTester();

            secondPermission.PermissionId = firstPermission.PermissionId - 1;
            notEqualTester();

            secondPermission.PermissionId = firstPermission.PermissionId;
            equalTester();

            secondPermission.ResourceId = firstPermission.ResourceId - 1;
            notEqualTester();

            secondPermission.ResourceId = firstPermission.ResourceId;
            equalTester();

            secondPermission.PrincipalId = firstPermission.PrincipalId - 1;
            notEqualTester();

            secondPermission.PrincipalId = firstPermission.PrincipalId;
            equalTester();
        }

        [TestMethod]
        public void TestEquals_SimplePermission_IsEqual()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5
                
            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action<bool> tester = (isEqual) =>
            {
                Assert.AreEqual(isEqual, firstPermission.Equals(secondPermission));
                Assert.AreEqual(isEqual, secondPermission.Equals(firstPermission));
            };
            tester(true);
        }

        [TestMethod]
        public void TestEquals_SimplePermission_ResourceIdNotEqual()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5

            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action<bool> tester = (isEqual) => 
            {
                Assert.AreEqual(isEqual, firstPermission.Equals(secondPermission));
                Assert.AreEqual(isEqual, secondPermission.Equals(firstPermission));
            };
            tester(true);

            secondPermission.ResourceId = secondPermission.ResourceId - 1;
            tester(false);
        }

        [TestMethod]
        public void TestEquals_SimplePermission_PermissionIdNotEqual()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5

            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action<bool> tester = (isEqual) =>
            {
                Assert.AreEqual(isEqual, firstPermission.Equals(secondPermission));
                Assert.AreEqual(isEqual, secondPermission.Equals(firstPermission));
            };
            tester(true);

            secondPermission.PermissionId = secondPermission.PermissionId - 1;
            tester(false);
        }


        [TestMethod]
        public void TestEquals_SimplePermission_IsAllowedNotEqual()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5

            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action<bool> tester = (isEqual) =>
            {
                Assert.AreEqual(isEqual, firstPermission.Equals(secondPermission));
                Assert.AreEqual(isEqual, secondPermission.Equals(firstPermission));
            };
            tester(true);

            secondPermission.IsAllowed = !secondPermission.IsAllowed;
            tester(false);
        }

        [TestMethod]
        public void TestEquals_SimplePermission_PrincipalIdNotEqual()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5
            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action<bool> tester = (isEqual) =>
            {
                Assert.AreEqual(isEqual, firstPermission.Equals(secondPermission));
                Assert.AreEqual(isEqual, secondPermission.Equals(firstPermission));
            };
            tester(true);

            secondPermission.PrincipalId = secondPermission.PrincipalId - 1;
            tester(false);
        }

        [TestMethod]
        public void TestEquals_SimplePermission_SimplePermissionIsNull()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5
            };
            SimplePermission secondPermission = null;
            Action<bool> tester = (isEqual) =>
            {
                Assert.AreEqual(isEqual, firstPermission.Equals(secondPermission));
            };
            tester(false);
        }

        [TestMethod]
        public void TestEquals_Object_IsEqual()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5

            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action<bool> tester = (isEqual) =>
            {
                Assert.AreEqual(isEqual, firstPermission.Equals(secondPermission as object));
                Assert.AreEqual(isEqual, secondPermission.Equals(firstPermission as object));
            };
            tester(true);
        }

        [TestMethod]
        public void TestEquals_Object_IsNull()
        {
            var firstPermission = new SimplePermission
            {
                ResourceId = 1,
                PermissionId = 2,
                PrincipalId = 3,
                IsAllowed = true,
                ForeignResourceId = 10,
                ResourceTypeId = 5

            };
            var secondPermission = new SimplePermission
            {
                ResourceId = firstPermission.ResourceId,
                PermissionId = firstPermission.PermissionId,
                IsAllowed = firstPermission.IsAllowed,
                PrincipalId = firstPermission.PrincipalId,
                ForeignResourceId = 100,
                ResourceTypeId = 50
            };
            Action<bool> tester = (isEqual) =>
            {
                Object o = null;
                Assert.AreEqual(isEqual, firstPermission.Equals(o));
                Assert.AreEqual(isEqual, secondPermission.Equals(o));
            };
            tester(false);
        }

    }
}
