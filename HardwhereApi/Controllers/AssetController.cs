using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            //return "values";
            // var result = db.Assets.ToList().Select(Mapper.Map<AssetDto>);
            var result = db.Assets.Include(i => i.AssetType).Include(f => f.AssetProperties).ToList().Select(Mapper.Map<AssetDto>);
            var types = db.TypeProperties.Select(Mapper.Map<TypePropertyDto>).ToList();

            foreach (var asset in result)
            {
                foreach (var assetProp in asset.AssetProperties)
                {
                    assetProp.TypeProperty = types.FirstOrDefault(i => i.Id == assetProp.TypePropertyId);
                }
            }

            var moreAwesomeness = new List<DynamicAssetDto>();

            
            foreach (var asset in result)
            {
                var dictionary = new Dictionary<string, object>();
                dictionary["Id"] = asset.Id;

                foreach (var prop in asset.AssetProperties)
                {
                    dictionary[prop.TypeProperty.PropertyName] = prop.Value;
                }

                moreAwesomeness.Add(new DynamicAssetDto(dictionary));
            }

            return moreAwesomeness;
        }

        // GET api/Asset/5
        [ResponseType(typeof(Asset))]
        public IHttpActionResult GetAsset(int id)
        {
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset);
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
        public IHttpActionResult PostAsset(Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Assets.Add(asset);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = asset.Id }, asset);
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