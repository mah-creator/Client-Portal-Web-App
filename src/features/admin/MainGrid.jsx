import ClockIcon from "../../icons/ClockIcon";
import SettingsIcon from "../../icons/SettingsIcon";
import ShieldIcon from "../../icons/ShieldIcon";
import UserBadgeIcon from "../../icons/UserBadgeIcon";

function MainGrid({ recentUsers, alerts }) {
  return (
    <section className="items-start gap-6 grid lg:grid-cols-3 mt-8">
      <div className="lg:col-span-2">
        <div className="flex justify-between items-center mb-4">
          <h2 className="font-semibold text-xl">Recent Users</h2>
          <button className="inline-flex items-center gap-2 bg-white hover:bg-slate-50 px-3 py-2 border border-slate-200 rounded-xl text-sm">
            <SettingsIcon /> Manage Users
          </button>
        </div>
        <div className="bg-white shadow p-2 border border-slate-200 rounded-2xl">
          {recentUsers.map((u, i) => (
            <UserRow key={u.id} {...u} last={i === recentUsers.length - 1} />
          ))}
        </div>
      </div>

      <aside>
        <h2 className="mb-4 font-semibold text-xl">System Alerts</h2>
        <div className="bg-white shadow p-4 border border-slate-200 rounded-2xl">
          {alerts.map((a) => (
            <AlertCard key={a.id} {...a} />
          ))}
        </div>
      </aside>
    </section>
  );
}

export default MainGrid;

function AlertCard({ level, title, meta, time }) {
  const tone =
    level === "high"
      ? "bg-rose-50 text-rose-700 border-rose-200"
      : level === "medium"
      ? "bg-amber-50 text-amber-700 border-amber-200"
      : "bg-sky-50 text-sky-700 border-sky-200";
  const chipTone =
    level === "high"
      ? "bg-rose-100 text-rose-700"
      : level === "medium"
      ? "bg-amber-100 text-amber-700"
      : "bg-sky-100 text-sky-700";

  return (
    <div className={`mb-3 rounded-xl border px-4 py-3 ${tone}`}>
      <div className="flex justify-between items-center mb-1">
        <p className="font-medium leading-snug">{title}</p>
        <span
          className={`rounded-full px-2 py-0.5 text-xs font-medium ${chipTone}`}
        >
          {level}
        </span>
      </div>
      {meta && <p className="opacity-90 text-sm">{meta}</p>}
      <div className="inline-flex items-center gap-2 opacity-80 mt-2 text-xs">
        <ClockIcon /> {time}
      </div>
    </div>
  );
}

function UserRow({ name, email, role, status, last }) {
  const statusTone =
    status === "Active"
      ? "bg-emerald-100 text-emerald-700"
      : "bg-rose-100 text-rose-700";
  return (
    <div
      className={`flex items-center justify-between gap-4 p-4 ${
        last ? "" : "border-b border-slate-100"
      }`}
    >
      <div className="flex items-center gap-4">
        <div className="place-items-center grid bg-slate-100 rounded-full w-10 h-10 text-slate-700">
          {initials(name)}
        </div>
        <div>
          <p className="font-medium">{name}</p>
          <p className="text-slate-500 text-sm">{email}</p>
        </div>
      </div>

      <div className="flex items-center gap-4">
        <span
          className={`rounded-full px-3 py-1 text-xs font-medium ${statusTone}`}
        >
          {status}
        </span>
        <div className="flex items-center gap-2 text-slate-500">
          <UserBadgeIcon title="View user" />
          <ShieldIcon title="Permissions" />
        </div>
      </div>

      <div className="w-28 text-right">
        <p className="text-slate-500 text-xs capitalize">{role}</p>
      </div>
    </div>
  );
}
function initials(name) {
  return name
    .split(" ")
    .map((s) => s[0] || "")
    .join("")
    .slice(0, 2)
    .toUpperCase();
}
