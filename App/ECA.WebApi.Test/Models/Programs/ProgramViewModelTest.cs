using AutoMapper;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Lookup;
using ECA.WebApi.Models.Programs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.WebApi.Test.Models.Programs
{
    [TestClass]
    public class ProgramViewModelTest
    {
        [TestMethod]
        public void TestConstructor_ZeroArgsConstructor()
        {
            var viewModel = new ProgramViewModel();
            Assert.IsNotNull(viewModel.Contacts);
            Assert.IsNotNull(viewModel.CountryIsos);
            Assert.IsNotNull(viewModel.RegionIsos);
            Assert.IsNotNull(viewModel.Themes);
            Assert.IsNotNull(viewModel.Goals);
        }

        [TestMethod]
        public void TestConstructor_ProgramDtoArg()
        {
            var dto = new ProgramDTO();
            dto.Contacts = ToListOfSimpleLookups(1, "contact");
            dto.CountryIsos = ToListOfSimpleLookups(2, "iso");
            dto.Description = "desc";
            dto.Focus = ToListOfSimpleLookups(3, "focus").First();
            dto.Goals = ToListOfSimpleLookups(4, "goal");
            dto.Id = 1;
            dto.Name = "name";
            dto.OwnerDescription = "owner desc";
            dto.OwnerName = "owner name";
            dto.OwnerOrganizationId = 2;
            dto.OwnerOfficeSymbol = "symbol";
            dto.ParentProgramId = 3;
            dto.RegionIsos = ToListOfSimpleLookups(5, "region iso");
            dto.RevisedOn = DateTimeOffset.UtcNow;
            dto.RowVersion = new byte[1] { (byte)1 };
            dto.StartDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            dto.Themes = ToListOfSimpleLookups(6, "theme");

            var viewModel = new ProgramViewModel(dto);
            Assert.AreEqual(dto.Description, viewModel.Description);
            Assert.AreEqual(dto.Id, viewModel.Id);
            Assert.AreEqual(dto.Name, viewModel.Name);
            Assert.AreEqual(dto.OwnerDescription, viewModel.OwnerDescription);
            Assert.AreEqual(dto.OwnerOrganizationId, viewModel.OwnerOrganizationId);
            Assert.AreEqual(dto.OwnerOfficeSymbol, viewModel.OwnerOfficeSymbol);
            Assert.AreEqual(dto.ParentProgramId, viewModel.ParentProgramId);
            Assert.AreEqual(dto.RevisedOn, viewModel.RevisedOn);
            Assert.AreEqual(Convert.ToBase64String(dto.RowVersion), viewModel.RowVersion);
            Assert.AreEqual(dto.StartDate, viewModel.StartDate);

            CollectionAssert.AreEqual(dto.Contacts.ToList(), viewModel.Contacts.ToList());
            CollectionAssert.AreEqual(dto.CountryIsos.ToList(), viewModel.CountryIsos.ToList());
            CollectionAssert.AreEqual(dto.Goals.ToList(), viewModel.Goals.ToList());
            CollectionAssert.AreEqual(dto.RegionIsos.ToList(), viewModel.RegionIsos.ToList());
            CollectionAssert.AreEqual(dto.Themes.ToList(), viewModel.Themes.ToList());
        }

        private List<SimpleLookupDTO> ToListOfSimpleLookups(int id, string value)
        {
            return new List<SimpleLookupDTO> { new SimpleLookupDTO { Id = id, Value = value } };
        }
    }
}
