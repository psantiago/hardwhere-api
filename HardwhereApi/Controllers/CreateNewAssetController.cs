using AutoMapper;
using HardwhereApi.Core.Dto;
using HardwhereApi.Core.Models;
using HardwhereApi.Core.Utilities;
using HardwhereApi.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;


namespace HardwhereApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class CreateNewAssetController : ApiController
    {
        private HardwhereApiContext db = new HardwhereApiContext();

        [ResponseType(typeof(DynamicAssetDto))]
        public IHttpActionResult GetCreateNewAsset(int id)
        {
            var assetType = db.AssetTypes.Include(i => i.TypeProperties).FirstOrDefault(i => i.Id == id);
            if (assetType == null) return NotFound();

            var valueTypes = db.ValueTypes.Select(Mapper.Map<ValueTypeDto>).ToDictionary(i => i.Id, j => j);

            var sections = db.Sections.Select(Mapper.Map<SectionDto>).ToDictionary(i => i.Id, j => j);

            var dictionary = new Dictionary<string, object>();
            dictionary["Id"] = 0;
            dictionary["AssetTypeId"] = id;

            foreach (var prop in assetType.TypeProperties)
            {
                dictionary[prop.PropertyName] = GetPropertyObject(prop, valueTypes, sections);
            }

            return Ok(new DynamicAssetDto(dictionary));
        }

        private SuperDynamic GetPropertyObject(
            TypeProperty typeProperty,
            Dictionary<int, ValueTypeDto> valueTypes,
            Dictionary<int, SectionDto> sections,
            object currentAssetPropertyValue = null)
        {
            var currentValueType = valueTypes[typeProperty.ValueTypeId];
            var currentSection = sections[typeProperty.SectionId];
            var dictionary = new Dictionary<string, object>();
            dictionary["Value"] = currentAssetPropertyValue;
            dictionary["Type"] = currentValueType.Name;
            dictionary["Regex"] = currentValueType.Regex;
            dictionary["ErrorMessage"] = currentValueType.ErrorMessage;
            dictionary["PropertyOrder"] = typeProperty.Order;
            dictionary["SectionName"] = currentSection.Name;
            dictionary["SectionOrder"] = currentSection.Order;

            return new SuperDynamic(dictionary);
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
