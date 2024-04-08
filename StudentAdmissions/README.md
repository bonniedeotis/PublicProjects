# Student Admissions Web App
## Full-stack ASP.NET w/ Razor Pages
### Features:
* ASP.NET MVC
* Entity Framework backend
* Web app for a super-villan student academy:
  * Enroll a new student in the academy
  * Sign up for classes
  * Add/edit/delete special powers and weaknesses
  * View lists of available courses
  * Reports of students with specific powers or weaknesses
* Frontend uses a mix of Razor, HTML and Bootstrap

### Challenges:
* ***Working with Razor Pages***
  * This was my first full stack project working with Razor. It was interesting learning how and when to work in C# code via Razor mixed in with the HTML and Bootstrap
* ***Creating a student profile page that gave access to their enrolled classes, strengths, weaknesses, and the ability to edit/update basic info.***
  * To solve this and create a clean and user-friendly experience I added in tabbed content so that the user could toggle between tables of the classes, strengths, or weaknesses based on what they wanted to see or access.

### Structure
* Application
  * Entity Framework connected to a pre-existing database running in Docker
  * Contains Interfaces for accesses each type of data (Student, StudentWeakness, StudentPower, Courses, etc)
  * DTOs for passing specific data to the controllers and one that is a custom Result object used for stating if a request was successful for not
* UI
  * Models
    * Used for passing data between the controllers and the views
  * Views
    * Razor pages 
  * Controllers
    *  Connect to and work with backend resources
    *  Set route paths
    *  Handle validations and return appropriate status codes
