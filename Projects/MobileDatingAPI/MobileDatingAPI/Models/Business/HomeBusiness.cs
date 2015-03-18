using MobileDatingAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileDatingAPI.Models.Business
{

    public class HomeBusiness : BaseBusiness
    {

        public PlainInfoListViewModels GetPlainInfoLists(PlainInfoListViewModels model)
        {
            model.InfoLists.Add(PlainInfoCategories.Gender,
                this.ApiEntities.Genders
                .Where(q => q.Active)
                .Select(q => new PlainInfo()
                {
                    ID = q.ID,
                    Name = q.Name,
                })
                .ToList());

            model.InfoLists.Add(PlainInfoCategories.InterestedIn,
                this.ApiEntities.InterestedIns
                .Where(q => q.Active)
                .Select(q => new PlainInfo()
                {
                    ID = q.ID,
                    Name = q.Name,
                })
                .ToList());

            model.InfoLists.Add(PlainInfoCategories.ReligiousView,
                this.ApiEntities.ReligiousViews
                .Where(q => q.Active)
                .Select(q => new PlainInfo()
                {
                    ID = q.ID,
                    Name = q.Name,
                })
                .ToList());

            model.InfoLists.Add(PlainInfoCategories.Relationship,
                this.ApiEntities.RelationshipStatus
                .Where(q => q.Active)
                .Select(q => new PlainInfo()
                {
                    ID = q.ID,
                    Name = q.Name,
                })
                .ToList());

            return model;
        }

    }

}