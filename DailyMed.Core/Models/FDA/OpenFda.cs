﻿namespace DailyMed.Core.Models.FDA
{

    public class OpenFda
    {
        public List<string> application_number { get; set; }
        public List<string> brand_name { get; set; }
        public List<string> generic_name { get; set; }
        public List<string> manufacturer_name { get; set; }
        public List<string> product_ndc { get; set; }
        public List<string> product_type { get; set; }
        public List<string> route { get; set; }
        public List<string> substance_name { get; set; }
        public List<string> rxcui { get; set; }
        public List<string> spl_id { get; set; }
        public List<string> spl_set_id { get; set; }
        public List<string> package_ndc { get; set; }
        public List<bool> is_original_packager { get; set; }
        public List<string> nui { get; set; }
        public List<string> pharm_class_epc { get; set; }
        public List<string> pharm_class_moa { get; set; }
        public List<string> pharm_class_cs { get; set; }
        public List<string> unii { get; set; }
    }


}
