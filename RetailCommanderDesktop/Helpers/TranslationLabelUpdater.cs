using RetailCommanderLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderDesktop.Helpers
{
    public class TranslationLabelUpdater
    {
        private readonly ITranslationManager _translationManager;
        private readonly Dictionary<string, string> _labels;

        public TranslationLabelUpdater(ITranslationManager translationManager)
        {
            _translationManager = translationManager;
            _labels = new Dictionary<string, string>();
            _translationManager.TranslationsUpdated += (s, e) => UpdateLabels();
        }

        public IReadOnlyDictionary<string, string> Labels => _labels;

        public void UpdateLabels()
        {
            _labels["ConfigurationBtn"] = _translationManager.GetTranslation("ConfigurationBtn");
            _labels["OtherControl"] = _translationManager.GetTranslation("OtherControl");
        }
    }
}
