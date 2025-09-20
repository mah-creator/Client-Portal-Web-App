export const freelancers = [
  {
    id: "u1",
    name: "Abdullah Abusharekh",
    role: "freelancer",
    initials: "AA",
    email: "john@example.com",
    password: "123456",
    stats: {
      activeProjects: 3,
      tasksCompleted: 19,
      pendingTasks: 13,
      revenueThisMonth: 4200,
    },
    projects: [
      {
        id: "p1",
        title: "E-commerce Website Redesign",
        client: "TechCorp Inc.",
        due: "2024-02-15",
        status: { label: "in progress", color: "bg-blue-100 text-blue-700" },
        tasksDone: 8,
        tasksTotal: 12,
        progress: 67,
      },
      {
        id: "p2",
        title: "Mobile App Development",
        client: "StartupXYZ",
        due: "2024-03-01",
        status: { label: "planning", color: "bg-amber-100 text-amber-700" },
        tasksDone: 3,
        tasksTotal: 20,
        progress: 15,
      },
      {
        id: "p3",
        title: "Brand Identity Package",
        client: "Creative Agency",
        due: "2024-01-30",
        status: { label: "review", color: "bg-emerald-100 text-emerald-700" },
        tasksDone: 8,
        tasksTotal: 8,
        progress: 100,
      },
    ],
    activity: [
      {
        id: "a1",
        text: "Task 'Homepage Design' marked as completed",
        time: "2 hours ago",
      },
      { id: "a2", text: "New comment from TechCorp Inc.", time: "4 hours ago" },
      {
        id: "a3",
        text: "Design assets uploaded to Mobile App project",
        time: "6 hours ago",
      },
      {
        id: "a4",
        text: "New project invitation sent to Creative Agency",
        time: "1 day ago",
      },
    ],
  },

  {
    id: "u2",
    name: "Sara Ali",
    role: "freelancer",
    initials: "SA",
    email: "sara@example.com",
    password: "password",

    stats: {
      activeProjects: 2,
      tasksCompleted: 5,
      pendingTasks: 7,
      revenueThisMonth: 950,
    },
    projects: [
      {
        id: "p10",
        title: "Landing Page Refresh",
        client: "Acme Co.",
        due: "2024-03-10",
        status: { label: "in progress", color: "bg-blue-100 text-blue-700" },
        tasksDone: 2,
        tasksTotal: 6,
        progress: 33,
      },
    ],
    activity: [
      { id: "a10", text: "Client approved wireframes", time: "3 hours ago" },
    ],
  },
];

export const customers = [
  {
    id: "c1",
    role: "customer",
    name: "Jane Smith",
    initials: "JS",
    email: "jane@example.com",
    password: "123456",
    projects: [
      {
        id: "p1",
        title: "E-commerce Website Redesign",
        owner: "John Doe",
        due: "2024-02-15",
        status: { label: "in progress", color: "bg-blue-100 text-blue-700" },
        tasksDone: 8,
        tasksTotal: 12,
        progress: 67,
        updated: "2 hours ago",
      },
      {
        id: "p2",
        title: "Brand Identity Package",
        owner: "Sarah Wilson",
        due: "2024-01-30",
        status: { label: "review", color: "bg-amber-100 text-amber-700" },
        tasksDone: 8,
        tasksTotal: 8,
        progress: 100,
        updated: "1 day ago",
      },
    ],
    notifications: [
      {
        id: "n1",
        title: "Task 'Homepage Design' has been completed",
        project: "E-commerce Website",
        time: "2 hours ago",
      },
      {
        id: "n2",
        title: "New comment on 'Logo Review' task",
        project: "Brand Identity",
        time: "4 hours ago",
      },
      {
        id: "n3",
        title: "New design files uploaded",
        project: "E-commerce Website",
        time: "6 hours ago",
      },
    ],
    files: [
      {
        id: "f1",
        name: "Homepage-Design-v2.figma",
        meta: "E-commerce Website • John Doe • 2 hours ago",
      },
      {
        id: "f2",
        name: "Logo-Concepts.pdf",
        meta: "Brand Identity • Sarah Wilson • 1 day ago",
      },
      {
        id: "f3",
        name: "Style-Guide.pdf",
        meta: "Brand Identity • Sarah Wilson • 2 days ago",
      },
    ],
  },

  {
    id: "c2",
    role: "customer",
    name: "Mark Johnson",
    initials: "MJ",
    email: "mark@example.com",
    password: "password",
    projects: [
      {
        id: "p3",
        title: "Mobile App Prototype",
        owner: "Alice Lee",
        due: "2024-03-10",
        status: { label: "planning", color: "bg-emerald-100 text-emerald-700" },
        tasksDone: 2,
        tasksTotal: 10,
        progress: 20,
        updated: "3 hours ago",
      },
    ],
    notifications: [
      {
        id: "n4",
        title: "Kickoff meeting scheduled",
        project: "Mobile App Prototype",
        time: "1 day ago",
      },
    ],
    files: [
      {
        id: "f4",
        name: "Wireframes.pdf",
        meta: "Mobile App Prototype • Alice Lee • 1 day ago",
      },
    ],
  },
];

export const admins = [
  {
    id: "a1",
    role: "admin",
    name: "Admin",
    initials: "AD",
    email: "admin@example.com",
    password: "admin123",

    stats: {
      totalUsers: { value: 124, delta: "+12%" },
      activeProjects: { value: 45, delta: "+8%" },
      systemAlerts: { value: 3, delta: "-2" },
      storageUsed: { value: 68, delta: "+5%" },
    },

    recentUsers: [
      {
        id: "u1",
        name: "John Doe",
        email: "john@example.com",
        role: "freelancer",
        status: "Active",
      },
      {
        id: "c1",
        name: "Jane Smith",
        email: "jane@company.com",
        role: "customer",
        status: "Active",
      },
      {
        id: "u3",
        name: "Mike Johnson",
        email: "mike@freelance.com",
        role: "freelancer",
        status: "Suspended",
      },
      {
        id: "u4",
        name: "Sarah Wilson",
        email: "sarah@design.com",
        role: "freelancer",
        status: "Active",
      },
    ],

    alerts: [
      {
        id: "al1",
        level: "high",
        title: "Multiple failed login attempts from IP 192.168.1.100",
        meta: "",
        time: "10 min ago",
      },
      {
        id: "al2",
        level: "medium",
        title: "Storage usage approaching 80% capacity",
        meta: "",
        time: "2 hours ago",
      },
      {
        id: "al3",
        level: "low",
        title: "Database response time increased by 15%",
        meta: "",
        time: "4 hours ago",
      },
    ],

    activities: [
      {
        id: "ac1",
        user: "John Doe",
        text: "file uploaded in",
        project: "E-commerce Website",
        time: "1 hour ago",
      },
      {
        id: "ac2",
        user: "Sarah Wilson",
        text: "task completed in",
        project: "Mobile App",
        time: "3 hours ago",
      },
      {
        id: "ac3",
        user: "Mike Johnson",
        text: "comment added in",
        project: "Brand Identity",
        time: "5 hours ago",
      },
    ],
  },
];
