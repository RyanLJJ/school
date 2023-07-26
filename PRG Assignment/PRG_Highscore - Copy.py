#Imports

import sys
import random

#Globals

numRows = 4

numCols = 4

buildingList = ['BCH','FAC','HSE','SHP','HWY']

buildingAmount = {}

map = []


buildings = []

turns = 1

scoreDict = {}

totalScore = 0

#Functions

def createMap(map):
    print('{:>40}{:>15}'.format('Buildings','Remaining'))
    print('     A     B     C     D {:>15}{:>15}'.format('---------','---------'))
    print('  +-----+-----+-----+-----+ {:>9}{:>14}'.format(buildingList[0],buildingAmount[buildingList[0]]))
    for row in range(numRows):  
        print(' {}'.format(row + 1), end = '')
        for column in range(numCols):
            print('| {:3s} '.format(map[row][column]), end = '')
        print('|')
        print('  ', end = '')
        for column in range(1, numCols + 1):
            print('+-----', end = '')
        print('+ {:>9}{:>14}'.format(buildingList[row+1],buildingAmount[buildingList[row+1]]))


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


def Game(buildings, buildingAmount, turns): #Function for game
    columns = 'abcd'
    turn16 = False
    validPos = True
    posList = ['a1','a2','a3','a4','b1','b2','b3','b4','c1','c2','c3','c4','d1','d2','d3','d4']

    while turn16 == False: #Loops the function when turns are not over 16
        print('Turn {}'.format(turns))
        createMap(map)
        newBuildings = [] #

        try:  #- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -EXCEPTION 1: INVALID CHOICE OPTION
            for i in range(2):
                newBuildings.append(buildings[random.randint(0, len(buildings) - 1)]) #Takes 2 random buildings from buildings list
                print('{}. Build a {}'.format(i + 1, newBuildings[i]))
            print('3. Show current score\n\n4. Save game\n0. Exit to main menu')
            choice = int(input('Your choice? '))
            
            assert -1 < choice < 5 #Checks if choice input is within this range, if not it creates an error

        except:  #- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -EXCEPTION 1 CLOSED
            print('Invalid choice, try again.') #Will loop the function again, because turn16 is still False

        while choice == 0: #Back to main menu
            mainMenu(buildings, buildingAmount, turns) 
                    
        if choice == 3: #Shows and calculates scores
            scoreCalculation(turns) 
              
        elif choice == 4: #Saves game into a file
                
            print('\nGame saved!\n')
            with open('data.txt', 'w') as f:

                f.write('{}\n'.format(turns)) #Writes in turns

                for i in range(len(map)): #For every row of map
                    s = ''
                    for j in range(len(map[i])): #For every column of map
                        s += map[i][j] + ',' #Values in positions of the map will be s, seperates each value with comma
                    f.write(s[:-1] + '\n') #Writes in the map layout line by line, removing the comma at the end

                for i in buildings: 
                    f.write('{} '.format(i)) #For every building in 
                f.write('\n')

                for i in buildingAmount:
                    f.write('{} {}\n'.format(i, buildingAmount[i]))                                             

        elif choice == 1 or choice == 2: #Asks user for position 
            try:# - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -EXCEPTION 2: NON-ADJACENT POSITIONS
                try: #- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - EXCEPTION 3: INVALID POSITION ON MAP
                    build = input('Build where? ')                                                                         
                    build = build.lower()                                                                                  
                    col = columns.find(build[0])                                                                           
                    row = int(build[1]) - 1                                                                                
                    assert build in posList                                                                                
                except: # - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -- - - - - -EXCEPTION 3 CLOSED
                    print('Invalid position, try again')
                    Game(buildings, buildingAmount, turns)

                if turns == 1: #Turn 1 building can be placed anywhere
                    validPos = True
                elif 17 > turns > 1: #Buildings after turn 1 follow adjacent rule
                    validPos = False

                    # Adjacent code
                    # First line: Same column, row above
                    # Second line: Same column, row below
                    # Third line: Same row, column above
                    # Fourth line: Same row, column below
                    if row-1 != -1 and map[row-1][col] != ' '\
                        or row+1 != 4 and map[row+1][col] != ' '\
                            or col-1 != -1 and map[row][col-1] != ' '\
                                or col+1 != 4 and map[row][col+1] != ' ': 
                                    validPos = True

                    if map[row][col] != ' ': #Same position
                        validPos = False
                        
                        
                    assert validPos #Exception handling for valid adjacent position
                        
                turns+=1 #After a valid input, turns + 1
                map[row][col] = newBuildings[choice - 1]
                buildings.remove(newBuildings[0])
                buildings.remove(newBuildings[1])
                buildingAmount[newBuildings[0]] = buildingAmount[newBuildings[0]] - 1
                buildingAmount[newBuildings[1]] = buildingAmount[newBuildings[1]] - 1
                print()
                    
            except:#- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -EXCEPTION 2 CLOSED
                print('Invalid option, buildings must be adjacent.')

            if turns > 16:
                turn16 = True
                print('Final layout of Simp City:\n')
                createMap(map)
                scoreCalculation(turns)
                mainMenu(buildings, buildingAmount, turns)
                
  
    return buildings, buildingAmount, turns


