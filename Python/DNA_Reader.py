import os

class DNA_Reader(): 
    def __init__(self):
        self.X = []
        self.Y = []

    def GetX(self): return self.X
    def GetY(self): return self.Y
    
    def GetFeed(self):
        import pandas as pd
        df = pd.DataFrame(self.X)
        return len(df.columns)

    def GetOutputNodes(self): return 1 #Either Black or White    
    
    def ReadByteToBits(self, data, num):
        base = int(num/8)
        shift = num % 8
        return (data[base] & (1<<shift)) >> shift 
    
    def ReadSpecimen(self, filepath, specimen, precisionoffset):
        #precisionoffset must be between 2 and intsize * 8 bits
        import sys
        maxsizeBits = "{0:b}".format(sys.maxsize)
        maxsize = len(maxsizeBits) + 1
        if(precisionoffset > maxsize):
            precisionoffset = maxsize
        if(precisionoffset < 2):
            precisionoffset = 2
            
        ChromosomeFiles = []
        for file in os.listdir("{0}/{1}".format(filepath, specimen)):
            if ".dna" in file:
                ChromosomeFiles.append(file)
        for file in os.listdir("{0}/{1}".format(filepath, specimen)):
            if "Result" in file:
            #for i in range(0, len(ChromosomeFiles)):
                if(file.split('.')[1] == "White"):
                    self.Y.append(1)
                else:
                    self.Y.append(0)
        Chromosomes = []
        ChromosomeBits = []
        for i in range(1, (len(ChromosomeFiles) + 1)):
            with open("{0}/{1}/Chromosome{2}.dna".format(filepath, specimen, i), "rb") as f:
                byte = f.read(1)           
                while byte != b"":
                    #Do stuff with byte.                  
                    for i in range(len(byte)*8):
                        ChromosomeBits.append(self.ReadByteToBits(byte, i))
                        
                    byte = f.read(1)
                    Chromosomes.append(ChromosomeBits)
        ChromosomeIntegers = []
    
        for i in range(0, len(ChromosomeBits), precisionoffset):
            bitarray = []
            if (i + precisionoffset) < len(ChromosomeBits):
                for k in range(0, precisionoffset):
                    bitarray.append(ChromosomeBits[i+k])
            else:
                remaining = len(ChromosomeBits) - i
                for k in range (0, remaining):
                    bitarray.append(ChromosomeBits[i+k])
            bitstring = ""
            for bit in bitarray:
                bitstring += str(bit)
                
            value = int(bitstring,2)
            ChromosomeIntegers.append(value)
        self.X.append(ChromosomeIntegers)
    
    def ReadAllSPecimenInFolder(self, filepath, precisionoffset):
        self = self.ClearXY()
        Specimens = []
            
        for specimen in os.listdir(filepath):
            Specimens.append(specimen)
    
        for specimen in Specimens:
            self.ReadSpecimen(filepath, specimen, precisionoffset)
                
    def ReadOneSpecimen(self,filepath, specimen, precisionoffset):
        self = self.ClearXY()       
        self.ReadSpecimen(filepath, specimen, precisionoffset)
    
    def ClearXY(self):
        self.X = []
        self.Y = []
        return self
    