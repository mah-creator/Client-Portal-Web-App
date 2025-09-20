import CalendarIcon from "../../icons/CalendarIcon";
import UsersIcon from "../../icons/UsersIcon";

function ProjectCard({
  title,
  client,
  due,
  status,
  tasksDone,
  tasksTotal,
  progress,
}) {
  return (
    <article className="bg-white shadow-sm mb-5 p-5 border border-slate-200 rounded-2xl">
      <div className="flex justify-between items-start gap-3">
        <div>
          <h3 className="font-semibold text-lg">{title}</h3>
          <div className="flex flex-wrap items-center gap-4 mt-2 text-slate-600 text-sm">
            <span className="inline-flex items-center gap-2">
              <UsersIcon /> {client}
            </span>
            <span className="inline-flex items-center gap-2">
              <CalendarIcon /> <span>Due {due}</span>
            </span>
          </div>
        </div>
        <div className="flex items-center gap-4">
          <span className="text-slate-500 text-sm">
            {tasksDone}/{tasksTotal} tasks
          </span>
          {status?.label && (
            <span
              className={`rounded-full px-3 py-1 text-xs font-medium ${status.color}`}
            >
              {status.label}
            </span>
          )}
        </div>
      </div>

      {/* Progress bar */}
      <div className="bg-slate-100 mt-4 rounded-full">
        <div
          className="bg-blue-200 rounded-full h-2"
          style={{ width: `${Math.min(Math.max(progress, 0), 100)}%` }}
        />
      </div>
      <p className="mt-2 text-slate-600 text-sm">{progress}% complete</p>
    </article>
  );
}

export default ProjectCard;
