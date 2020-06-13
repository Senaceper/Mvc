using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace denemee.Models
{
    public class ViewModel
    {
        public IEnumerable<Urunler> UrunModel { get; set; }
        public IEnumerable<Sepet> SepetModel { get; set; }
    }
}