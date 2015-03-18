using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDatingAPI.Models.ViewModels
{
    public class PlainInfo : IPlainInfo
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

    }

    public interface IPlainInfo
    {
        int ID { get; set; }
        string Name { get; set; }
    }

}