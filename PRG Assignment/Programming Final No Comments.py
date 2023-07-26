#Imports

import random
import sys
from typing import final

#Globals

buildingList = ['BCH','FAC','HSE','SHP','HWY']

buildingAmount = {}

map = []

buildings = []

turns = 1

scoreDict = {}

totalScore = 0

building1 = ''

building2 = ''

newBuildings = []

#Functions

def createMap(map): #Creates map layout
    print('{:>40}{:>15}'.format('Buildings','Remaining')) 
    print('     A     B     C     D {:>15}{:>15}'.format('---------','---------'))
    print('  +-----+-----+-----+-----+ {:>9}{:>14}'.format(buildingList[0],buildingAmount[buildingList[0]])) #Prints first building and how many remaining.
    for row in range(4):  #For every row
        print(' {}'.format(row + 1), end = '') #Prints numbers
        for column in range(4): #For every column
            print('| {:3s} '.format(map[row][column]), end = '') #Adds map values into the grid.
        print('|') #Closes off the grid for every column
        print('  ', end = '') #Offsets each +----- with spaces for the map grid.
        for column in range(1, 5):
            print('+-----', end = '') #Prints +----- for every column.
        print('+ {:>9}{:>14}'.format(buildingList[row+1],buildingAmount[buildingList[row+1]])) #Closes off every +----- with +. Prints the rest of the buildings and how many remaining.

def newgame(): #Resets values for new game
    #So that variables can be accessed from in the function
    global turns
    global map
    global buildings
    global buildingAmount

    #Resets map values, everything is empty
    map = [ [' ', ' ', ' ', ' '],\
        [' ', ' ', ' ', ' '],\
        [' ', ' ', ' ', ' '],\
        [' ', ' ', ' ', ' ',]
      ]
     
    turns = 1 #Resets turn count

    buildings = buildingList*8 #Resets building count
    
    for building in buildingList: #Resets building count in the buildingAmount dictionary
        buildingAmount[building] = 8

    
    return buildings, buildingAmount, turns

