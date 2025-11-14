using AutoMapper;
using CartonCaps.Application.DTOs;
using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Services;
using CartonCaps.Domain.Entities;
using Moq;

namespace CartonCaps.Tests
{
    public class ReferralServiceTests
    {
        private readonly Mock<IReferralRepository> _repositoryMock;
        private readonly IMapper _mapper;
        private readonly ReferralService _service;


        public ReferralServiceTests()
        {
            //Mock repository
            _repositoryMock = new Mock<IReferralRepository>();

            //Configure AutoMapper(CartonCapsProfile)
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CartonCaps.Application.Mappings.CartonCapsProfile>();
            });
            _mapper = config.CreateMapper();

            //Create the service mock
            _service = new ReferralService(_repositoryMock.Object, _mapper);
        }


        //Verify that the service calls the repository.
        //Verify that the service returns the correct response.
        [Fact]
        public async Task GetReferralByUserAsync_ReturnsMappedReferralList()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var repositoryData = new List<UserReferralEntity>{
            new UserReferralEntity { InviteeName = "Carlos", Status = "Completed" }};

            _repositoryMock
                .Setup(r => r.GetReferralByUserAsync(userId))
                .ReturnsAsync(repositoryData);

            //Act
            var result = await _service.GetReferralByUserAsync(userId);

            //Assert
            Assert.Single(result);
            Assert.Equal("Carlos", result.First().InviteeName);
            Assert.Equal("Completed", result.First().Status);

            _repositoryMock.Verify(r => r.GetReferralByUserAsync(userId), Times.Once);
        }

        //Validate the generation of the referral code
        //Validate the correct return of the response
        //Validate that the link is constructed correctly
        [Fact]
        public async Task CreateReferralAsync_GeneratesCodeAndSavesReferral()
        {
            //Arrange
            var request = new CreateReferralInviteRequest
            {
                ReferrerUserId = Guid.NewGuid()
            };

            string baseLink = "https://cartoncaps.link/referral";
            string chars = "ABCDEFG123456";

            _repositoryMock
                .Setup(r => r.ReferralCodeExistsAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(r => r.CreateReferralAsync(It.IsAny<ReferralEntity>()))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _service.CreateReferralAsync(request, baseLink, chars);

            //Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.ReferralCode));
            Assert.True(result.ReferralCode.Length == 7);
            Assert.StartsWith(baseLink, result.LinkGenerated);

            _repositoryMock.Verify(r => r.CreateReferralAsync(It.IsAny<ReferralEntity>()), Times.Once);
        }

    }
}
