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
using HardwhereApi.Core.Models;
using HardwhereApi.Infrastructure;

namespace HardwhereApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AssetTypeController : ApiController
    {
        private HardwhereApiContext db = new HardwhereApiContext();

        // GET api/AssetType
        public IQueryable<AssetType> GetAssetTypes()
        {
            return db.AssetTypes;
        }

        // GET api/AssetType/5
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult GetAssetType(int id)
        {
            AssetType assettype = db.AssetTypes.Find(id);
            if (assettype == null)
            {
                return NotFound();
            }

            return Ok(assettype);
        }

        // PUT api/AssetType/5
        public IHttpActionResult PutAssetType(int id, AssetType assettype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assettype.Id)
            {
                return BadRequest();
            }

            db.Entry(assettype).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetTypeExists(id))
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

        // POST api/AssetType
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult PostAssetType(AssetType assettype)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AssetTypes.Add(assettype);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = assettype.Id }, assettype);
        }

        // DELETE api/AssetType/5
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult DeleteAssetType(int id)
        {
            AssetType assettype = db.AssetTypes.Find(id);
            if (assettype == null)
            {
                return NotFound();
            }

            db.AssetTypes.Remove(assettype);
            db.SaveChanges();

            return Ok(assettype);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssetTypeExists(int id)
        {
            return db.AssetTypes.Count(e => e.Id == id) > 0;
        }
    }
}