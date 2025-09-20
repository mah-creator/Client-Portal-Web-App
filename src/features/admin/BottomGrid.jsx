import BarChartIcon from "../../icons/BarChartIcon";
import PulseIcon from "../../icons/PulseIcon";
import SettingsIcon from "../../icons/SettingsIcon";
import UserCogIcon from "../../icons/UserCogIcon";

function BottomGrid({ activities }) {
  return (
    <section className="items-start gap-6 grid lg:grid-cols-3 mt-8">
      <div className="lg:col-span-2">
        <h2 className="mb-4 font-semibold text-xl">Recent Activity</h2>
        <div className="bg-white shadow p-2 border border-slate-200 rounded-2xl">
          {activities.map((a, i) => (
            <ActivityRow key={a.id} {...a} last={i === activities.length - 1} />
          ))}
        </div>
      </div>

      <aside>
        <h2 className="mb-4 font-semibold text-xl">Quick Actions</h2>
        <div className="bg-white shadow p-2 border border-slate-200 rounded-2xl">
          <ActionRow icon={<UserCogIcon />} label="User Management" />
          <ActionRow icon={<PulseIcon />} label="System Logs" />
          <ActionRow icon={<BarChartIcon />} label="Analytics" />
          <ActionRow icon={<SettingsIcon />} label="System Settings" last />
        </div>
      </aside>
    </section>
  );
}

export default BottomGrid;

function ActivityRow({ user, text, project, time, last }) {
  return (
    <div
      className={`flex items-start gap-3 p-4 ${
        last ? "" : "border-b border-slate-100"
      }`}
    >
      <span className="bg-blue-500 mt-2 rounded-full w-2 h-2" />
      <div className="flex-1">
        <p className="text-sm">
          <span className="font-medium">{user}</span> {text}{" "}
          <span className="font-medium">{project}</span>
        </p>
        <p className="mt-1 text-slate-500 text-xs">
          Client: {projectClient(project)} • {time}
        </p>
      </div>
    </div>
  );
}

function ActionRow({ icon, label, last }) {
  return (
    <button
      type="button"
      className={`flex w-full items-center gap-3 rounded-xl px-4 py-3 text-left hover:bg-slate-50 ${
        last ? "" : "border-b border-slate-100"
      }`}
    >
      <span className="place-items-center grid bg-slate-100 rounded-lg w-8 h-8 text-slate-700">
        {icon}
      </span>
      <span className="font-medium text-sm">{label}</span>
    </button>
  );
}
function projectClient(project) {
  if (project.includes("E-commerce")) return "TechCorp";
  if (project.includes("Mobile App")) return "StartupXYZ";
  if (project.includes("Brand Identity")) return "Creative Agency";
  return "Client";
}
