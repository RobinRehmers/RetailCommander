using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderLibrary.Data
{
    public class TranslationManager : ITranslationManager, INotifyPropertyChanged
    {
        private readonly SqliteData _sqliteData;
        private Dictionary<string, string> _translations;

        public event PropertyChangedEventHandler TranslationsUpdated;

        public TranslationManager(SqliteData sqliteData)
        {
            _sqliteData = sqliteData;
            _translations = new Dictionary<string, string>();
        }

        public void LoadTranslations(string language)
        {
            _translations = _sqliteData.LoadTranslations(language);
            OnPropertyChanged(nameof(_translations));
            TranslationsUpdated?.Invoke(this, new PropertyChangedEventArgs(nameof(_translations)));
        }

        public void SaveTranslation(string controlName, string language, string text)
        {
            _sqliteData.SaveTranslation(controlName, language, text);
        }

        public string GetTranslation(string controlName)
        {
            return _translations.ContainsKey(controlName) ? _translations[controlName] : controlName;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