def Game(buildings, buildingAmount, turns, building1, building2, newBuildings): #Function for game
    columns = 'abcd'
    turn16 = False
    validPos = True
    posList = ['a1','a2','a3','a4','b1','b2','b3','b4','c1','c2','c3','c4','d1','d2','d3','d4']

    #Makes it so that getting random buildings only happens at the start of the game, does not loop when player selects an option
    if turns == 1:
        newBuildings = []
        num1 = random.randint(0, len(buildings) - 1)
        num2 = random.randint(0, len(buildings) - 1)
        while num1 == num2:
            num2 = random.randint(0, len(buildings) - 1) #To prevent choosing the same building index. Prevents situations like 2 BCH appearing when there is only 1 left.
                    
        building1 = buildings[num1]
        building2 = buildings[num2]
        newBuildings.append(building1)
        newBuildings.append(building2)

    while turn16 == False: #Loops the function when turns are not over 16
        print('Turn {}'.format(turns))
        createMap(map)
        while True:
            try:  #- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -EXCEPTION 1: INVALID CHOICE OPTION
                print('1. Build a {}\n2. Build a {}'.format(building1, building2))
                    
                print('3. Show current score\n\n4. Save game\n0. Exit to main menu')
                choice = int(input('Your choice? '))
                
                if 5 > choice > -1:
                    validChoice = True
                assert validChoice #Checks if choice input is within this range, if not it creates an error
                break
            except:  #- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -EXCEPTION 1 CLOSED
                print('Invalid choice, try again.') #Will loop the function again, because turn16 is still False
                print('Turn {}'.format(turns))
                createMap(map)

        while choice == 0: #Back to main menu
            mainMenu(buildings, buildingAmount, turns, building1, building2, newBuildings) 
                    
        if choice == 3: #Shows and calculates scores
            scoreCalculation(turns) 
              
        elif choice == 4: #Saves game into a file
                
            print('\nGame saved!\n')
            with open('data.txt', 'w') as f:

                f.write('{}\n'.format(turns)) #Writes turns

                for i in range(len(map)): #For every row of map
                    s = ''
                    for j in range(len(map[i])): #For every column of map
                        s += map[i][j] + ',' #Values in positions of the map will be s, seperates each value with comma
                    f.write(s[:-1] + '\n') #Writes in the map layout line by line, removes the comma at the end

                for i in buildings: 
                    f.write('{} '.format(i)) #Writes every building in buildings list
                f.write('\n')

                for i in buildingAmount:
                    f.write('{} {}\n'.format(i, buildingAmount[i])) #Writes all keys and values in buildingAmount dictionary                                            

                f.write('{} {}'.format(building1, building2))


        elif choice == 1 or choice == 2: #Asks user for position 
            try:# - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -EXCEPTION: NON-ADJACENT POSITIONS AND INVALAID OPTION
                build = input('Build where? ')                                                                         
                build = build.lower()                                                                                  
                col = columns.find(build[0])                                                                           
                row = int(build[1]) - 1                                                                                
                assert build in posList #Exception handling for valid position in the map
                                                                                              
                if turns == 1: #Turn 1 building can be placed anywhere
                    validPos = True
                elif 17 > turns > 1: #Buildings after turn 1 follow adjacent rule
                    validPos = False

                    if row-1 != -1 and map[row-1][col] != ' '\
                        or row+1 != 4 and map[row+1][col] != ' '\
                            or col-1 != -1 and map[row][col-1] != ' '\
                                or col+1 != 4 and map[row][col+1] != ' ': 
                                    validPos = True

                    ''' Adjacent code:
                     First line: Same column, row above
                     Second line: Same column, row below
                     Third line: Same row, column above
                     Fourth line: Same row, column below

                     "row/col-1 != -1" and "row/col + 1 != 4" are used to check if they are the first or last rows/columns,
                     so it will run the other lines in the adjacent code instead.'''


                    if map[row][col] != ' ': #Same position
                        validPos = False
                        
                assert validPos  #Exception handling for valid adjacent position

                turns+=1 #After a valid input, turns + 1
                map[row][col] = newBuildings[choice - 1] #Puts selected building into map
                buildings.remove(building1) #Removes building from buildings list for both options
                buildings.remove(building2) 
                buildingAmount[building1] = buildingAmount[building1] - 1 #Removes building from buildingAmount dictionary for both options
                buildingAmount[building2] = buildingAmount[building2] - 1

                #Resets building choices after valid position
                newBuildings = []
                num1 = random.randint(0, len(buildings) - 1)
                num2 = random.randint(0, len(buildings) - 1)
                while num1 == num2:
                    num2 = random.randint(0, len(buildings) - 1)
                    
                building1 = buildings[num1]
                building2 = buildings[num2]
                newBuildings.append(building1)
                newBuildings.append(building2)
            except:#- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -EXCEPTION 2 CLOSED
                print('\nInvalid option. Check if:\nYou made an error in your typing.\nYour placement was not adjacent to another building.\n')
            
            if turns > 16: #Ends the game, shows final map and score, brings player back to main menu
                turn16 = True
                print('Final layout of Simp City:\n')
                createMap(map)
                scoreCalculation(turns)
                mainMenu(buildings, buildingAmount, turns, building1, building2, newBuildings)
  
    return buildings, buildingAmount, turns, building1, building2, newBuildings

