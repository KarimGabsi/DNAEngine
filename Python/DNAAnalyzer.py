import keras
from keras.models import Sequential
from keras.layers import Dense

def dash():
    dash = []
    dash.append("[")
    for i in range(0, 20):
        dash.append("=")
    dash.append("]")
    return str(dash)

def find_between(s, first, last):
    try:
        start = s.index( first ) + len( first )
        end = s.index( last, start )
        return s[start:end]
    except ValueError:
        return ""
def main():
    
    displayresult = []
    displayresult.append(dash())
    
    filepath = "./DNAs"  
    import DNA_Reader
    dna_reader = DNA_Reader.DNA_Reader()
    
    #precision offset must be between 2 and 64-bit, 32 if on 32-bitsystem
    precisionoffset = 8
    #Read DNA Folder
    dna_reader.ReadAllSPecimenInFolder(filepath, precisionoffset)
    
    # X (DNA)
    X = dna_reader.GetX()
    # Y (results)
    Y = dna_reader.GetY()
    
    #Transform to numpy array
    import numpy as np
    X = np.array(X)
    Y = np.array(Y)
    
    #Feed(input): number of bits (bytes*8)
    feed = dna_reader.GetFeed()
    #Output: Either Black(0) or White(1), 1 output
    output = dna_reader.GetOutputNodes()
    
    displayresult.append("DNA READER with precision offset: {}".format(precisionoffset))
    displayresult.append("X (DNA) Shape: {}".format(X.shape))
    displayresult.append("Y (Result) Shape: {}".format(X.shape))
    displayresult.append("Feed: {}".format(feed))
    displayresult.append("Output: {}".format(output))
    displayresult.append(dash())
    
    displayresult.append(dash())
    displayresult.append("BEST PARAMETER GENERATION")
    #njobs (processes) set to 1 if on windows (slower), set to n=number of processes on linux
    njobs = -1 #if linux ex. 4 (4 processes))
      
    #Set parameters to test on....
    parameters = {'batch_size': [10, 20],
                  'epochs': [10, 50, 100],
                  'optimizer': ['adam', 'rmsprop'],}
    displayresult.append("Parameters to test: {}".format(parameters))
    #Generate best parameters (WARNING: takes a while...)
    bestparameters = GenerateBestParameterSet(X, Y, feed, output, parameters, njobs)
    displayresult.append("Parameters Result: {}".format(bestparameters))
    displayresult.append(dash())
    
    displayresult.append(dash())
    displayresult.append("GENERATE ANN USING BEST PARAMETERS")
    
    #Use best ANN
    bestOptimizer = find_between(bestparameters[0], "optimizer", "}")
    bestOptimizer = find_between(bestOptimizer, "': '", "'")
    optimizer = bestOptimizer
    displayresult.append("Best Optimizer: {}".format(optimizer))
    #Set batchsize
    bestbatchsize = find_between(bestparameters[0], "batch_size\':", ",")
    batchsize = int(bestbatchsize)
    displayresult.append("Best Batchsize: {}".format(batchsize))
    #Set epochs
    bestepochs = find_between(bestparameters[0], "epochs\':", ",")
    epochs = int(bestepochs)
    displayresult.append("Best Epochs: {}".format(epochs))
    
    classifier = CreateANN(X,Y,feed, output, optimizer, batchsize, epochs) 
    #displayresult.append("Classifier: {}".format(classifier.get_config()))
    
    evaluation = EvaluateANN(X, Y, feed, output, optimizer, batchsize, epochs)
    displayresult.append("Evaluation: {}".format(evaluation))
    displayresult.append(dash())
    
    displayresult.append(dash())
    displayresult.append("PREDICTING ONE SPECIMEN")
    #count = 0
    #for layer in classifier.layers: 
        #print (layer.get_weights())
        #count +=1
    #print ("Weights: {}".format(count))
    
    import DNA_Reader
    dna_reader = DNA_Reader.DNA_Reader()
    precisionoffset = 8
    #Read one specimen and test if is white or black
    filepath = "./DNAs"
    specimen = "Mouse10"
    dna_reader.ReadOneSpecimen(filepath, specimen, precisionoffset)
    # X (chromosomes)
    X = dna_reader.GetX()
    # Y (results)
    Y = dna_reader.GetY()
    
    #Transform to numpy array
    import numpy as np
    X = np.array(X)
    Y = np.array(Y)
    
    displayresult.append("DNA READER with precision offset: {}".format(precisionoffset))
    displayresult.append("ONE SPECIMEN")
    displayresult.append("Filepath : {}".format(filepath))
    displayresult.append("Specimen: {}".format(specimen))
    displayresult.append("X (DNA) Shape: {}".format(X.shape))
    displayresult.append("Y (Result) Shape: {}".format(X.shape))
    displayresult.append("Feed: {}".format(feed))
    displayresult.append("Output: {}".format(output))
    
    new_prediction = classifier.predict(X)
    outcome = (new_prediction >= 0.5)
    if(outcome):
        displayresult.append("Mouse prediction is White ({})".format(new_prediction))
    else:
        displayresult.append("Mouse prediction is Black ({})".format(new_prediction))
    
    displayresult.append(dash())
    print(np.array(displayresult))

def GenerateBestParameterSet(X, Y, feed, output, parameters, njobs):
    import DNA_ANN
    dna_ann = DNA_ANN.DNA_ANN(feed, output, None)
    bestparameters = dna_ann.GenerateBestParameterSet(X, Y, parameters, njobs)
    #Generate best ANN using the parameters
    return bestparameters

def CreateANN(X, Y, feed, output, optimizer, batchsize, epochs):  
    import DNA_ANN
    dna_ann = DNA_ANN.DNA_ANN(feed, output, optimizer)
    return dna_ann.CreateANN(X, Y, batchsize, epochs)

def EvaluateANN(X,Y,feed, output, optimizer, batchsize, epochs):
    import DNA_ANN
    dna_ann = DNA_ANN.DNA_ANN(feed, output, optimizer)
    return dna_ann.EvaluateANN(X, Y, batchsize, epochs) 
    
if __name__ == "__main__":
    #__spec__ = "ModuleSpec(name='builtins', loader=<class '_frozen_importlib.BuiltinImporter'>)"
    main()       
