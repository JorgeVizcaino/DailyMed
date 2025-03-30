namespace DailyMed.Core.Models.FDA
{

    public class Result
    {
        public List<string> spl_product_data_elements { get; set; }
        public List<string> recent_major_changes { get; set; }
        public List<string> recent_major_changes_table { get; set; }
        public List<string> indications_and_usage { get; set; }
        public List<string> dosage_and_administration { get; set; }
        public List<string> dosage_and_administration_table { get; set; }
        public List<string> dosage_forms_and_strengths { get; set; }
        public List<string> contraindications { get; set; }
        public List<string> warnings_and_cautions { get; set; }
        public List<string> adverse_reactions { get; set; }
        public List<string> adverse_reactions_table { get; set; }
        public List<string> use_in_specific_populations { get; set; }
        public List<string> pregnancy { get; set; }
        public List<string> pediatric_use { get; set; }
        public List<string> geriatric_use { get; set; }
        public List<string> overdosage { get; set; }
        public List<string> description { get; set; }
        public List<string> clinical_pharmacology { get; set; }
        public List<string> mechanism_of_action { get; set; }
        public List<string> pharmacodynamics { get; set; }
        public List<string> pharmacokinetics { get; set; }
        public List<string> nonclinical_toxicology { get; set; }
        public List<string> carcinogenesis_and_mutagenesis_and_impairment_of_fertility { get; set; }
        public List<string> clinical_studies { get; set; }
        public List<string> clinical_studies_table { get; set; }
        public List<string> how_supplied { get; set; }
        public List<string> how_supplied_table { get; set; }
        public List<string> storage_and_handling { get; set; }
        public List<string> information_for_patients { get; set; }
        public List<string> spl_unclassified_section { get; set; }
        public List<string> spl_patient_package_insert { get; set; }
        public List<string> spl_patient_package_insert_table { get; set; }
        public List<string> instructions_for_use { get; set; }
        public List<string> instructions_for_use_table { get; set; }
        public List<string> package_label_principal_display_panel { get; set; }
        public string set_id { get; set; }
        public string idResult { get; set; }
        public string effective_time { get; set; }
        public string version { get; set; }
        public OpenFda openfda { get; set; }
    }

}
