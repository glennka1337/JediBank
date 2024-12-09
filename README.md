# Welcome to the JEDIBank
- - - -
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

## UML: 
![image](https://github.com/user-attachments/assets/7b5acf24-ab41-40bd-bcf9-1c2c2afaa902)







