using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ECA.WebApi.Test.Security
{
    public class ABindingModel
    {
        public int Id { get; set; }

        public string S { get; set; }

        public BBindingModel B { get; set; }
    }

    public class BBindingModel
    {
        public int OtherId { get; set; }
    }

    [TestClass]
    public class ModelPermissionTest
    {

        [TestMethod]
        public void TestGetResourceId_PropertyIsNotNumeric()
        {
            var resourceType = "project";
            var permissionName = "edit";
            var instance = new ABindingModel();
            var permission = new ModelPermission("model.S", typeof(ABindingModel), permissionName, resourceType);
            var dictionary = GetActionArgumentsDictionary(instance, "model");
            permission.Invoking(y => y.GetResourceId(dictionary)).ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The nested property [{0}] in the model's property hierarchy [{1}] must be an integer.", "S", "model.S"));
        }

        [TestMethod]
        public void TestGetResourceId_PropertyDoesNotExist()
        {
            var resourceType = "project";
            var permissionName = "edit";
            var instance = new ABindingModel();
            var permission = new ModelPermission("model.X", typeof(ABindingModel), permissionName, resourceType);
            var dictionary = GetActionArgumentsDictionary(instance, "model");
            permission.Invoking(y => y.GetResourceId(dictionary)).ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The property [{0}] does not exist in the model of type [{1}].", "X", "ABindingModel"));
        }

        [TestMethod]
        public void TestGetResourceId_ArgumentIsFirstLevelNode()
        {
            var resourceType = "project";
            var permissionName = "edit";
            var value = 1;
            var instance = new ABindingModel();
            instance.Id = value;
            var permission = new ModelPermission("model.Id", typeof(ABindingModel), permissionName, resourceType);
            var dictionary = GetActionArgumentsDictionary(instance, "model");

            Assert.AreEqual(value, permission.GetResourceId(dictionary));
        }

        [TestMethod]
        public void TestGetResourceId_ArgumentIsFirstLevelNode_MultipleDictionaryKeys()
        {
            var resourceType = "project";
            var permissionName = "edit";
            var value = 1;
            var instance = new ABindingModel();
            instance.Id = value;
            var permission = new ModelPermission("model.Id", typeof(ABindingModel), permissionName, resourceType);
            var dictionary = GetActionArgumentsDictionary(instance, "model");
            dictionary.Add("key", "value");

            Assert.AreEqual(value, permission.GetResourceId(dictionary));
        }

        [TestMethod]
        public void TestGetResourceId_ArgumentIsSecondLevelNode()
        {
            var resourceType = "project";
            var permissionName = "edit";
            var value = 1;
            var instance = new ABindingModel();
            instance.B = new BBindingModel
            {
                OtherId = value
            };
            var permission = new ModelPermission("model.B.OtherId", typeof(ABindingModel), permissionName, resourceType);
            var dictionary = GetActionArgumentsDictionary(instance, "model");

            Assert.AreEqual(value, permission.GetResourceId(dictionary));
        }

        [TestMethod]
        public void TestGetResourceId_DictionaryKeyNotPresnt_OneKeyInDictionary()
        {
            var resourceType = "project";
            var permissionName = "edit";
            var value = 1;
            var instance = new ABindingModel();
            instance.B = new BBindingModel
            {
                OtherId = value
            };

            var permission = new ModelPermission("B.OtherId", typeof(ABindingModel), permissionName, resourceType);
            var dictionary = GetActionArgumentsDictionary(instance, "model");

            Assert.AreEqual(value, permission.GetResourceId(dictionary));
        }

        [TestMethod]
        public void TestGetResourceId_DictionaryKeyNotPresnt_TwoKeysInDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("key1", 1);
            dictionary.Add("key2", 2);
            var resourceType = "project";
            var permissionName = "edit";
            var permission = new ModelPermission("B.OtherId", typeof(ABindingModel), permissionName, resourceType);
            permission.Invoking(x => x.GetResourceId(dictionary)).ShouldThrow<NotSupportedException>()
                .WithMessage("There more than one action arguments for the web api.  You must specify the name of the variable in the property path.");
        }

        [TestMethod]
        public void TestGetResourceId_DictionaryKeyNotPresnt_NoKeysInDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            var resourceType = "project";
            var permissionName = "edit";
            var permission = new ModelPermission("B.OtherId", typeof(ABindingModel), permissionName, resourceType);
            permission.Invoking(x => x.GetResourceId(dictionary)).ShouldThrow<NotSupportedException>()
                .WithMessage("There are no action arguments to check permissions with.  There must be at least one action argument.");
        }


        [TestMethod]
        public void TestToString()
        {
            var resourceType = "project";
            var permissionName = "edit";
            var instance = new ABindingModel();
            var permission = new ModelPermission("model.S", typeof(ABindingModel), permissionName, resourceType);
            Assert.AreEqual(
                String.Format("Permission Name:  [{0}], Resource Type:  [{1}], Model Type:  [{2}], Property:  [{3}]", 
                    permission.PermissionName, 
                    permission.ResourceType, 
                    typeof(ABindingModel).Name, 
                    "model.S"), 
                permission.ToString());

        }

        private IDictionary<string, object> GetActionArgumentsDictionary(object instance, string modelKey)
        {
            return new Dictionary<string, object> { { modelKey, instance } };
        }
    }
}
