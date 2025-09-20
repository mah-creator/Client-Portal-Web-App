import Header from "../features/customer/Header";
import MainGrid from "../features/customer/MainGrid";
import TopBar from "../ui/TopBar";

export default function Customer({ customer }) {
  const data = customer ?? demoCustomer; // fallback demo data

  return (
    <div className="bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 min-h-screen text-slate-800">
      <TopBar name={data.name} role="customer" initials={data.initials} />
      <main className="mx-auto px-6 py-8 max-w-7xl">
        <Header data={data} />
        <MainGrid data={data} />
      </main>
    </div>
  );
}

/* ----------------------------- Demo data ----------------------------- */

const demoCustomer = {
  id: "c1",
  role: "customer",
  name: "Jane Smith",
  initials: "JS",
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
};

/* ------------------------------ Icons ------------------------------ */
