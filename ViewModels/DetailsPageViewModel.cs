using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reggie.Models;


namespace Reggie.ViewModels
{
    public class DetailsPageViewModel
    {
        public int Id { get; set; }
        public string Payor { get; set; }
        public bool IsReggiePublished { get; set; }
        public bool IsPlanActive { get; set; }
        public string PayorWebsite { get; set; }
        public string OtherContactInfo { get; set; }
        public string Plan { get; set; }
        public string Keywords { get; set; }
        public bool Type_Commercial { get; set; }
        
        public bool Type_CommercialMang { get; set; }
        
        public bool Type_Medicare { get; set; }
        
        public bool Type_MedicareMang { get; set; }
        
        public bool Type_MedicareSup { get; set; }
        
        public bool Type_Medicaid { get; set; }
        
        public bool Type_MedicaidMang { get; set; }
        
        public bool Type_Military { get; set; }
        
        public bool Product_EPO { get; set; }
        
        public bool Product_HMO { get; set; }
        
        public bool Product_Ind { get; set; }
        
        public bool Product_PEBB { get; set; }
        
        public bool Product_PPO { get; set; }
        public bool Location_AK { get; set; }
        public bool Location_CA { get; set; }
        public bool Location_MT { get; set; }
        public bool Location_OR { get; set; }
        public bool Location_WA { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
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
        public string EDI_Number { get; set; }
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

    }
}