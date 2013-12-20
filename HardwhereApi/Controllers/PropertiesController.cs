using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HardwhereApi.Core.Models;
using HardwhereApi.Infrastructure;

namespace HardwhereApi.Controllers
{
    public class PropertiesController : ApiController
    {
        private HardwhereApiContext db = new HardwhereApiContext();

        // GET api/Properties
        public IQueryable<TypeProperty> GetTypeProperties()
        {
            return db.TypeProperties;
        }

        // GET api/Properties/5
        [ResponseType(typeof(TypeProperty))]
        public IHttpActionResult GetTypeProperty(int id)
        {
            TypeProperty typeproperty = db.TypeProperties.Find(id);
            if (typeproperty == null)
            {
                return NotFound();
            }

            return Ok(typeproperty);
        }

        // PUT api/Properties/5
        public IHttpActionResult PutTypeProperty(int id, TypeProperty typeproperty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != typeproperty.Id)
            {
                return BadRequest();
            }

            db.Entry(typeproperty).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypePropertyExists(id))
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

        // POST api/Properties
        [ResponseType(typeof(TypeProperty))]
        public IHttpActionResult PostTypeProperty(TypeProperty typeproperty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TypeProperties.Add(typeproperty);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = typeproperty.Id }, typeproperty);
        }

        // DELETE api/Properties/5
        [ResponseType(typeof(TypeProperty))]
        public IHttpActionResult DeleteTypeProperty(int id)
        {
            TypeProperty typeproperty = db.TypeProperties.Find(id);
            if (typeproperty == null)
            {
                return NotFound();
            }

            db.TypeProperties.Remove(typeproperty);
            db.SaveChanges();

            return Ok(typeproperty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TypePropertyExists(int id)
        {
            return db.TypeProperties.Count(e => e.Id == id) > 0;
        }
    }
}