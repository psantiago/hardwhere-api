using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HardwhereApi.Core.Models;
using ValueType = HardwhereApi.Core.Models.ValueType;

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


            context.AssetTypes.Add(new AssetType { Name = "Desktop", IconName = "fa-desktop" });
            context.SaveChanges();

            context.AssetTypes.Add(new AssetType { Name = "Laptop", IconName = "fa-laptop" });
            context.SaveChanges();

            context.AssetTypes.Add(new AssetType { Name = "Server", IconName = "fa-upload" });
            context.SaveChanges();

            context.AssetTypes.Add(new AssetType { Name = "Printer", IconName = "fa-print" });
            context.SaveChanges();

            var typeProperties = new List<TypeProperty>
            {
                new TypeProperty {AssetTypeId = 1, PropertyName = "Name", SectionId = 1, ValueTypeId = 1, Order = 1},
                new TypeProperty {AssetTypeId = 1, PropertyName = "Description", SectionId = 1, ValueTypeId = 1, Order = 2},
                new TypeProperty {AssetTypeId = 1, PropertyName = "Serial Number", SectionId = 1, ValueTypeId = 1, Order = 3},
                new TypeProperty {AssetTypeId = 1, PropertyName = "MAC Address", SectionId = 2, ValueTypeId = 1, Order = 4}
            };

            var sections = new List<Section>
            {
                new Section {AssetTypeId = 1, Name = "General", Order = 1, IsGeneral = true},
                new Section {AssetTypeId = 1, Name = "Networking", Order = 2, IsGeneral = false}
            };

            var valueTypes = new List<ValueType>
            {
                new ValueType {Name="string", Regex=".", ErrorMessage = "must be a string"},
                new ValueType {Name="bool", Regex="true|false|0|1", ErrorMessage = "must be a boolean"},
                new ValueType {Name="date", Regex=".", ErrorMessage = "must be a date"},
                new ValueType {Name="guid", Regex=".", ErrorMessage = "must be a guid"},
                new ValueType {Name="int", Regex=".", ErrorMessage = "must be a int"},
                new ValueType {Name="number", Regex=".", ErrorMessage = "must be a number"},
                new ValueType {Name="blob", Regex=".", ErrorMessage = "must be a blob"},
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
                new AssetProperty {AssetId = 1, TypePropertyId = 4, Value = "555:111:222"},
                new AssetProperty {AssetId = 2, TypePropertyId = 1, Value = "second name"},
                new AssetProperty {AssetId = 2, TypePropertyId = 2, Value = "arnold broke this"},
                new AssetProperty {AssetId = 2, TypePropertyId = 3, Value = "#123dasd"},
                new AssetProperty {AssetId = 2, TypePropertyId = 4, Value = "555:123:6645"},
            };

            users.ForEach(i => context.Users.Add(i));

            sections.ForEach(i => context.Sections.Add(i));
            context.SaveChanges();

            valueTypes.ForEach(i => context.ValueTypes.Add(i));
            context.SaveChanges();

            typeProperties.ForEach(i => context.TypeProperties.Add(i));
            context.SaveChanges();

            assets.ForEach(i => context.Assets.Add(i));
            context.SaveChanges();

            assetProperties.ForEach(i => context.AssertProperties.Add(i));

            var goodAssets = new List<GoodAsset>
            {
                new GoodAsset{Type="Desktop", Description = "hello", Name = "first", Location = "2041", SerialNumber = "#13123"},
                new GoodAsset{Type="Desktop", Description = "desc2", Name = "second", Location = "2042", SerialNumber = "#1311323"},
                new GoodAsset{Type="Desktop", Description = "desc4", Name = "third", Location = "2043", SerialNumber = "#131dasd23"},
                new GoodAsset{Type="Desktop", Description = "desc5", Name = "fif", Location = "2044", SerialNumber = "#1555523"},
            };

            goodAssets.ForEach(i => context.GoodAssets.Add(i));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
