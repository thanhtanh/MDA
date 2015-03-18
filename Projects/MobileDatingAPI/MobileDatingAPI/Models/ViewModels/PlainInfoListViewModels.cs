using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{

    public class PlainInfoListViewModels : BaseApiViewModels
    {

        public Dictionary<string, List<PlainInfo>> InfoLists { get; set; }

        public PlainInfoListViewModels()
        {
            this.InfoLists = new Dictionary<string, List<PlainInfo>>();
        }

    }

    public static class PlainInfoCategories 
    {
        public const string Gender = "Gender";
        public const string InterestedIn = "InterestedIn";
        public const string ReligiousView = "ReligiousView";
        public const string Relationship = "Relationship";
    }

}