def scoreCalculation(turns): #Calculates the score for each building
    #Beach Calculation
    global totalScore

    beachTotal = 0
    beachString = ''
    for row in range(len(map)):
        for col in range(len(map[row])):
            if "BCH" in map[row][col]:
                if col == 0 or col == 3:
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
                    other = len(factoryList)-4
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
            if "HSE" in map[row][col]:
                if row-1 != -1 and "FAC" in map[row-1][col]\
                    or row+1 != 4 and "FAC" in map[row+1][col]\
                        or col-1 != -1 and "FAC" in map[row][col-1]\
                            or col+1 != 4 and "FAC" in map[row][col+1]:
                            HSE = 1

                else:
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
            if "SHP" in map[row][col]:
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
            if "HWY" in map[row][col]:
                HWY = 1
                if col-1 != -1 and "HWY" in map[row][col-1]:
                    HWY+=1

                    if col-2 > -1 and "HWY" in map[row][col-2]:
                        HWY+=1

                        if col-3 > -1 and "HWY" in map[row][col-3]:
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
    
    if turns > 16:
        print('Total score: {} + {} + {} + {} + {} = {}\n'.format(beachTotal, factoryTotal, houseTotal, shopTotal, highwayTotal, totalScore))
        
        highscores()
    return beachTotal, factoryTotal, houseTotal, shopTotal, highwayTotal, totalScore


def highscores(): #To get the highscores
    global scoreDict
    global totalScore
    scoreList = []
    scoreList.append(totalScore)
    updatedScores = []
    with open('scores.txt', 'a') as s:
        for i in scoreList:
            s.write('{} '.format(i))

    with open('scores.txt', 'r') as s:
        scores = s.readline()
        scores = scores[:-1]
        scores = scores.split(" ")

        updatedScores = scores

    updatedScores.sort()
    updatedScores.reverse()

    if len(updatedScores) > 10:
        updatedScores = updatedScores[:10]
    print(updatedScores)

    scoreDict = {}
    if str(totalScore) in updatedScores:
        try:
            name = input('Please enter a name (Max 20 characters): ')
            assert 0 < len(name) < 21
        except:
            print('Invalid name')
    
        with open('highscore.txt', 'a') as h:
            h.write('{} {}\n'.format(name,scoreList[0]))

        with open('highscore.txt', 'r') as h:
                    
            for line in h:
                highscores = line.strip('\n')
                scoreIndex = highscores.split(" ")
                scoreDict[str(scoreIndex[0])] = int(scoreIndex[1])
                        

        highscoreDict = dict(sorted(scoreDict.items(), key=sortKey, reverse=True))
        if len(highscoreDict) > 10:
            highscoreDict = highscoreDict[:10]
        print(highscoreDict)

        scoreList.clear()
    return scoreDict, highscoreDict


