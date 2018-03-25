﻿using DNAEngine.GUI;
using DNAEngine.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNAEngine.Core
{
    class Core
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("DNA Engine");
            DNAMachine DNAMachine = new DNAMachine();
            DNAMachine.Start("");

            Console.WriteLine("DNA Data:");
            Console.WriteLine(DNAMachine.DNAData);

            Console.WriteLine("MRNA Data:");
            Console.WriteLine(DNAMachine.MRNADATA);

            Console.WriteLine("TRNA Data:");
            Console.WriteLine(DNAMachine.TRNADATA);

            Console.WriteLine("Amino Acids:");
            Console.WriteLine(string.Join(" - ", DNAMachine.AminoAcids));

            var app = new App();
            app.Run(new DNAVisual(DNAMachine));
        }
    }
}
