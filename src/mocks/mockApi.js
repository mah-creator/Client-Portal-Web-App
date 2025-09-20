import { freelancers, customers, admins } from "./mockDb";

const allUsers = [...freelancers, ...customers, ...admins];

function sleep(ms) {
  return new Promise((r) => setTimeout(r, ms));
}

export async function signUp(
  { name, email, password, role },
  { delayMs = 500 } = {}
) {
  await sleep(delayMs);

  if (!name?.trim() || !email?.trim() || !password || !role) {
    throw new Error("Missing required fields.");
  }
  if (password.length < 6) {
    throw new Error("Password must be at least 6 characters.");
  }

  const initials = name
    .split(" ")
    .map((s) => s[0] || "")
    .join("")
    .slice(0, 2)
    .toUpperCase();

  const idPrefix = role === "freelancer" ? "u" : "c";
  const id = `${idPrefix}${Date.now()}`;

  return { id, name: name.trim(), initials, role, email: email.trim() };
}

export async function login(email, password, { delayMs = 400 } = {}) {
  await new Promise((r) => setTimeout(r, delayMs));
  const u = allUsers.find((x) => x.email === email && x.password === password);
  if (!u) throw new Error("Invalid email or password.");
  const { id, name, initials, role } = u;
  return { id, name, initials, role, email };
}

export function fetchFreelancerDashboard(freelancerId, { delayMs = 500 } = {}) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const f = freelancers.find((x) => x.id === freelancerId);
      if (!f) return reject(new Error("Freelancer not found"));
      resolve({
        user: { id: f.id, name: f.name, role: f.role, initials: f.initials },
        stats: f.stats,
        projects: f.projects,
        activity: f.activity,
      });
    }, delayMs);
  });
}

export function fetchCustomerDashboard(customerId, { delayMs = 500 } = {}) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const c = customers.find((x) => x.id === customerId);
      if (!c) return reject(new Error("Customer not found"));
      resolve({
        user: { id: c.id, name: c.name, role: c.role, initials: c.initials },
        projects: c.projects,
        notifications: c.notifications,
        files: c.files,
      });
    }, delayMs);
  });
}

export function fetchAdminDashboard(adminId, { delayMs = 500 } = {}) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const a = admins.find((x) => x.id === adminId);
      if (!a) return reject(new Error("Admin not found"));
      resolve({
        user: { id: a.id, name: a.name, role: a.role, initials: a.initials },
        stats: a.stats,
        recentUsers: a.recentUsers,
        alerts: a.alerts,
        activities: a.activities,
      });
    }, delayMs);
  });
}
