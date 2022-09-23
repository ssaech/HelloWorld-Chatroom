using System;
using AutoFixture;
using FluentAssertions;
using PocketMonstersAPI.Models;
using NSubstitute;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace PocketMonstersAPI.UnitTests
{
    public class PagerTests
    {
        private readonly Pager _sut;
        private readonly int _totalitems = 9;
        private readonly int _page = 1;
        private readonly int _pagesize;

        public PagerTests()
        {
            _sut = new Pager(_totalitems, _page);
        }

        [Fact]
        public void PageSize_ShouldBe5WhenNothing()
        {
            // Arrange
            //Act
            var result = _sut;

            //Assert
            result.PageSize.Should().Be(5);
        }
    }
}

