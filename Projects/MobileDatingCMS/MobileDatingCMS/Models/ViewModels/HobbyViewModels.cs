using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MobileDatingCMS.Models.ViewModels
{

    public class HobbyViewModels : BaseViewModels
    {

        public int? ParentHobbyID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }


    }

    public class HobbyTreeItemViewModels
    {

        public int id { get; set; }
        public string text { get; set; }
        public bool children { get; set; }

        public HobbyTreeItemViewModels() { }

        public HobbyTreeItemViewModels(Hobby hobby)
        {
            this.id = hobby.ID;
            this.text = hobby.Name;

            this.children = hobby.SubHobbies.Any();
        }

    }


}