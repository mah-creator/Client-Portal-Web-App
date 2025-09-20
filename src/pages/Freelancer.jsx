import { useEffect, useState } from "react";
import { fetchFreelancerDashboard } from "../mocks/mockApi";

import { Navigate } from "react-router";
import { useAuth } from "../contexts/AuthContext";
import StatCards from "../features/freelancer/StatCards";
import ProjectCards from "../features/freelancer/ProjectCards";
import TopBar from "../ui/TopBar";

export default function Freelancer() {
  const { user, loading: authLoading } = useAuth();
  const [data, setData] = useState(null);
  const [err, setErr] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (authLoading) return;
    if (!user || user.role !== "freelancer") return; // handled below by Navigate

    setLoading(true);
    setErr("");
    let dead = false;
    fetchFreelancerDashboard(user.id)
      .then((res) => !dead && setData(res))
      .catch((e) => !dead && setErr(e.message || "Failed to load"))
      .finally(() => !dead && setLoading(false));
    return () => {
      dead = true;
    };
  }, [user, authLoading]);

  if (authLoading) {
    return (
      <div className="place-items-center grid bg-slate-50 min-h-screen">
        Loading…
      </div>
    );
  }

  if (!user || user.role !== "freelancer") {
    // Not logged in or wrong role → bounce to login
    return <Navigate to="/login" replace />;
  }

  if (loading) {
    return (
      <div className="place-items-center grid bg-slate-50 min-h-screen">
        Loading dashboard…
      </div>
    );
  }

  if (err) {
    return (
      <div className="place-items-center grid bg-slate-50 p-6 min-h-screen text-center">
        <div>
          <p className="font-semibold text-rose-600 text-lg">Error</p>
          <p className="text-slate-600">{err}</p>
        </div>
      </div>
    );
  }

  const { stats, projects, activity } = data;

  return (
    <div className="bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 min-h-screen text-slate-800">
      <TopBar name={user.name} initials={user.initials} />
      <main className="mx-auto px-6 py-8 max-w-7xl">
        <StatCards stats={stats} />
        <ProjectCards projects={projects} activity={activity} />
      </main>
    </div>
  );
}
