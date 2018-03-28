using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNAEngine.Machine
{
    public class DNAMachine
    {
        private byte[] _DNAData;
        public byte[] DNAData
        {
            get { return _DNAData; }
            set { _DNAData = value; }
        }

        static readonly Dictionary<char, byte> _DNALetters = new Dictionary<char, byte> {
            { 'A', 0},
            { 'T', 1},
            { 'C', 2},
            { 'G', 3}};
        static readonly Dictionary<int, char> _ReverseDNALetters = new Dictionary<int, char> {
            {0, 'A'},
            {1, 'T'},
            {2, 'C'},
            {3, 'G'}};
        static readonly Dictionary<char, byte> _MRNALetters = new Dictionary<char, byte> {
            { 'U', 0},
            { 'A', 1},
            { 'G', 2},
            { 'C', 3}};
        static readonly Dictionary<int, char> _ReverseMRNALetters = new Dictionary<int, char> {
            {0, 'U'},
            {1, 'A'},
            {2, 'G'},
            {3, 'C'}};
        static readonly Dictionary<char, byte> _TRNALetters = new Dictionary<char, byte> {
            { 'A', 0},
            { 'U', 1},
            { 'C', 2},
            { 'G', 3}};
        static readonly Dictionary<int, char> _ReverseTRNALetters = new Dictionary<int, char> {
            {0, 'A'},
            {1, 'U'},
            {2, 'C'},
            {3, 'G'}};

        private static readonly Random random = new Random();

        public DNAMachine()
        {

        }

        //public DNAMachine(string DNAFile)
        //{
        //    if (String.IsNullOrEmpty(DNAFile))
        //    {
        //        string filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        string DNASequenceFile = "DNASequenceGenerated.dna";
        //        WriteDNASequence(filepath + @"\" + DNASequenceFile);
        //    }
        //    else
        //    {
        //        _DNAData = File.ReadAllBytes(DNAFile);
        //    }
        //}
        public void GenerateAndLoadDNASequence(string dnaFile)
        {
            long BasePairs = 3000000000;
            FileStream fileStream = new FileStream(dnaFile, FileMode.Create);

            long chunkSize = (BasePairs / 2) / 4; // 8 million pairs at once (since each byte is 4 nucleotides)

            byte[] chunk = new byte[chunkSize];
            random.NextBytes(chunk);
            fileStream.Write(chunk, 0, chunk.Length);
            
            fileStream.Close();

            _DNAData = File.ReadAllBytes(dnaFile);
        }
        public void LoadDNASequence(string dnaFile)
        {
            _DNAData = File.ReadAllBytes(dnaFile);
        }
        public byte[] StringToDNA(string dnaString, string outputfilelocation)
        {
            List<byte> dnaData = new List<byte>();

            int remainder = dnaString.Length % 4;

            if (remainder == 1)
            {
                byte one = Convert.ToByte(64);// 0100 0000 => 64
                dnaData.Add(one);
            }
            else if (remainder == 2)
            {
                byte two = Convert.ToByte(16);// 0001 0000 => 16
                dnaData.Add(two);
            }
            else if (remainder == 3)
            {
                byte three = Convert.ToByte(4);// 0000 0100 => 4
                dnaData.Add(three);
            }

            byte packed = 0;
            FileStream fileStream = new FileStream(outputfilelocation, FileMode.Create);

            for (int s = 0; s < dnaString.Length; s+=4)
            {
                string toPack = "";
                if ((s + 4) <= dnaString.Length)
                {
                    toPack = dnaString.Substring(s, 4);
                }
                else
                {
                    for (int i = 0; i <= (dnaString.Length - s); i++)
                    {
                        toPack += "A";
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    packed = (byte)(packed | _DNALetters[toPack[i]] << (i * 2));
                }
                dnaData.Add(packed);
            }
            fileStream.Write(dnaData.ToArray(), 0, dnaData.ToArray().Length);
            fileStream.Close();
            return dnaData.ToArray();
            
        }
        public string Read(int begin, int end, Language language)
        {
            string output = "";
            for (int i = begin; i <= end; i++)
            {
                output += UnPack(DNAData[i], language);
            }
            return output;
        }
        private string UnPack(byte pack, Language language)
        {
            string unpacked;
            switch (language)
            {
                case Language.DNA:
                    unpacked = new string(new[] {
                        _ReverseDNALetters[pack & 0b11],
                        _ReverseDNALetters[(pack & 0b1100) >> 2],
                        _ReverseDNALetters[(pack & 0b110000) >> 4],
                        _ReverseDNALetters[(pack & 0b11000000) >> 6],});
                    break;
                case Language.MRNA:
                    unpacked = new string(new[] {
                        _ReverseMRNALetters[pack & 0b11],
                        _ReverseMRNALetters[(pack & 0b1100) >> 2],
                        _ReverseMRNALetters[(pack & 0b110000) >> 4],
                        _ReverseMRNALetters[(pack & 0b11000000) >> 6],});
                    break;
                case Language.TRNA:
                    unpacked = new string(new[] {
                        _ReverseTRNALetters[pack & 0b11],
                        _ReverseTRNALetters[(pack & 0b1100) >> 2],
                        _ReverseTRNALetters[(pack & 0b110000) >> 4],
                        _ReverseTRNALetters[(pack & 0b11000000) >> 6],});
                    break;
                default:
                    unpacked = "No language given";
                    break;
            }
            return unpacked;
            
        }

        public List<string> ReadAminoAcids(int begin, int end)
        {
            string tRNASequence = Read(begin, end, Language.TRNA);
            Dictionary<string, string> codonCode = GetCodonCode();
            List<string> aminoacides = new List<string>();
            string codon = "";
            foreach (char letter in tRNASequence)
            {
                codon += letter;
                if (codon.Length == 3)
                {
                    aminoacides.Add(codonCode[codon]);
                    codon = "";
                }
            }
            return aminoacides;
        }
        public List<List<string>> ReadPeptineBonds(int begin, int end)
        {
            List<string> aminoacids = ReadAminoAcids(begin, end);
            List<List<string>> peptidebonds = new List<List<string>>();
            List<string> peptidebond = new List<string>();
            bool readMode = false;
            foreach (string aa in aminoacids)
            {
                if (isStart(aa))
                {
                    readMode = true;
                }
                else if (isStop(aa))
                {
                    if (readMode)
                    {
                        peptidebond.Add(aa);
                        peptidebonds.Add(peptidebond);
                    }
                    readMode = false;
                }

                if (readMode)
                {
                    peptidebond.Add(aa);
                }
            }
            return peptidebonds;
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

        private bool isStart(string aminoacid)
        {
            if (aminoacid == "Methionine")
                return true;
            else
                return false;
        }
        private bool isStop(string aminoacid)
        {
            if (aminoacid == "Stop")
                return true;
            else
                return false;
        }
    }

}
