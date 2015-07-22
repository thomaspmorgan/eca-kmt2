using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Core.Exceptions;
using ECA.Data;
using FluentAssertions;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class SocialMediaServiceTest
    {
        private TestEcaContext context;
        private SocialMediaService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new SocialMediaService(context);
        }
        #region Create
        [TestMethod]
        public async Task TestCreate_CheckProperties()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });

                var organization = new Organization
                {
                    OrganizationId = 1
                };

                var socialMediaType = new SocialMediaType
                {
                    SocialMediaTypeId = SocialMediaType.Facebook.Id,
                    SocialMediaTypeName = SocialMediaType.Facebook.Value
                };
                var userId = 1;
                var user = new User(userId);
                var value = "value";
                var socialMediaTypeId = socialMediaType.SocialMediaTypeId;
                var socialMediaPresense = new OrganizationSocialMediaPresence(user, socialMediaTypeId, value, organization.GetId());

                context.SetupActions.Add(() =>
                {
                    organization.SocialMedias.Clear();
                    context.SocialMediaTypes.Add(socialMediaType);
                    context.Organizations.Add(organization);
                });

                Action beforeServiceTester = () =>
                {
                    Assert.AreEqual(0, context.SocialMedias.Count());
                    Assert.AreEqual(0, context.Organizations.First().SocialMedias.Count);
                };

                Action<SocialMedia> tester = (serviceResult) =>
                {
                    Assert.IsNotNull(serviceResult);
                    Assert.AreEqual(1, organization.SocialMedias.Count);
                    var firstSocialMedia = context.Organizations.First().SocialMedias.First();
                    Assert.IsTrue(Object.ReferenceEquals(serviceResult, firstSocialMedia));

                    Assert.AreEqual(socialMediaType.SocialMediaTypeId, serviceResult.SocialMediaTypeId);
                    Assert.AreEqual(value, serviceResult.SocialMediaValue);
                    Assert.AreEqual(userId, serviceResult.History.CreatedBy);
                    Assert.AreEqual(userId, serviceResult.History.RevisedBy);
                    DateTimeOffset.Now.Should().BeCloseTo(serviceResult.History.CreatedOn, 2000);
                    DateTimeOffset.Now.Should().BeCloseTo(serviceResult.History.RevisedOn, 2000);
                };

                context.Revert();
                beforeServiceTester();
                var socialMedia = service.Create<Organization>(socialMediaPresense);
                tester(socialMedia);

                context.Revert();
                beforeServiceTester();
                socialMedia = await service.CreateAsync<Organization>(socialMediaPresense);
                tester(socialMedia);
            }
        }

        [TestMethod]
        public async Task TestCreate_SocialableEntityDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });
                var socialMediaType = new SocialMediaType
                {
                    SocialMediaTypeId = SocialMediaType.Facebook.Id,
                    SocialMediaTypeName = SocialMediaType.Facebook.Value
                };
                var userId = 1;
                var id = 10;
                var user = new User(userId);
                var value = "value";
                var socialMediaTypeId = socialMediaType.SocialMediaTypeId;
                var socialMediaPresense = new OrganizationSocialMediaPresence(user, socialMediaTypeId, value, id);

                context.SocialMediaTypes.Add(socialMediaType);

                Func<Task> f = () =>
                {
                    return service.CreateAsync<Organization>(socialMediaPresense);
                };
                service.Invoking(x => x.Create<Organization>(socialMediaPresense)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The sociable entity with id [{0}] was not found.", id));
                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The sociable entity with id [{0}] was not found.", id));
            }
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_CheckProperties()
        {
            var creator = 1;
            var facebookType = new SocialMediaType
            {
                SocialMediaTypeId = SocialMediaType.Facebook.Id,
                SocialMediaTypeName = SocialMediaType.Facebook.Value
            };
            var twitter = new SocialMediaType
            {
                SocialMediaTypeId = SocialMediaType.Twitter.Id,
                SocialMediaTypeName = SocialMediaType.Twitter.Value
            };
            
            var oldSocialMediaTypeId = facebookType.SocialMediaTypeId;
            var oldValue = "oldValue";
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var socialMedia = new SocialMedia
            {
                SocialMediaId = 1
            };
            context.SetupActions.Add(() =>
            {
                context.SocialMedias.Add(socialMedia);
                context.SocialMediaTypes.Add(facebookType);
                context.SocialMediaTypes.Add(twitter);
                socialMedia.History.CreatedBy = creator;
                socialMedia.History.CreatedOn = yesterday;
                socialMedia.History.RevisedBy = creator;
                socialMedia.History.RevisedOn = yesterday;
                socialMedia.SocialMediaTypeId = facebookType.SocialMediaTypeId;
                socialMedia.SocialMediaType = facebookType;
                socialMedia.SocialMediaValue = oldValue;
            });
            var updatorId = 2;
            var updator = new User(updatorId);
            var updatedInstance = new UpdatedSocialMediaPresence(updator, socialMedia.SocialMediaId, "newValue", twitter.SocialMediaTypeId);
            Action tester = () =>
            {
                Assert.AreEqual(1, context.SocialMedias.Count());
                Assert.AreEqual(2, context.SocialMediaTypes.Count());
                Assert.AreEqual(updatedInstance.Value, socialMedia.SocialMediaValue);
                Assert.AreEqual(updatedInstance.SocialMediaTypeId, socialMedia.SocialMediaTypeId);
                Assert.AreEqual(creator, socialMedia.History.CreatedBy);
                Assert.AreEqual(yesterday, socialMedia.History.CreatedOn);
                DateTimeOffset.Now.Should().BeCloseTo(socialMedia.History.RevisedOn, 2000);
                Assert.AreEqual(updatorId, socialMedia.History.RevisedBy);
            };
            context.Revert();
            service.Update(updatedInstance);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedInstance);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_SocialMediaDoesNotExist()
        {
            var updatorId = 2;
            var updator = new User(updatorId);
            var updatedInstance = new UpdatedSocialMediaPresence(updator, 1, "value", SocialMediaType.Twitter.Id);

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedInstance);
            };

            service.Invoking(x => x.Update(updatedInstance)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The social media with id [{0}] was not found.", updatedInstance.Id));
            f.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The social media with id [{0}] was not found.", updatedInstance.Id));

        }
        #endregion

        #region Get
        [TestMethod]
        public async Task TestGetById_CheckProperties()
        {
            var facebook = new SocialMediaType
            {
                SocialMediaTypeId = SocialMediaType.Facebook.Id,
                SocialMediaTypeName = SocialMediaType.Facebook.Value
            };
            var socialMedia = new SocialMedia
            {
                SocialMediaId = 1,
                SocialMediaType = facebook,
                SocialMediaTypeId = facebook.SocialMediaTypeId,
                SocialMediaValue = "value"
            };
            context.SocialMediaTypes.Add(facebook);
            context.SocialMedias.Add(socialMedia);
            Action<SocialMediaDTO> tester = (dto) =>
            {
                Assert.AreEqual(facebook.SocialMediaTypeId, dto.SocialMediaTypeId);
                Assert.AreEqual(facebook.SocialMediaTypeName, dto.SocialMediaType);
                Assert.AreEqual(socialMedia.SocialMediaId, dto.Id);
                Assert.AreEqual(socialMedia.SocialMediaValue, dto.Value);
            };
            var result = service.GetById(socialMedia.SocialMediaId);
            var resultAsync = await service.GetByIdAsync(socialMedia.SocialMediaId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetById_SocialMediaDoesNotExist()
        {
            Action<SocialMediaDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };
            var result = service.GetById(1);
            var resultAsync = await service.GetByIdAsync(1);
            tester(result);
            tester(resultAsync);
        }
        #endregion
    }
}
