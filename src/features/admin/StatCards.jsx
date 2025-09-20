import AlertTriangleIcon from "../../icons/AlertTriangleIcon";
import ChartIcon from "../../icons/ChartIcon";
import DocIcon from "../../icons/DocIcon";
import UsersIcon from "../../icons/UsersIcon";

function StatCards({ stats }) {
  return (
    <section className="gap-4 grid sm:grid-cols-2 lg:grid-cols-4">
      <StatCard
        title="Total Users"
        value={stats.totalUsers.value}
        delta={stats.totalUsers.delta}
        deltaTone="pos"
        icon={<UsersIcon />}
      />
      <StatCard
        title="Active Projects"
        value={stats.activeProjects.value}
        delta={stats.activeProjects.delta}
        deltaTone="pos"
        icon={<DocIcon />}
      />
      <StatCard
        title="System Alerts"
        value={stats.systemAlerts.value}
        delta={stats.systemAlerts.delta}
        deltaTone="neg"
        icon={<AlertTriangleIcon />}
      />
      <StatCard
        title="Storage Used"
        value={`${stats.storageUsed.value}%`}
        delta={stats.storageUsed.delta}
        deltaTone="pos"
        icon={<ChartIcon />}
      />
    </section>
  );
}

export default StatCards;

function StatCard({ title, value, delta, deltaTone = "pos", icon }) {
  const tone =
    deltaTone === "neg"
      ? "text-rose-600 bg-rose-50"
      : "text-emerald-600 bg-emerald-50";
  return (
    <div className="flex justify-between items-center bg-white shadow-sm p-5 border border-slate-200 rounded-2xl">
      <div>
        <p className="text-slate-500 text-sm">{title}</p>
        <p className="mt-1 font-semibold text-2xl">{value}</p>
      </div>
      <div className="flex items-center gap-3">
        <span className={`rounded-full px-2 py-1 text-xs font-medium ${tone}`}>
          {delta}
        </span>
        <span className="place-items-center grid bg-blue-50 rounded-xl w-10 h-10 text-blue-600">
          {icon}
        </span>
      </div>
    </div>
  );
}
