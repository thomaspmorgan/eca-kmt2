using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ECA.WebApi.Test.Security
{
    [TestClass]
    public class ActionPermissionTest
    {

        [TestMethod]
        public void TestGetResourceId()
        {
            var value = 1;
            var argumentName = "id";
            var resourceType = "type";
            var permissionName = "name";

            var actionPermission = new ActionPermission
            {
                ArgumentName = argumentName,
                ResourceType = resourceType,
                PermissionName = permissionName
            };

            var dictionary = new Dictionary<string, object>
            {
                {argumentName, value}
            };
            Assert.AreEqual(value, actionPermission.GetResourceId(dictionary));
        }

        [TestMethod]
        public void TestGetResourceId_ValueIsNotAnInt()
        {
            var value = "S";
            var argumentName = "id";
            var resourceType = "type";
            var permissionName = "name";

            var actionPermission = new ActionPermission
            {
                ArgumentName = argumentName,
                ResourceType = resourceType,
                PermissionName = permissionName
            };

            var dictionary = new Dictionary<string, object>
            {
                {argumentName, value}
            };
            actionPermission.Invoking(x => x.GetResourceId(dictionary))
                .ShouldThrow<NotSupportedException>()
                .WithMessage("The action argument must be an integer.");
        }

        [TestMethod]
        public void TestGetResourceId_ToString()
        {
            var value = 1;
            var argumentName = "id";
            var resourceType = "type";
            var permissionName = "name";

            var actionPermission = new ActionPermission
            {
                ArgumentName = argumentName,
                ResourceType = resourceType,
                PermissionName = permissionName
            };

            var expectedString = String.Format("Permission Name:  [{0}], Resource Type:  [{1}],  Action Argument:  [{2}]", permissionName, resourceType, argumentName);
            Assert.AreEqual(expectedString, actionPermission.ToString());
        }

    }
}
