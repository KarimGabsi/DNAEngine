using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNAEngine.Core
{
    public class DNAMachine
    {
        private string _DNAData;
        public string DNAData
        {
            get { return _DNAData; }
        }

        private string _MRNAData;
        public string MRNADATA
        {
            get { return _MRNAData; }
        }

        private string _TRNAData;
        public string TRNADATA
        {
            get { return _TRNAData; }
        }

        private List<string> _AminoAcids;
        public List<string> AminoAcids
        {
            get { return _AminoAcids; }
        }

        string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string DNASequenceFile = "DNASequence.txt";
        private string mRNASequenceFile = "mDNASequence.txt";
        private string TRNASequenceFile = "tDNASequence.txt";

        public DNAMachine()
        {

        }

        public void Start(string DNAFile)
        {
            //Did we receive a DNA File? If not, generate random DNA
            if (String.IsNullOrEmpty(DNAFile))
            {
                WriteDNASequence(DNASequenceFile);
                ReadDNASequence(DNASequenceFile);
                ReadMRNASequence();
                ReadTRNASequence();
                ReadAminoAcids();
            }
            else
            {
                WriteDNASequence(DNAFile);
                ReadDNASequence(DNAFile);
                ReadMRNASequence();
                ReadTRNASequence();
                ReadAminoAcids();
            }
            
        }

        private void WriteDNASequence(string dnaFile)
        {
            Dictionary<int, string> neucleotides = new Dictionary<int, string>();
            neucleotides.Add(0, "A");
            neucleotides.Add(1, "T");
            neucleotides.Add(2, "C");
            neucleotides.Add(3, "G");

            int BasePairs = 300;
            
            using (StreamWriter file = new StreamWriter(filepath + @"\" + dnaFile))
            {
                for (int i = 0; i < (BasePairs/2); i++)
                {
                    int neucleotide = RandomNumber(0, 4);
                    string test = neucleotides[neucleotide];
                    file.Write(neucleotides[neucleotide]);
                }             
            }
        }
        //Function to get a random number 
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        private string ReadDNASequence(string dnaFile)
        {
            _DNAData = "";
            using (StreamReader file = new StreamReader(filepath + @"\" + dnaFile))
            {
                _DNAData = file.ReadToEnd();
            }
            return _DNAData;
        }
        private string ReadMRNASequence()
        {
            if (String.IsNullOrEmpty(_DNAData))
                throw new Exception("No DNA Sequence to read.");

            Dictionary<string, string> mRNACode = new Dictionary<string, string>();
            mRNACode.Add("A", "U");
            mRNACode.Add("T", "A");
            mRNACode.Add("C", "G");
            mRNACode.Add("G", "C");

            _MRNAData = "";
            foreach (char letter in _DNAData)
            {
                _MRNAData += mRNACode[letter.ToString()];
            }

            return _MRNAData;
        }
        private string ReadTRNASequence()
        {
            if (String.IsNullOrEmpty(_MRNAData))
                throw new Exception("No MRNA Sequence to read.");

            Dictionary<string, string> tRNACode = new Dictionary<string, string>();
            tRNACode.Add("A", "U");
            tRNACode.Add("U", "A");
            tRNACode.Add("C", "G");
            tRNACode.Add("G", "C");

            _TRNAData = "";
            foreach (char letter in _MRNAData)
            {
                _TRNAData += tRNACode[letter.ToString()];
            }

            return _TRNAData;
        }

        private List<string> ReadAminoAcids()
        {
            if (String.IsNullOrEmpty(_TRNAData))
                throw new Exception("No TRNA Sequence to read.");

            _AminoAcids = new List<string>();
            string codon = "";
            Dictionary<string, string> codonCode = GetCodonCode();
            foreach (char letter in _TRNAData)
            {
                codon += letter;
                if (codon.Length == 3)
                {
                    _AminoAcids.Add(codonCode[codon]);
                }
            }
            return _AminoAcids;
        }
        private Dictionary<string, string> GetCodonCode()
        {
            Dictionary<string, string> codonCode = new Dictionary<string, string>();

            #region U Starter
            codonCode.Add("UUU", "Phenylalanine");
            codonCode.Add("UUC", "Phenylalanine");
            codonCode.Add("UUA", "Leucine");
            codonCode.Add("UUG", "Leucine");

            codonCode.Add("UCU", "Serine");
            codonCode.Add("UCC", "Serine");
            codonCode.Add("UCA", "Serine");
            codonCode.Add("UCG", "Serine");

            codonCode.Add("UAU", "Tyrosine");
            codonCode.Add("UAC", "Tyrosine");
            codonCode.Add("UAA", "Stop");
            codonCode.Add("UAG", "Stop");

            codonCode.Add("UGU", "Cysteine");
            codonCode.Add("UGC", "Cysteine");
            codonCode.Add("UGA", "Stop");
            codonCode.Add("UGG", "Tryptophan");
            #endregion

            #region C Starter
            codonCode.Add("CUU", "Leucine");
            codonCode.Add("CUC", "Leucine");
            codonCode.Add("CUA", "Leucine");
            codonCode.Add("CUG", "Leucine");

            codonCode.Add("CCU", "Proline");
            codonCode.Add("CCC", "Proline");
            codonCode.Add("CCA", "Proline");
            codonCode.Add("CCG", "Proline");

            codonCode.Add("CAU", "Histidine");
            codonCode.Add("CAC", "Histidine");
            codonCode.Add("CAA", "Glutamine");
            codonCode.Add("CAG", "Glutamine");

            codonCode.Add("CGU", "Arginine");
            codonCode.Add("CGC", "Arginine");
            codonCode.Add("CGA", "Arginine");
            codonCode.Add("CGG", "Arginine");
            #endregion

            #region A Starter
            codonCode.Add("AUU", "Isoleucine");
            codonCode.Add("AUC", "Isoleucine");
            codonCode.Add("AUA", "Isoleucine");
            codonCode.Add("AUG", "Methionine");

            codonCode.Add("ACU", "Threonine");
            codonCode.Add("ACC", "Threonine");
            codonCode.Add("ACA", "Threonine");
            codonCode.Add("ACG", "Threonine");

            codonCode.Add("AAU", "Asparagine");
            codonCode.Add("AAC", "Asparagine");
            codonCode.Add("AAA", "Lysine");
            codonCode.Add("AAG", "Lysine");

            codonCode.Add("AGU", "Serine");
            codonCode.Add("AGC", "Serine");
            codonCode.Add("AGA", "Arginine");
            codonCode.Add("AGG", "Arginine");
            #endregion

            #region G Starter
            codonCode.Add("GUU", "Valine");
            codonCode.Add("GUC", "Valine");
            codonCode.Add("GUA", "Valine");
            codonCode.Add("GUG", "Valine");

            codonCode.Add("GCU", "Alanine");
            codonCode.Add("GCC", "Alanine");
            codonCode.Add("GCA", "Alanine");
            codonCode.Add("GCG", "Alanine");

            codonCode.Add("GAU", "Aspartate");
            codonCode.Add("GAC", "Aspartate");
            codonCode.Add("GAA", "Glutamate");
            codonCode.Add("GAG", "Glutamate");

            codonCode.Add("GGU", "Glycine");
            codonCode.Add("GGC", "Glycine");
            codonCode.Add("GGA", "Glycine");
            codonCode.Add("GGG", "Glycine");
            #endregion

            return codonCode;
        }
    }
}
