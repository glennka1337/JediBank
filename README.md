# Welcome to the JEDIBank
- - - -
# The program
The main logic for the program is found in the method RunProgram within the class Bank. And it uses a nestled while loop. The first one initiates the program and displays the login menu. next we have an if statement that checks if the user chose login and then uses the method login to assign the user as the current user. After that we have a while that checks that the current user is not null, and while it is not we display the appropriate menu. Logging out will set the current user to null and thus it will break the inner loop. Choosing exit will close the program.
# Classes:
### Bank
This class handles logic such as loading and archiving users and the starting up the program with the RunProgram() method. This class also contains the ActionMap which is a dictionary mapping the menu options to it's corresponding method. 
### Account
The Account class is the bank account object that represents the users bankaccount. It has properties such as AccountId, Name, Balance and Currency.
### User
This is the class that represents the user object, every user of the bank is an object of this class. It contains the password and username properties. 
### Transaction
Using the method ExecuteTransaction() a user can initiate a new transaction. If the transaction is between two different currencies the logic in this class will use methods for converting them to the same. This is also the object type that gets saved as transaction history in the Account class.
### Loan
The loan class has properties such as Interest, Amount and AmountPaid. It also contains methods for paying of the loan.
### Window & UI
These classes provide the menus in the program. Allowing the user to navigate more easily and gives a more appealing look.
### Button
The classes in ButtonFolder inherit from the class button and provides a way to draw an option with the appearens of a button.
### Language
Provides the methods for translation of text by reading through a JSON file.

## UML: 
![image](https://github.com/user-attachments/assets/7b5acf24-ab41-40bd-bcf9-1c2c2afaa902)







