using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailCommanderLibrary.Data
{
    public interface ITranslationManager
    {
        void LoadTranslations(string language);
        void SaveTranslation(string controlName, string language, string text);
        string GetTranslation(string controlName);
        event PropertyChangedEventHandler TranslationsUpdated;
    }
}
