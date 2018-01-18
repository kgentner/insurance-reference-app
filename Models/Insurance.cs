using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Reggie.Controllers;
using Microsoft.AspNetCore.Mvc;


namespace Reggie.Models
{
    public class Insurance : IValidatableObject
    {
        public int Id { get; set; }
        
        [Required]
        [Remote(action: "ValidateUniqueInsurance", controller: "Insurance", AdditionalFields = "InitialPayor,Plan,InitialPlan,Location_AK,InitialAK,Location_CA,InitialCA,Location_MT,InitialMT,Location_OR,InitialOR,Location_WA,InitialWA" )]
        public string Payor { get; set; }
        [Required]
        public string Plan { get; set; }
        public string PayorWebsite { get; set; }
        public string OtherContactInfo { get; set; }
        public string EDI_Number { get; set; }

        [Required]
        public bool Location_AK { get; set; }
        [Required]
        public bool Location_CA { get; set; }
        [Required]
        public bool Location_MT { get; set; }
        [Required]
        public bool Location_OR { get; set; }
        [Required]
        public bool Location_WA { get; set; }
        [Required]
        public bool Type_Commercial { get; set; }
        [Required]
        public bool Type_CommercialMang { get; set; }
        [Required]
        public bool Type_Medicare { get; set; }
        [Required]
        public bool Type_MedicareMang { get; set; }
        [Required]
        public bool Type_MedicareSup { get; set; }
        [Required]
        public bool Type_Medicaid { get; set; }
        [Required]
        public bool Type_MedicaidMang { get; set; }
        [Required]
        public bool Type_Military { get; set; }
        [Required]
        public bool Product_EPO { get; set; }
        [Required]
        public bool Product_HMO { get; set; }
        [Required]
        public bool Product_Ind { get; set; }
        [Required]
        public bool Product_PEBB { get; set; }
        [Required]
        public bool Product_PPO { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        
        [RegularExpression(@"(^(?!0{5})(\d{5})(?!-?0{4})(-?\d{4})?$)", ErrorMessage = "Improperly formatted zip code.  It must be entered as nnnnn or nnnnn-nnnnn.")]
        public string Zip { get; set; }
        public string ID_Description { get; set; }
        public string ReadingHint1 { get; set; }
        public string ReadingHint2 { get; set; }
        public string ReadingHint3 { get; set; }
        public string ReadingHint4 { get; set; }
        public string ReadingHint5 { get; set; }
        public string RegistrationHint1 { get; set; }
        public string RegistrationHint2 { get; set; }
        public string RegistrationHint3 { get; set; }
        public string RegistrationHint4 { get; set; }
        public string RegistrationHint5 { get; set; }
        public string RTE_Info1 { get; set; }
        public string RTE_Info2 { get; set; }
        public string RTE_Info3 { get; set; }
        public string RTE_Info4 { get; set; }
        public string RTE_Info5 { get; set; }
        public string Assoc_Workflow1 { get; set; }
        public string Assoc_Workflow1_Title { get; set; }
        public string Assoc_Workflow2 { get; set; }
        public string Assoc_Workflow2_Title { get; set; }
        public string Assoc_Workflow3 { get; set; }
        public string Assoc_Workflow3_Title { get; set; }
        public string Assoc_Workflow4 { get; set; }
        public string Assoc_Workflow4_Title { get; set; }
        public string Assoc_Workflow5 { get; set; }
        public string Assoc_Workflow5_Title { get; set; }
        public string HB_OfficeInfo1 { get; set; }
        public string HB_OfficeInfo2 { get; set; }
        public string HB_OfficeInfo3 { get; set; }
        public string HB_OfficeInfo4 { get; set; }
        public string HB_OfficeInfo5 { get; set; }
        public string PB_OfficeInfo1 { get; set; }
        public string PB_OfficeInfo2 { get; set; }
        public string PB_OfficeInfo3 { get; set; }
        public string PB_OfficeInfo4 { get; set; }
        public string PB_OfficeInfo5 { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? PlanActiveDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PlanInactiveDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReggiePublishStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReggiePublishEndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NewsfeedStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NewsfeedEndDate { get; set; }
        public string News { get; set; }
        public string Keywords { get; set; }

        //Class-level validation: 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Validate that at least one type is selected (true).
            if (Type_Commercial == false && Type_CommercialMang == false && Type_Medicare == false && Type_MedicareMang == false && Type_MedicareSup == false && Type_Medicaid == false && Type_MedicaidMang == false && Type_Military == false )
            {
                yield return new ValidationResult(
                    $"At least one type must be selected.",
                    new[] { "Type_Commercial" });
            }
            // Validate that at least one location is selected (true).
            if (Location_AK == false && Location_CA == false && Location_MT == false && Location_OR == false && Location_WA == false )
            {
                yield return new ValidationResult(
                    $"At least one location must be selected.",
                    new[] { "Location_AK" });
            }
            // Validate that only 1 in subtype is selected.
            if (Type_Commercial == true && Type_CommercialMang == true )
            {
                yield return new ValidationResult(
                    $"Plan may not be BOTH Commercial & Commercial Managed.",
                    new[] { "Type_Commercial" });
            }

            // Validate that only 1 in subtype is selected.
            if (Type_Medicare == true && Type_MedicareMang == true)
            {
                yield return new ValidationResult(
                    $"Plan may not be BOTH Medicare & Medicare Managed.",
                    new[] { "Type_Medicare" });
            }

            // Validate that only 1 in subtype is selected.
            if (Type_Medicare == true && Type_MedicareSup == true)
            {
                yield return new ValidationResult(
                    $"Plan may not be BOTH Medicare & Medicare Supplement.",
                    new[] { "Type_Medicare" });
            }

            // Validate that only 1 in subtype is selected.
            if (Type_MedicareMang == true && Type_MedicareSup == true)
            {
                yield return new ValidationResult(
                    $"Plan may not be BOTH Medicare Managed & Medicare Supplement.",
                    new[] { "Type_Medicare" });
            }

            // Validate that only 1 in subtype is selected.
            if (Type_Medicaid == true && Type_MedicaidMang == true )
            {
                yield return new ValidationResult(
                    $"Plan may not be BOTH Medicaid & Medicaid Managed.",
                    new[] { "Type_Medicaid" });
            }

            // Validate that Associated Workflow Title exists, if URL is entered
            if (!String.IsNullOrEmpty(Assoc_Workflow1) && String.IsNullOrEmpty(Assoc_Workflow1_Title) || 
                !String.IsNullOrEmpty(Assoc_Workflow2) && String.IsNullOrEmpty(Assoc_Workflow2_Title) || 
                !String.IsNullOrEmpty(Assoc_Workflow3) && String.IsNullOrEmpty(Assoc_Workflow3_Title) || 
                !String.IsNullOrEmpty(Assoc_Workflow4) && String.IsNullOrEmpty(Assoc_Workflow4_Title) || 
                !String.IsNullOrEmpty(Assoc_Workflow5) && String.IsNullOrEmpty(Assoc_Workflow5_Title))
            {
                yield return new ValidationResult(
                    $"Associated Workflow URL must have a corresponding Associated Workflow Title.",
                    new[] { "Assoc_Workflow1_Title" });
            }

            // Validate that Associated Workflow URL exists, if Title is entered
            if (String.IsNullOrEmpty(Assoc_Workflow1) && !String.IsNullOrEmpty(Assoc_Workflow1_Title) || 
                String.IsNullOrEmpty(Assoc_Workflow2) && !String.IsNullOrEmpty(Assoc_Workflow2_Title) || 
                String.IsNullOrEmpty(Assoc_Workflow3) && !String.IsNullOrEmpty(Assoc_Workflow3_Title) || 
                String.IsNullOrEmpty(Assoc_Workflow4) && !String.IsNullOrEmpty(Assoc_Workflow4_Title) || 
                String.IsNullOrEmpty(Assoc_Workflow5) && !String.IsNullOrEmpty(Assoc_Workflow5_Title))
            {
                yield return new ValidationResult(
                    $"Associated Workflow Title must have a corresponding Associated Workflow URL.",
                    new[] { "Assoc_Workflow1_Title" });
            }

            // Validate secondary street address
            if (!String.IsNullOrEmpty(Address2) && String.IsNullOrEmpty(Address1))
            {
                yield return new ValidationResult(
                    $"Address 2 must have a corresponding Address 1 entry",
                    new[] { "Address1" });
            }

            //Date validations
            if (PlanActiveDate.HasValue && PlanInactiveDate.HasValue && PlanActiveDate >= PlanInactiveDate)
            {
                yield return new ValidationResult(
                    $"'Plan Active Date' must NOT be ON or AFTER 'Plan Inactive Date'.",
                    new[] { "PlanActiveDate" });
                    
            }

            if (ReggiePublishStartDate.HasValue && !PlanActiveDate.HasValue && !PlanInactiveDate.HasValue)
            {
                yield return new ValidationResult(
                    $"'REGGIE Publish Start Date' requires a valid 'Plan Active Date' or valid 'Plan Inactive Date'.",
                    new[] { "PlanActiveDate" });
                    
            }

            if (ReggiePublishStartDate.HasValue && PlanActiveDate.HasValue && !PlanInactiveDate.HasValue && PlanActiveDate > ReggiePublishStartDate )
            {
                yield return new ValidationResult(
                    $" If there is no 'Plan Inactive Date', the 'Plan Active Date' must be ON or BEFORE 'REGGIE Publish Start Date'.",
                    new[] { "PlanActiveDate" });
            }

            if (ReggiePublishStartDate.HasValue && !PlanActiveDate.HasValue && PlanInactiveDate.HasValue && PlanInactiveDate > ReggiePublishStartDate )
            {
                yield return new ValidationResult(
                    $" If there is no 'Plan Active Date', the 'Plan Inactive Date' must be ON or BEFORE 'Reggie Publish Start Date'.",
                    new[] { "PlanInactiveDate" });
            }

            if (ReggiePublishStartDate.HasValue && PlanActiveDate.HasValue && PlanInactiveDate.HasValue && PlanActiveDate < PlanInactiveDate && PlanActiveDate > ReggiePublishStartDate )
            {
                yield return new ValidationResult(
                    $" If 'Plan Inactive Date' is AFTER the 'Plan Active Date', the 'Plan Active Date' must be ON or BEFORE 'Reggie Publish Start Date'.",
                    new[] { "PlanActiveDate" });
            }

            if (ReggiePublishStartDate.HasValue && ReggiePublishEndDate.HasValue && ReggiePublishStartDate >= ReggiePublishEndDate)
            {
                yield return new ValidationResult(
                    $"'Reggie Publish Start Date' must NOT be ON or AFTER the 'Reggie Publish End Date'.",
                    new[] { "ReggiePublishStartDate" });
            }

            if (NewsfeedStartDate.HasValue && NewsfeedEndDate.HasValue && NewsfeedStartDate >= NewsfeedEndDate)
            {
                yield return new ValidationResult(
                    $"'Newsfeed Start Date' must NOT be ON or AFTER the 'Newsfeed End Date'.",
                    new[] { "NewsfeedStartDate" });
            }

        }


    }

}