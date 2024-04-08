# Cafe Point of Sale System
## N-tier Project w/ Console Interface
### Features
* N-Tier Architecture (Repository, Service, & UI layers + a Core layer for shared resources)
* Entity Framework + LINQ
* Menu changes based on time of day and has 4 different menus
* Basic sales reporting
* Training mode that allows full functionality of the system without affecting the database

### Challenges
* Creating a training mode that doesn't connect to the database
  * Created a 'FakeDatabase' that consisted of Lists which mimicked the Entity Framework database entities
  * Made a repo layer that interacted with the Lists and performed the same CRUD functions as the database repo layer  
