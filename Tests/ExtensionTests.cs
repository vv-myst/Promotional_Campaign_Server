#region Auto generated information. Please do not modify

// DunnhumbyHomeWork Tests ExtensionTests.cs
// bila007 Bilangi, Vivek-Vardhan, IT Collection International
// 2019-05-01 16:11
// 2019-05-01 13:08

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using DunnhumbyHomeWork.Controllers;
using DunnhumbyHomeWork.DbModel;
using DunnhumbyHomeWork.DTOModel;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ExtensionTests
    {

        [SetUp]
        public void Init()
        {
            var fixture = new Fixture();

            var startDate = DateTime.Parse(fixture.Create<DateTime>().ToString("u"));
            var endDate = startDate.AddDays(fixture.Create<int>());

            mockProductDbo = new Product
            {
                Id = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                Category = fixture.Create<string>(),
                Creator = fixture.Create<string>(),
                CreationTime = fixture.Create<DateTime>(),
                LastUser = fixture.Create<string>(),
                LastUpdate = fixture.Create<DateTime>()
            };

            mockCampaignDbo = new Campaign
            {
                Id = fixture.Create<string>(),
                Name = fixture.Create<string>(),
                Product = mockProductDbo,
                StartDate = startDate,
                EndDate = endDate,
                IsActive = fixture.Create<bool>(),
                ProductId = mockProductDbo.Id,
                Creator = fixture.Create<string>(),
                CreationTime = fixture.Create<DateTime>(),
                LastUser = fixture.Create<string>(),
                LastUpdate = fixture.Create<DateTime>()
            };

            mockProductDto = new ProductDTO
            {
                Id = mockProductDbo.Id,
                Name = mockProductDbo.Name,
                Category = mockProductDbo.Category
            };

            mockCampaignDto = new CampaignDTO
            {
                Id = mockCampaignDbo.Id,
                Name = mockCampaignDbo.Name,
                Product = mockProductDto,
                StartDate = startDate.ConvertLocalDateTimeToUtcUnixString(),
                EndDate = endDate.ConvertLocalDateTimeToUtcUnixString(),
                IsActive = mockCampaignDbo.IsActive
            };
        }

        private Product mockProductDbo;
        private Campaign mockCampaignDbo;
        private ProductDTO mockProductDto;
        private CampaignDTO mockCampaignDto;

        [TestCase(2019, 5, 1, 11, 51, 30, "1556711490")]
        public void ConvertLocalDateTimeToUtcUnixString_ReturnsDateAsAUnixString(
            int year, int month, int day, int hr, int min, int sec, string expectedResult)
        {
            var dateTime = new DateTime(year, month, day, hr, min, sec, DateTimeKind.Utc);
            var sut = dateTime.ConvertLocalDateTimeToUtcUnixString();
            Assert.That(sut, Is.EqualTo(expectedResult));
        }

        [TestCase("-1514798120", 1921, 12, 31, 15, 44, 40)]
        [TestCase("1514801720", 2018, 1, 1, 11, 15, 20)]
        [TestCase("1569924920", 2019, 10, 1, 12, 15, 20)]
        public void ConvertUtcUnixStringToLocalDateTime_ReturnsDateTimeInUtc(
            string inputUnixTime, int expYr, int expMon, int expDay, int expHr, int expMin,
            int expSec)
        {
            var sut = inputUnixTime.ConvertUtcUnixStringToLocalDateTime();

            Assert.That(sut.Year, Is.EqualTo(expYr));
            Assert.That(sut.Month, Is.EqualTo(expMon));
            Assert.That(sut.Day, Is.EqualTo(expDay));
            Assert.That(sut.Hour, Is.EqualTo(expHr));
            Assert.That(sut.Minute, Is.EqualTo(expMin));
            Assert.That(sut.Second, Is.EqualTo(expSec));
        }

        [TestCase("test")]
        public void ConvertUtcUnixStringToLocalDateTime_InvalidUtcDate_ThrowsInvalidCastException(
            string inputString)
        {
            Assert.Throws<InvalidCastException>(() =>
                inputString.ConvertUtcUnixStringToLocalDateTime());
        }

        [Test]
        public void GetCampaignDbo_ConvertsCampaignDtoToDbo()
        {
            var expectedResult = mockCampaignDbo;

            var sut = mockCampaignDto.GetCampaignDbo();
            Assert.That(sut.Id, Is.EqualTo(expectedResult.Id));
            Assert.That(sut.Name, Is.EqualTo(expectedResult.Name));
            Assert.That(sut.StartDate, Is.EqualTo(expectedResult.StartDate));
            Assert.That(sut.EndDate, Is.EqualTo(expectedResult.EndDate));
            Assert.That(sut.IsActive, Is.EqualTo(expectedResult.IsActive));
            Assert.That(sut.ProductId, Is.EqualTo(expectedResult.ProductId));
            Assert.That(sut.Creator, Is.Not.Empty);
            Assert.That(sut.Creator, Is.EqualTo(It.IsAny<string>()));
            Assert.That(sut.CreationTime, Is.Not.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(It.IsAny<DateTime>()));
            Assert.That(sut.LastUser, Is.Not.Empty);
            Assert.That(sut.LastUser, Is.EqualTo(It.IsAny<string>()));
            Assert.That(sut.LastUpdate, Is.Not.Null);
            Assert.That(sut.LastUpdate, Is.EqualTo(It.IsAny<DateTime>()));
        }

        [Test]
        public void GetCampaignDbo_StartGreatAfterEndDate_ThrowsException()
        {
            var startDate = DateTime.Now.ConvertLocalDateTimeToUtcUnixString();
            var endDate = DateTime.Now.AddDays(-10).ConvertLocalDateTimeToUtcUnixString();
            mockCampaignDto.StartDate = startDate;
            mockCampaignDto.EndDate = endDate;

            Assert.Throws<Exception>(() => mockCampaignDto.GetCampaignDbo());
        }

        [Test]
        public void GetCampaignDto_ReturnsACampaignDto()
        {
            var expectedResult = mockCampaignDto;

            var sut = mockCampaignDbo.GetCampaignDto();

            Assert.That(sut.Id, Is.EqualTo(expectedResult.Id));
            Assert.That(sut.Name, Is.EqualTo(expectedResult.Name));
            Assert.That(sut.Product.Id, Is.EqualTo(expectedResult.Product.Id));
            Assert.That(sut.Product.Name, Is.EqualTo(expectedResult.Product.Name));
            Assert.That(sut.Product.Category, Is.EqualTo(expectedResult.Product.Category));
            Assert.That(sut.StartDate, Is.EqualTo(expectedResult.StartDate));
            Assert.That(sut.EndDate, Is.EqualTo(expectedResult.EndDate));
            Assert.That(sut.IsActive, Is.EqualTo(expectedResult.IsActive));
        }

        [Test]
        public void GetDtoCampaignList_ConvertsAllCampaignDboToDto()
        {
            var dboCampaignList = new List<Campaign> {mockCampaignDbo};

            var sut = dboCampaignList.GetCampaignDtoList().ToList();
            var expectedResult = mockCampaignDto;

            Assert.That(sut.Any(), Is.True);
            Assert.That(sut.Count, Is.EqualTo(1));
            Assert.That(sut[0].Id, Is.EqualTo(expectedResult.Id));
            Assert.That(sut[0].Name, Is.EqualTo(expectedResult.Name));
            Assert.That(sut[0].Product.Id, Is.EqualTo(expectedResult.Product.Id));
            Assert.That(sut[0].Product.Name, Is.EqualTo(expectedResult.Product.Name));
            Assert.That(sut[0].Product.Category, Is.EqualTo(expectedResult.Product.Category));
            Assert.That(sut[0].StartDate, Is.EqualTo(expectedResult.StartDate));
            Assert.That(sut[0].EndDate, Is.EqualTo(expectedResult.EndDate));
            Assert.That(sut[0].IsActive, Is.EqualTo(expectedResult.IsActive));
        }

        [Test]
        public void GetProductDbo_ConvertsProductDtoToDbo()
        {
            var expectedResult = mockProductDbo;

            var sut = mockProductDto.GetProductDbo();
            Assert.That(sut.Id, Is.EqualTo(expectedResult.Id));
            Assert.That(sut.Name, Is.EqualTo(expectedResult.Name));
            Assert.That(sut.Category, Is.EqualTo(expectedResult.Category));
            Assert.That(sut.Creator, Is.Not.Empty);
            Assert.That(sut.Creator, Is.EqualTo(It.IsAny<string>()));
            Assert.That(sut.CreationTime, Is.Not.Null);
            Assert.That(sut.CreationTime, Is.EqualTo(It.IsAny<DateTime>()));
            Assert.That(sut.LastUser, Is.Not.Empty);
            Assert.That(sut.LastUser, Is.EqualTo(It.IsAny<string>()));
            Assert.That(sut.LastUpdate, Is.Not.Null);
            Assert.That(sut.LastUpdate, Is.EqualTo(It.IsAny<DateTime>()));
        }

        [Test]
        public void GetProductDto_ReturnsAProductDto()
        {
            var expectedResult = new ProductDTO
            {
                Id = mockProductDbo.Id,
                Name = mockProductDbo.Name,
                Category = mockProductDbo.Category
            };

            var sut = mockProductDbo.GetProductDto();
            Assert.That(sut.Id, Is.EqualTo(expectedResult.Id));
            Assert.That(sut.Name, Is.EqualTo(expectedResult.Name));
            Assert.That(sut.Category, Is.EqualTo(expectedResult.Category));
        }
    }
}
