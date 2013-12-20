using System;
using System.Collections.Generic;
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
using HardwhereApi.Infrastructure;

namespace HardwhereApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class GoodAssetController : ApiController
    {
        private HardwhereApiContext db = new HardwhereApiContext();

        // GET api/Asset
        public IEnumerable<GoodAssetDto> GetAssets()
        {
            //return "values";
            var result = db.GoodAssets.ToList().Select(Mapper.Map<GoodAssetDto>);
            return result;
        }

        // GET api/Asset/5
        [ResponseType(typeof(GoodAsset))]
        public IHttpActionResult GetAsset(int id)
        {
            var asset = db.GoodAssets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset);
        }

        // PUT api/Asset/5
        public IHttpActionResult PutAsset(int id, GoodAsset asset)
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
        public IHttpActionResult PostAsset(GoodAsset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GoodAssets.Add(asset);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = asset.Id }, asset);
        }

        // DELETE api/Asset/5
        [ResponseType(typeof(GoodAsset))]
        public IHttpActionResult DeleteAsset(int id)
        {
            GoodAsset asset = db.GoodAssets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }

            db.GoodAssets.Remove(asset);
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
            return db.GoodAssets.Count(e => e.Id == id) > 0;
        }
    }
}
