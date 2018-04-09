import os
def ReadByteToBits(data, num):
    base = int(num/8)
    shift = num % 8
    return (data[base] & (1<<shift)) >> shift 

class Stream():  
    X = []
    Y = []

def GetFeed():
    import pandas as pd
    df = pd.DataFrame(Stream.X)
    return len(df.columns)

def GetOutputNodes():
    return 1 #Either Black or White    

def ReadSpecimen(filepath, specimen, precisionoffset):
    ChromosomeFiles = []
    for file in os.listdir("{0}/{1}".format(filepath, specimen)):
        if ".dna" in file:
            ChromosomeFiles.append(file)
    for file in os.listdir("{0}/{1}".format(filepath, specimen)):
        if "Result" in file:
        #for i in range(0, len(ChromosomeFiles)):
            if(file.split('.')[1] == "White"):
                Stream.Y.append(1)
            else:
                Stream.Y.append(0)
    Chromosomes = []
    ChromosomeBits = []
    for i in range(1, (len(ChromosomeFiles) + 1)):
        with open("{0}/{1}/Chromosome{2}.dna".format(filepath, specimen, i), "rb") as f:
            byte = f.read(1)           
            while byte != b"":
                #Do stuff with byte.                  
                for i in range(len(byte)*8):
                    ChromosomeBits.append(ReadByteToBits(byte, i))
                    
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
    Stream.X.append(ChromosomeIntegers)
    
def ReadAllSPecimenInFolder(filepath, precisionoffset):
    Stream.X = []
    Stream.Y = []
    Specimens = []
    
    import sys
    if(precisionoffset > sys.getsizeof(int)):
        precisionoffset = sys.getsizeof(int)
        
    for specimen in os.listdir(filepath):
        Specimens.append(specimen)

    for specimen in Specimens:
        ReadSpecimen(filepath, specimen, precisionoffset)
                
def ReadOneSpecimen(filepath, specimen, precisionoffset):
    Stream.X = []
    Stream.Y = []
    
    ReadSpecimen(filepath, specimen, precisionoffset)
    