import keras
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import Dropout

class StandardParameters:
    Feed = 0
    Output = 0
    Optimizer = ''
    LayerCount = 0
    Dropout = 0.0
    
def SetANNParameters(feed, output, optimizer, layercount, dropout):
    #Set standard Parameters
    StandardParameters.Feed = feed
    StandardParameters.Output = output
    StandardParameters.Optimizer = optimizer
    StandardParameters.LayerCount = layercount
    StandardParameters.Dropout = dropout
#Libraries
def BuildANN(X,Y):  
    
    #Splitting the dataset into Training set and test set
    from sklearn.model_selection import train_test_split
    X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size = 0.5, random_state = 0)
     
    #Initialize ANN
    classifier = BuildStandardClassifier()  
    #Fitting the ANN to the training set
    classifier.fit(X_train, Y_train, batch_size = 10, epochs = 100)
    
    #Predicting the Test set results
    y_pred = classifier.predict(X_test)
    y_pred = (y_pred > 0.5)
    
    #Making the confusion matrix
    from sklearn.metrics import confusion_matrix
    con = confusion_matrix(Y_test, y_pred)
    print("Con: {0}".format(con))
    
    #Evaluating the ANN
    from keras.wrappers.scikit_learn import KerasClassifier
    from sklearn.model_selection import cross_val_score
    
    classifier = KerasClassifier(build_fn = BuildStandardClassifier, batch_size = 10, epochs = 100)
    accuracies = cross_val_score(estimator = classifier, X = X_train, y = Y_train, cv = 10)
    
    mean = accuracies.mean()
    variance = accuracies.std()
    
    print ("Mean Accuracy: {0}".format(mean))
    print ("Variance: {0}".format(variance))
            
    #Tuning 
    from sklearn.model_selection import GridSearchCV
    classifier = KerasClassifier(build_fn = BuildBestStandardClassifier)
    parameters = {'batch_size': [4, 5, 10, 18, 24, 30, 32, 35],
                  'epochs': [10, 50, 100, 200, 500 ],
                  'optimizer': ['adam', 'rmsprop'],
                  'layercount' : [3,4,5],
                  'dropout': [0.1, 0.2, 0.3, 0.4]}
    grid_search = GridSearchCV(estimator = classifier,
                               param_grid = parameters,
                               scoring = 'accuracy',
                               cv = 10)
    grid_search = grid_search.fit(X_train, Y_train)
    best_parameters = grid_search.best_params_
    best_accuracy = grid_search.best_score_
    
    print ("Best Parameters: {0}".format(best_parameters))
    print ("Best Accuracy: {0}".format(best_accuracy))

    # -*- coding: utf-8 -*-

def BuildStandardClassifier():
    return BuildClassifier(StandardParameters.Feed, 
                           StandardParameters.Output, 
                           StandardParameters.Optimizer, 
                           StandardParameters.LayerCount,
                           StandardParameters.Dropout)

def BuildBestStandardClassifier(optimizer, layercount, dropout):
    return BuildClassifier(StandardParameters.Feed, 
                           StandardParameters.Output, 
                           optimizer, 
                           layercount, 
                           dropout)

def BuildClassifier(feed, output, optimizer, layercount, dropout):
    #Initialize ANN   
    classifier = Sequential() 
    #Adding the input layer and first hidden layer   
    classifier.add(Dense(units = (feed*(layercount + 1)), kernel_initializer = 'uniform', activation = 'relu', input_dim = feed))  
    classifier.add(Dropout(rate = dropout)) #Disable % of nodes
    
    for i in range(layercount, 1, -1):
        #Add hidden layer    
        classifier.add(Dense(units = (feed*i), kernel_initializer = 'uniform', activation = 'relu')) 
        classifier.add(Dropout(rate = dropout))

    #Adding the output layer
    classifier.add(Dense(units = output, kernel_initializer = 'uniform', activation = 'sigmoid'))
    #Compile ANN
    classifier.compile(optimizer = optimizer, loss = 'binary_crossentropy', metrics = ['accuracy']) 
    return classifier