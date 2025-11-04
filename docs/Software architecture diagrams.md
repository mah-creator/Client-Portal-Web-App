---
title: Software Architecture Diagrams

---

\pagebreak

## Client Portal Web Application

**Version:** 1.0  
**Date:** 2025-10-26  
**Author:** nizarLubbad

---

## Table of Contents

- [Client Portal Web Application](#client-portal-web-application)
- [Table of Contents](#table-of-contents)
- [1. System Architecture Diagram](#1-system-architecture-diagram)
- [2. Database Entity Relationship Diagram (ERD)](#2-database-entity-relationship-diagram-erd)
- [3. Authentication Flow Diagram](#3-authentication-flow-diagram)
- [4. User Role Hierarchy](#4-user-role-hierarchy)
- [5. API Request Flow](#5-api-request-flow)
- [6. Component Architecture](#6-component-architecture)
- [7. File Upload Flow](#7-file-upload-flow)
- [8. Real-Time Notification Flow](#8-real-time-notification-flow)
- [9. Project Workflow Diagram](#9-project-workflow-diagram)
- [10. Task Status State Machine](#10-task-status-state-machine)
- [11. Deployment Architecture](#11-deployment-architecture)
- [12. Sequence Diagrams](#12-sequence-diagrams)
  - [12.1 Complete Project Creation Flow](#121-complete-project-creation-flow)
  - [12.2 Task Comment and Notification Flow](#122-task-comment-and-notification-flow)
  - [12.3 File Download Flow](#123-file-download-flow)
  - [12.4 Admin User Suspension Flow](#124-admin-user-suspension-flow)
- [Class Diagram](#class-diagram)
- [Use Case Diagram](#use-case-diagram)
- [Technology Stack Diagram](#technology-stack-diagram)
- [Summary](#summary)
  - [How to Use These Diagrams](#how-to-use-these-diagrams)
  - [Rendering These Diagrams](#rendering-these-diagrams)

---

## 1. System Architecture Diagram

```{.mermaid format=pdf}
graph TB
    subgraph "Client Layer"
        A[React Frontend<br/>TypeScript + Vite]
        A1[Login/Signup Pages]
        A2[Dashboard Components]
        A3[Project Management]
        A4[Task Management]
        A5[File Management]
    end
    
    subgraph "Network Layer"
        B[HTTPS/HTTP]
        C[WebSocket<br/>SignalR]
    end
    
    subgraph "API Layer - ASP.NET Core"
        D[Controllers]
        D1[AuthController]
        D2[ProjectsController]
        D3[TasksController]
        D4[FilesController]
        D5[UserController]
        D6[AdminController]
        
        E[Middleware]
        E1[JWT Authentication]
        E2[CORS Policy]
        E3[Exception Handling]
    end
    
    subgraph "Business Logic Layer"
        F[Services]
        F1[TokenService]
        F2[FileService]
        F3[NotificationService]
        F4[ProjectService]
    end
    
    subgraph "Data Access Layer"
        G[Entity Framework Core]
        G1[AppDbContext]
        G2[Migrations]
    end
    
    subgraph "Data Storage"
        H[(SQLite Database<br/>clientportal.db)]
        I[File System<br/>wwwroot/uploads]
    end
    
    subgraph "Real-Time Communication"
        J[SignalR Hubs]
        J1[NotificationHub]
    end
    
    A --> A1 & A2 & A3 & A4 & A5
    A1 & A2 & A3 & A4 & A5 --> B
    B --> E
    A --> C
    C --> J
    J --> J1
    E --> E1 & E2 & E3
    E1 & E2 & E3 --> D
    D --> D1 & D2 & D3 & D4 & D5 & D6
    D1 & D2 & D3 & D4 & D5 & D6 --> F
    F --> F1 & F2 & F3 & F4
    F1 & F2 & F3 & F4 --> G
    G --> G1 & G2
    G1 --> H
    F2 --> I
    J1 --> F3
    
    style A fill:#61dafb,stroke:#333,stroke-width:2px
    style D fill:#c491fd,stroke:#333,stroke-width:2px
    style H fill:#97fd91,stroke:#333,stroke-width:2px,shape:h-cyl
    style J fill:#ff6b6b,stroke:#333,stroke-width:2px
```

---

## 2. Database Entity Relationship Diagram (ERD)

```{.mermaid format=pdf}
erDiagram
    USERS ||--o{ PROJECTS : owns
    USERS ||--o{ PROJECTMEMBERS : "is member of"
    USERS ||--o{ TASKITEMS : creates
    USERS ||--o{ COMMENTS : writes
    USERS ||--o{ FILES : uploads
    USERS ||--|| PROFILE : has
    
    PROJECTS ||--o{ PROJECTMEMBERS : "has members"
    PROJECTS ||--o{ TASKITEMS : contains
    PROJECTS ||--o{ FILES : "has files"
    
    TASKITEMS ||--o{ COMMENTS : "has comments"
    TASKITEMS ||--o{ FILES : "has attachments"
    
    USERS {
        string Id PK
        string Email UK
        string PasswordHash
        string Name
        enum Role
        boolean IsSuspended
        datetime CreatedAt
    }
    
    PROFILE {
        string Id PK,FK
        string Bio
        string Phone
        string AvatarPath
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    PROJECTS {
        string Id PK
        string Title
        string Description
        string OwnerId FK
        enum Status
        datetime DueDate
        datetime CreatedAt
    }
    
    PROJECTMEMBERS {
        string ProjectId PK,FK
        string UserId PK,FK
        enum Role
        datetime JoinedAt
    }
    
    TASKITEMS {
        string Id PK
        string ProjectId FK
        string CreatorId FK
        string Title
        string Description
        enum Status
        datetime DueDate
        datetime CreatedAt
        datetime UpdatedAt
    }
    
    FILES {
        int Id PK
        string UploaderId FK
        string ProjectId FK
        string TaskId FK
        string Filename
        string Path
        long Size
        string Mime
        datetime UploadedAt
    }
    
    COMMENTS {
        string Id PK
        string UserId FK
        string TaskId FK
        string Body
        datetime CreatedAt
    }
```

---

## 3. Authentication Flow Diagram

```{.mermaid format=pdf}
sequenceDiagram
    participant U as User
    participant F as Frontend
    participant API as Auth Controller
    participant TS as Token Service
    participant DB as Database
    participant LS as LocalStorage

    rect rgb(240, 248, 255)
        Note over U,LS: Login Flow
        U->>F: Enter credentials
        F->>API: POST /api/Auth/login
        API->>DB: Query user by email
        DB-->>API: User data
        API->>API: Verify password hash
        alt Password Valid
            API->>TS: Generate JWT token
            TS-->>API: JWT token + expiry
            API-->>F: AuthResponse (token, user, expiresAt)
            F->>LS: Store token
            F->>F: Store user data
            F-->>U: Redirect to dashboard
        else Password Invalid
            API-->>F: 401 Unauthorized
            F-->>U: Display error
        end
    end

    rect rgb(255, 248, 240)
        Note over U,LS: Signup Flow
        U->>F: Enter registration data
        F->>API: POST /api/Auth/signup
        API->>DB: Check email uniqueness
        alt Email Available
            API->>API: Hash password
            API->>DB: Create user record
            DB-->>API: User created
            API->>API: Auto-login (call Login)
            API-->>F: AuthResponse
            F->>LS: Store token
            F-->>U: Redirect to dashboard
        else Email Exists
            API-->>F: 400 Bad Request
            F-->>U: Display error
        end
    end

    rect rgb(240, 255, 240)
        Note over U,LS: Authenticated Request
        U->>F: Access protected resource
        F->>LS: Retrieve token
        LS-->>F: JWT token
        F->>API: Request + Bearer token
        API->>API: Validate JWT
        alt Token Valid
            API->>DB: Fetch data
            DB-->>API: Data
            API-->>F: 200 OK + data
            F-->>U: Display content
        else Token Invalid/Expired
            API-->>F: 401 Unauthorized
            F->>LS: Clear token
            F-->>U: Redirect to login
        end
    end
```

---

## 4. User Role Hierarchy

```{.mermaid format=pdf}
graph TD
    A[User Roles]
    
    A --> B[Admin]
    A ----> C[Freelancer]
    A ------> D[Customer]
    
    B --> B1[Full System Access]
    B --> B2[User Management]
    B --> B3[Suspend/Unsuspend Users]
    B --> B4[View All Projects]
    B --> B5[System Configuration]
    
    C --> C1[Create Projects]
    C --> C2[Manage Own Projects]
    C --> C3[Create Tasks]
    C --> C4[Update Task Status]
    C --> C5[Upload Files]
    C --> C6[View Assigned Projects]
    C --> C7[Invite Clients]
    
    D --> D1[View Assigned Projects]
    D --> D2[View Tasks]
    D --> D3[Download Files]
    D --> D4[Comment on Tasks]
    D --> D5[View Project Progress]
    
    style A fill:#f9f,stroke:#333,stroke-width:3px
    style B fill:#ff6b6b,stroke:#333,stroke-width:2px
    style C fill:#4ecdc4,stroke:#333,stroke-width:2px
    style D fill:#95e1d3,stroke:#333,stroke-width:2px
```

---

## 5. API Request Flow

```{.mermaid format=pdf}
flowchart TD
    A[Client Request] --> B{Has Auth Token?}
    B -->|No| C[Return 401<br/>Unauthorized]
    B -->|Yes| D[CORS Middleware]
    
    D --> E[JWT Authentication<br/>Middleware]
    E --> F{Token Valid?}
    F -->|No| C
    F -->|Yes| G[Extract User Claims]
    
    G --> H[Route to Controller]

    H <--> AC1[[Authorization procedure]]

    AC2[[Authorization procedure]] --> I{Authorization Check}

    I -->|Fail| J[Return 403<br/>Forbidden]
    I -->|Pass| K[Execute Controller Action]

    K --> L[Call Service Layer]
    L --> M[Access Data Layer<br/>EF Core]
    M --> N[(Database)]

    N --> O[Return Data]
    
    O --> P[Transform to DTO]
    P --> Q[Return Response]
    Q --> R{Success?}
    R -->|Yes| S[Return 200/201<br/>with Data]
    R -->|No| T[Return 400/404/500<br/>with Error]

    style A fill:#61dafb,stroke:#333,stroke-width:2px
    style C fill:#ff6b6b,stroke:#333,stroke-width:2px
    style J fill:#ff6b6b,stroke:#333,stroke-width:2px
    style S fill:#51cf66,stroke:#333,stroke-width:2px
    style T fill:#ff6b6b,stroke:#333,stroke-width:2px
    style AC1 fill:#85f1ff
    style AC2 fill:#85f1ff
```

---

## 6. Component Architecture

```{.mermaid format=pdf}
graph TB
    subgraph "Frontend Architecture"
        A[App.tsx<br/>Root Component]
        
        subgraph "Context Providers"
            B[AuthContext]
            C[QueryClientProvider]
        end
        
        subgraph "Pages"
            D[Login Page]
            E[Admin Dashboard]
            F[Freelancer Dashboard]
            G[Customer Dashboard]
            H[Project Details]
            I[Task Management]
        end
        
        subgraph "Shared Components"
            J[Header/Navigation]
            K[Cards & Tables]
            L[Forms]
            M[File Upload]
            N[Notification Toast]
        end
        
        subgraph "Custom Hooks"
            O[useAuth]
            P[useProjects]
            Q[useTasks]
            R[useFiles]
            S[useRecentFiles]
        end
        
        subgraph "Services"
            T[API Client]
            U[apiClient.login]
            V[apiClient.getProjects]
            W[apiClient.getTasks]
            X[apiClient.uploadFile]
        end
    end
    
    A --> B & C
    B --> D & E & F & G
    C --> E & F & G & H & I
    D --> T
    E & F & G --> J & K & H
    H --> I & M
    I --> L & K
    
    O --> T
    P & Q & R & S --> T
    T --> U & V & W & X
    
    E & F & G --> O & P & Q & R & S
    
    style A fill:#61dafb,stroke:#333,stroke-width:3px
    style T fill:#ff6b6b,stroke:#333,stroke-width:2px
```

---

## 7. File Upload Flow

```{.mermaid format=pdf}
sequenceDiagram
    participant U as User
    participant FC as File Component
    participant API as Files Controller
    participant FS as File Service
    participant DB as Database
    participant DISK as File System

    U->>FC: Select file(s)
    FC->>FC: Validate file<br/>(type, size)
    
    alt Validation Pass
        FC->>API: POST /api/Files/upload<br/>(multipart/form-data)
        Note over FC,API: file, projectId, taskId
        
        API->>API: Verify project/task exists
        
        alt Resource Exists
            API->>FS: SaveFileAsync()
            FS->>FS: Generate unique filename<br/>(GUID + original name)
            FS->>DISK: Write file to<br/>wwwroot/uploads
            DISK-->>FS: File saved
            
            FS->>DB: Create FileEntity record
            DB-->>FS: Record created
            FS-->>API: FileEntity
            
            API->>API: Build FileResponse DTO
            API-->>FC: 200 OK + FileResponse
            
            FC->>FC: Update progress (100%)
            FC-->>U: Upload successful
            FC->>FC: Refresh file list
        else Resource Not Found
            API-->>FC: 400 Bad Request
            FC-->>U: Display error
        end
    else Validation Fail
        FC-->>U: Display validation error
    end
```

---

## 8. Real-Time Notification Flow

```{.mermaid format=pdf}
sequenceDiagram
    participant U1 as User 1<br/>(Action)
    participant F1 as Frontend 1
    participant API as API Controller
    participant NS as Notification Service
    participant HUB as SignalR Hub
    participant F2 as Frontend 2
    participant U2 as User 2<br/>(Receive)

    rect rgb(240, 248, 255)
        Note over U1,U2: Task Creation Notification
        
        U1->>F1: Create new task
        F1->>API: POST /api/projects/{id}/Tasks
        API->>API: Create task in DB
        API->>NS: NotifyProjectMembersAsync()
        
        NS->>NS: Get all project members
        
        loop For each member
            NS->>HUB: Send notification
            HUB->>F2: Push notification via WebSocket
            F2->>F2: Display toast/alert
            F2-->>U2: Show notification
        end
        
        API-->>F1: 201 Created
        F1-->>U1: Task created successfully
    end

    rect rgb(255, 248, 240)
        Note over U1,U2: Connection Lifecycle
        
        F2->>HUB: Connect with JWT token
        HUB->>HUB: Validate token
        HUB->>HUB: Join user-specific group
        HUB-->>F2: Connection established
        
        Note over F2,HUB: Maintain connection<br/>with heartbeat
        
        F2->>HUB: Disconnect
        HUB->>HUB: Remove from groups
        HUB-->>F2: Connection closed
    end
```

---

## 9. Project Workflow Diagram

```{.mermaid format=pdf}
stateDiagram-v2
    [*] --> Pending: Project Created<br/>(Freelancer)
    
    Pending --> InProgress: Start Project
    InProgress --> InProgress: Add Tasks<br/>Invite Members<br/>Upload Files
    
    InProgress --> OnHold: Pause Project
    OnHold --> InProgress: Resume Project
    
    InProgress --> Completed: All Tasks Done
    Completed --> [*]: Archive
    
    note right of Pending
        Default status
        Awaiting approval
    end note
    
    note right of InProgress
        Active development
        Tasks being worked on
        Files being exchanged
    end note
    
    note right of OnHold
        Temporarily paused
        Waiting for client input
    end note
    
    note right of Completed
        All deliverables done
        Client approved
    end note
```

---

## 10. Task Status State Machine

```{.mermaid format=pdf}
stateDiagram-v2
    [*] --> Todo: Task Created
    
    Todo --> In_progress: Start Working
    In_progress --> Pending_review: Submit for Review
    
    Pending_review --> In_progress: Needs Changes
    Pending_review --> Done: Approved
    
    In_progress --> Done: Complete (no review needed)
    
    Todo --> Canceled: Cancel
    In_progress --> Canceled: Cancel
    Pending_review --> Canceled: Cancel
    
    Done --> [*]
    Canceled --> [*]
    
    note right of Todo
        Initial state
        Not started
    end note
    
    note right of In_progress
        Being worked on
        Freelancer assigned
    end note
    
    note right of Pending_review
        Awaiting client approval
        Review in progress
    end note
    
    note right of Done
        Completed and approved
        Final state (success)
    end note
    
    note right of Canceled
        Task cancelled
        Final state (terminated)
    end note
```

---

## 11. Deployment Architecture

```{.mermaid format=pdf}
graph TB
    subgraph "Client Device"
        A[Web Browser]
    end
    
    subgraph "CDN / Static Hosting"
        B[React App<br/>Static Files]
    end
    
    subgraph "Application Server"
        C[ASP.NET Core API<br/>Port 5001/56545]
        D[SignalR Hub]
    end
    
    subgraph "Data Layer"
        E[(SQLite DB<br/>clientportal.db)]
        F[File Storage<br/>wwwroot/uploads]
    end
    
    subgraph "Production Alternative"
        G[Load Balancer]
        H[API Server 1]
        I[API Server 2]
        J[(SQL Server/<br/>PostgreSQL)]
        K[Cloud Storage<br/>AWS S3/Azure Blob]
    end
    
    A -->|HTTPS| B
    A -->|HTTPS/WSS| C
    B -->|API Calls| C
    A -->|WebSocket| D
    C --> E & F
    D --> C
    
    A -.->|Production| G
    G -.-> H & I
    H & I -.-> J & K
    
    style A fill:#61dafb,stroke:#333,stroke-width:2px
    style C fill:#c491fd,stroke:#333,stroke-width:2px
    style E fill:#97fd91,stroke:#333,stroke-width:2px
    style G fill:#ff6b6b,stroke:#333,stroke-width:2px
```

---

## 12. Sequence Diagrams

### 12.1 Complete Project Creation Flow

```{.mermaid format=pdf}
sequenceDiagram
    actor F as Freelancer
    participant UI as Frontend
    participant Auth as Auth Middleware
    participant PC as Projects Controller
    participant DB as Database
    participant NS as Notification Service

    F->>UI: Click "Create Project"
    UI->>UI: Display project form
    F->>UI: Fill details & submit
    
    UI->>PC: POST /api/Projects<br/>(with JWT token)
    PC->>Auth: Validate token
    Auth-->>PC: Token valid, User claims
    
    PC->>PC: Check user role<br/>(Freelancer required)
    
    alt User is Freelancer
        PC->>DB: Create Project record
        DB-->>PC: Project created (ID)
        
        PC->>DB: Add ProjectMember<br/>(Freelancer as Collaborator)
        DB-->>PC: Member added
        
        PC-->>UI: 201 Created + ProjectDTO
        UI->>UI: Navigate to project details
        UI-->>F: Project created successfully
        
        UI->>PC: GET /api/Projects/{id}
        PC->>DB: Fetch project details
        DB-->>PC: Project data
        PC-->>UI: 200 OK + ProjectDTO
        UI-->>F: Display project dashboard
    else User not Freelancer
        PC-->>UI: 403 Forbidden
        UI-->>F: Display error message
    end
```

### 12.2 Task Comment and Notification Flow

```{.mermaid format=pdf}
sequenceDiagram
    actor C as Customer
    participant UI as Frontend
    participant TC as Tasks Controller
    participant DB as Database
    participant NS as Notification Service
    participant HUB as SignalR Hub
    actor F as Freelancer

    C->>UI: View task details
    UI->>TC: GET /api/projects/{pid}/Tasks/{tid}
    TC->>DB: Fetch task + comments
    DB-->>TC: Task data
    TC-->>UI: 200 OK + TaskResponse
    UI-->>C: Display task & comments
    
    C->>UI: Add comment
    UI->>TC: POST .../Tasks/{id}/comment
    TC->>DB: Create Comment record
    DB-->>TC: Comment created
    
    TC->>DB: Fetch comment with user
    DB-->>TC: Comment + User data
    
    TC->>NS: Notify project members
    NS->>DB: Get project members
    DB-->>NS: Member list
    
    loop For each member
        NS->>HUB: Send notification
        HUB-->>F: Push notification
    end
    
    TC-->>UI: 200 OK + CommentResponse
    UI->>UI: Append comment to list
    UI-->>C: Comment added
    
    Note over F: Receives real-time notification
    F->>F: Display toast: "New comment on Task"
```

### 12.3 File Download Flow

```{.mermaid format=pdf}
sequenceDiagram
    actor U as User
    participant UI as Frontend
    participant FC as Files Controller
    participant FS as File Service
    participant DISK as File System

    U->>UI: Click download button
    UI->>FC: GET /api/Files/{fileId}
    
    FC->>FS: GetFile(fileId)
    FS->>FS: Query database for file metadata
    
    alt File exists
        FS->>DISK: Read file from path
        DISK-->>FS: File bytes
        FS-->>FC: FileAttr (path, contentType, name)
        
        FC->>DISK: ReadAllBytesAsync()
        DISK-->>FC: File bytes
        
        FC->>FC: Set headers<br/>(Content-Disposition: attachment)
        FC-->>UI: File stream + headers
        UI->>UI: Trigger browser download
        UI-->>U: File downloaded
    else File not found
        FS-->>FC: null
        FC-->>UI: 404 Not Found
        UI-->>U: Display error
    end
```

### 12.4 Admin User Suspension Flow

```{.mermaid format=pdf}
sequenceDiagram
    actor A as Admin
    participant UI as Admin Dashboard
    participant Auth as Auth Middleware
    participant AC as Admin Controller
    participant DB as Database
    participant Cache as Token Cache

    A->>UI: View user list
    A->>UI: Click "Suspend User"
    
    UI->>AC: POST /api/admin/suspend/{userId}<br/>(with JWT token)
    AC->>Auth: Validate token
    Auth-->>AC: Token valid + claims
    
    AC->>AC: Check role claim
    
    alt User is Admin
        AC->>DB: Find user by ID
        DB-->>AC: User record
        
        alt User exists
            AC->>DB: Set IsSuspended = true
            DB-->>AC: Update successful
            
            Note over AC,Cache: Optional: Invalidate<br/>user's active tokens
            
            AC-->>UI: 200 OK
            UI->>UI: Update user list
            UI-->>A: User suspended successfully
        else User not found
            AC-->>UI: 404 Not Found
            UI-->>A: Display error
        end
    else User not Admin
        AC-->>UI: 403 Forbidden
        UI-->>A: Insufficient permissions
    end
```

---

## Class Diagram

```{.mermaid format=pdf}
classDiagram
    class User {
        +string Id
        +string Email
        +string PasswordHash
        +string Name
        +Role Role
        +bool IsSuspended
        +DateTime CreatedAt
        +Profile Profile
    }
    
    class Profile {
        +string Id
        +string Bio
        +string Phone
        +string AvatarPath
        +DateTime CreatedAt
        +DateTime UpdatedAt
    }
    
    class Project {
        +string Id
        +string Title
        +string Description
        +string OwnerId
        +ProjectStatus Status
        +DateTime? DueDate
        +DateTime CreatedAt
        +List~ProjectMember~ Members
    }
    
    class ProjectMember {
        +string ProjectId
        +string UserId
        +MemberRole Role
        +DateTime JoinedAt
    }
    
    class TaskItem {
        +string Id
        +string ProjectId
        +string CreatorId
        +string Title
        +string Description
        +TaskStatus Status
        +DateTime? DueDate
        +DateTime CreatedAt
        +DateTime UpdatedAt
    }
    
    class FileEntity {
        +int Id
        +string UploaderId
        +string ProjectId
        +string TaskId
        +string Filename
        +string Path
        +long Size
        +string Mime
        +DateTime UploadedAt
    }
    
    class Comment {
        +string Id
        +string UserId
        +string TaskId
        +string Body
        +DateTime CreatedAt
    }
    
    User "1" --> "1" Profile : has
    User "1" --> "*" Project : owns
    User "1" --> "*" ProjectMember : participates
    User "1" --> "*" TaskItem : creates
    User "1" --> "*" FileEntity : uploads
    User "1" --> "*" Comment : writes
    
    Project "1" --> "*" ProjectMember : has
    Project "1" --> "*" TaskItem : contains
    Project "1" --> "*" FileEntity : has
    
    TaskItem "1" --> "*" Comment : has
    TaskItem "1" --> "*" FileEntity : has
```

---

## Use Case Diagram

```{.mermaid format=pdf}
graph LR
    subgraph Actors
        A[Admin]
        F[Freelancer]
        C[Customer]
    end
    
    subgraph System["Client Portal System"]
        UC1[Login/Logout]
        UC2[Manage Profile]
        UC3[Create Project]
        UC4[View Projects]
        UC5[Create Task]
        UC6[Update Task Status]
        UC7[Upload Files]
        UC8[Download Files]
        UC9[Add Comments]
        UC10[Invite Users]
        UC11[View Dashboard]
        UC12[Suspend Users]
        UC13[View Statistics]
    end
    
    A --> UC1
    A --> UC2
    A --> UC4
    A --> UC11
    A --> UC12
    A --> UC13
    
    F --> UC1
    F --> UC2
    F --> UC3
    F --> UC4
    F --> UC5
    F --> UC6
    F --> UC7
    F --> UC8
    F --> UC9
    F --> UC10
    F --> UC11
    F --> UC13
    
    C --> UC1
    C --> UC2
    C --> UC4
    C --> UC8
    C --> UC9
    C --> UC11
    C --> UC13
    
    style A fill:#ff6b6b
    style F fill:#4ecdc4
    style C fill:#95e1d3
```

---

## Technology Stack Diagram

```{.mermaid format=pdf}
mindmap
  root((Client Portal<br/>Tech Stack))
    Frontend
      React 18+
      TypeScript
      Vite
      Tailwind CSS
      Lucide Icons
      React Query
      React Router
    Backend
      ASP.NET Core 8.0
      C#
      Entity Framework Core
      SignalR
      JWT Bearer Auth
      Swagger/OpenAPI
    Database
      SQLite (Dev)
      SQL Server (Prod)
      PostgreSQL (Prod)
    Infrastructure
      HTTPS/HTTP
      WebSocket
      CORS
      RESTful API
    DevOps
      Git/GitHub
      dotnet CLI
      npm/pnpm
      Migrations
```

---

## Summary

These diagrams provide comprehensive visual documentation for:

✅ **System Architecture** - High-level overview of all components  
✅ **Database Design** - Entity relationships and schema  
✅ **Authentication** - Security flow and token management  
✅ **User Roles** - Permission hierarchy  
✅ **API Flow** - Request/response lifecycle  
✅ **Component Structure** - Frontend architecture  
✅ **File Handling** - Upload/download processes  
✅ **Real-Time** - SignalR notification flow  
✅ **Workflows** - Project and task lifecycles  
✅ **Deployment** - Production architecture options  
✅ **Sequence Diagrams** - Detailed interaction flows  
✅ **Class Diagram** - Domain model structure  
✅ **Use Cases** - Role-based functionality  

### How to Use These Diagrams

1. **For Development:** Use sequence diagrams to understand feature implementation
2. **For Documentation:** Include in technical specifications and onboarding materials
3. **For Presentations:** Use architecture diagrams to explain system design
4. **For Database:** Reference ERD when writing queries or migrations
5. **For Testing:** Use state machines to create test cases

### Rendering These Diagrams

These Mermaid diagrams can
