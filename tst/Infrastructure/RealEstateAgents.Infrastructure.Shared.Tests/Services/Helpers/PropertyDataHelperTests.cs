using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using AutoMapper;

using FakeItEasy;

using FluentAssertions;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using RealEstateAgents.Application.DTOs.Property;
using RealEstateAgents.Application.Interfaces.Clients;
using RealEstateAgents.Application.Interfaces.Services.AgentService.Helpers;
using RealEstateAgents.Application.Mappings;
using RealEstateAgents.Infrastructure.Shared.Services.AgentService.Helpers;

using RestEase;

namespace RealEstateAgents.Infrastructure.Shared.Tests.Services.Helpers
{
    [TestClass]
    public class PropertyDataHelperTests
    {
        private const string PropertiesForSaleTypeOfSearch = "koop";
        private const string PropertiesInAmsterdamSearchQuery = "/amsterdam/";
        private const string PropertiesInAmsterdamWithGardenSearchQuery = "/amsterdam/tuin/";

        private const int PageSize = 25;

        private IPropertiesApi _propertiesApi;
        private ILogger<PropertyDataHelper> _logger;
        private IMapper _mapper;
        private IPropertyDataHelper _propertyDataHelper;

        [TestInitialize]
        public void InitializeTest()
        {
            this._propertiesApi = A.Fake<IPropertiesApi>();
            this._logger = A.Fake<ILogger<PropertyDataHelper>>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GeneralProfile());
            });
            this._mapper = mockMapper.CreateMapper();

            this._propertyDataHelper = new PropertyDataHelper(this._propertiesApi, this._logger, this._mapper);
        }

        [DataTestMethod]
        [DataRow(null, "validSearchQuery", "typeOfSearch")]
        [DataRow("ValidTypeOfSearch", null, "searchQuery")]
        public void FetchAllProperties_ThrowsException_WhenClassPropertiesAreNullOrEmpty(string typeOfSearch, string searchQuery, string expectedExceptionParamName)
        {
            // Arrange
            Func<Task> f = async () => await this._propertyDataHelper.FetchAllProperties(typeOfSearch, searchQuery);

            // Act & Assert
            f.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be(expectedExceptionParamName);
        }

        [TestMethod]
        public async Task FetchAllProperties_ReturnsAllProperties_WhenSearchQueryMatchesAllProperties()
        {
            // Arrange
            const int page = 1;

            var testDataFilePath = Directory.GetCurrentDirectory() + "/TestData/ApiResponse-Properties.json";
            var jsonData = await File.ReadAllTextAsync(testDataFilePath);
            var propertiesApiResponseContent = JsonConvert.DeserializeObject<PropertiesApiResponse>(jsonData);

            var apiResponse = new Response<PropertiesApiResponse>(null, new HttpResponseMessage(HttpStatusCode.OK),
                () => propertiesApiResponseContent);

            A.CallTo(() => this._propertiesApi.GetPropertiesForSaleAsync(PropertiesForSaleTypeOfSearch, PropertiesInAmsterdamSearchQuery, page, PageSize))
                .Returns(apiResponse);

            // Act
            var result = await this._propertyDataHelper.FetchAllProperties(PropertiesForSaleTypeOfSearch, PropertiesInAmsterdamSearchQuery);

            // Assert
            A.CallTo(() => this._propertiesApi.GetPropertiesForSaleAsync(PropertiesForSaleTypeOfSearch, PropertiesInAmsterdamSearchQuery, page, PageSize))
                .WhenArgumentsMatch((string type, string zo, int page, int pageSize) =>
                {
                    type.Should().Be(PropertiesForSaleTypeOfSearch);
                    zo.Should().Be(PropertiesInAmsterdamSearchQuery);
                    page.Should().Be(page);
                    pageSize.Should().Be(PageSize);

                    return true;
                })
                .MustHaveHappened();

            result.Should().NotBeNull();
            result.Count.Should().Be(propertiesApiResponseContent.TotalNumberOfProperties);
        }

        [TestMethod]
        public async Task FetchAllProperties_ReturnsAllPropertiesWithGarden_WhenSearchQueryMatchesAllPropertiesWithGarden()
        {
            // Arrange
            const int page = 1;

            var testDataFilePath = Directory.GetCurrentDirectory() + "/TestData/ApiResponse-PropertiesWithGarden.json";
            var jsonData = await File.ReadAllTextAsync(testDataFilePath);
            var propertiesApiResponseContent = JsonConvert.DeserializeObject<PropertiesApiResponse>(jsonData);

            var apiResponse = new Response<PropertiesApiResponse>(null, new HttpResponseMessage(HttpStatusCode.OK),
                () => propertiesApiResponseContent);

            A.CallTo(() => this._propertiesApi.GetPropertiesForSaleAsync(PropertiesForSaleTypeOfSearch, PropertiesInAmsterdamWithGardenSearchQuery, page, PageSize))
                .Returns(apiResponse);

            // Act
            var result = await this._propertyDataHelper.FetchAllProperties(PropertiesForSaleTypeOfSearch, PropertiesInAmsterdamWithGardenSearchQuery);

            // Assert
            A.CallTo(() => this._propertiesApi.GetPropertiesForSaleAsync(PropertiesForSaleTypeOfSearch, PropertiesInAmsterdamWithGardenSearchQuery, page, PageSize))
                .WhenArgumentsMatch((string type, string zo, int page, int pageSize) =>
                {
                    type.Should().Be(PropertiesForSaleTypeOfSearch);
                    zo.Should().Be(PropertiesInAmsterdamWithGardenSearchQuery);
                    page.Should().Be(page);
                    pageSize.Should().Be(PageSize);

                    return true;
                })
                .MustHaveHappened();

            result.Should().NotBeNull();
            result.Count.Should().Be(propertiesApiResponseContent.TotalNumberOfProperties);
        }
    }
}