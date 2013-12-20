using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HardwhereApi.Core.Models;

namespace HardwhereApi.Infrastructure
{
    public class HardwhereApiInitializer : DropCreateDatabaseAlways<HardwhereApiContext>
    {
        private string GenerateHashedPassword(string password, string salt)
        {
            return System.Text.Encoding.Default.GetString(SHA256Managed.Create().ComputeHash(System.Text.Encoding.Default.GetBytes(password + salt)));
        }

        protected override void Seed(HardwhereApiContext context)
        {

            var users = new List<User>
            {
                new User
                {
                    Name = "admin",
                    Password = GenerateHashedPassword("password", "1234") //hashed version of '1234' + salt
                }
            };


            var assetTypes = new List<AssetType> { new AssetType { Name = "desktop" } };

            var typeProperties = new List<TypeProperty>
            {
                new TypeProperty {PropertyName = "Name"},
                new TypeProperty {PropertyName = "Description"},
                new TypeProperty {PropertyName = "Serial Number"}
            };

            var assets = new List<Asset>
            {
                new Asset { AssetTypeId = 1 }, 
                new Asset { AssetTypeId = 1 }
            };

            var assetProperties = new List<AssetProperty>
            {
                new AssetProperty {AssetId = 1, TypePropertyId = 1, Value = "Aname"},
                new AssetProperty {AssetId = 1, TypePropertyId = 2, Value = "Descriptionone"},
                new AssetProperty {AssetId = 1, TypePropertyId = 3, Value = "8675309"},
                new AssetProperty {AssetId = 2, TypePropertyId = 1, Value = "second name"},
                new AssetProperty {AssetId = 2, TypePropertyId = 2, Value = "arnold broke this"},
                new AssetProperty {AssetId = 2, TypePropertyId = 3, Value = "#123dasd"},
            };

            users.ForEach(i => context.Users.Add(i));
            assetTypes.ForEach(i => context.AssetTypes.Add(i));
            typeProperties.ForEach(i => context.TypeProperties.Add(i));
            context.SaveChanges();

            assets.ForEach(i => context.Assets.Add(i));
            context.SaveChanges();

            assetProperties.ForEach(i => context.AssertProperties.Add(i));
            base.Seed(context);
        }
    }
}
