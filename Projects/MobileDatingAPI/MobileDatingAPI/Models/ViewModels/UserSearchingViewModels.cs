using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{
    public class UserSearchingViewModels
    {

        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }
        public int? GenderID { get; set; }
        public int? InterestedInID { get; set; }
        public int? ReligiousViewID { get; set; }
        public string School { get; set; }
        public bool? SchoolGraduated { get; set; }
        public string Work { get; set; }
        public string Workplace { get; set; }

        public double? MaximumDistance { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool ValidateConditions()
        {
            return
                (this.AgeMin.HasValue && this.AgeMax.HasValue) ||
                this.GenderID.HasValue ||
                this.InterestedInID.HasValue ||
                this.ReligiousViewID.HasValue ||
                !string.IsNullOrWhiteSpace(this.School) ||
                this.SchoolGraduated.HasValue ||
                !string.IsNullOrWhiteSpace(this.Work) ||
                !string.IsNullOrWhiteSpace(this.Workplace) ||
                (this.MaximumDistance.HasValue && this.Latitude.HasValue && this.Longitude.HasValue);
        }

    }
}