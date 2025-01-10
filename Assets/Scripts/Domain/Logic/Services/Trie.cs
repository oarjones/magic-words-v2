using System.Collections.Generic;

namespace MagicWords.Domain.Logic.Services
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
        public bool IsEndOfWord { get; set; }
    }

    public class Trie
    {
        private TrieNode root;

        public Trie()
        {
            root = new TrieNode();
        }

        public void Insert(string word)
        {
            TrieNode current = root;
            foreach (char c in word.ToUpper())
            {
                if (!current.Children.ContainsKey(c))
                {
                    current.Children[c] = new TrieNode();
                }
                current = current.Children[c];
            }
            current.IsEndOfWord = true;
        }

        public bool Search(string word)
        {
            TrieNode current = root;
            foreach (char c in word.ToUpper())
            {
                if (!current.Children.ContainsKey(c))
                {
                    return false;
                }
                current = current.Children[c];
            }
            return current.IsEndOfWord;
        }

        public bool StartsWith(string prefix)
        {
            TrieNode current = root;
            foreach (char c in prefix.ToUpper())
            {
                if (!current.Children.ContainsKey(c))
                {
                    return false;
                }
                current = current.Children[c];
            }
            return true;
        }
    }
}