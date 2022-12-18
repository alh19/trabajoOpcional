using Sandwich2Go.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Sandwich2Go.Data
{
    public static class SeedData
    {
        //public static void Initialize(UserManager<ApplicationUser> userManager,
        //            RoleManager<IdentityRole> roleManager)
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var role = serviceProvider.GetRequiredService(typeof(RoleManager<IdentityRole>));
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            
            
            List<string> rolesNames = new List<string> { "Gerente", "Cliente" };

            SeedRoles(roleManager, rolesNames);
            SeedUsers(userManager, rolesNames);

            DirectoryInfo directoryData = new DirectoryInfo(".\\Data");

            // Se obtienen todos los ficheros localizados en .\AppForMovies\Data que contienen un ".sql" en su nombre
            foreach (FileInfo item in directoryData.GetFiles().Where(m => m.Name.Contains(".sql")))
            {
                try
                {
                    // Se lee el contenido del fichero ".sql"
                    string commandSQL = item.OpenText().ReadToEnd();
 

                    // Se ejecuta el contenido del fichero ".sql"
                    dbContext.Database.ExecuteSqlRaw(commandSQL);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }



        }

       
        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {    

            foreach (string roleName in roles) { 
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<IdentityUser> userManager, List<string> roles)
        {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.com").Result == null)
            {
                Gerente user = new Gerente();
                user.Id = "1";
                user.UserName = "elena@uclm.com";
                user.Email = "elena@uclm.com";
                user.Nombre = "Elena";
                user.Apellido = "Navarro Martinez";
                user.EmailConfirmed = true;
                user.Direccion = "";

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user,roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("gregorio@uclm.com").Result == null)
            {
                Cliente user = new Cliente();
                user.Id = "2";
                user.UserName = "gregorio@uclm.com";
                user.Email = "gregorio@uclm.com";
                user.Nombre = "Gregorio";
                user.Apellido = "Diaz Descalzo";
                user.EmailConfirmed = true;
                user.Direccion = "";

                var result = userManager.CreateAsync(user, "APassword1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                }
            }

        }


    }



}


