# Library Management System
## N-Tier Project w/ Console Interface
### Features:
* N-tier architecture (Repository, Application, UI, and Core layers)
* Allowed created a new borrower, editing the borrower, & checking out and returning media
* Performed checks on whether the borrower had any overdue items or had hit the checkout limit (3 items)
  * If they had overdue or were at the max, it would not allow any additional items to be checked out
* Provided reports that showed the status of each borrower and each media item
* Created two different repository layers to gain experience with different ORMs - Entity Framework & Dapper

### Challenges:
* ***Creating two different repository layers - one with Entity Framework, the other with Dapper/ADO***
  * First I created the EF repo because that was slightly easier to code due to being able to use LINQ vs SQL. Then I began work on the Dapper/ADO repo by first working with the SQL queries in Azure Data Studio to make sure they were grabbing the data intented and then turning that query into the appropriate Dapper/ADO CRUD function.