def scoreCalculation(turns): #Calculates the score for each building
    #Beach Calculation
    global totalScore
    print()
    beachTotal = 0
    beachString = ''
    for row in range(len(map)):
        for col in range(len(map[row])):
            if "BCH" in map[row][col]:
                if col == 0 or col == 3: #If beach is in column A or D
                    BCH = 3
                    beachTotal+=BCH
                else:
                    BCH = 1
                    beachTotal+=BCH
                beachString = beachString + ' + {}'.format(BCH)
                      
    beachString = 'BCH: ' + beachString[3:] + ' = {}'.format(beachTotal)
    print(beachString)
    
    #Factory Calculation
    factoryTotal = 0
    factoryString = ''
    factoryList = []
    FAC = 0
    
    for row in range(len(map)):
        for col in range(len(map[row])):
            if "FAC" in map[row][col]:
                factoryList.append('Factory')
                FAC = len(factoryList)
                if len(factoryList) <= 4:
                    factoryTotal = FAC*FAC
                    factoryString = ' + {}'.format(FAC)*len(factoryList)           
                else:
                    FAC = 1
                    other = len(factoryList)-4 #After 4 factories
                    factoryTotal = 16 + 1*other
                    factoryString = '   4 + 4 + 4 + 4'
                    factoryString = factoryString + ' + {}'.format(FAC)*other


    factoryString = 'FAC: ' + factoryString[3:] + ' = {}'.format(factoryTotal)
    print(factoryString)

    #House Calculation
    houseTotal = 0
    houseString = ''
    HSE = 0

    for row in range(len(map)):
        for col in range(len(map[row])):
            if "HSE" in map[row][col]: #Using adjacent code, if there is a factory around it, HSE = 1
                if row-1 != -1 and "FAC" in map[row-1][col]\
                    or row+1 != 4 and "FAC" in map[row+1][col]\
                        or col-1 != -1 and "FAC" in map[row][col-1]\
                            or col+1 != 4 and "FAC" in map[row][col+1]:
                            HSE = 1

                else: #Using adjacent code, checks for houses, shops, and beaches around it. 
                    #Does not use elif/or as it needs to check for each position regardless of whether there are other adjacent buildings.
                    if row-1 != -1 and "HSE" in map[row-1][col]:
                        HSE+=1
                    if row+1 != 4 and "HSE" in map[row+1][col]:
                        HSE+=1
                    if col-1 != -1 and "HSE" in map[row][col-1]:
                        HSE+=1
                    if col+1 != 4 and "HSE" in map[row][col+1]:
                        HSE+=1

                    if row-1 != -1 and "SHP" in map[row-1][col]:
                        HSE+=1
                    if row+1 != 4 and "SHP" in map[row+1][col]:
                        HSE+=1
                    if col-1 != -1 and "SHP" in map[row][col-1]:
                        HSE+=1
                    if col+1 != 4 and "SHP" in map[row][col+1]:
                        HSE+=1

                    if row-1 != -1 and "BCH" in map[row-1][col]:
                        HSE+=2
                    if row+1 != 4 and "BCH" in map[row+1][col]:
                        HSE+=2
                    if col-1 != -1 and "BCH" in map[row][col-1]:
                        HSE+=2
                    if col+1 != 4 and "BCH" in map[row][col+1]:
                        HSE+=2

                houseTotal+=HSE
                houseString = houseString + ' + {}'.format(HSE)
                HSE = 0
                
    houseString = 'HSE: ' + houseString[3:] + ' = {}'.format(houseTotal)
    print(houseString)


    #Shop Calculation
    shopTotal = 0
    shopString = ''
    SHP = 0

    for row in range(len(map)):
        for col in range(len(map[row])):
            if "SHP" in map[row][col]: #Checks for each different adjacent building around it.
                #This can use elif/or as each type of building only adds 1 point, so it only needs to check for a type of adjacent building once.
                if row-1 != -1 and "SHP" in map[row-1][col]\
                    or row+1 != 4 and "SHP" in map[row+1][col]\
                        or col-1 != -1 and "SHP" in map[row][col-1]\
                            or col+1 != 4 and "SHP" in map[row][col+1]:
                            SHP+=1

                if row-1 != -1 and "FAC" in map[row-1][col]\
                    or row+1 != 4 and "FAC" in map[row+1][col]\
                        or col-1 != -1 and "FAC" in map[row][col-1]\
                            or col+1 != 4 and "FAC" in map[row][col+1]:
                            SHP+=1

                if row-1 != -1 and "BCH" in map[row-1][col]\
                    or row+1 != 4 and "BCH" in map[row+1][col]\
                        or col-1 != -1 and "BCH" in map[row][col-1]\
                            or col+1 != 4 and "BCH" in map[row][col+1]:
                            SHP+=1

                if row-1 != -1 and "HSE" in map[row-1][col]\
                    or row+1 != 4 and "HSE" in map[row+1][col]\
                        or col-1 != -1 and "HSE" in map[row][col-1]\
                            or col+1 != 4 and "HSE" in map[row][col+1]:
                            SHP+=1

                if row-1 != -1 and "HWY" in map[row-1][col]\
                    or row+1 != 4 and "HWY" in map[row+1][col]\
                        or col-1 != -1 and "HWY" in map[row][col-1]\
                            or col+1 != 4 and "HWY" in map[row][col+1]:
                            SHP+=1

                shopTotal+=SHP
                shopString = shopString + ' + {}'.format(SHP)

                SHP = 0

    shopString = 'SHP: ' + shopString[3:] + ' = {}'.format(shopTotal)
    print(shopString)

    #Highway calculation
    highwayTotal = 0
    highwayString = ''
    
    HWY = 0

    for row in range(len(map)):
        for col in range(len(map[row])):
            if "HWY" in map[row][col]: #Checks if there is another highway to the left or right of an existing highway.
                #Runs an elif statement for the main if statements. Checks to the left or right of the highway, whichever is available.
                HWY = 1
                if col-1 != -1 and "HWY" in map[row][col-1]: #Main if statement 1
                    HWY+=1

                    if col-2 > -1 and "HWY" in map[row][col-2]: #Main if statement 2
                        HWY+=1

                        if col-3 > -1 and "HWY" in map[row][col-3]: #Main if statement 3
                            HWY+=1

                        elif col+1 != 4 and "HWY" in map[row][col+1]:
                            HWY+=1
                        
                            if col+2 < 4 and "HWY" in map[row][col+2]:
                                HWY+=1

                                if col+3 < 4 and "HWY" in map[row][col+3]:
                                    HWY+1

                    elif col+1 != 4 and "HWY" in map[row][col+1]:
                        HWY+=1
                        
                        if col+2 < 4 and "HWY" in map[row][col+2]:
                            HWY+=1

                            if col+3 < 4 and "HWY" in map[row][col+3]:
                                HWY+1

                elif col+1 != 4 and "HWY" in map[row][col+1]:
                        HWY+=1
                        
                        if col+2 < 4 and "HWY" in map[row][col+2]:
                            HWY+=1

                            if col+3 < 4 and "HWY" in map[row][col+3]:
                                HWY+=1

                highwayString = highwayString + ' + {}'.format(HWY) 
                highwayTotal+=HWY
    
    highwayString = 'HWY: ' + highwayString[3:] + ' = {}'.format(highwayTotal)
    print(highwayString)

    totalScore = beachTotal + factoryTotal + houseTotal + shopTotal + highwayTotal
    
    if turns <= 16:
        print('Total score: {}'.format(totalScore))

    else:
        print('Total score: {} + {} + {} + {} + {} = {}\n'.format(beachTotal, factoryTotal, houseTotal, shopTotal, highwayTotal, totalScore))

    if turns > 16:
        highscores()
    return beachTotal, factoryTotal, houseTotal, shopTotal, highwayTotal, totalScore

