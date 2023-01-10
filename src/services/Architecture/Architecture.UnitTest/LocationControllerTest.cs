using Architecture.API.Application.Models;
using Architecture.API.Application.Queries;
using Architecture.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Architecture.UnitTest
{
    public class LocationControllerTest
    {
        #region Property  
        private Mock<IMediator> _mediator;
        private Mock<ILocationQueries> _locationQueries;
        private Mock<ILogger<LocationController>> _logger;
        private LocationController controller;
        #endregion
        public LocationControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _locationQueries = new Mock<ILocationQueries>();
            _logger = new Mock<ILogger<LocationController>>();
            controller = new LocationController(_mediator.Object, _locationQueries.Object, _logger.Object);
        }

        [Fact]
        public async Task GetDistance_ForInvalidLocationParam()
        {
            //Arrange
            var location_one = "123";
            var location_two = "ANS";
            var mockResponse = new Response
            {
                errors = new System.Collections.Generic.List<Error>() {
                new Error {location="params",param="code",value="123",msg="Invalid code"} },
                distance = 0
            };

            _locationQueries.Setup(x => x.GetDistanceAsync(location_one, location_two)).Returns(Task.FromResult(mockResponse));

            //Act
            var result = await controller.getDistanceAsync(location_one, location_two);

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockResponse, viewResult.Value);
        }

        [Fact]
        public async Task GetDistance_ForValidLocationParam()
        {
            //Arrange
            var location_one = "AMN";
            var location_two = "ANS";
            var mockResponse = new Response { errors = null, distance = 6458013.53395002 };

            _locationQueries.Setup(x => x.GetDistanceAsync(location_one, location_two)).Returns(Task.FromResult(mockResponse));

            //Act
            var result = await controller.getDistanceAsync(location_one, location_two);

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(mockResponse, viewResult.Value);
        }
    }
}
