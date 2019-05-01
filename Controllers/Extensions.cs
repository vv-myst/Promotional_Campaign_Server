#region Auto generated information. Please do not modify

// DunnhumbyHomeWork DunnhumbyHomeWork Extensions.cs
// BILA007 Bilangi, Vivek-Vardhan, IT Collection International
// 2019-04-28 23:41
// 2019-04-28 22:56

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using DunnhumbyHomeWork.DbModel;
using DunnhumbyHomeWork.DTOModel;

[assembly: InternalsVisibleTo("Tests")]
namespace DunnhumbyHomeWork.Controllers
{
    public static class Extensions
    {
        /// <summary>
        ///     Transform a collection of campaign objects from database object to a
        ///     data transfer object
        /// </summary>
        /// <param name="dboCampaignList"></param>
        /// <returns></returns>
        internal static IEnumerable<CampaignDTO> GetCampaignDtoList(
            this IEnumerable<Campaign> dboCampaignList)
        {
            var dtoCampaignList =
                dboCampaignList.Select(dbCampaign => dbCampaign.GetCampaignDto()).ToList();
            return dtoCampaignList;
        }

        /// <summary>
        ///     Transform a campaign from a database object to a data transfer object
        /// </summary>
        /// <param name="dboCampaign"></param>
        /// <returns></returns>
        internal static CampaignDTO GetCampaignDto(this Campaign dboCampaign)
        {
            var dtoCampaign = new CampaignDTO
            {
                Id = dboCampaign.Id,
                Name = dboCampaign.Name,
                Product = dboCampaign.Product.GetProductDto(),
                StartDate = dboCampaign.StartDate.ConvertLocalDateTimeToUtcUnixString(),
                EndDate = dboCampaign.EndDate.ConvertLocalDateTimeToUtcUnixString(),
                IsActive = dboCampaign.IsActive
            };

            return dtoCampaign;
        }

        /// <summary>
        ///     Transform a product from a database object to a data transfer object
        /// </summary>
        /// <param name="dboProduct"></param>
        /// <returns></returns>
        internal static ProductDTO GetProductDto(this Product dboProduct)
        {
            var dtoProduct = new ProductDTO
            {
                Id = dboProduct.Id, Name = dboProduct.Name, Category = dboProduct.Category
            };

            return dtoProduct;
        }

        internal static string ConvertLocalDateTimeToUtcUnixString(this DateTime localDateTime)
        {
            var unixTime = ((DateTimeOffset) localDateTime.ToUniversalTime()).ToUnixTimeSeconds();
            return unixTime.ToString();
        }

        /// <summary>
        ///     Transform a product from a data transfer object to a database object
        /// </summary>
        /// <param name="dtoProduct"></param>
        /// <returns></returns>
        internal static Product GetProductDbo(this ProductDTO dtoProduct)
        {
            var dboProduct = new Product
            {
                Id = dtoProduct.Id,
                Name = dtoProduct.Name,
                Category = dtoProduct.Category
            };

            return dboProduct;
        }

        /// <summary>
        ///     Transform a campaign from a data transfer object to a database object 
        /// </summary>
        /// <param name="dtoCampaign"></param>
        /// <returns></returns>
        internal static Campaign GetCampaignDbo(this CampaignDTO dtoCampaign)
        {
            var startDate = dtoCampaign.StartDate.ConvertUtcUnixStringToLocalDateTime();
            var endDate = dtoCampaign.EndDate.ConvertUtcUnixStringToLocalDateTime();

            if(startDate > endDate)
                throw new Exception($"StartDate: {startDate} | {dtoCampaign.StartDate} "
                                    + $"cannot be greater than the EndDate: {endDate} | {dtoCampaign.EndDate}");
            
            var dboCampaign = new Campaign
            {
                Id = dtoCampaign.Id,
                Name = dtoCampaign.Name,
                ProductId = dtoCampaign.Product.Id,
                StartDate = startDate,
                EndDate = endDate,
                IsActive = dtoCampaign.IsActive
            };

            return dboCampaign;
        }

        internal static DateTime ConvertUtcUnixStringToLocalDateTime(this string utcUnixDateString)
        {
            long seconds;
            if(!long.TryParse(utcUnixDateString, NumberStyles.Integer | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out seconds))
                throw new InvalidCastException(
                    $"Cannot cast string into a valid date: {utcUnixDateString}");

            var dateTime = DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime.ToLocalTime();
            return dateTime;
        }
    }
}