def highscores(): #To get the highscores
    global scoreDict
    global totalScore

    scoreList = []
    updatedScores = []
    scoreDict = {}

    scoreList.append(totalScore) #Appends total score to scoreList

    with open('scores.txt', 'a') as s:
        for i in scoreList:
            s.write('{} '.format(i)) #Appends score into scores file

    with open('scores.txt', 'r') as s:
        scores = s.readline() #Reads line of scores
        scores = scores[:-1] #Removes space at end
        scores = scores.split(" ") #Seperates each score with space, makes it into a list

        updatedScores = scores

    updatedScores.sort()
    updatedScores.reverse() #Arranges list in descending order

    if len(updatedScores) > 10:
        updatedScores = updatedScores[:10] #Only top 10 highest scores
    
    if str(totalScore) in updatedScores: #If score is in the top 10 list
        addPos = scores.count(str(totalScore))-1 #Finds and counts the number of other scores that are the same.
        finalPos = updatedScores.index(str(totalScore))+addPos+1 #Calculates final position of the player.
        if finalPos <= 10: #Checks if final position of player is less than or equal to 10.
            print('Congratulations! You made the highscore board at position {}!\n'.format(finalPos)) #Congratulations message printed.

            while True:
                name = input('Please enter a name (Max 20 characters): ') #Asks for name input.
                if len(name)>20 or len(name) == 0: #If name is blank or has over 20 characters, prompts for valid name.
                    print('Invalid name, try again.')
                else:
                    pass
                    break

            with open('highscore.txt', 'a') as h:
                h.write('{} {}\n'.format(name,scoreList[0])) #Appends name and score associated with the name into highscore.txt
        else:
            pass
                        
        scoreList.clear() #Clears scoreList at the end to make sure there are no duplicates after finishing a new game.
    return scoreDict

