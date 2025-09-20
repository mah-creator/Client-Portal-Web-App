import { useEffect, useState } from "react";
import { Navigate } from "react-router";
import { useAuth } from "../contexts/AuthContext";
import { fetchAdminDashboard } from "../mocks/mockApi";
import BottomGrid from "../features/admin/BottomGrid";
import MainGrid from "../features/admin/MainGrid";
import StatCards from "../features/admin/StatCards";
import TopBar from "../ui/TopBar";

export default function Admin() {
  const { user, loading: authLoading } = useAuth();
  const [data, setData] = useState(null);
  const [err, setErr] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (authLoading) return;
    if (!user || user.role !== "admin") return;

    let dead = false;
    setLoading(true);
    setErr("");

    fetchAdminDashboard(user.id)
      .then((res) => !dead && setData(res))
      .catch((e) => !dead && setErr(e.message || "Failed to load admin data"))
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
  if (!user) return <Navigate to="/login" replace />;
  if (user.role !== "admin") return <Navigate to="/" replace />;

  if (loading) {
    return (
      <div className="bg-slate-50 min-h-screen">
        <div className="mx-auto px-6 py-10 max-w-7xl">
          <div className="bg-slate-200 rounded w-56 h-8 animate-pulse" />
          <div className="gap-4 grid sm:grid-cols-2 lg:grid-cols-4 mt-6">
            {[...Array(4)].map((_, i) => (
              <div
                key={i}
                className="bg-white/70 shadow-sm rounded-2xl ring-1 ring-slate-200 h-28 animate-pulse"
              />
            ))}
          </div>
        </div>
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

  const { stats, recentUsers, alerts, activities } = data;

  return (
    <div className="bg-gradient-to-b from-sky-100 via-sky-50 to-sky-100 min-h-screen text-slate-800">
      <TopBar name={user.name} initials={user.initials} />

      <main className="mx-auto px-6 py-8 max-w-7xl">
        <header className="mb-6">
          <h1 className="font-semibold text-2xl">Admin Panel</h1>
          <p className="text-slate-500">System Management</p>
        </header>
        <StatCards stats={stats} />
        <MainGrid recentUsers={recentUsers} alerts={alerts} />
        <BottomGrid activities={activities} />
      </main>
    </div>
  );
}
