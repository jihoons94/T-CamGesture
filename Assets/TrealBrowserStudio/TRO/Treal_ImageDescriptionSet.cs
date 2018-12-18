 using System;
using System.Collections.Generic;

namespace Treal.Browser.AR
{
    [Serializable]
    public class Treal_ImageDescriptionSet
    {
        public string tro_id;
        public string tro_name;

        public List<Treal_ImageDescription> descriptor_list;
    }

}