def sortKey(element): #Takes the last element in dictionary for sorted().
    return(element[-1])

def displayScore(): #Displays highscores
    global scoreDict

    with open('highscore.txt', 'r') as h: 
        for line in h: #For every line in highscore.txt
            highscores = line.strip('\n')
            scoreIndex = highscores.split(" ") #Splits line by name and score (e.g. Ryan 56 -> ['Ryan', '56'])
            scoreDict[str(scoreIndex[0])] = int(scoreIndex[1]) #Sets key and value for scoreDict, (e.g. key = Ryan, value = 56)
                            
    highscoreDict = dict(sorted(scoreDict.items(), key=lambda x: x[1], reverse=True))
    '''The "sorted" function can sort any sequence, like lists, strings, or dictionaries. There can be 3 parameters and only 1 is required.

        First parameter is required. It is what needs to be sorted, in this case it is the dictionary scoreDict.

        Second parameter is optional, key = xyz. It decides how the scoreDict is sorted, in which case I put key = sortKey. 
        This is a function that takes in the last element of scoreDict, in this case it is the values of scoreDict.

        Third parameter is also optional, reverse = True/False. It reverses the order of the object after sorting it.
        This is used to get the first 10 values in the dictionary as the highest scores.'''    


    print('--------- HIGH SCORES ---------')
    print('{:3} {:>4} {:>20}\n{} {:>3} {:>20}'.format('Pos', 'Player', 'Score', '---', '------', '-----'))
    num = 0
    if highscoreDict == {}: #If it is empty
        print('No scores have been set.\n')
    else:
        for i in highscoreDict:
            print('{:2}   {:24}{}'.format(num+1, i, highscoreDict[i])) #Prints rank, player, score
            num+=1
            if num == 10: #Stops after top 10 scores
                break
    
