# RMIT Course: Web Development Technologies
Assignment #2: ASP.NET Core Internet Banking website with .NET Core

## Members
The enslaved (team) members responsible for the delivery of the project are as follows:
- Atirach Intaraudom (s3750202) - @atirudom
- Jiahui Li (s3614541) - @CatnisLee

## Design Overview (Business Objects)
Our codes in the program was implemented with a number of design patterns and friendly business objects to keep the codes and controllers clean to read as well as easy to follow. 

### Controller Implementation Overview
The design is straight-forward in simple controllers that has no complex logic such as controllers that include only View redirection.
However, the large controllers such as transferring money will include complex logic and here we will describe how those large controllers are implemented in a simplified manner. 

Briefly, the processes in each controller are divided into 3 main steps. (1) The first step is the set up and input validation step written in the controller. (2) The second step is the business logics of the controller where most of the codes are hidden inside a business object which is applied in form of Mediator pattern to avoid bloating in controllers. (3) The last step contains result functions, written in the controller, which mainly commands and decides their next move after all logics in the previous step are finished ex: redirect to view, throw error, etc.

We believe the step 1 and step 3 should be written in the controller because if the process is not oversized, it is more reasonable to reveal plain observable processes, such as input validation and conditional redirection process, in the controller. This is because we believe the controllers should be intuitive to read meaning that coupling design might be more preferable than cohesion when designing plain observable processes. 

### Controller Process Example
All controllers that include This is an example of how the flow goes in the following order when user fills all inputs and click transfer money correctly. 
1. Start journey by calling the controller. The controller instantiates required objects and validate user input. This block of codes is written in the controller.
2. The controller processes business logics by calling the transfer process hidden in the `Mediator Function` which is implemented in form of Mediator pattern. The `Mediator Function` then calls `AccountTransferAdapter` implemented in form of Adapter pattern to execute transferring process between the account objects. This block of codes is written in business objects.
3. The controller checks the result of the business process from step 2 then perform final action. In this case, if the transfer was success, then the view for success is redirected to, otherwise throw error to the same page. This block of codes is written in the controller.

### Implemented Design Patterns
Patterns used in this project are as follows:
1. Mediator - Acts as an interface between controllers and complex business logics. Directory: Controller > Functions
2. Adapter - Wrap responsible objects and enable specific features wihtout touching the model. Most are account transaction features. Directory: Model > Adapter
3. Factory - Ensure the model creation behavior. Directory: Model > Factory
4. Builder - Build the view statement in the specific behavior. Directory: Model > Builder
5. MVC - We are forced to follow her anyway. 😜

### What are the advantages of all these implementations ?
Our business objects are implemented outside the controller with rich design patterns and reasonable seperation of code blocks. With all the design patterns used in this project, this brings all benefits of efficient software designs such as cohesion, neat codes, seperation of responsibility, intuitive for all readers. The complex business logics or business objects are also separated from controllers which leads to a productive cohesion and intuitive codes in controllers.

## Git Strategies
### Branching
1. Always start journey by branching out from `develop` into a friendly branch name of choosing.
2. Once we've performed our magic, create a pull request (PR) to merge our changes back into `develop`.
3. Once we've met a major milestone a pull request (PR) can be made from `develop` back into `master` which requires at least one approval to reach a consensus on the changes 😜
