import keras
from keras.models import Sequential
from keras.layers import Dense

def main():
    
    filepath = "./DNAs"  
    import DNA_Reader as dna_reader
    
    #Read DNA Folder
    dna_reader.ReadAllInFolder(filepath)
    
    # X (chromosomes)
    X = dna_reader.Stream.X
    # Y (results)
    Y = dna_reader.Stream.Y
    
    #Transform to numpy array
    import numpy as np
    X = np.array(X)
    Y = np.array(Y)
       
    #Feed(input): number of bits (bytes*8)
    feed = dna_reader.GetFeed()
    #Output: Either Black(0) or White(1), 1 output
    output = dna_reader.GetOutputNodes()
    
    #njobs (processes) set to 1 if on windows (slower), set to n=number of processes on linux
    njobs = 1 #if linux ex. 4 (4 processes))
    
    #Set parameters to test on....
    parameters = {'batch_size': [4, 5, 10],
                  'epochs': [10, 50],
                  'optimizer': ['adam', 'rmsprop'],
                  'layercount' : [3,4],
                  'dropout': [0.1, 0.2]}
    #Generate best parameters (WARNING: takes a while...)
    #GenerateBestParameterSet(X, Y, feed, output, parameters, njobs)
    #Best Parameters: {'batch_size': 4, 'dropout': 0.2, 'epochs': 50, 'layercount': 4, 'optimizer': 'rmsprop'}
    #Best Accuracy: 0.5
    
    #Use best ANN
    #Set optimizer to rmsprop
    optimizer = 'rmsprop'
    #Set n hidden layers
    layercount = 4
    #Set dropout rate per layer
    dropout = 0.2
    #Set batchsize
    batchsize = 4
    #Set epochs
    epochs = 50
    
    classifier = CreateANN(X,Y,feed, output, optimizer, layercount, dropout, batchsize, epochs)   
    
    #EvaluateANN(X, Y, feed, output, optimizer, layercount, dropout, batchsize, epochs, njobs)
    count = 0
    for layer in classifier.layers: 
        print (layer.get_weights())
        count +=1
    print ("Weights: {}".format(count))
    
    #Read one specimen and test if is white or black
    filepath = "./DNAs/Mouse2"  
    dna_reader.ReadOneSpecimen(filepath)
    # X (chromosomes)
    X = dna_reader.Stream.X
    # Y (results)
    Y = dna_reader.Stream.Y
    
    #Transform to numpy array
    import numpy as np
    X = np.array(X)
    Y = np.array(Y)
    
    new_prediction = classifier.predict(X)
    outcome = (new_prediction > 0.5)
    if(outcome):
        print("Mouse prediction is White ({})".format(new_prediction))
    else:
        print("Mouse prediction is Black ({})".format(new_prediction))
    
    

def GenerateBestParameterSet(X, Y, feed, output, parameters, njobs):
    import DNA_ANN as dna_ann  
    #Set Standard Parameters
    dna_ann.SetANNParameters(feed, output, None, None, None)
    #Generate best ANN using the parameters
    dna_ann.GenerateBestANN(X,Y, parameters, njobs)

def CreateANN(X, Y, feed, output, optimizer, layercount, dropout, batchsize, epochs):  
    import DNA_ANN as dna_ann  
    dna_ann.SetANNParameters(feed, output, optimizer, layercount, dropout)
    classifier = dna_ann.CreateANN(X, Y, batchsize, epochs)
    return classifier

def EvaluateANN(X,Y,feed, output, optimizer, layercount, dropout, batchsize, epochs, njobs):
    import DNA_ANN as dna_ann
    dna_ann.SetANNParameters(feed, output, optimizer, layercount, dropout)
    dna_ann.EvaluateANN(X, Y, batchsize, epochs, njobs) 
    
if __name__ == "__main__":
    __spec__ = "ModuleSpec(name='builtins', loader=<class '_frozen_importlib.BuiltinImporter'>)"
    main()       
