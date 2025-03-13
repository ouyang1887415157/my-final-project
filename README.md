# My-Final-Project
This is the folder of my final project in Unity Version.

This is the final project of my University of London Computer Science Degree. (ZIXI OUYANG, STUDENT ID: 210300411)
This is a game called "The richest in Tang Dynasty" game that aims to entertain players with monopoly-like content and teach them knowledge about Chinese culture and history.
All the land tiles are named in the names of lands in Tang Dynasty in China, and there are over 50 Chinese knowledge questions waiting for the players in the Imperial Examination Hall.

## This is the Unity Version of my final project
You can open it with Unity Hub with any Unity Version that is compatible with Unity 6000.0.26f1.

## The Game process of "The richest in Tang Dynasty":
1. Each player selects their preferred players. There are four players with different colors. The player will move in the sequence of age, from the youngest to the oldest. Initially, each player receives 20,000 coins, with the remaining funds considered as the "bank".

2. All players place their pieces at the starting point, and the youngest player begins by rolling the dice to move forward, following the direction of the arrow from the starting point.

3. When a player lands on an unoccupied land space, they may choose to pay an amount equal to the land's value to purchase the property rights and land may be changed into the color, indicating they have bought this piece of land.

4. When a player lands on a land space occupied by another player, they must pay the landlord the corresponding rent. Each piece of land has different rent prices. (Rent is half of the land’s price)

5. When a player lands on a land space occupied by himself, he can choose to buy a house on his own land. The price of the house is the price of the land, and the land’s price will be doubled. A new house will be generated on the land and the rent will be doubled accordingly.

6. When landing on a "Notice Board”, the notice board will generate a random event and a player must execute its content. There are five events totally.

7. When a player lands on the “ChangLe Fang”, the player should offer 2000 coins to guess whether the number of the dice is big or small. If the player guesses the number correctly, he can receive 4000 coins as reward.

8. When a player lands on the “black hotel”, the player can offer 2000 coins to buy one-time ownership of the black hotel. When the next player lands on the same tile of the black hotel, he needs to give 4000 coins to the owner. After that, the black hotel returns default stage where it does not have an owner.

9. When players go pass the “starting point” which is “Gate of Divine Prowess”, they can get 2000 coins as their salaries.

10. The most important and distinguished feature of this game: when a player goes pass the imperial examination system (Tile 11), the player can answer several questions to get money as rewarded to teach them knowledge about the history of ancient Chinese.

11. The player can get bankrupted when their coins become lower than zero. When they get bankrupted, they cannot move again nor collect any rents. Player can only become winner if they are not bankrupted.

## Victory Condition
Finally, in original monopoly game, all players except the winner go bankrupt, and they cannot sell their land or house to pay their debt.
Since the game only lasts for shorter amount of time, the victory condition is changed into that the person with the greatest amount of wealth wins after dozens of rounds. In this case, the number of max rounds is 36 rounds. (The rounds can be changed according to the time players want to play) (Finally, the computer calculates the total amount of money which includes the prices of the lands and determines the winner)
