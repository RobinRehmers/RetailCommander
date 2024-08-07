using RetailCommanderLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderDesktop.Helpers
{
    public class TranslationLabelUpdater : INotifyPropertyChanged
    {
        private readonly ITranslationManager _translationManager;
        private readonly Dictionary<string, string> _labels;

        public TranslationLabelUpdater(ITranslationManager translationManager)
        {
            _translationManager = translationManager;
            _labels = new Dictionary<string, string>();
            _translationManager.TranslationsUpdated += (s, e) => UpdateLabels();
            UpdateLabels();
        }

        public IReadOnlyDictionary<string, string> Labels => _labels;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateLabels()
        {
            _labels["ConfigurationBtn"] = _translationManager.GetTranslation("ConfigurationBtn");
            _labels["LanguageLabel"] = _translationManager.GetTranslation("LanguageLabel");
            OnPropertyChanged(nameof(Labels));
        }
    }
}
