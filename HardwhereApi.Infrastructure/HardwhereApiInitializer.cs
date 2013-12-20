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
    public class HardwhereApiInitializer : DropCreateDatabaseIfModelChanges<HardwhereApiContext>
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
                    Name = "Admin",
                    Password = GenerateHashedPassword("password", "1234") //hashed version of '1234' + salt
                }
            };
            users.ForEach(i => context.Users.Add(i));


            base.Seed(context);
        }
    }
}
