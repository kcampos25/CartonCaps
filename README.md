# **Carton Caps App - Referral API**

**Description:**
  
Development of an API for the new Carton Caps app feature that allows users to refer friends so they can install the app.

---

## **Table of Contents**
1. [Objective](#objective)
2. [Project Architecture](#project-architecture)
3. [Technologies](#technologies)
4. [Running the Project](#running-the-project)
5. [User Stories](#user-stories)
6. [Overview of the Referral Flow](#overview-of-the-referral-flow)
7. [Evaluation Notes](#evaluation-notes)

---

## **Objective**

The objective of this project is to manage the referral process by developing the API in .NET Core.
The API exposes endpoints to register referred users, generate invitation links, and track visits to those links in order to monitor click activity and prevent abuse.

## **Project Architecture**

The designed architecture follows clean architecture and layered principles, ensuring a clear separation of responsibilities.
The solution is structured to keep business rules independent from infrastructure concerns, making the system easier to test, maintain, and scale.

- **CartonCaps.Api:** Endpoints, Program.cs, appsettings.json  
- **CartonCaps.Application:** Services, business logic, DTOs  
- **CartonCaps.Domain:** Domain entities
- **CartonCaps.Infrastructure:** Data, repositories, mockData, EF Core 
- **CartonCaps.Common:** Middleware and shared components
- **CartonCaps.Tests:** Unit tests  

**Note:** In the `CartonCaps/Documentation/` folder of this repository, you can find a PDF named Architecture.pdf, which explains the architecture and the interaction between layers in detail.

---

## **Technologies**

- C# / .NET Core 6
- Entity Framework Core (InMemory)
- FluentValidation
- AutoMapper
- Swagger
- Tests/ MOQ/ FluentAssertions

`macOS Compatibility`
The project does not use Windows-specific dependencies (such as a local SQL Server).
Everything works with EF Core InMemory, allowing it to run without any additional installation.

---

## **Running the Project**

Requirements: .NET 6 SDK

1. Clone the repository
`git clone https://github.com/kcampos25/CartonCaps.git`
`cd CartonCaps`

2. Restore dependencies
`dotnet restore`

3. Build the solution (recommended for cross-OS testing)
`dotnet build`

4. Running tests
`dotnet test`

5. Run the project
`dotnet run --project CartonCaps.Api`

6. Access Swagger : https://localhost:<PORT>/swagger

---

## **User Stories**

Deliverable #1: REST API Endpoints - User Stories :

In the `CartonCaps/Documentation/`, you can find a PDF named CartonCaps User Stories – Referrals.pdf, which contains the user stories.  

---

## **Overview of the Referral Flow**

Based on the analysis performed, the following flow was designed and implemented through the corresponding API endpoints.

**1. The Referrer Views Their Existing Referrals**

An authenticated user (User A) opens the Invites screen in the app to review the list of friends they have referred.

The following endpoint provides this information:

`GET /api/Referrals/{referrerUserId}/referrals`

**Note:**
For the purposes of this challenge, the endpoint only returns the friend’s name and the referral status, as suggested in the provided mockups.

**2. The Referrer Sends an Invitation**

From the Invites screen, User A can send an invitation to a friend through various channels such as email, WhatsApp, or SMS.

When the invitation is sent, the following endpoint is triggered:

`POST /api/Referrals/create`

This endpoint performs the following operations:

- Generates a unique referral code.
- Saves the invitation record in the referral table.
- Builds the referral link, returned to the app.
- The link is generated as: Base URL + referral code, where the Base URL points to the /api/ReferralsVisit/redirect endpoint, responsible for managing and tracking link visits.
- Returns both the referral link and the referral code.

**Note:**
For this challenge, only the link and referral code are returned. In a real-world scenario, the backend should also return the full message template to be sent to the friend (email or SMS), as shown in the mockups. Those message templates can be managed centrally from the API.

**3. The Friend Clicks the Invitation Link**

When the friend (User B) receives the message and taps the link, the following endpoint is executed:

`GET /api/ReferralsVisit/redirect`

This endpoint redirects the user to the appropriate app store so they can download the application.

In addition to redirecting, this endpoint implements anti-abuse measures:

- Records the visit with relevant data such as ipAddress, and associates the entry with the original referral invitation.
- The visit is only recorded if the corresponding referral is not in the Completed status. Based on the analysis, a referral becomes Completed once the friend successfully registers and becomes a valid app user.
- A time-window validation is applied to prevent click abuse.
- The service checks the most recent click for the same referral code and IP address within a defined time window. If the limit is exceeded, the click is ignored (but the redirect still occurs).

**Notes:**

- Even when the visit is not recorded, the user is always redirected to the App Store.
- This prevents exposing internal logic and ensures the user experience is not affected.


## **Evaluation Notes**

**1. Out-of-Scope Items (According to the Challenge)**

According to the instructions:

- “You can assume that we will be using a third-party vendor for deferred deep link support.”
Therefore, deep link navigation within the app was not implemented. The external vendor is responsible for directing the friend to the correct in-app screen.
- The challenge also specifies not to implement:

  1. New user registration

  2. Referral redemption as part of new user registration


Recommended Considerations for a Real Implementation

**Although out of scope for this challenge, the following recommendations are suggested:**

- Save the newly registered user and associate them with the referrer using a field in the Users table.
- Update the referral’s status to Completed using the referral code once the friend finishes registration.
- Prevent users from referring themselves.
The referral table already stores enough information to validate this.
- Consider storing a device identifier.
Combined with ipAddress, this allows additional checks to prevent duplicate or fraudulent registrations.

**2. General Notes**

- The project is configured to run without any external dependencies, allowing it to start immediately on macOS.
- A 1-minute time window was added to make the anti-abuse rules easier to verify during testing. This shortened interval is intended specifically for the challenge.
- The redirect functionality works correctly. Any errors seen when opening the link from Swagger occur because the Swagger client does not support external redirects.
- A centralized middleware was added to handle common errors and validations.
- The CartonCaps.Infrastructure project includes a MockData folder containing preloaded test data.
- **Important:** The following endpoint is outside the scope of the challenge and was implemented exclusively for testing:
**/api/ReferralsVisit/referralVisitHistory**
It allows reviewing each click recorded when the /api/ReferralsVisit/redirect endpoint is executed.

---


