﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Infrastructure;
using WebMvc.Models;

namespace WebMvc.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly string _baseUrl;
        private readonly IHttpClient _client;

        public CatalogService(IConfiguration config, IHttpClient client)
        {
            _baseUrl = $"{config["CatalogUrl"]}/api/Event/";
            _client = client;
        }

        public async Task<Event> GetCatalogItemsAsync(int page, int size, int? zipcode, int? type, int? category, int? subCategory)
        {
            var catalogItemsUri = ApiPaths.Catalog.GetAllCatalogItems(_baseUrl, page, size, zipcode, type, category, subCategory);
            var dataString = await _client.GetStringAsync(catalogItemsUri);
            return JsonConvert.DeserializeObject<Event>(dataString);
        }

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            var typeUri = ApiPaths.Catalog.GetAllTypes(_baseUrl);
            var dataString = await _client.GetStringAsync(typeUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value=null,
                    Text="All",
                    Selected=true
                }
            };
            var types = JArray.Parse(dataString);
            foreach (var type in types)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = type.Value<string>("id"),
                        Text = type.Value<string>("type"),
                    });
            }
            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetCategoriesAsync()
        {
            var categoryUri = ApiPaths.Catalog.GetAllCategories(_baseUrl);
            var dataString = await _client.GetStringAsync(categoryUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value=null,
                    Text="All",
                    Selected=true
                }
            };
            var categories = JArray.Parse(dataString);
            foreach (var category in categories)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = category.Value<string>("id"),
                        Text = category.Value<string>("category"),
                    });
            }
            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetSubCategoriesAsync()
        {
            var subcategoryUri = ApiPaths.Catalog.GetAllSubCategories(_baseUrl);
            var dataString = await _client.GetStringAsync(subcategoryUri);
            var items = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value=null,
                        Text="All",
                        Selected=true
                    }
                };
            var subcategories = JArray.Parse(dataString);
            foreach (var subcategory in subcategories)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = subcategory.Value<string>("id"),
                        Text = subcategory.Value<string>("subCategory"),
                    });
            }
            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetZipCodesAsync()
        {
            var zipcodeUri = ApiPaths.Catalog.GetAllEventZipCodes(_baseUrl);
            var dataString = await _client.GetStringAsync(zipcodeUri);
            var items = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value=null,
                        Text="All",
                        Selected=true
                    }
                };
            var zipcodes = JArray.Parse(dataString);
         
                foreach (var zipcode in zipcodes)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = zipcode.Value<string>("id"),
                        Text = zipcode.Value<string>("zipcode"),
                    });
            }
            return items;
        }
    }
}
