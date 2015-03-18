using MobileDatingCMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingCMS.Models.Business
{
    public class HobbyBusiness : BaseBusiness
    {

        public IEnumerable<Hobby> GetHobbies(int? parentID)
        {
            return this.CmsEntities.Hobbies
                .Where(q => q.Active && q.ParentHobbyID == parentID)
                .OrderBy(q => q.Name);
        }

        public void AddHobby(HobbyViewModels model)
        {
            var hobby = new Hobby()
            {
                Name = model.Name,
                Description = model.Description,
                ParentHobbyID = model.ParentHobbyID,
                Active = model.Active,
            };

            this.CmsEntities.Hobbies.Add(hobby);
            this.CmsEntities.SaveChanges();
        }

    }
}