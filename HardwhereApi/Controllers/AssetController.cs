using System;
using System.Collections.Generic;
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
            var result = db.Assets.Include(i => i.AssetType).Include(f => f.AssetProperties).ToList().Select(Mapper.Map<AssetDto>);

            //get the type properties and map to their dto
            var types = db.TypeProperties.Select(Mapper.Map<TypePropertyDto>).ToList();

            var sections = db.Sections.Select(Mapper.Map<SectionDto>).ToList();

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
            AssetDto assetWithAssetTypeAndAssetProperty,
            bool generalOnly = true,
            List<TypePropertyDto> typePropertyDtos = null,
            List<SectionDto> sectionDtos = null)
        {
            sectionDtos = sectionDtos ?? db.Sections.Select(Mapper.Map<SectionDto>).ToList();
            var generalSectionIds = generalOnly ? sectionDtos.Where(i => i.IsGeneral).Select(i => i.Id).ToList() : sectionDtos.Select(i => i.Id).ToList();

            //get the type property DTOs if they weren't passed in.
            typePropertyDtos = (typePropertyDtos ?? db.TypeProperties.Select(Mapper.Map<TypePropertyDto>)).Where(i => generalSectionIds.Contains(i.SectionId)).ToList();

            foreach (var assetProp in assetWithAssetTypeAndAssetProperty.AssetProperties)
            {
                assetProp.TypeProperty = typePropertyDtos.FirstOrDefault(i => i.Id == assetProp.TypePropertyId);
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
            var asset = Mapper.Map<AssetDto>(db.Assets.Include(i => i.AssetType).Include(f => f.AssetProperties).FirstOrDefault(i => i.Id == id));
            if (asset == null)
            {
                return NotFound();
            }

            var sections = db.Sections.ToList();
            var generalSectionIds = sections.Select(i => i.Id).ToList();

            //get the type property DTOs if they weren't passed in.
            var typePropertyDtos = db.TypeProperties.Select(Mapper.Map<TypePropertyDto>).Where(i => generalSectionIds.Contains(i.SectionId)).ToList();

            foreach (var assetProp in asset.AssetProperties)
            {
                assetProp.TypeProperty = typePropertyDtos.FirstOrDefault(i => i.Id == assetProp.TypePropertyId);
            }

            var dictionary = new Dictionary<string, object>();
            dictionary["Id"] = asset.Id;

            foreach (var prop in asset.AssetProperties.Where(i => i.TypeProperty != null))
            {
                //dictionary[prop.TypeProperty.PropertyName] = prop.Value;
                dictionary[prop.TypeProperty.PropertyName] = GetPropertyObject(prop.TypeProperty, sections.ToDictionary(i => i.Id, j => j), prop.Value);
            }

            return Ok(new DynamicAssetDto(dictionary));
        }

        private DynamicAssetDto getDynamicAssetDto(int id)
        {
              var asset = Mapper.Map<AssetDto>(db.Assets.Include(i => i.AssetType).Include(f => f.AssetProperties).FirstOrDefault(i => i.Id == id));
            if (asset == null)
            {
                return null;
            }

            var sections = db.Sections.ToList();
            var generalSectionIds = sections.Select(i => i.Id).ToList();

            //get the type property DTOs if they weren't passed in.
            var typePropertyDtos = db.TypeProperties.Select(Mapper.Map<TypePropertyDto>).Where(i => generalSectionIds.Contains(i.SectionId)).ToList();

            foreach (var assetProp in asset.AssetProperties)
            {
                assetProp.TypeProperty = typePropertyDtos.FirstOrDefault(i => i.Id == assetProp.TypePropertyId);
            }

            var dictionary = new Dictionary<string, object>();
            dictionary["Id"] = asset.Id;

            foreach (var prop in asset.AssetProperties.Where(i => i.TypeProperty != null))
            {
                //dictionary[prop.TypeProperty.PropertyName] = prop.Value;
                dictionary[prop.TypeProperty.PropertyName] = GetPropertyObject(prop.TypeProperty, sections.ToDictionary(i => i.Id, j => j), prop.Value);
            }

            return new DynamicAssetDto(dictionary);
        }

        private SuperDynamic GetPropertyObject(
            TypePropertyDto typeProperty,
            Dictionary<int, Section> sections,
            object currentAssetPropertyValue = null)
        {
            var currentValueType = sections[typeProperty.SectionId];
            var dictionary = new Dictionary<string, object>();
            dictionary["Value"] = currentAssetPropertyValue;
            dictionary["SectionName"] = currentValueType.Name;
            dictionary["SectionOrder"] = currentValueType.Order;
            dictionary["PropertyOrder"] = typeProperty.Order;

            return new SuperDynamic(dictionary);
        }

        // PUT api/Asset/5
        public IHttpActionResult PutAsset(int id, Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != asset.Id)
            {
                return BadRequest();
            }

            db.Entry(asset).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Asset
        [ResponseType(typeof(Asset))]
        public IHttpActionResult PostAsset()
        {
            try
            {
                //generate stuff from the request form
                var content = Request.Content;
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

                var assetTypeId = int.Parse(dictionary["AssetTypeId"].ToString());
                var assetType = db.AssetTypes.Include(i => i.TypeProperties).FirstOrDefault(i => i.Id == assetTypeId);

                var asset = new Asset {AssetTypeId = assetTypeId};
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

                db.SaveChanges();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return CreatedAtRoute("DefaultApi", new { id = asset.Id }, getDynamicAssetDto(asset.Id));
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

        private bool AssetExists(int id)
        {
            return db.Assets.Count(e => e.Id == id) > 0;
        }
    }
}