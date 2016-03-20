using BusinessServices;
using BusinessServices.Interfaces;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Packages;
using Model.ReferenceData;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class PackageTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private IAuditLogger _auditLogger;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _auditLogger = new AuditLogger(_unitOfWork.Object);

            CompetitionType pointsLeague = new CompetitionType() { Name = "PointsLeague" };

            List<Package> packages = new List<Package>() {
                new Package() 
                { 
                    Name = "Free",
                    NumberOfSidesPerCompetition = 10,
                    NumberOfCompetitions = 1,
                    NumberOfSportTypes = 1,
                    CompetitionTypes = new List<CompetitionType>() { pointsLeague },
                    HasLeagueDivisions = false,
                    CanGenerateFixtures = true,
                    CanViewStats = false,
                    CanAccessApp = true,
                    AdvertisedCostPerMonth = 0.00M,
                    AdvertisedCostPerYear = 0.00M,
                    IsDiscountActive = false,
                    ActiveFrom = DateTime.Now.AddDays(-5),
                    ActiveTo = DateTime.Now.AddDays(5),
                    IsActive = true
                },
                new Package()
                {
                    Name = "Standard",
                    NumberOfSidesPerCompetition = 10,
                    NumberOfCompetitions = 5,
                    NumberOfSportTypes = 1,
                    CompetitionTypes = new List<CompetitionType>() { pointsLeague },
                    HasLeagueDivisions = false,
                    CanGenerateFixtures = true,
                    CanViewStats = false,
                    CanAccessApp = true,
                    AdvertisedCostPerMonth = 5.00M,
                    AdvertisedCostPerYear = 60.00M,
                    IsDiscountActive = false,
                    ActiveFrom = DateTime.Now.AddDays(-5),
                    ActiveTo = DateTime.Now.AddDays(5),
                    IsActive = true
                },
                new Package()
                {
                    Name = "Pro",
                    NumberOfSidesPerCompetition = 10,
                    NumberOfCompetitions = 999,
                    NumberOfSportTypes = 1,
                    CompetitionTypes = new List<CompetitionType>() { pointsLeague },
                    HasLeagueDivisions = true,
                    CanGenerateFixtures = true,
                    CanViewStats = false,
                    CanAccessApp = true,
                    AdvertisedCostPerMonth = 10.00M,
                    AdvertisedCostPerYear = 120.00M,
                    IsDiscountActive = false,
                    ActiveFrom = DateTime.Now.AddDays(-5),
                    ActiveTo = DateTime.Now.AddDays(5),
                    IsActive = true
                }
            };

            _unitOfWork.Setup(x => x.GetRepository<Package>().All()).Returns(packages);
        }

        [TestMethod]
        public void Get_Active_Packages()
        {
            IPackageService packageService = new PackageService(_unitOfWork.Object);

            List<Package> packages = packageService.GetPackages().ToList();

            Assert.AreEqual(packages.Count, 3);
        }
    }
}
