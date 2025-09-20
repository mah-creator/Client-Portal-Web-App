// src/mocks/mockApi.js
import { freelancers, customers } from "./mockDb";

// helper: combine users for auth
const allUsers = [...freelancers, ...customers];

function sleep(ms) {
  return new Promise((r) => setTimeout(r, ms));
}

export async function signUp(
  { name, email, password, role },
  { delayMs = 500 } = {}
) {
  await sleep(delayMs);

  // Basic validations (frontend safety; real checks happen server-side)
  if (!name?.trim() || !email?.trim() || !password || !role) {
    throw new Error("Missing required fields.");
  }
  if (password.length < 6) {
    throw new Error("Password must be at least 6 characters.");
  }

  // Generate a temporary ID (just for the success screen)
  const initials = name
    .split(" ")
    .map((s) => s[0] || "")
    .join("")
    .slice(0, 2)
    .toUpperCase();

  const idPrefix = role === "freelancer" ? "u" : "c";
  const id = `${idPrefix}${Date.now()}`; // e.g., u1699999999999

  // Return what a real API would typically return
  return { id, name: name.trim(), initials, role, email: email.trim() };
}

export async function login(email, password, { delayMs = 400 } = {}) {
  await new Promise((r) => setTimeout(r, delayMs));
  const u = allUsers.find((x) => x.email === email && x.password === password);
  if (!u) throw new Error("Invalid email or password.");
  const { id, name, initials, role } = u;
  return { id, name, initials, role, email };
}

// ---------- Dashboards ----------
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
