using System;
using System.Threading.Tasks;
using PocketMonstersAPI.Services;
using PocketMonstersAPI.Models;
using PocketMonstersAPI.Controllers;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ReturnsExtensions;

namespace PocketMonstersAPI.UnitTests
{
    public class PaginationMessagesControllerTest
    {
        private readonly PaginationMessagesController _sut;

        private readonly IPaginationMessagesService _pageService = Substitute.For<IPaginationMessagesService>();
        private readonly IFixture _fixture = new Fixture();

        public PaginationMessagesControllerTest()
        {
            _sut = new PaginationMessagesController(_pageService);
        }

        [Fact]
        public async Task GetMessageListJustData_ShouldReturnDataIfNotNull()
        {
            // Arrange
            const int pageNum = 1;

            // Act
            var result = await _sut.GetMessageListJustData(pageNum);

            // Assert
            result.Should().Be(result);


        }

        [Fact]
        public async Task GetMessageListJustData_ShouldReturnNotFound_IfDataNull()
        {
            // Arrange
            const int pageNum = 1;
            var expectedResult = new NotFoundResult();
            _pageService.GetAllAsync(Arg.Is(pageNum)).ReturnsNull();

            // Act
            var result = await _sut.GetMessageListJustData(pageNum);

            // Assert
            result.Should().Be(expectedResult);

        }
    }
}

