using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reggie.Models;
using Reggie.ViewModels;
using Reggie.Data;


namespace Reggie.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw, string testUserNm)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Seeding the test admin.
                // The password is set with the following command:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin has all permissions.

                var adminID1 = await EnsureUser(serviceProvider, testUserPw, testUserNm);
                await EnsureRole(serviceProvider, adminID1, "Admin");

                SeedDB(context);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, 
                                                    string testUserPw, string email)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(email);
            if (user == null)
            {
                user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true};
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }  

                
        public static void SeedDB(ApplicationDbContext context)
            {
                //If there is already payor-plans, abort
                if (context.Insurances.Any())
                {
                    return;   // DB has been seeded
                }
                context.Insurances.AddRange(
                    new Insurance
                    {
                        Payor = "ABC Insurance Company",
                        Plan = "ABC PPO",
                        Keywords = "Choice, Easy",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        PayorWebsite = "http://www.example.com/",
                        OtherContactInfo = "Mental Health/Substance Abuse Questions: 800-555-5555",
                        Type_Commercial = true,
                        Type_CommercialMang = false,
                        Type_Medicare = false,
                        Type_MedicareMang = false,
                        Type_MedicareSup = false,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = false,
                        Product_HMO = false,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        Address1 = "PO Box 55555",
                        Address2 = "",
                        City = "Lexington",
                        State = "KY",
                        Zip = "40512",
                        ID_Description = "Policy Numbers are 10 digits long. Most begin with “W” or “8”.",
                        ReadingHint1 = "Some cards and RTE results indicate Easy Choice, but in OR, WA, and MT, these are ABC PPO outside of California.",
                        RegistrationHint1 = "Do not use payor 'ABC Managed Care' in OR, WA, MT, or AK (this is a CA specific payor). Enter only the first six characters of the group number.",
                        RTE_Info1 = "True effective date, Copay, Group number, Subscriber ID.",
                        Assoc_Workflow1 = "https://www.mlssoccer.com/",
                        Assoc_Workflow1_Title = "MLS",
                        Assoc_Workflow2 = "https://www.mlb.com",
                        Assoc_Workflow2_Title = "MLB",
                        Assoc_Workflow3 = "https://www.nfl.com",
                        Assoc_Workflow3_Title = "NFL",
                        Assoc_Workflow4 = "http://www.nba.com",
                        Assoc_Workflow4_Title = "NBA",
                        Assoc_Workflow5 = "https://www.nhl.com",
                        Assoc_Workflow5_Title = "NHL",
                        EDI_Number = "55555",
                        News = "Members began receiving new identification cards with a new four digit alpha prefix of WACW. It is extremely important to review the RTE response for all AMP and AMP Plus members after the first of the year. The new ID number is automatically updated and filed in the patient’s existing registration.",
                        NewsfeedStartDate = new DateTime(2017, 08, 01),
                        NewsfeedEndDate = new DateTime(2020, 012, 01),
                        ReggiePublishStartDate = new DateTime(2017, 08, 01),
                        ReggiePublishEndDate = new DateTime(2020, 12, 01),
                        PlanActiveDate = new DateTime(2017, 08, 01),
                        PlanInactiveDate = new DateTime(2020, 12, 01),

                    },
                    new Insurance
                    {
                        Payor = "XYZ Insurance Inc.",
                        Plan = "XYZ State of AK",
                        Keywords = "Baked Alaska",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        Type_Commercial = false,
                        Type_CommercialMang = true,
                        Type_Medicare = false,
                        Type_MedicareMang = false,
                        Type_MedicareSup = false,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = false,
                        Product_HMO = true,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        News="Please be sure to review the RTE response when sending subsequent RTE requests for patients who may already be registered with the Jetnik PPO plan, as you may need to update the registration. Most Jetnik EPO cards do not indicate Exclusive Provider Organization on the card.",
                        NewsfeedStartDate = new DateTime(2017, 08, 01),
                        NewsfeedEndDate = new DateTime(2020, 012, 01),
                        ReggiePublishStartDate = new DateTime(2017, 08, 01),
                        ReggiePublishEndDate = new DateTime(2020, 12, 01),
                        PlanActiveDate = new DateTime(2017, 08, 01),
                        PlanInactiveDate = new DateTime(2020, 12, 01),
                    
                    },
                    new Insurance
                    {
                        Payor = "ABC Insurance Company",
                        Plan = "ABC State of AK Retirees",
                        Keywords = "AlaskaTask",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        Type_Commercial = false,
                        Type_CommercialMang = false,
                        Type_Medicare = false,
                        Type_MedicareMang = true,
                        Type_MedicareSup = false,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = false,
                        Product_HMO = false,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        News="Real Time Eligibility (RTE) will be reinstated for ABCCARE/ABCCARE FOR LIFE. For this specific plan, you must send the request to the exact payor/plan combination of ABCcare/ABCcare for Life.",
                        NewsfeedStartDate = new DateTime(2017, 08, 01),
                        NewsfeedEndDate = new DateTime(2020, 012, 01),
                        ReggiePublishStartDate = new DateTime(2017, 08, 01),
                        ReggiePublishEndDate = new DateTime(2020, 12, 01),
                        PlanActiveDate = new DateTime(2017, 08, 01),
                        PlanInactiveDate = new DateTime(2020, 12, 01),
                    },
                    new Insurance
                    {
                        Payor = "XYZ Insurance Inc.",
                        Plan = "XYZ Care PPO",
                        Keywords = "Local 404, Local 206 Teamsters, Employee Trust Health and Welfare, Alaska Toolfitters, North Star Borough,",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        Type_Commercial = true,
                        Type_CommercialMang = false,
                        Type_Medicare = false,
                        Type_MedicareMang = true,
                        Type_MedicareSup = false,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = true,
                        Product_EPO = false,
                        Product_HMO = false,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        News="The XYZ PPO Original plan was deactivated due to payor updates. A new plan, XYZ PPO was created as its replacement. Please be sure to expire the deactivated coverage, XYZ PPO, using March 21, 2016 as the expiration date. Create a new coverage record using the new plan, XYZ PPO and the Effective from date of March 22, 2016 to avoid overlapping coverage.",
                        NewsfeedStartDate = new DateTime(2017, 08, 29),
                        NewsfeedEndDate = new DateTime(2020, 012, 01),
                        ReggiePublishStartDate = new DateTime(2017, 08, 01),
                        ReggiePublishEndDate = new DateTime(2020, 12, 01),
                        PlanActiveDate = new DateTime(2017, 08, 01),
                        PlanInactiveDate = new DateTime(2020, 12, 01)
                    },
                    new Insurance
                    {
                        Payor = "ABC Insurance Company",
                        Plan = "ABC EPO",
                        Keywords = "Frosty Foods, Local 792",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = false,
                        Location_OR = true,
                        Location_WA = false,
                        Type_Commercial = true,
                        Type_CommercialMang = true,
                        Type_Medicare = false,
                        Type_MedicareMang = false,
                        Type_MedicareSup = false,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = true,
                        Product_HMO = false,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        ReggiePublishStartDate = new DateTime(2017, 08, 01),
                        ReggiePublishEndDate = new DateTime(2020, 12, 01),
                        PlanActiveDate = new DateTime(2017, 08, 01),
                        PlanInactiveDate = new DateTime(2020, 12, 01)
                     
                    },
                    new Insurance
                    {
                        Payor = "XYZ Insurance Inc.",
                        Plan = "XYZ Student Health PPO",
                        Keywords = "Student Health",
                        Location_AK = false,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        Type_Commercial = false,
                        Type_CommercialMang = false,
                        Type_Medicare = false,
                        Type_MedicareMang = false,
                        Type_MedicareSup = false,
                        Type_Medicaid = true,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = false,
                        Product_HMO = false,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        ReggiePublishStartDate = new DateTime(2017, 08, 01),
                        ReggiePublishEndDate = new DateTime(2020, 12, 01),
                        PlanActiveDate = new DateTime(2017, 08, 01),
                        PlanInactiveDate = new DateTime(2020, 12, 01)
      
                    },
                    new Insurance
                    {
                        Payor = "ABC Insurance Company",
                        Plan = "ABC Global Benefits",
                        Keywords = "ABC International, Global Benefits, Multi-plan",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        Type_Commercial = true,
                        Type_CommercialMang = false,
                        Type_Medicare = false,
                        Type_MedicareMang = false,
                        Type_MedicareSup = false,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = false,
                        Product_HMO = false,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        ReggiePublishStartDate = new DateTime(2017, 08, 01),
                        ReggiePublishEndDate = new DateTime(2020, 12, 01),
                        PlanInactiveDate = new DateTime(2017, 08, 01)
 
                    },
                    new Insurance
                    {
                        Payor = "XYZ Insurance Inc.",
                        Plan = "XYZ Indemnity",
                        Keywords = "Multi-plan",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        Type_Commercial = false,
                        Type_CommercialMang = false,
                        Type_Medicare = true,
                        Type_MedicareMang = false,
                        Type_MedicareSup = false,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = false,
                        Product_HMO = false,
                        Product_Ind = true,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        ReggiePublishStartDate = new DateTime(2017, 08, 01),
                        ReggiePublishEndDate = new DateTime(2020, 12, 01),
                        PlanInactiveDate = new DateTime(2017, 08, 01)
       
                    },
                    new Insurance
                    {
                        Payor = "ABC Insurance Company",
                        Plan = "ABC HMO",
                        Keywords = "Easy as 123, Easy like Sunday morning",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        Type_Commercial = false,
                        Type_CommercialMang = false,
                        Type_Medicare = false,
                        Type_MedicareMang = false,
                        Type_MedicareSup = true,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = false,
                        Product_HMO = true,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        PlanActiveDate = new DateTime(2017, 08, 01),
                        PlanInactiveDate = new DateTime(2020, 12, 01)
  
                    },
                    new Insurance
                    {
                        Payor = "XYZ Insurance Inc.",
                        Plan = "XYZ PEBB",
                        Keywords = "Mister PEBB, ChoiceSelect",
                        Location_AK = true,
                        Location_CA = true,
                        Location_MT = true,
                        Location_OR = true,
                        Location_WA = true,
                        Type_Commercial = false,
                        Type_CommercialMang = false,
                        Type_Medicare = false,
                        Type_MedicareMang = false,
                        Type_MedicareSup = true,
                        Type_Medicaid = false,
                        Type_MedicaidMang = false,
                        Type_Military = false,
                        Product_EPO = false,
                        Product_HMO = true,
                        Product_Ind = false,        
                        Product_PEBB = false,
                        Product_PPO = true,
                        PlanActiveDate = new DateTime(2017, 08, 01),
                        PlanInactiveDate = new DateTime(2020, 12, 01)
  
                    }
                );
                context.SaveChanges();
            }
    }
}
