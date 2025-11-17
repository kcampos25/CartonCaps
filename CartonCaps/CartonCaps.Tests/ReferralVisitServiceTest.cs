using AutoMapper;
using CartonCaps.Application.DTOs;
using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Validators;
using CartonCaps.Application.Mappings;
using CartonCaps.Application.Services;
using CartonCaps.Domain.Entities;
using FluentAssertions;
using Moq;

namespace CartonCaps.Tests
{
    public class ReferralVisitServiceTest
    {
        private readonly Mock<IReferralRepository> _referralRepoMock;
        private readonly Mock<IReferralVisitRepository> _visitRepoMock;
        private readonly Mock<IReferralServiceValidator> _referralServiceValidatorMock;
        private readonly IMapper _mapper;
        private readonly ReferralVisitService _service;

        public ReferralVisitServiceTest()
        {
            _referralRepoMock = new Mock<IReferralRepository>();
            _visitRepoMock = new Mock<IReferralVisitRepository>();
            _referralServiceValidatorMock = new Mock<IReferralServiceValidator>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CartonCapsProfile>();
            });

            _mapper = config.CreateMapper();

            _service = new ReferralVisitService(
                 _visitRepoMock.Object,
                  _mapper,
                _referralServiceValidatorMock.Object              
            );
        }

        //Test to verify that a visit is registered when the referral code exists
        [Fact]
        public async Task CreateReferralVisit_ShouldSaveVisit_WhenReferralExists()
        {
            //Arrange
            var referralCode = "XTYY37D";
            var referral = new ReferralEntity { Id = Guid.NewGuid(), ReferralCode = referralCode };

            _referralServiceValidatorMock
                  .Setup(v => v.GetReferralOrThrowAsync(referralCode))
                  .ReturnsAsync(referral);

            _visitRepoMock
                .Setup(r => r.CreateReferralVisit(It.IsAny<ReferralVisitEntity>()))
                .Returns(Task.CompletedTask);

            var request = new ReferralRedirectRequest
            {
                ReferralCode = referralCode,
                IpAddress = "10.0.0.0"
            };

            //Act
            await _service.CreateReferralVisit(request);

            //Assert
            _visitRepoMock.Verify(r => r.CreateReferralVisit(It.IsAny<ReferralVisitEntity>()), Times.Once);
        }

        //Test to verify the behavior when the referral code does not exist
        [Fact]
        public async Task CreateReferralVisit_ShouldThrow_WhenReferralDoesNotExist()
        {
            //Arrange
            _referralServiceValidatorMock
            .Setup(v => v.GetReferralOrThrowAsync(It.IsAny<string>()))
            .ThrowsAsync(new KeyNotFoundException("Referral code not found."));

            var request = new ReferralRedirectRequest
            {
                ReferralCode = "XTYF43D",
                IpAddress = "1.1.1.1"
            };

            //Act
            var act = () => _service.CreateReferralVisit(request);

            //Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                     .WithMessage("Referral code not found.");
        }

        //Test to validate if a user has already made a recent click from the same IP;
        //the system blocks the new click and does NOT register another visit.
        [Fact]
        public async Task CreateReferralVisit_ShouldReturnNotRegistered_WhenClickIsBlocked()
        {
            //Arrange
            var referralCode = "XGT456Y";
            var referral = new ReferralEntity
            {
                Id = Guid.NewGuid(),
                ReferralCode = referralCode,
                Status = "Pending"
            };

            _referralServiceValidatorMock
                .Setup(v => v.GetReferralOrThrowAsync(referralCode))
                .ReturnsAsync(referral);

            _visitRepoMock
                .Setup(r => r.GetLastClickByIpAsync(referral.Id, "10.0.0.1", It.IsAny<TimeSpan>()))
                .ReturnsAsync(new ReferralVisitEntity
                {
                    Id = Guid.NewGuid(),
                    ReferralId = referral.Id,
                    IpAddress = "10.0.0.1",
                    VisitedAt = DateTime.UtcNow
                });

            var request = new ReferralRedirectRequest
            {
                ReferralCode = referralCode,
                IpAddress = "10.0.0.1",
                RedirectUrl = "https://play.google.com/store/apps/details?id=cartoncaps"
            };

            //Act
            var result = await _service.CreateReferralVisit(request);

            //Assert
            result.Registered.Should().BeFalse();
            result.Message.ToLower().Should().Contain("ignored");

            _visitRepoMock.Verify(r => r.CreateReferralVisit(It.IsAny<ReferralVisitEntity>()), Times.Never);
        }

        //Test to validate that the visit history returned by the repository is correctly mapped to DTOs
        [Fact]
        public async Task GetReferralVisitHistoryAsync_ShouldReturnMappedList()
        {
            //Arrange
            var repoData = new List<ReferralVisitHistoryEntity>
            {
                new ReferralVisitHistoryEntity
                {
                    IpAddress = "10.0.0.1",
                    ReferrerUser = "Juan Pérez",
                    VisitedAt = DateTime.UtcNow
                }
            };

            _visitRepoMock.Setup(r => r.GetReferralVisitHistoryAsync())
                          .ReturnsAsync(repoData);

            //Act
            var result = await _service.GetReferralVisitHistoryAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().IpAddress.Should().Be("10.0.0.1");

            _visitRepoMock.Verify(r => r.GetReferralVisitHistoryAsync(), Times.Once);
        }

    }
}