def loadGame(): #Loads game from save file

    with open('data.txt','r') as r:
        
        global turns
        global map
        global buildings
        global buildingAmount
        global building1
        global building2
        global newBuildings

        #Reads the turns
        firstLine = r.readline()
        firstLine = firstLine.strip('\n')
        turns = int(firstLine)
            
        #Reads map
        newlist = []
        for i in range(4):
            a = r.readline().strip('\n')
            a = a.split(',')
            newlist.append(a)
        
        map = newlist

        #Reads for buildings list
        b = r.readline().strip('\n')
        b = b[:-1]
        b = list(b.split(" "))
        
        buildings = b
        
        #Reads for buildingAmount list
        for i in range(5):
            temp = r.readline().strip('\n').split(' ')
            buildingAmount[temp[0]] = int(temp[1])

        #Reads building choices, makes building choice same as saved game    
        randomList = r.readline().split(" ")
        building1 = randomList[0]
        building2 = randomList[1]
        newBuildings = []
        newBuildings.append(building1)
        newBuildings.append(building2)

    return buildings, buildingAmount, turns, building1, building2, newBuildings

def mainMenu(buildings, buildingAmount, turns, building1, building2, newBuildings): #Displays main menu
    
    while True: #Prints out main menu options
        print('\nWelcome, mayor of Simp City!\n----------------------------')
        print('\n1. Start new game\n2. Load saved game\n3. Show high scores\n4. Rules\n\n0. Exit game\n')
        while True:
            try:
                validOption = False
                startOption = int(input('Your choice? '))
                if -1 < startOption < 5:
                    validOption = True
                assert validOption #Checks if valid choice was picked
                break
            except:
                print('\nInvalid choice, try again.')
                print('\n1. Start new game\n2. Load saved game\n3. Show high scores\n4. Rules\n\n0. Exit game\n')

        if startOption == 1: #Resets values of variables
            buildings, buildingAmount, turns = newgame() #Sets variables to newgame function
            Game(buildings, buildingAmount, turns, building1, building2, newBuildings)
                
        elif startOption == 2: #Loads save file
            try:
                buildings, buildingAmount, turns, building1, building2, newBuildings = loadGame() #Sets variables to loadGame function
                Game(buildings, buildingAmount, turns, building1, building2, newBuildings)
                
            except FileNotFoundError: #When there is no save file
                print('\nThere is no save file found. Please start a new game.')
                mainMenu(buildings, buildingAmount, turns, building1, building2, newBuildings)

        elif startOption == 3: #Displays high scores
            try:
                displayScore()
                
            except FileNotFoundError: #When there are no high scores in the file
                print('\nThere are no high scores found. Please start a new game.')
                mainMenu(buildings, buildingAmount, turns, building1, building2, newBuildings)
                

        elif startOption == 4: #Rules
            print('\nWelcome to Simp City! In this game, you are the mayor of Simp City, and you will have to build the happiest and most prosperous city possible.\n')
            print('This city-building strategy game is played over 16 turns. In each turn, you will build one of two randomly-selected buildings in your 4x4 city.')
            print('In the first turn, you can build anywhere in the city. In subsequent turns, you can only build on squares that are connected to existing buildings.')
            print('The other building that you did not build is discarded.\n')
            print('Each building scores in a different way. The objective of the game is to build a city that scores as many points as possible.\nThere are 5 types of buildings, with 8 copies of each:\n\
    • Beach (BCH): Scores 3 points if it is built on the left or right side of the city, or 1 point otherwise.\n\
    • Factory (FAC): Scores 1 point per factory (FAC) in the city, up to a maximum of 4 points for the first 4 factories. All subsequent factories only score 1 point each.\n\
    • House (HSE): If it is next to a factory (FAC), then it scores 1 point only. Otherwise, it scores 1 point for each adjacent house (HSE) or shop (SHP), and 2 points for each adjacent beach (BCH).\n\
    • Shop (SHP): Scores 1 point per different type of building adjacent to it.\n\
    • Highway (HWY): Scores 1 point per connected highway (HWY) in the same row.\n\
    ')
            print('Good luck and have fun building your city!\n')
            
            mainMenu(buildings, buildingAmount, turns, building1, building2, newBuildings)

        else: #Closes game
            sys.exit(0)

mainMenu(buildings, buildingAmount, turns, building1, building2, newBuildings) #Starts the game

