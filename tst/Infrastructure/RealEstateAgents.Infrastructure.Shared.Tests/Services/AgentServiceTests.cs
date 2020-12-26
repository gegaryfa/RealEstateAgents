using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FakeItEasy;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RealEstateAgents.Application.DTOs.Agent;
using RealEstateAgents.Application.Features.Agents.Queries.GetTopAgents;
using RealEstateAgents.Application.Interfaces.Services.AgentService.Helpers;
using RealEstateAgents.Domain.Entities;
using RealEstateAgents.Infrastructure.Shared.Services.AgentService;

namespace RealEstateAgents.Infrastructure.Shared.Tests.Services
{
    [TestClass]
    public class AgentServiceTests
    {
        private IPropertyDataHelper _propertyDataHelper;
        private AgentService _agentService;

        [TestInitialize]
        public void InitializeTest()
        {
            this._propertyDataHelper = A.Fake<IPropertyDataHelper>();
            this._agentService = new AgentService(this._propertyDataHelper);
        }

        [TestMethod]
        public void GetTopAgents_WhenInputIsNull_ThrowsException()
        {
            Func<Task> action = async () => await this._agentService.GetTopAgents(null);

            action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("request");
        }

        [TestMethod]
        public async Task GetTopAgent_WithValidInput_ShouldReturnCorrectAmountOfTopAgents()
        {
            // Arrange
            var getTopAgentsRequest = new GetTopAgentsRequest
            {
                Garden = true,
                NumberOfAgents = 2,
                Region = "Amsterdam",
                TypeOfSearch = TypeOfSearch.Buy
            };

            var propertiesList = new List<Property>
            {
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 1,
                        Name = "first"
                    }
                },
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 2,
                        Name = "second"
                    }                },
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 3,
                        Name = "third"
                    }                }
            };
            A.CallTo(() => this._propertyDataHelper.FetchAllProperties("koop", A<string>._)).Returns(propertiesList);

            // Act
            var topAgents = await _agentService.GetTopAgents(getTopAgentsRequest);

            // Assert
            topAgents.Agents.Count().Should().Be(2);
        }

        [TestMethod]
        public async Task GetTopAgent_WithValidInput_ShouldReturnTopAgentsInTheCorrectOrder()
        {
            // Arrange
            var getTopAgentsRequest = new GetTopAgentsRequest
            {
                Garden = true,
                NumberOfAgents = 2,
                Region = "Amsterdam",
                TypeOfSearch = TypeOfSearch.Buy
            };

            var propertiesList = new List<Property>
            {
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 1,
                        Name = "first"
                    }                },
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 2,
                        Name = "second"
                    }                },
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 2,
                        Name = "second"
                    }                },
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 3,
                        Name = "third"
                    }                },
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 3,
                        Name = "third"
                    }
                },
                new Property
                {
                    Agent = new Agent
                    {
                        Id = 3,
                        Name = "third"
                    }
                }
            };

            A.CallTo(() => this._propertyDataHelper.FetchAllProperties("koop", A<string>._)).Returns(propertiesList);

            // Act
            var topAgents = await _agentService.GetTopAgents(getTopAgentsRequest);

            // Assert
            topAgents.Agents.First().Id.Should().Be(3);
            topAgents.Agents.Last().Id.Should().Be(2);
        }
    }
}