def displayScore(): #Displays highscores
    global scoreDict
    with open('highscore.txt', 'r') as h:
                    
        for line in h:
            highscores = line.strip('\n')
            scoreIndex = highscores.split(" ")
            scoreDict[str(scoreIndex[0])] = int(scoreIndex[1])
                        

    highscoreDict = dict(sorted(scoreDict.items(), key=lambda x: x[1], reverse=True))
    if len(highscoreDict) > 10:
            highscoreDict = highscoreDict[:10]

    print('--------- HIGH SCORES ---------')
    print('{} {:>4} {:>20}\n{} {:>3} {:>20}'.format('Pos', 'Player', 'Score', '---', '------', '-----'))
    num = 0
    for i in highscoreDict:
        print('{}   {:24}{}'.format(num+1, i, highscoreDict[i]))
        num+=1

def loadGame(): #Loads game from save file
    with open('data.txt','r') as r:
        
        global turns
        global map
        global buildings
        global buildingAmount

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
        print(newlist)
        map = newlist

        #For building list
        b = r.readline().strip('\n')
        b = b[:-1]
        b = list(b.split(" "))
        
        buildings = b
        print(buildings)

        for i in range(5):
            temp = r.readline().strip('\n').split(' ')
            buildingAmount[temp[0]] = int(temp[1])
            print(buildingAmount[temp[0]])
            
        return buildings, buildingAmount, turns

def sortKey(element):
    return(element[-1])

def mainMenu(buildings, buildingAmount, turns): #Displays main menu
    
    while True:
        print('Welcome, mayor of Simp City!\n----------------------------')
        print('\n1. Start new game\n2. Load saved game\n3. Show high scores\n4. Rules\n\n0. Exit game\n')
        try:
            startOption = int(input('Your choice? '))
            assert -1 < startOption < 5
        except:
            print('Invalid option, try again.')

        if startOption == 1: #Resets values of variables
            buildings, buildingAmount, turns = newgame()
            
            Game(buildings, buildingAmount, turns)
                
        elif startOption == 2: #Loads save file
            buildings, buildingAmount, turns = loadGame()

            Game(buildings, buildingAmount, turns)

        elif startOption == 3:
            
            displayScore()


        elif startOption == 4: #Rules
            print('\nWelcome to Simp City! In this game, you are the mayor of Simp City, and you will have to build the happiest and most prosperous city possible.\n')
            print('This city-building strategy game is played over 16 turns. In each turn, you will build one of two randomly-selected buildings in your 4x4 city.')
            print('In the first turn, you can build anywhere in the city. In subsequent turns, you can only build on squares that are connected to existing buildings.')
            print('The other building that you did not build is discarded.\n')
            print('Each building scores in a different way. The objective of the game is to build a city that scores as many points as possible.\
    There are 5 types of buildings, with 8 copies of each:\n\
    • Beach (BCH): Scores 3 points if it is built on the left or right side of the city, or 1 point otherwise.\n\
    • Factory (FAC): Scores 1 point per factory (FAC) in the city, up to a maximum of 4 points for the first 4 factories. All subsequent factories only score 1 point each.\n\
    • House (HSE): If it is next to a factory (FAC), then it scores 1 point only. Otherwise, it scores 1 point for each adjacent house (HSE) or shop (SHP), and 2 points for each adjacent beach (BCH).\n\
    • Shop (SHP): Scores 1 point per different type of building adjacent to it.\n\
    • Highway (HWY): Scores 1 point per connected highway (HWY) in the same row.\n\
    ')
            print('Good luck and have fun building your city!\n')
            
            mainMenu(buildings, buildingAmount, turns)

        else: #Closes game
            sys.exit(0)


mainMenu(buildings, buildingAmount, turns) #Starts the game

