using System.Collections.Generic;
using System.IO;
using System.Linq;
using MagicWords.Domain.Data.Models;
using UnityEngine;

namespace MagicWords.Domain.Logic.Services
{
    [CreateAssetMenu(menuName = "Services/Dictionary Service")]
    public class DictionaryService : ScriptableObject
    {
        [SerializeField] private string language = "es-ES"; // Idioma por defecto
        private Dictionary<string, Trie> triesByPrefix = new Dictionary<string, Trie>(); // Trie para búsqueda por prefijo
        private Dictionary<string, Trie> triesByLength = new Dictionary<string, Trie>(); // Trie para búsqueda por rango de letras
        private string currentLoadedPrefix = "";

        public void SetLanguage(string lang)
        {
            language = lang;
        }

        public void LoadDictionaryByPrefix(char prefix)
        {
            string prefixStr = prefix.ToString().ToLower();
            if (!triesByPrefix.ContainsKey(prefixStr))
            {
                TextAsset dictionaryAsset = Resources.Load<TextAsset>($"dictionary/{language}/{prefixStr}");
                if (dictionaryAsset != null)
                {
                    Trie trie = new Trie();
                    string[] words = dictionaryAsset.text.Split(' ');
                    foreach (string word in words)
                    {
                        trie.Insert(word.Trim());
                    }
                    triesByPrefix[prefixStr] = trie;
                }
                else
                {
                    Debug.LogError($"Dictionary file not found for prefix: {prefixStr} in language: {language}");
                }
            }
            currentLoadedPrefix = prefixStr;
        }

        public void LoadDictionaryByLengthRange(int minLength, int maxLength)
        {
            string rangeKey = $"{minLength}_{maxLength}";
            if (!triesByLength.ContainsKey(rangeKey))
            {
                TextAsset dictionaryAsset = Resources.Load<TextAsset>($"dictionary/{language}/search_dict_{minLength}_{maxLength}");
                if (dictionaryAsset != null)
                {
                    Trie trie = new Trie();
                    string[] words = dictionaryAsset.text.Split(' ');
                    foreach (string word in words)
                    {
                        trie.Insert(word.Trim());
                    }
                    triesByLength[rangeKey] = trie;
                }
                else
                {
                    Debug.LogError($"Dictionary file not found for length range: {minLength}-{maxLength} in language: {language}");
                }
            }
        }

        public bool IsValidWord(string word)
        {
            // Comprobar primero en el Trie de prefijos
            if (!string.IsNullOrEmpty(currentLoadedPrefix) && triesByPrefix.ContainsKey(currentLoadedPrefix))
            {
                if (triesByPrefix[currentLoadedPrefix].Search(word))
                {
                    return true;
                }
            }

            // Si no se encuentra en el Trie de prefijos, buscar en los Tries de rangos de longitud
            foreach (var trie in triesByLength.Values)
            {
                if (trie.Search(word))
                {
                    return true;
                }
            }

            // Si no se encuentra en ningún Trie, se valida online (implementar en el futuro)
            // ...

            return false;
        }

        public bool IsValidPrefix(string prefix)
        {
            // Comprobar primero en el Trie de prefijos
            if (!string.IsNullOrEmpty(currentLoadedPrefix) && triesByPrefix.ContainsKey(currentLoadedPrefix))
            {
                if (triesByPrefix[currentLoadedPrefix].StartsWith(prefix))
                {
                    return true;
                }
            }
            // Si no se encuentra en el Trie de prefijos, buscar en los Tries de rangos de longitud
            foreach (var trie in triesByLength.Values)
            {
                if (trie.StartsWith(prefix))
                {
                    return true;
                }
            }
            return false;
        }
    }
}