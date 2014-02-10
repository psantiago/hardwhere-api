using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using AutoMapper;
using HardwhereApi.Core.Dto;
using HardwhereApi.Core.Models;
using HardwhereApi.Core.Utilities;
using HardwhereApi.Infrastructure;
using WebGrease.Css.Extensions;

namespace HardwhereApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AssetController : ApiController
    {
        private HardwhereApiContext db = new HardwhereApiContext();

        // GET api/Asset
        public IEnumerable<DynamicAssetDto> GetAssets()
        {
            //get assets with their types and properties, and map to the asset dto
            var result = db.Assets.Include(i => i.AssetType).Include(f => f.AssetProperties).ToList();

            //get the type properties and map to their dto
            var types = db.TypeProperties.ToList();

            var sections = db.Sections.ToList();

            var moreAwesomeness = new List<DynamicAssetDto>();
            result.ForEach(i => moreAwesomeness.Add(CreateDynamicAssetDto(i, true, types, sections)));

            return moreAwesomeness;
        }

        /// <summary>
        /// Give an assetdto which includes it's assettype and asset properties,
        /// generate the DynamicAssetDto to easier client-side consumption.
        /// If using this multiple times in one call, it's HIGHLY recommended to pass in the typePropertyDtos 
        /// (so we don't have to retrieve these every time the function is called).
        /// </summary>
        /// <param name="assetWithAssetTypeAndAssetProperty"></param>
        /// <param name="typePropertyDtos"></param>
        /// <returns></returns>
        private DynamicAssetDto CreateDynamicAssetDto(
            Asset assetWithAssetTypeAndAssetProperty,
            bool generalOnly = true,
            List<TypeProperty> typeProperties = null,
            List<Section> sections = null)
        {
            sections = sections ?? db.Sections.ToList();
            var generalSectionIds = generalOnly ? sections.Where(i => i.IsGeneral).Select(i => i.Id).ToList() : sections.Select(i => i.Id).ToList();

            //get the type property DTOs if they weren't passed in.
            typeProperties = (typeProperties ?? db.TypeProperties.ToList()).Where(i => generalSectionIds.Contains(i.SectionId)).ToList();

            foreach (var assetProp in assetWithAssetTypeAndAssetProperty.AssetProperties)
            {
                assetProp.TypeProperty = typeProperties.FirstOrDefault(i => i.Id == assetProp.TypePropertyId);
            }

            var dictionary = new Dictionary<string, object>();
            dictionary["Id"] = assetWithAssetTypeAndAssetProperty.Id;

            foreach (var prop in assetWithAssetTypeAndAssetProperty.AssetProperties.Where(i => i.TypeProperty != null))
            {
                dictionary[prop.TypeProperty.PropertyName] = prop.Value;
            }

            return new DynamicAssetDto(dictionary);
        }

        // GET api/Asset/5
        [ResponseType(typeof(DynamicAssetDto))]
        public IHttpActionResult GetAsset(int id)
        {
            var asset = db.Assets.Include(i => i.AssetType).Include(f => f.AssetProperties).FirstOrDefault(i => i.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            var sections = db.Sections.ToList();
            var generalSectionIds = sections.Select(i => i.Id).ToList();

            //get the type property DTOs if they weren't passed in.
            var typeProperties = db.TypeProperties.Where(i => generalSectionIds.Contains(i.SectionId)).ToList();

            foreach (var assetProp in asset.AssetProperties)
            {
                assetProp.TypeProperty = typeProperties.FirstOrDefault(i => i.Id == assetProp.TypePropertyId);
            }

            var dictionary = new Dictionary<string, object>();
            dictionary["Id"] = asset.Id;

            foreach (var prop in asset.AssetProperties.Where(i => i.TypeProperty != null))
            {
                //dictionary[prop.TypeProperty.PropertyName] = prop.Value;
                dictionary[prop.TypeProperty.PropertyName] = new DynamicAssetPropertyDto(prop.TypeProperty, sections.ToDictionary(i => i.Id, j => j), prop.Value);
            }

            return Ok(new DynamicAssetDto(dictionary));
        }

        private Dictionary<string, object> ProcessRequest(HttpRequestMessage request)
        {
            //generate stuff from the request form
            var content = request.Content;
            string urlEncodedContent = content.ReadAsStringAsync().Result;
            //Id=6&Name=tacos&Description=tunafish
            var dictionary = new Dictionary<string, object>();
            urlEncodedContent.Split('&').ToList().ForEach(i =>
            {
                var kvp = i.Split('=');
                var key = HttpUtility.UrlDecode(kvp.First());
                var value = HttpUtility.UrlDecode(kvp.Last());
                dictionary[key] = value;
            });

            return dictionary;
        }

        /// <summary>
        /// This may be used for both inserts or updates. If an id field is passed and it is not null, an update will be attempted.
        /// Otherwise, an insert will be attempted.
        /// Must use Content-Type: application/x-www-form-urlencoded.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Asset))]
        public IHttpActionResult PostAsset()
        {
            try
            {
                var dictionary = ProcessRequest(Request);

                var assetTypeId = int.Parse(dictionary["AssetTypeId"].ToString());
                var typeProperties = db.TypeProperties.Where(i => i.AssetTypeId == assetTypeId);
                var assetType = db.AssetTypes.Include(i => i.TypeProperties).FirstOrDefault(i => i.Id == assetTypeId);

                if (dictionary.ContainsKey("Id") && dictionary["Id"].ToString() != "0" && !String.IsNullOrWhiteSpace(dictionary["Id"].ToString()))
                {
                    var assetId = int.Parse(dictionary["Id"].ToString());
                    db.AssertProperties.RemoveRange(db.AssertProperties.Where(i => i.AssetId == assetId));

                    foreach (var typeProperty in typeProperties)
                    {
                        if (dictionary.ContainsKey(typeProperty.PropertyName))
                        {
                            db.AssertProperties.Add(new AssetProperty
                            {
                                AssetId = assetId,
                                TypePropertyId = typeProperty.Id,
                                Value = dictionary[typeProperty.PropertyName].ToString()
                            });
                        }
                    }
                }
                else
                {
                    var asset = new Asset { AssetTypeId = assetTypeId };
                    db.Assets.Add(asset);
                    db.SaveChanges();

                    foreach (var typeProperty in assetType.TypeProperties)
                    {
                        if (dictionary.ContainsKey(typeProperty.PropertyName))
                        {
                            db.AssertProperties.Add(new AssetProperty
                            {
                                AssetId = asset.Id,
                                TypePropertyId = typeProperty.Id,
                                Value = dictionary[typeProperty.PropertyName].ToString()
                            });
                        }
                    }
                }

                db.SaveChanges();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(true);
            }
            catch (Exception)
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE api/Asset/5
        [ResponseType(typeof(Asset))]
        public IHttpActionResult DeleteAsset(int id)
        {
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }

            db.Assets.Remove(asset);
            db.SaveChanges();

            return Ok(asset);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}